/*=====================================================================
  
  This file is part of the Autodesk Vault API Code Samples.

  Copyright (C) Autodesk Inc.  All rights reserved.

THIS CODE AND INFORMATION ARE PROVIDED "AS IS" WITHOUT WARRANTY OF ANY
KIND, EITHER EXPRESSED OR IMPLIED, INCLUDING BUT NOT LIMITED TO THE
IMPLIED WARRANTIES OF MERCHANTABILITY AND/OR FITNESS FOR A
PARTICULAR PURPOSE.
=====================================================================*/

using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Configuration;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.Globalization;
using System.Text;
using System.Windows.Forms;

using Autodesk.Connectivity.WebServices;
using Autodesk.Connectivity.WebServicesTools;
using VDF = Autodesk.DataManagement.Client.Framework;

namespace VaultBrowserSample
{

	/// <summary>
	/// The form containg our Vault file finder sample utility.
	/// </summary>
	public class FinderForm : System.Windows.Forms.Form
	{
        private const int BUFFERSIZE = 16384;

        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label PropertyComboBoxLabel;
        private System.Windows.Forms.ComboBox m_propertyComboBox;
        private System.Windows.Forms.ComboBox m_conditionComboBox;
        private System.Windows.Forms.Label ConditionComboBoxLabel;
        private System.Windows.Forms.TextBox m_valueTextBox;
        private System.Windows.Forms.Label ValueTextBoxLabel;
        private System.Windows.Forms.ListBox m_criteriaListBox;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Button m_addCriteriaButton;
        private System.Windows.Forms.Button m_removeCriteriaButton;
        private System.Windows.Forms.ListBox m_searchResultsListBox;
        private System.Windows.Forms.Label SearchResultLabel;
        private System.Windows.Forms.Label m_itemsCountLabel;
        private System.Windows.Forms.Button m_findButton;

        private VDF.Vault.Currency.Connections.Connection m_connection;

        private ContextMenuStrip contextMenuStrip1;
        private ToolStripMenuItem m_openFileToolStripMenuItem2;
        private IContainer components;



        public FinderForm(VDF.Vault.Currency.Connections.Connection connection)
		{

			//
			// Required for Windows Form Designer support
			//
			InitializeComponent();

            //
            // Form configuration and intialization
            //
            m_connection = connection;

            InitializePropertyComboBox();

            InitializeConditionComboBox();

		}

        /// <summary>
        /// Intializes the combox box containing the searchable properties available.
        /// </summary>
        private void InitializePropertyComboBox() 
        {

            //get the entire extended list of properties
            PropDef[] defs = m_connection.WebServiceManager.PropertyService.GetPropertyDefinitionsByEntityClassId(VDF.Vault.Currency.Entities.EntityClassIds.Files);
                
            if (defs != null && defs.Length > 0) 
            {
                Array.Sort(defs, new PropertyDefinitionSorter());

                //wait to draw the combo box until we've added all of the properties
                m_propertyComboBox.BeginUpdate();

                m_propertyComboBox.Items.Clear();

                foreach (PropDef def in defs) 
                {
                    //create a list item type that will hold the property
                    ListBoxPropDefItem item = new ListBoxPropDefItem(def);
               
                    m_propertyComboBox.Items.Add(item);
                }

                //indicate that we've finished updated the combobox and it can now be re-drawn
                m_propertyComboBox.EndUpdate();

            }
        
        }

        private void InitializeConditionComboBox() 
        {

            //wait to draw the combo box until we've populated it with conditions
            m_conditionComboBox.BeginUpdate();

            //populate the combo box with the conditions
            m_conditionComboBox.Items.AddRange( new Condition[] 
                {
                    Condition.CONTAINS, 
                    Condition.EQUALS, 
                    Condition.DOES_NOT_CONTAIN,
                    Condition.IS_EMPTY,
                    Condition.IS_NOT_EMPTY,
                    Condition.LESS_THAN_OR_EQUAL,
                    Condition.LESS_THAN,
                    Condition.GREATER_THAN_OR_EQUAL,
                    Condition.GREATER_THAN,
                    Condition.NOT_EQUAL
                } );

            //indicated that we're finished populating the combobox and that it can be re-drawn
            m_conditionComboBox.EndUpdate();

        }

