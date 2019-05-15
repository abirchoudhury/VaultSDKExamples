/*=====================================================================
  
  This file is part of the Autodesk Vault API Code Samples.

  Copyright (C) Autodesk Inc.  All rights reserved.

THIS CODE AND INFORMATION ARE PROVIDED "AS IS" WITHOUT WARRANTY OF ANY
KIND, EITHER EXPRESSED OR IMPLIED, INCLUDING BUT NOT LIMITED TO THE
IMPLIED WARRANTIES OF MERCHANTABILITY AND/OR FITNESS FOR A
PARTICULAR PURPOSE.
=====================================================================*/

using System;
using System.Linq;
using System.IO;

using ACJE = Autodesk.Connectivity.JobProcessor.Extensibility;
using ACW = Autodesk.Connectivity.WebServices;
using ACWT = Autodesk.Connectivity.WebServicesTools;
using VDF = Autodesk.DataManagement.Client.Framework;


namespace JobProcessorApiSamples
{
    public class FolderPublishJobHandler : ACJE.IJobHandler
    {
        private string TargetFolder { get; set; }


        public FolderPublishJobHandler()
        {
            TargetFolder = "C:\\PublishFolder\\";
        }

        #region IJobHandler Members

        public bool CanProcess(string jobType)
        {
            if (jobType.ToLower().Equals("MyCompany.File.Publish".ToLower()))
            {
                return true;
            }

            return false;
        }

        public ACJE.JobOutcome Execute(ACJE.IJobProcessorServices context, ACJE.IJob job)
        {
            long fileMasterId = Convert.ToInt64(job.Params["FileMasterId"]);

            try 
            {
                // Retrieve the file object from the server
                //
                ACW.File file = context.Connection.WebServiceManager.DocumentService.GetLatestFileByMasterId(fileMasterId);
                VDF.Vault.Currency.Entities.FileIteration fileIter = 
                    context.Connection.FileManager.GetFilesByIterationIds(new long[] { file.Id }).First().Value;

                // Download and publish the file
                //
                Publish(fileIter, context.Connection);

                return ACJE.JobOutcome.Success;
            }
            catch
            {
                return ACJE.JobOutcome.Failure;
            }
        }

        public void OnJobProcessorStartup(ACJE.IJobProcessorServices context) { }

        public void OnJobProcessorShutdown(ACJE.IJobProcessorServices context) { }

        public void OnJobProcessorWake(ACJE.IJobProcessorServices context) { }

        public void OnJobProcessorSleep(ACJE.IJobProcessorServices context) { }

        #endregion


        private void Publish(VDF.Vault.Currency.Entities.FileIteration fileIter, VDF.Vault.Currency.Connections.Connection connection)
        {
            System.IO.DirectoryInfo targetDir = new System.IO.DirectoryInfo(TargetFolder);
            if (!targetDir.Exists)
            {
                targetDir.Create();
            }

            VDF.Vault.Settings.AcquireFilesSettings downloadSettings = new VDF.Vault.Settings.AcquireFilesSettings(connection)
            {
                LocalPath = new VDF.Currency.FolderPathAbsolute(targetDir.FullName),
            };
            downloadSettings.AddFileToAcquire(fileIter, VDF.Vault.Settings.AcquireFilesSettings.AcquisitionOption.Download);
            connection.FileManager.AcquireFiles(downloadSettings);
        }
    }
} 
        
