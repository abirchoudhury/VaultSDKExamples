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
using System.ComponentModel;
using System.Windows.Forms;

using Autodesk.Connectivity.WebServices;
using Autodesk.Connectivity.WebServicesTools;
using VDF = Autodesk.DataManagement.Client.Framework;

namespace ItemEditor
{
	/// <summary>
	/// Summary description for ChangeRevisionForm.
	/// </summary>
    public class ChangeRevisionForm : System.Windows.Forms.Form
    {
        private System.Windows.Forms.Button m_okButton;
        private System.Windows.Forms.Button m_cancelButton;
        private System.Windows.Forms.RadioButton m_primaryRadioButton;
        private System.Windows.Forms.RadioButton m_secondaryRadioButton;
        private System.Windows.Forms.RadioButton m_tertiaryRadioButton;
        private System.Windows.Forms.RadioButton m_customRadioButton;
        private System.Windows.Forms.GroupBox m_groupBox;
        private System.Windows.Forms.TextBox m_customTextBox;
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.Container components = null;

        public string SelectedRevisionNumber
        {
            get
            {
                if (m_primaryRadioButton.Checked)
                    return m_primaryRadioButton.Text;
                else if (m_secondaryRadioButton.Checked)
                    return m_secondaryRadioButton.Text;
                else if (m_tertiaryRadioButton.Checked)
                    return m_tertiaryRadioButton.Text;
                else if (m_customRadioButton.Checked)
                    return m_customTextBox.Text;
                else
                    return null;
            }
        }

		public ChangeRevisionForm(Item item, VDF.Vault.Currency.Connections.Connection connection)
		{
			//
			// Required for Windows Form Designer support
			//
			InitializeComponent();

            ItemService itemSvc = connection.WebServiceManager.ItemService;
            long[] revDefIds = connection.WebServiceManager.RevisionService.GetRevisionDefinitionIdsByMasterIds(new long [] { item.MasterId });
            StringArray revNumbers = connection.WebServiceManager.RevisionService.GetNextRevisionNumbersByMasterIds(new long [] { item.MasterId }, revDefIds)[0];

            this.m_primaryRadioButton.Text = revNumbers.Items[0];
            this.m_secondaryRadioButton.Text = revNumbers.Items[1];
            this.m_tertiaryRadioButton.Text = revNumbers.Items[2];
		}

		/// <summary>
		/// Clean up any resources being used.
		/// </summary>
		protected override void Dispose( bool disposing )
		{
			if( disposing )
			{
				if(components != null)
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
            this.m_okButton = new System.Windows.Forms.Button();
            this.m_cancelButton = new System.Windows.Forms.Button();
            this.m_groupBox = new System.Windows.Forms.GroupBox();
            this.m_customTextBox = new System.Windows.Forms.TextBox();
            this.m_customRadioButton = new System.Windows.Forms.RadioButton();
            this.m_tertiaryRadioButton = new System.Windows.Forms.RadioButton();
            this.m_secondaryRadioButton = new System.Windows.Forms.RadioButton();
            this.m_primaryRadioButton = new System.Windows.Forms.RadioButton();
            this.m_groupBox.SuspendLayout();
            this.SuspendLayout();
            // 
            // m_okButton
            // 
            this.m_okButton.Location = new System.Drawing.Point(96, 168);
            this.m_okButton.Name = "m_okButton";
            this.m_okButton.Size = new System.Drawing.Size(88, 24);
            this.m_okButton.TabIndex = 5;
            this.m_okButton.Text = "OK";
            this.m_okButton.Click += new System.EventHandler(this.m_okButton_Click);
            // 
            // m_cancelButton
            // 
            this.m_cancelButton.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.m_cancelButton.Location = new System.Drawing.Point(192, 168);
            this.m_cancelButton.Name = "m_cancelButton";
            this.m_cancelButton.Size = new System.Drawing.Size(88, 24);
            this.m_cancelButton.TabIndex = 6;
            this.m_cancelButton.Text = "Cancel";
            // 
            // m_groupBox
            // 
            this.m_groupBox.Controls.Add(this.m_customTextBox);
            this.m_groupBox.Controls.Add(this.m_customRadioButton);
            this.m_groupBox.Controls.Add(this.m_tertiaryRadioButton);
            this.m_groupBox.Controls.Add(this.m_secondaryRadioButton);
            this.m_groupBox.Controls.Add(this.m_primaryRadioButton);
            this.m_groupBox.Location = new System.Drawing.Point(8, 8);
            this.m_groupBox.Name = "m_groupBox";
            this.m_groupBox.Size = new System.Drawing.Size(272, 152);
            this.m_groupBox.TabIndex = 7;
            this.m_groupBox.TabStop = false;
            this.m_groupBox.Text = "Revision Number";
            // 
            // m_customTextBox
            // 
            this.m_customTextBox.Location = new System.Drawing.Point(80, 120);
            this.m_customTextBox.Name = "m_customTextBox";
            this.m_customTextBox.Size = new System.Drawing.Size(184, 20);
            this.m_customTextBox.TabIndex = 4;
            this.m_customTextBox.Text = "";
            // 
            // m_customRadioButton
            // 
            this.m_customRadioButton.Location = new System.Drawing.Point(16, 112);
            this.m_customRadioButton.Name = "m_customRadioButton";
            this.m_customRadioButton.Size = new System.Drawing.Size(64, 32);
            this.m_customRadioButton.TabIndex = 3;
            this.m_customRadioButton.Text = "Custom";
            // 
            // m_tertiaryRadioButton
            // 
            this.m_tertiaryRadioButton.Location = new System.Drawing.Point(16, 80);
            this.m_tertiaryRadioButton.Name = "m_tertiaryRadioButton";
            this.m_tertiaryRadioButton.Size = new System.Drawing.Size(248, 32);
            this.m_tertiaryRadioButton.TabIndex = 2;
            this.m_tertiaryRadioButton.Text = "Tertiary";
            // 
            // m_secondaryRadioButton
            // 
            this.m_secondaryRadioButton.Location = new System.Drawing.Point(16, 48);
            this.m_secondaryRadioButton.Name = "m_secondaryRadioButton";
            this.m_secondaryRadioButton.Size = new System.Drawing.Size(248, 32);
            this.m_secondaryRadioButton.TabIndex = 1;
            this.m_secondaryRadioButton.Text = "Secondary";
            // 
            // m_primaryRadioButton
            // 
            this.m_primaryRadioButton.Location = new System.Drawing.Point(16, 16);
            this.m_primaryRadioButton.Name = "m_primaryRadioButton";
            this.m_primaryRadioButton.Size = new System.Drawing.Size(248, 32);
            this.m_primaryRadioButton.TabIndex = 0;
            this.m_primaryRadioButton.Text = "Primary";
            // 
            // ChangeRevisionForm
            // 
            this.AutoScaleBaseSize = new System.Drawing.Size(5, 13);
            this.ClientSize = new System.Drawing.Size(288, 197);
            this.Controls.Add(this.m_groupBox);
            this.Controls.Add(this.m_okButton);
            this.Controls.Add(this.m_cancelButton);
            this.Name = "ChangeRevisionForm";
            this.Text = "Select New Revision Number";
            this.m_groupBox.ResumeLayout(false);
            this.ResumeLayout(false);

        }
		#endregion

        private void m_okButton_Click(object sender, System.EventArgs e)
        {
            DialogResult = DialogResult.OK;
        }
	}
}
