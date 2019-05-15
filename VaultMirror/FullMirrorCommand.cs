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
using System.Threading;
using Autodesk.Connectivity.WebServices;
using Autodesk.DataManagement.Client.Framework.Currency;
using ADSK = Autodesk.Connectivity.WebServices;

namespace VaultMirror
{
	/// <summary>
	/// Summary description for FullMirrorCommand.
	/// </summary>
	sealed class FullMirrorCommand : Command
	{
        public FullMirrorCommand(ICommandReporter commandReporeter, string username, string password,
            string server, string vault, string outputFolder, bool useWorkingFolder, bool failOnError, CancellationToken ct)
            : base(commandReporeter, username, password, server, vault, outputFolder, useWorkingFolder, failOnError, ct)
		{
		}

        public override void Execute_Impl()
        {
            ChangeStatusMessage("Starting Full Mirror...");
            // cycle through all of the files in the Vault and place them on disk if needed
            Folder root = Connection.WebServiceManager.DocumentService.GetFolderRoot();

            // cycle through all of the files on disk and make sure that they are in the Vault
            string localPath = OutputFolder;
            FullMirrorVaultFolder(root, localPath);

            // smithmat [2-20-2015]:
            //   https://jira.autodesk.com/browse/PDM-3725
            //   There is a defect here where the FullMirrorLocalFolder deletes all _V folders/files
            //   placed on disk.  This is because it looks for files that do not exist in Vault and deletes them.
            //   One attempt at eliminating this bug was to do FullMirroLocalFolder before FullMirroVaultFolder above.
            //   This fixes the problem on the first run, but on the next run, the _V files are deleted and not re-added because
            //   the code in FullMirroVaultFolder realizes that the files are up to date and doesn't re-download.
            // 
            FullMirrorLocalFolder(root, localPath);
        }

        private void FullMirrorVaultFolder(Folder folder, string localFolder)
        {
            FullMirrorVaultFolderRecursive(folder, localFolder);
            DownloadFiles();
        }

        private void FullMirrorVaultFolderRecursive(Folder folder, string localFolder)
        {
            ThrowIfCancellationRequested();

            if (folder.Cloaked)
                return;

            if (!UseWorkingFolder && !Directory.Exists(localFolder))
                Directory.CreateDirectory(localFolder);

            ADSK.File[] files = Connection.WebServiceManager.DocumentService.GetLatestFilesByFolderId(
                folder.Id, true);
            if (files != null)
            {
                foreach (ADSK.File file in files)
                {
                    ThrowIfCancellationRequested();

                    if (file.Cloaked)
                        continue;

                    if (UseWorkingFolder)
                        AddFileToDownload(file, null);
                    else
                    {
                        string filePath = Path.Combine(localFolder, file.Name);
                        if (System.IO.File.Exists(filePath))
                        {
                            if (file.CreateDate != System.IO.File.GetCreationTime(filePath))
                                AddFileToDownload(file, filePath);
                        }
                        else
                            AddFileToDownload(file, filePath);
                    }
                }
            }

            Folder[] subFolders = Connection.WebServiceManager.DocumentService.GetFoldersByParentId(folder.Id, false);
            if (subFolders != null)
            {
                foreach (Folder subFolder in subFolders)
                {
                    if (!UseWorkingFolder)
                        FullMirrorVaultFolderRecursive(subFolder, Path.Combine(localFolder, subFolder.Name));
                    else
                        FullMirrorVaultFolderRecursive(subFolder, null);
                }
            }
        }

        private void FullMirrorLocalFolder(Folder folder, string localFolder)
        {
            ThrowIfCancellationRequested();
            if (folder.Cloaked)
                return;

            // delete any files on disk that are not in the vault
            string loc = localFolder;
            if (UseWorkingFolder)
            {
                FolderPathAbsolute folderPath = Connection.WorkingFoldersManager.GetWorkingFolder(folder.FullName);
                loc = folderPath.FullPath;
            }
            if (!Directory.Exists(loc))
                return;
            string[] localFiles = Directory.GetFiles(loc);

            ADSK.File[] vaultFiles = Connection.WebServiceManager.DocumentService.GetLatestFilesByFolderId(folder.Id, true);

            if (vaultFiles == null && localFiles != null)
            {
                foreach (string localFile in localFiles)
                {
                    ThrowIfCancellationRequested();
                    DeleteFile(localFile);
                }
            }
            else
            {
                foreach (string localFile in localFiles)
                {
                    ThrowIfCancellationRequested();
                    bool fileFound = false;
                    string filename = Path.GetFileName(localFile);
                    foreach (ADSK.File vaultFile in vaultFiles)
                    {
                        if (!vaultFile.Cloaked && vaultFile.Name == filename)
                        {
                            fileFound = true;
                            break;
                        }
                    }

                    if (!fileFound)
                        DeleteFile(localFile);
                }
            }

            // recurse the subdirectories and delete any folders not in the Vault
            if (UseWorkingFolder)
            {
                // working folders may not be in the same configuration on disk as in the Vault,
                // so we can't assume sub folders are tied to Vault.  Do folders on disk should be deleted.
                Folder[] vaultSubFolders = Connection.WebServiceManager.DocumentService.GetFoldersByParentId(folder.Id, false);

                if (vaultSubFolders == null)
                    return;

                foreach (Folder vaultSubFolder in vaultSubFolders)
                {
                    FullMirrorLocalFolder(vaultSubFolder, null);
                }

            }
            else
            {
                string[] localFullPaths = Directory.GetDirectories(localFolder);

                if (localFullPaths != null)
                {
                    foreach (string localFullPath in localFullPaths)
                    {
                        ThrowIfCancellationRequested();

                        string vaultPath = folder.FullName + "/" + Path.GetFileName(localFullPath);
                        Folder[] vaultSubFolder = Connection.WebServiceManager.DocumentService.FindFoldersByPaths(
                            new string[] { vaultPath });

                        if (vaultSubFolder[0].Id < 0)
                            DeleteFolder(localFullPath);
                        else
                            FullMirrorLocalFolder(vaultSubFolder[0], localFullPath);
                    }
                }
            }
        }

	}
}
