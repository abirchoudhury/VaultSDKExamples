/*=====================================================================
  
  This file is part of the Autodesk Vault API Code Samples.

  Copyright (C) Autodesk Inc.  All rights reserved.

THIS CODE AND INFORMATION ARE PROVIDED "AS IS" WITHOUT WARRANTY OF ANY
KIND, EITHER EXPRESSED OR IMPLIED, INCLUDING BUT NOT LIMITED TO THE
IMPLIED WARRANTIES OF MERCHANTABILITY AND/OR FITNESS FOR A
PARTICULAR PURPOSE.
=====================================================================*/

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Diagnostics;
using System.IO;
using System.Windows.Forms;

using ACW = Autodesk.Connectivity.WebServices;
using ACWT = Autodesk.Connectivity.WebServicesTools;
using VDF = Autodesk.DataManagement.Client.Framework;

namespace VaultBrowserSample
{
    class OpenFileCommand
    {
        private static List<string> m_downloadedFiles = new List<string>();

        /// <summary>
        /// Downloads a file from Vault and opens it.  The program used to load the file is 
        /// based on the user's OS settings.
        /// </summary>
        /// <param name="fileId"></param>
        public static void Execute(VDF.Vault.Currency.Entities.FileIteration file, VDF.Vault.Currency.Connections.Connection connection)
        {
            string filePath = Path.Combine(Application.LocalUserAppDataPath, file.EntityName);

            //determine if the file already exists
            if (System.IO.File.Exists(filePath))
            {
                //we'll try to delete the file so we can get the latest copy
                try
                {
                    System.IO.File.Delete(filePath);

                    //remove the file from the collection of downloaded files that need to be removed when the application exits
                    if (m_downloadedFiles.Contains(filePath))
                        m_downloadedFiles.Remove(filePath);
                }
                catch (System.IO.IOException)
                {
                    throw new Exception("The file you are attempting to open already exists and can not be overwritten. This file may currently be open, try closing any application you are using to view this file and try opening the file again.");
                }
            }

            downloadFile(connection, file, Path.GetDirectoryName(filePath));
            m_downloadedFiles.Add(filePath);

            //Create a new ProcessStartInfo structure.
            ProcessStartInfo pInfo = new ProcessStartInfo();
            //Set the file name member. 
            pInfo.FileName = filePath;
            //UseShellExecute is true by default. It is set here for illustration.
            pInfo.UseShellExecute = true;
            Process p = Process.Start(pInfo);
        }

        private static void downloadFile(VDF.Vault.Currency.Connections.Connection connection, VDF.Vault.Currency.Entities.FileIteration file, string folderPath)
        {
            VDF.Vault.Settings.AcquireFilesSettings settings = new VDF.Vault.Settings.AcquireFilesSettings(connection);
            settings.AddEntityToAcquire(file);
            settings.LocalPath = new VDF.Currency.FolderPathAbsolute(folderPath);
            connection.FileManager.AcquireFiles(settings);
        }

        /// <summary>
        /// This should be called when the application exits
        /// </summary>
        public static void OnExit()
        {
            // try and clean up any files which were downloaded
            foreach (string file in m_downloadedFiles)
            {
                try
                {
                    System.IO.File.Delete(file);
                }
                catch (Exception) { }
            }
        }
    }
}
