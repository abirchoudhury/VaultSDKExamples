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
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

using RestrictOperations;

namespace Configurator
{
    public partial class Form1 : Form
    {
        private List<string> m_commands = new List<string>()
        {
            // File commands
            "AddFile",
            "CheckinFile",
            "CheckoutFile" ,
            "DeleteFile",
            "DownloadFile",
            "UpdateFileLifecycleState",

            // Folder Events
            "AddFolder",
            "DeleteFolder",

            // Item Events
            "AddItem", 
            "CommitItem", 
            "ItemRollbackLifeCycleStates",
            "DeleteItem",
            "EditItem",
            "PromoteItem", 
            "UpdateItemLifecycleState",
            
            // Change Order Events
            "AddChangeOrder",
            "CommitChangeOrder",
            "DeleteChangeOrder", 
            "EditChangeOrder",
            "UpdateChangeOrderLifecycleState",

            // Custom Entity Events
            "UpdateCustomEntityLifeCycleState",
        };

        public Form1()
        {
            InitializeComponent();


            m_tableLayoutPanel.ColumnCount = 1;
            m_tableLayoutPanel.RowCount = 0;

            RestrictSettings settings = RestrictSettings.Load();
            foreach (string command in m_commands)
            {
                CheckBox checkbox = new CheckBox();
                checkbox.Text = command + " Command";
                checkbox.Tag = command;
                checkbox.Height = 20;
                checkbox.AutoSize = true;

                if (settings.RestrictedOperations.Contains(command))
                    checkbox.Checked = true;
                else
                    checkbox.Checked = false;

                checkbox.CheckedChanged += new EventHandler(checkbox_CheckedChanged);

                m_tableLayoutPanel.Controls.Add(checkbox);
                
            }
        }

        void checkbox_CheckedChanged(object sender, EventArgs e)
        {
            // do a complete update of the settings file

            RestrictSettings settings = new RestrictSettings();
            foreach (Control c in m_tableLayoutPanel.Controls)
            {
                CheckBox checkbox = c as CheckBox;
                if (c == null)
                    continue;

                if (checkbox.Checked)
                    settings.RestrictedOperations.Add((string)checkbox.Tag);
            }

            settings.Save();
        }
    }
}