        private void AddCriteriaButton_Click(object sender, System.EventArgs e)
        {
            //get a local reference of the currently selected PropertyDefinition object
            ListBoxPropDefItem propertyItem = m_propertyComboBox.SelectedItem as ListBoxPropDefItem;
            PropDef property;
            property = (propertyItem == null) ? null : propertyItem.PropDef;

            //get local reference of the condition combo boxes currently selected item
            Condition condition = m_conditionComboBox.SelectedItem as Condition;

            if (property == null || condition == null)
                return;

            //create a SearchCondition object
            SrchCond searchCondition = new SrchCond();
            searchCondition.PropDefId = property.Id;
            searchCondition.PropTyp = PropertySearchType.SingleProperty;
            searchCondition.SrchOper = condition.Code;
            searchCondition.SrchTxt = m_valueTextBox.Text;

            //create the list item to contain the condition
            ListBoxSrchCondItem searchItem = new ListBoxSrchCondItem(searchCondition, property);

            //add the SearchCondition object to the search criteria list box
            m_criteriaListBox.Items.Add(searchItem);
        }

        private void RemoveCriteriaButton_Click(object sender, System.EventArgs e)
        {
            //remove the currently selected search condition item from the list box
            if (m_criteriaListBox.SelectedItem != null)
                m_criteriaListBox.Items.RemoveAt(m_criteriaListBox.SelectedIndex);
        }

        private void FindButton_Click(object sender, System.EventArgs e)
        {
            //make sure search conditions have been added
            if (m_criteriaListBox.Items.Count > 0) 
            {
                //clear out previous search results if they exist
                m_searchResultsListBox.Items.Clear();

                //build our array of SearchConditions to use for the file search
                SrchCond[] conditions = new SrchCond[m_criteriaListBox.Items.Count];

                for (int i = 0; i < m_criteriaListBox.Items.Count; i++) 
                {
                    conditions[i] = ((ListBoxSrchCondItem)m_criteriaListBox.Items[i]).SrchCond;
                }

                string bookmark = string.Empty;
                SrchStatus status = null;

                //search for files
                List<File> fileList = new List<File>();

                while (status == null || fileList.Count < status.TotalHits)
                {
                    File[] files = m_connection.WebServiceManager.DocumentService.FindFilesBySearchConditions(
                        conditions, null, null, true, true,
                        ref bookmark, out status);

                    if (files != null)
                        fileList.AddRange(files);
                }

                if (fileList.Count > 0) 
                {
                    //iterate through found files and display them in the search results list box
                    foreach (File file in fileList) 
                    {
                        //create the list item that will wrap the File
                        ListBoxFileItem fileItem = new ListBoxFileItem(new VDF.Vault.Currency.Entities.FileIteration(m_connection, file));

                        m_searchResultsListBox.Items.Add(fileItem);
                    }
                }

                //update the items count label
                m_itemsCountLabel.Text = (fileList.Count > 0) ? fileList.Count + " Items" : "0 Items";
            }   
        }
        
        #region PropertyDefinitionSorter Class
        /// <summary>
        /// Used for sorting collections of PropertyDefinition's.
        /// </summary>
        private class PropertyDefinitionSorter : IComparer
        {
            /// <summary>
            /// Class (static) constructor that creates a static Comparer class instane used for sorting PropertyDefinition's.
            /// </summary>
            static PropertyDefinitionSorter() 
            {

                m_comparer = new Comparer(Application.CurrentCulture);

            }

            private static Comparer m_comparer;

            public int Compare(object x, object y)
            {
                PropDef propDefX = x as PropDef;
                PropDef propDefY = y as PropDef;

                lock (m_comparer) 
                {
                    
                    return m_comparer.Compare(propDefX.DispName, propDefY.DispName);

                }
                
            }

        }
        #endregion

