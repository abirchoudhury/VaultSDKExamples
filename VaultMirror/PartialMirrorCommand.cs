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
using System.Collections.Generic;

using Autodesk.Connectivity.WebServices;
using Autodesk.Connectivity.WebServicesTools;
using System.Threading;

namespace VaultMirror
{
	/// <summary>
	/// Summary description for PartialMirrorCommand.
	/// </summary>
	sealed class PartialMirrorCommand : Command
	{
        private readonly DateTime m_lastSyncTime;

		public PartialMirrorCommand(ICommandReporter commandReporter, string username, string password, 
            string server, string vault, string outputFolder, DateTime lastSyncTime,
            bool useWorkingFolder, bool failOnError, CancellationToken ct)
            : base(commandReporter, username, password, server, vault, outputFolder, useWorkingFolder, failOnError, ct)
		{
			m_lastSyncTime = lastSyncTime;
		}

        public override void Execute_Impl()
        {
            ThrowIfCancellationRequested();

            ChangeStatusMessage("Starting Partial Mirror...");

            // find the Create Date property
            PropDef[] props = Connection.WebServiceManager.PropertyService.FindPropertyDefinitionsBySystemNames(
                "FILE", new string [] {"CheckInDate"});

            if (props.Length != 1 || props[0].Id < 0)
                throw new Exception("Error looking up CheckInDate property");

            // grab all of the files added or checked in since the last sync time
            SrchCond condition = new SrchCond();
            condition.SrchTxt = m_lastSyncTime.ToUniversalTime().ToString("MM/dd/yyyy HH:mm:ss");
            condition.SrchOper = 7; // greater than or equal to
            condition.PropDefId = props[0].Id;
            condition.PropTyp = PropertySearchType.SingleProperty;

            List<FileFolder> resultList = new List<FileFolder>();
            SrchStatus status = null;
            string bookmark = string.Empty;

            while (status == null || resultList.Count < status.TotalHits)
            {
                ThrowIfCancellationRequested();
                FileFolder[] results = Connection.WebServiceManager.DocumentService.FindFileFoldersBySearchConditions(
                    new SrchCond[] { condition },
                    new SrchSort[]{new SrchSort() { PropDefId = props[0].Id, SortAsc = false }},
                    null, true, false, ref bookmark, out status);

                if (results != null)
                    resultList.AddRange(results);
            }

            string localPath = OutputFolder;

            HashSet<long> mirroredFiles = new HashSet<long>();

            if (resultList.Count == 0)
                return;

            foreach (FileFolder result in resultList)
            {
                ThrowIfCancellationRequested();

                if (result == null || result.File.Cloaked || result.Folder.Cloaked)
                    continue;

                if (mirroredFiles.Contains(result.File.MasterId))
                    continue;

                string vaultFolder = result.Folder.FullName;
                if (vaultFolder == "$")
                    vaultFolder = "";
                else
                    vaultFolder = vaultFolder.Substring(2);  // remove the $/ at the beginning

                string localFolder = null;

                if (!UseWorkingFolder)
                {
                    localFolder = Path.Combine(localPath, vaultFolder);
                    if (!Directory.Exists(localFolder))
                        Directory.CreateDirectory(localFolder);
                    localFolder = Path.Combine(localFolder, result.File.Name);
                }

                AddFileToDownload(result.File, localFolder);

                mirroredFiles.Add(result.File.MasterId);
            }

            DownloadFiles();
        }
    }
}