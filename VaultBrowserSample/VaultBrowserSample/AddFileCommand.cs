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
using System.IO;
using System.Text;

using ACW = Autodesk.Connectivity.WebServices;
using ACWT = Autodesk.Connectivity.WebServicesTools;
using VDF = Autodesk.DataManagement.Client.Framework;

namespace VaultBrowserSample
{
    class AddFileCommand
    {
        /// <summary>
        /// Upload a file to Vault
        /// </summary>
        /// <param name="filePath">The full path to a local file.</param>
        /// <param name="folderId">The ID of the Vault folder where the file will be uploaded.</param>
        public static void Execute(string filePath, VDF.Vault.Currency.Entities.Folder parent, VDF.Vault.Currency.Connections.Connection connection)
        {
            using (FileStream stream = new FileStream(filePath, FileMode.Open, FileAccess.Read))
            {
                connection.FileManager.AddFile(parent,
                    Path.GetFileName(filePath), "Added by Vault Browser",
                    DateTime.Now, null, null,
                    ACW.FileClassification.None, false, stream);
            }
        }
    }
}