		#region Windows Form Designer generated code
		/// <summary>
		/// Required method for Designer support - do not modify
		/// the contents of this method with the code editor.
		/// </summary>
		private void InitializeComponent()
		{
            this.components = new System.ComponentModel.Container();
            this.label1 = new System.Windows.Forms.Label();
            this.PropertyComboBoxLabel = new System.Windows.Forms.Label();
            this.m_propertyComboBox = new System.Windows.Forms.ComboBox();
            this.m_conditionComboBox = new System.Windows.Forms.ComboBox();
            this.ConditionComboBoxLabel = new System.Windows.Forms.Label();
            this.m_valueTextBox = new System.Windows.Forms.TextBox();
            this.ValueTextBoxLabel = new System.Windows.Forms.Label();
            this.m_criteriaListBox = new System.Windows.Forms.ListBox();
            this.label2 = new System.Windows.Forms.Label();
            this.m_addCriteriaButton = new System.Windows.Forms.Button();
            this.m_removeCriteriaButton = new System.Windows.Forms.Button();
            this.m_searchResultsListBox = new System.Windows.Forms.ListBox();
            this.SearchResultLabel = new System.Windows.Forms.Label();
            this.m_itemsCountLabel = new System.Windows.Forms.Label();
            this.m_findButton = new System.Windows.Forms.Button();
            this.contextMenuStrip1 = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.m_openFileToolStripMenuItem2 = new System.Windows.Forms.ToolStripMenuItem();
            this.contextMenuStrip1.SuspendLayout();
            this.SuspendLayout();
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label1.ForeColor = System.Drawing.Color.Blue;
            this.label1.Location = new System.Drawing.Point(8, 16);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(63, 13);
            this.label1.TabIndex = 0;
            this.label1.Text = "Search For:";
            // 
            // PropertyComboBoxLabel
            // 
            this.PropertyComboBoxLabel.AutoSize = true;
            this.PropertyComboBoxLabel.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.PropertyComboBoxLabel.Location = new System.Drawing.Point(24, 40);
            this.PropertyComboBoxLabel.Name = "PropertyComboBoxLabel";
            this.PropertyComboBoxLabel.Size = new System.Drawing.Size(53, 13);
            this.PropertyComboBoxLabel.TabIndex = 1;
            this.PropertyComboBoxLabel.Text = "Property:";
            // 
            // m_propertyComboBox
            // 
            this.m_propertyComboBox.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.m_propertyComboBox.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.m_propertyComboBox.ItemHeight = 13;
            this.m_propertyComboBox.Location = new System.Drawing.Point(24, 64);
            this.m_propertyComboBox.Name = "m_propertyComboBox";
            this.m_propertyComboBox.Size = new System.Drawing.Size(152, 21);
            this.m_propertyComboBox.TabIndex = 2;
            // 
            // m_conditionComboBox
            // 
            this.m_conditionComboBox.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.m_conditionComboBox.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.m_conditionComboBox.ItemHeight = 13;
            this.m_conditionComboBox.Location = new System.Drawing.Point(192, 64);
            this.m_conditionComboBox.Name = "m_conditionComboBox";
            this.m_conditionComboBox.Size = new System.Drawing.Size(112, 21);
            this.m_conditionComboBox.TabIndex = 3;
            // 
            // ConditionComboBoxLabel
            // 
            this.ConditionComboBoxLabel.AutoSize = true;
            this.ConditionComboBoxLabel.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.ConditionComboBoxLabel.Location = new System.Drawing.Point(192, 40);
            this.ConditionComboBoxLabel.Name = "ConditionComboBoxLabel";
            this.ConditionComboBoxLabel.Size = new System.Drawing.Size(56, 13);
            this.ConditionComboBoxLabel.TabIndex = 4;
            this.ConditionComboBoxLabel.Text = "Condition:";
            // 
            // m_valueTextBox
            // 
            this.m_valueTextBox.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.m_valueTextBox.Location = new System.Drawing.Point(320, 64);
            this.m_valueTextBox.Name = "m_valueTextBox";
            this.m_valueTextBox.Size = new System.Drawing.Size(272, 21);
            this.m_valueTextBox.TabIndex = 5;
            // 
            // ValueTextBoxLabel
            // 
            this.ValueTextBoxLabel.AutoSize = true;
            this.ValueTextBoxLabel.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.ValueTextBoxLabel.Location = new System.Drawing.Point(320, 40);
            this.ValueTextBoxLabel.Name = "ValueTextBoxLabel";
            this.ValueTextBoxLabel.Size = new System.Drawing.Size(37, 13);
            this.ValueTextBoxLabel.TabIndex = 6;
            this.ValueTextBoxLabel.Text = "Value:";
            // 
            // m_criteriaListBox
            // 
            this.m_criteriaListBox.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.m_criteriaListBox.Location = new System.Drawing.Point(24, 128);
            this.m_criteriaListBox.Name = "m_criteriaListBox";
            this.m_criteriaListBox.Size = new System.Drawing.Size(568, 95);
            this.m_criteriaListBox.TabIndex = 7;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label2.Location = new System.Drawing.Point(24, 104);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(170, 13);
            this.label2.TabIndex = 8;
            this.label2.Text = "Find items that match this criteria:";
            // 
            // m_addCriteriaButton
            // 
            this.m_addCriteriaButton.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.m_addCriteriaButton.Location = new System.Drawing.Point(416, 96);
            this.m_addCriteriaButton.Name = "m_addCriteriaButton";
            this.m_addCriteriaButton.Size = new System.Drawing.Size(75, 23);
            this.m_addCriteriaButton.TabIndex = 9;
            this.m_addCriteriaButton.Text = "Add";
            this.m_addCriteriaButton.Click += new System.EventHandler(this.AddCriteriaButton_Click);
            // 
            // m_removeCriteriaButton
            // 
            this.m_removeCriteriaButton.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.m_removeCriteriaButton.Location = new System.Drawing.Point(512, 96);
            this.m_removeCriteriaButton.Name = "m_removeCriteriaButton";
            this.m_removeCriteriaButton.Size = new System.Drawing.Size(75, 23);
            this.m_removeCriteriaButton.TabIndex = 10;
            this.m_removeCriteriaButton.Text = "Remove";
            this.m_removeCriteriaButton.Click += new System.EventHandler(this.RemoveCriteriaButton_Click);
            // 
            // m_searchResultsListBox
            // 
            this.m_searchResultsListBox.ContextMenuStrip = this.contextMenuStrip1;
            this.m_searchResultsListBox.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.m_searchResultsListBox.Location = new System.Drawing.Point(24, 264);
            this.m_searchResultsListBox.Name = "m_searchResultsListBox";
            this.m_searchResultsListBox.Size = new System.Drawing.Size(568, 95);
            this.m_searchResultsListBox.TabIndex = 11;
            this.m_searchResultsListBox.MouseDoubleClick += new System.Windows.Forms.MouseEventHandler(this.m_searchResultsListBox_MouseDoubleClick);
            // 
            // SearchResultLabel
            // 
            this.SearchResultLabel.AutoSize = true;
            this.SearchResultLabel.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.SearchResultLabel.Location = new System.Drawing.Point(24, 240);
            this.SearchResultLabel.Name = "SearchResultLabel";
            this.SearchResultLabel.Size = new System.Drawing.Size(82, 13);
            this.SearchResultLabel.TabIndex = 12;
            this.SearchResultLabel.Text = "Search Results:";
            // 
            // m_itemsCountLabel
            // 
            this.m_itemsCountLabel.AutoSize = true;
            this.m_itemsCountLabel.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.m_itemsCountLabel.Location = new System.Drawing.Point(24, 368);
            this.m_itemsCountLabel.Name = "m_itemsCountLabel";
            this.m_itemsCountLabel.Size = new System.Drawing.Size(43, 13);
            this.m_itemsCountLabel.TabIndex = 13;
            this.m_itemsCountLabel.Text = "0 Items";
            // 
            // m_findButton
            // 
            this.m_findButton.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.m_findButton.Location = new System.Drawing.Point(616, 64);
            this.m_findButton.Name = "m_findButton";
            this.m_findButton.Size = new System.Drawing.Size(75, 23);
            this.m_findButton.TabIndex = 14;
            this.m_findButton.Text = "Find Now";
            this.m_findButton.Click += new System.EventHandler(this.FindButton_Click);
            // 
            // contextMenuStrip1
            // 
            this.contextMenuStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.m_openFileToolStripMenuItem2});
            this.contextMenuStrip1.Name = "contextMenuStrip1";
            this.contextMenuStrip1.Size = new System.Drawing.Size(120, 26);
            // 
            // m_openFileToolStripMenuItem2
            // 
            this.m_openFileToolStripMenuItem2.Name = "m_openFileToolStripMenuItem2";
            this.m_openFileToolStripMenuItem2.Size = new System.Drawing.Size(152, 22);
            this.m_openFileToolStripMenuItem2.Text = "Open File";
            this.m_openFileToolStripMenuItem2.Click += new System.EventHandler(this.m_openFileToolStripMenuItem2_Click);
            // 
            // FinderForm
            // 
            this.AutoScaleBaseSize = new System.Drawing.Size(5, 13);
            this.ClientSize = new System.Drawing.Size(712, 409);
            this.Controls.Add(this.m_findButton);
            this.Controls.Add(this.m_itemsCountLabel);
            this.Controls.Add(this.SearchResultLabel);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.ValueTextBoxLabel);
            this.Controls.Add(this.m_valueTextBox);
            this.Controls.Add(this.ConditionComboBoxLabel);
            this.Controls.Add(this.PropertyComboBoxLabel);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.m_searchResultsListBox);
            this.Controls.Add(this.m_removeCriteriaButton);
            this.Controls.Add(this.m_addCriteriaButton);
            this.Controls.Add(this.m_criteriaListBox);
            this.Controls.Add(this.m_conditionComboBox);
            this.Controls.Add(this.m_propertyComboBox);
            this.MaximumSize = new System.Drawing.Size(720, 436);
            this.MinimumSize = new System.Drawing.Size(720, 436);
            this.Name = "FinderForm";
            this.Text = "Vault File Finder";
            this.contextMenuStrip1.ResumeLayout(false);
            this.ResumeLayout(false);
            this.PerformLayout();

        }
		#endregion


        private void m_openFileToolStripMenuItem2_Click(object sender, EventArgs e)
        {
            OpenFile();
        }

        private void OpenFile()
        {
            if (m_searchResultsListBox.SelectedItem != null)
            {
                ListBoxFileItem fileItem = (ListBoxFileItem)m_searchResultsListBox.SelectedItem;
                OpenFileCommand.Execute(fileItem.File, m_connection);
            }
        }

        private void m_searchResultsListBox_MouseDoubleClick(object sender, MouseEventArgs e)
        {
            OpenFile();
        }

	}
}
