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
using System.Windows.Forms;
using System.Xml;
using System.Linq;

using ACW = Autodesk.Connectivity.WebServices;


namespace ItemEditor
{
	/// <summary>
	/// Parses the data from a server error.
	/// </summary>
	public class ErrorHandler
	{
        public static void HandleError(Exception e)
        {
            ACW.VaultServiceErrorException vseEx = e as ACW.VaultServiceErrorException;

            if (vseEx == null)
            {
                // error was not thrown by the server
                MessageBox.Show(e.ToString(), "Error");
                return;
            }

            if (vseEx.ErrorCode == 0)
            {
                MessageBox.Show(e.ToString());
                return;
            }

            if (vseEx.ErrorCode == 1387)
            {
                // print restriction data
                string msgString = "";
                vseEx.Restrictions.Select(r => msgString += "Restriction Code = " + r.Code + Environment.NewLine);
                MessageBox.Show(msgString, "Server Error");
            }
            else
                MessageBox.Show("Error Code = " + vseEx.ErrorCode, "Server Error");
        }
	}
}
