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
using System.Collections.Generic;
using System.ComponentModel;
using System.Windows.Forms;
using System.Linq;

using Autodesk.Connectivity.WebServices;
using Autodesk.Connectivity.WebServicesTools;
using VDF = Autodesk.DataManagement.Client.Framework;

namespace ItemEditor
{
	/// <summary>
	/// Summary description for ChangeLifeCycleForm.
	/// </summary>
	public class ChangeLifeCycleForm : System.Windows.Forms.Form
	{
        private System.Windows.Forms.ListBox m_lifeCycleListBox;
        private System.Windows.Forms.Button m_okButton;
        private System.Windows.Forms.Button m_cancelButton;
		/// <summary>
		/// Required designer variable.
		/// </summary>
		private System.ComponentModel.Container components = null;

        public long SelectedLifeCycleStateId
        {
            get 
            { 
                ListLifeCycle listobject = (ListLifeCycle)m_lifeCycleListBox.SelectedItem;
                return listobject.LfCycState.Id;  
            }
        }

        public ChangeLifeCycleForm(long currentLifeCycleDefId, long currentLifeCycleStateId, VDF.Vault.Currency.Connections.Connection connection)
        {
            //
            // Required for Windows Form Designer support
            //
            InitializeComponent();

            LifeCycleService lifecycleSvc = connection.WebServiceManager.LifeCycleService;

            // get all of the life cycle definitions
            LfCycDef[] definitions = lifecycleSvc.GetAllLifeCycleDefinitions();
            Dictionary<long, LfCycDef> lifeCycleMap = new Dictionary<long, LfCycDef>();

            // put the life cycle definitions into a hashtable for easy lookup
            foreach (LfCycDef definition in definitions)
            {
                lifeCycleMap[definition.Id] = definition;
            }

            LfCycDef currentLifeCycleDef = lifeCycleMap[currentLifeCycleDefId];
            
            // list each life cycle that the current Item can move to
            foreach (LfCycTrans lifeCycleTrans in currentLifeCycleDef.TransArray)
            {
                LfCycState state = currentLifeCycleDef.StateArray.FirstOrDefault(lfState => lfState.Id == lifeCycleTrans.ToId);
                if(state != null)
                    m_lifeCycleListBox.Items.Add(new ListLifeCycle(state));
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
            this.m_lifeCycleListBox = new System.Windows.Forms.ListBox();
            this.m_okButton = new System.Windows.Forms.Button();
            this.m_cancelButton = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // m_lifeCycleListBox
            // 
            this.m_lifeCycleListBox.Location = new System.Drawing.Point(8, 8);
            this.m_lifeCycleListBox.Name = "m_lifeCycleListBox";
            this.m_lifeCycleListBox.Size = new System.Drawing.Size(272, 82);
            this.m_lifeCycleListBox.TabIndex = 0;
            // 
            // m_okButton
            // 
            this.m_okButton.Location = new System.Drawing.Point(96, 104);
            this.m_okButton.Name = "m_okButton";
            this.m_okButton.Size = new System.Drawing.Size(88, 24);
            this.m_okButton.TabIndex = 5;
            this.m_okButton.Text = "OK";
            this.m_okButton.Click += new System.EventHandler(this.m_okButton_Click);
            // 
            // m_cancelButton
            // 
            this.m_cancelButton.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.m_cancelButton.Location = new System.Drawing.Point(192, 104);
            this.m_cancelButton.Name = "m_cancelButton";
            this.m_cancelButton.Size = new System.Drawing.Size(88, 24);
            this.m_cancelButton.TabIndex = 6;
            this.m_cancelButton.Text = "Cancel";
            // 
            // ChangeLifeCycleForm
            // 
            this.AcceptButton = this.m_okButton;
            this.AutoScaleBaseSize = new System.Drawing.Size(5, 13);
            this.CancelButton = this.m_cancelButton;
            this.ClientSize = new System.Drawing.Size(292, 141);
            this.Controls.Add(this.m_okButton);
            this.Controls.Add(this.m_cancelButton);
            this.Controls.Add(this.m_lifeCycleListBox);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.Name = "ChangeLifeCycleForm";
            this.Text = "Select Life Cycle State";
            this.ResumeLayout(false);

        }
		#endregion

        private void m_okButton_Click(object sender, System.EventArgs e)
        {
            DialogResult = DialogResult.OK;
        }
	}

    /// <summary>
    /// Used to display life cycles in a list box
    /// </summary>
    public class ListLifeCycle
    {
        public LfCycState LfCycState;

        public ListLifeCycle(LfCycState lifeCycleState)
        {
            this.LfCycState = lifeCycleState;
        }

        public override string ToString()
        {
            return this.LfCycState.Name;
        }
    }
}
