/*=====================================================================

  This file is part of the Autodesk Vault API Code Samples.

  Copyright (C) Autodesk Inc.  All rights reserved.

THIS CODE AND INFORMATION ARE PROVIDED "AS IS" WITHOUT WARRANTY OF ANY
KIND, EITHER EXPRESSED OR IMPLIED, INCLUDING BUT NOT LIMITED TO THE
IMPLIED WARRANTIES OF MERCHANTABILITY AND/OR FITNESS FOR A
PARTICULAR PURPOSE.
=====================================================================*/

using System;
using System.Drawing;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Windows.Forms;
using System.Data;
using System.Linq;

using Autodesk.Connectivity.WebServices;
using Autodesk.Connectivity.WebServicesTools;
using VDF = Autodesk.DataManagement.Client.Framework;

namespace VaultList
{
	/// <summary>
	/// This is our main form.
	/// </summary>
	public class Form1 : System.Windows.Forms.Form
	{
		private System.Windows.Forms.Button button1;
        private System.Windows.Forms.ListBox m_listBox;

		/// <summary>
		/// Required designer variable.
		/// </summary>
		private System.ComponentModel.Container components = null;

		public Form1()
		{
			//
			// Required for Windows Form Designer support
			//
			InitializeComponent();
		}

		/// <summary>
		/// Clean up any resources being used.
		/// </summary>
		protected override void Dispose( bool disposing )
		{
			if( disposing )
			{
				if (components != null)
				{
					components.Dispose();
				}
			}
			base.Dispose( disposing );
		}

		#region Windows Form Designer generated code
		/// <summary>
		/// Required method for Designer support - do not modify
		/// the contents of this method with the code editor.
		/// </summary>
		private void InitializeComponent()
		{
            this.button1 = new System.Windows.Forms.Button();
            this.m_listBox = new System.Windows.Forms.ListBox();
            this.SuspendLayout();
            // 
            // button1
            // 
            this.button1.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.button1.Location = new System.Drawing.Point(0, 250);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(292, 23);
            this.button1.TabIndex = 1;
            this.button1.Text = "List Files";
            this.button1.Click += new System.EventHandler(this.button1_Click);
            // 
            // m_listBox
            // 
            this.m_listBox.Dock = System.Windows.Forms.DockStyle.Fill;
            this.m_listBox.Location = new System.Drawing.Point(0, 0);
            this.m_listBox.Name = "m_listBox";
            this.m_listBox.Size = new System.Drawing.Size(292, 238);
            this.m_listBox.TabIndex = 0;
            // 
            // Form1
            // 
            this.AutoScaleBaseSize = new System.Drawing.Size(5, 13);
            this.ClientSize = new System.Drawing.Size(292, 273);
            this.Controls.Add(this.m_listBox);
            this.Controls.Add(this.button1);
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "Form1";
            this.Text = "Form1";
            this.ResumeLayout(false);

        }
		#endregion

		/// <summary>
		/// The main entry point for the application.
		/// </summary>
		[STAThread]
		static void Main()
		{
            Form1 appobject = new Form1();

            Application.Run(appobject);
		}

        private void button1_Click(object sender, System.EventArgs e)
        {
            ListAllFiles();
        }

        /// <summary>
        /// This function lists all the files in the Vault and displays them in the form's ListBox.
        /// </summary>
        public void ListAllFiles()
        {
            // For demonstration purposes, the information is hard-coded.
            VDF.Vault.Results.LogInResult results = VDF.Vault.Library.ConnectionManager.LogIn(
                "localhost", "Vault", "Administrator", "", VDF.Vault.Currency.Connections.AuthenticationFlags.Standard, null
                );

            if (!results.Success)
                return;

            VDF.Vault.Currency.Connections.Connection connection = results.Connection;

            try
            {
                // Start at the root Folder.
                VDF.Vault.Currency.Entities.Folder root = connection.FolderManager.RootFolder;

                this.m_listBox.Items.Clear();
                    
                // Call a function which prints all files in a Folder and sub-Folders.
                PrintFilesInFolder(root, connection);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString(), "Error");
                return;
            }

            VDF.Vault.Library.ConnectionManager.LogOut(connection);
		}


        /// <summary>
        /// Prints all the files in the current Folder and any sub Folders.
        /// </summary>
        /// <param name="parentFolder">The folder we want to print.</param>
        /// <param name="connection">The manager object for making Vault Server calls.</param>
        private void PrintFilesInFolder(VDF.Vault.Currency.Entities.Folder parentFolder, VDF.Vault.Currency.Connections.Connection connection)
		{
            // get all the Files in the current Folder.
            File[] childFiles = connection.WebServiceManager.DocumentService.GetLatestFilesByFolderId(parentFolder.Id, false);

            // print out any Files we find.
            if (childFiles != null && childFiles.Any())
			{
				foreach (File file in childFiles)
				{
                    // print the full path, which is Folder name + File name.
					this.m_listBox.Items.Add(parentFolder.FullName + "/" + file.Name);
				}
			}

            // check for any sub Folders.
            IEnumerable<VDF.Vault.Currency.Entities.Folder> folders = connection.FolderManager.GetChildFolders(parentFolder, false, false);
			if (folders != null && folders.Any())
			{
				foreach (VDF.Vault.Currency.Entities.Folder folder in folders)
				{
                    // recursively print the files in each sub-Folder
                    PrintFilesInFolder(folder, connection);
				}
			}
		}
	}
}
