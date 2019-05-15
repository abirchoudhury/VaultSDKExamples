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
	/// Summary description for CreateItemForm.
	/// </summary>
	public class CreateItemForm : System.Windows.Forms.Form
	{
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.ComboBox m_itemTypeComboBox;
        private System.Windows.Forms.TextBox m_itemTitleTextBox;
        private System.Windows.Forms.Button m_cancelButton;
        private System.Windows.Forms.Button m_okButton;
		/// <summary>
		/// Required designer variable.
		/// </summary>
		private System.ComponentModel.Container components = null;

        public string ItemTitle
        {
            get { return m_itemTitleTextBox.Text;   }
        }

        public Cat Category
        {
            get 
            {
                ListCategory listobject = (ListCategory)m_itemTypeComboBox.SelectedItem;
                return listobject.Category;  
            }
        }

		public CreateItemForm(VDF.Vault.Currency.Connections.Connection connection)
		{
			//
			// Required for Windows Form Designer support
			//
			InitializeComponent();

            CategoryService catSvc = connection.WebServiceManager.CategoryService;

            Cat[] categories = catSvc.GetCategoriesByEntityClassId("ITEM", true);
            foreach (Cat category in categories)
            {
                m_itemTypeComboBox.Items.Add(new ListCategory(category));
            }
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
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.m_itemTypeComboBox = new System.Windows.Forms.ComboBox();
            this.m_itemTitleTextBox = new System.Windows.Forms.TextBox();
            this.m_cancelButton = new System.Windows.Forms.Button();
            this.m_okButton = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // label1
            // 
            this.label1.Location = new System.Drawing.Point(8, 8);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(72, 16);
            this.label1.TabIndex = 0;
            this.label1.Text = "Title:";
            // 
            // label2
            // 
            this.label2.Location = new System.Drawing.Point(8, 32);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(72, 16);
            this.label2.TabIndex = 1;
            this.label2.Text = "Type:";
            // 
            // m_itemTypeComboBox
            // 
            this.m_itemTypeComboBox.Location = new System.Drawing.Point(88, 32);
            this.m_itemTypeComboBox.Name = "m_itemTypeComboBox";
            this.m_itemTypeComboBox.Size = new System.Drawing.Size(192, 21);
            this.m_itemTypeComboBox.TabIndex = 2;
            // 
            // m_itemTitleTextBox
            // 
            this.m_itemTitleTextBox.Location = new System.Drawing.Point(88, 8);
            this.m_itemTitleTextBox.Name = "m_itemTitleTextBox";
            this.m_itemTitleTextBox.Size = new System.Drawing.Size(192, 20);
            this.m_itemTitleTextBox.TabIndex = 1;
            this.m_itemTitleTextBox.Text = "";
            // 
            // m_cancelButton
            // 
            this.m_cancelButton.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.m_cancelButton.Location = new System.Drawing.Point(192, 64);
            this.m_cancelButton.Name = "m_cancelButton";
            this.m_cancelButton.Size = new System.Drawing.Size(88, 24);
            this.m_cancelButton.TabIndex = 4;
            this.m_cancelButton.Text = "Cancel";
            // 
            // m_okButton
            // 
            this.m_okButton.Location = new System.Drawing.Point(96, 64);
            this.m_okButton.Name = "m_okButton";
            this.m_okButton.Size = new System.Drawing.Size(88, 24);
            this.m_okButton.TabIndex = 3;
            this.m_okButton.Text = "OK";
            this.m_okButton.Click += new System.EventHandler(this.m_okButton_Click);
            // 
            // CreateItemForm
            // 
            this.AcceptButton = this.m_okButton;
            this.AutoScaleBaseSize = new System.Drawing.Size(5, 13);
            this.CancelButton = this.m_cancelButton;
            this.ClientSize = new System.Drawing.Size(292, 101);
            this.Controls.Add(this.m_okButton);
            this.Controls.Add(this.m_cancelButton);
            this.Controls.Add(this.m_itemTitleTextBox);
            this.Controls.Add(this.m_itemTypeComboBox);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.label1);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.Name = "CreateItemForm";
            this.Text = "CreateItemForm";
            this.ResumeLayout(false);

        }
		#endregion

        private void m_okButton_Click(object sender, System.EventArgs e)
        {
            DialogResult = DialogResult.OK;
        }

	}

    // Wraps the ItemTyp object to display in combo box
    public class ListCategory
    {
        public Cat Category;

        public ListCategory(Cat category)
        {
            this.Category = category;
        }

        public override string ToString()
        {
            return this.Category.Name;
        }

    }
}
