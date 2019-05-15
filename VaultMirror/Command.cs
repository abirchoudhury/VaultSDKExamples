/*=====================================================================
  
  This file is part of the Autodesk Vault API Code Samples.

  Copyright (C) Autodesk Inc.  All rights reserved.

THIS CODE AND INFORMATION ARE PROVIDED "AS IS" WITHOUT WARRANTY OF ANY
KIND, EITHER EXPRESSED OR IMPLIED, INCLUDING BUT NOT LIMITED TO THE
IMPLIED WARRANTIES OF MERCHANTABILITY AND/OR FITNESS FOR A
PARTICULAR PURPOSE.
=====================================================================*/

using System;
using System.IO;
using System.Linq;
using System.Windows.Forms;

using ADSK = Autodesk.Connectivity.WebServices;
using Autodesk.Connectivity.WebServicesTools;
using System.Collections.Generic;
using Autodesk.DataManagement.Client.Framework.Vault.Currency.Connections;
using Autodesk.DataManagement.Client.Framework.Vault.Settings;
using System.Threading;
using Autodesk.DataManagement.Client.Framework.Vault.Results;
using Autodesk.DataManagement.Client.Framework.Vault;
using Autodesk.DataManagement.Client.Framework.Vault.Currency.Entities;
using Autodesk.DataManagement.Client.Framework.Currency;


namespace VaultMirror
{
	/// <summary>
	/// The base class for a Vault Mirror command.
	/// Each command is self contained.  In other words, nothing is cached between commands,
	/// the client is re-logged in for each command.
	/// </summary>
    /// <remarks>Commands are "one use" objects.  After executing, a new command must be created and used</remarks>
	abstract class Command
	{
        private const long MAX_ERRORS_PRINTED = 500;

        private readonly Connection m_conn;
        private readonly string m_vault;
        private readonly ICommandReporter m_commandReporter;
        private readonly bool m_useWorkingFolder;
        private readonly bool m_failOnError;
        private readonly string m_outputFolder;
        private readonly AcquireFilesSettings m_aquireFilesSettings;
        private readonly CancellationToken m_ct;
 
		public Command(ICommandReporter commandReporter, string username, string password, string server, string vault, string outputFolder,
            bool useWorkingFolder, bool failOnError, CancellationToken ct)
		{
            if (commandReporter == null)
                throw new ArgumentNullException("commandReporter)");

            m_commandReporter = commandReporter;
            m_vault = vault;

            // add the vault name to the local path.  This prevents users from accedently
            // wiping out their C drive or something like that.
            m_useWorkingFolder = useWorkingFolder;
            if (!m_useWorkingFolder)
                m_outputFolder = UseWorkingFolder ? outputFolder : Path.Combine(outputFolder, VaultName);
            
            m_failOnError = failOnError;
            m_ct = ct;

            ChangeStatusMessage("Logging in...");
            Func<string, LoginStates, bool> progressCallback = (m, s) => !m_ct.IsCancellationRequested;
            LogInResult result = Library.ConnectionManager.LogIn(server, vault, username, password, AuthenticationFlags.ReadOnly, progressCallback);
            ThrowIfCancellationRequested();
            m_conn = result.Connection;
            if (!result.Success && result.Exception != null)
                throw result.Exception;
            else if (!result.Success && result.ErrorMessages.Any())
                throw new Exception(result.ErrorMessages.Values.FirstOrDefault());
            else if (!result.Success)
                throw new Exception("Unknown Error");

            m_aquireFilesSettings = CreateAcquireFileSettings();
		}

        protected Connection Connection { get { return m_conn;  } }
        protected string VaultName { get { return m_vault; } }
        protected bool UseWorkingFolder { get { return m_useWorkingFolder;  } }
        protected string OutputFolder { get { return m_outputFolder;  } }

        protected void ThrowIfCancellationRequested()
        {
            m_ct.ThrowIfCancellationRequested();
        }

        protected void ChangeStatusMessage(string message)
        {
            m_commandReporter.ReportStatus(message);
        }

        protected void DeleteFile(string filePath)
        {
            ChangeStatusMessage("Deleting " + filePath);

            System.IO.File.SetAttributes(filePath, FileAttributes.Normal);
            System.IO.File.Delete(filePath);
        }

        protected void DeleteFolder(string dirPath)
        {
            string [] subFolderPaths = Directory.GetDirectories(dirPath);
            if (subFolderPaths != null)
            {
                foreach (string subFolderPath in subFolderPaths)
                {
                    ThrowIfCancellationRequested();
                    DeleteFolder(subFolderPath);
                }
            }

            string [] filePaths = Directory.GetFiles(dirPath);
            if (filePaths != null)
            {
                foreach (string filePath in filePaths)
                {
                    ThrowIfCancellationRequested();
                    DeleteFile(filePath);
                }
            }

            Directory.Delete(dirPath);
        }


        protected void AddFileToDownload(ADSK.File file, string filePath)
        {
            ChangeStatusMessage("Evaluating " + file.Name);

            // remove the read-only attribute
            if (System.IO.File.Exists(filePath))
                System.IO.File.SetAttributes(filePath, FileAttributes.Normal);

            FileIteration fileIter = new FileIteration(m_conn, file);
            if (m_useWorkingFolder)
            {
                m_aquireFilesSettings.AddFileToAcquire(fileIter, AcquireFilesSettings.AcquisitionOption.Download);
            }
            else
            {
                m_aquireFilesSettings.AddFileToAcquire(fileIter, AcquireFilesSettings.AcquisitionOption.Download, 
                    new FilePathAbsolute(filePath));
            }
        }

        protected void DownloadFiles()
        {
            ChangeStatusMessage("Downloading...");

            AcquireFilesResults results = m_conn.FileManager.AcquireFiles(m_aquireFilesSettings);
            IEnumerable<FileAcquisitionResult> fileResults = results.FileResults;
            var failedFileResults = fileResults.Where(r => r.Exception != null);
            if (!failedFileResults.Any())
                return;

            if (m_failOnError)
                throw failedFileResults.First().Exception; // Throw the first error

            int errorCount = 0;
            foreach (var failedFileResult in failedFileResults)
            {
                ++errorCount;
                if (errorCount < MAX_ERRORS_PRINTED)
                {
                    m_commandReporter.Log("Error downloading file " + failedFileResult.File.EntityName);
                    m_commandReporter.Log(failedFileResult.Exception.ToString());
                    m_commandReporter.Log("-----------------------------------------------" + Environment.NewLine);
                }
                else if (errorCount == MAX_ERRORS_PRINTED)
                {
                    m_commandReporter.Log("Max error logging limit (" + MAX_ERRORS_PRINTED + ") reached.  Further errors will not be logged.");
                    break;
                }
            }
        }

        private AcquireFilesSettings CreateAcquireFileSettings()
        {
            var aquireFilesSettings = new AcquireFilesSettings(m_conn);
            var optionsResolution = aquireFilesSettings.OptionsResolution;
            optionsResolution.OverwriteOption = AcquireFilesSettings.AcquireFileResolutionOptions.OverwriteOptions.ForceOverwriteAll;

            var updateReferencesModel = optionsResolution.UpdateReferencesModel;
            updateReferencesModel.UpdateVaultStatus = true;

            var extensibilityOptions = aquireFilesSettings.OptionsExtensibility;
            extensibilityOptions.ProgressHandler = new DownloadProgressReporter(this);

            var threadingOptions = aquireFilesSettings.OptionsThreading;
            threadingOptions.CancellationToken = CancellationTokenSource.CreateLinkedTokenSource(new[] { m_ct });
            return aquireFilesSettings;
        }

        public void Execute()
        {
            try
            {
                ThrowIfCancellationRequested();
                Execute_Impl();
            }
            finally
            {
                Library.ConnectionManager.LogOut(m_conn);
            }
        }

        public abstract void Execute_Impl();

        class DownloadProgressReporter : Autodesk.DataManagement.Client.Framework.Interfaces.IProgressReporter
        {
            readonly Command m_command;

            public DownloadProgressReporter(Command command)
            {
                m_command = command;
            }

            public void ReportProgress(string header, IEnumerable<ProgressValue> progressValues, int percComplete = -1)
            {
                ReportProgress(String.Empty, header, progressValues, percComplete);
            }

            public void ReportProgress(string caption, string header, IEnumerable<ProgressValue> progressValues, int percComplete = -1)
            {
                // We don't want to display all the various strings that come in for the header value, we only want to display "Downloading..." and a percentage.
                // The problem is that the percentage comes in for both "Reading local files" and "Downloading".  Thus, I've used the fact that progressValues is
                // populated during download, to only show percentage during that time frame.
                if (percComplete != -1 && progressValues.Any())
                {
                    string message = string.Format("Downloading... {0}%", percComplete);

                    m_command.ChangeStatusMessage(message);
                }
            }
        }
    }

    interface ICommandReporter
    {
        void ReportStatus(string status);
        void Log(string message);
    }

}
