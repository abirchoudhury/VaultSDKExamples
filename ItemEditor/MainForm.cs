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
using System.Data;
using System.Linq;
using System.IO;

using Autodesk.Connectivity.WebServices;
using Autodesk.Connectivity.WebServicesTools;
using VDF = Autodesk.DataManagement.Client.Framework;

namespace ItemEditor
{
	/// <summary>
	/// Summary description for Form1.
	/// </summary>
	public class MainForm : System.Windows.Forms.Form
	{
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.Button m_createItemButton;
        private System.Windows.Forms.Button m_changeRevisionButton;
        private System.Windows.Forms.DataGrid m_itemsGrid;
        private System.Windows.Forms.DataGridTableStyle m_tableStyle;

		/// <summary>
		/// Required designer variable.
		/// </summary>
		private System.ComponentModel.Container components = null;

        private DataTable m_dataTable;

        /// <summary>
        /// A mapping between rows in the table and an Item object.
        /// </summary>
        private Dictionary<string, Item> m_tableMap;
        private System.Windows.Forms.Button m_changeLifeCycleButton;

        /// <summary>
        /// The selected Item.
        /// </summary>
        private Item m_selectedItem;
        private System.Windows.Forms.Button m_rollbackButton;
        private System.Windows.Forms.Label m_gridLabel;
        private System.Windows.Forms.Button m_fileLinkButton;
        private Button m_exportButton;
        private Button m_importButton;

        /// <summary>
        /// A mapping between life cycle definition IDs and life cycle definition objects.
        /// </summary>
        private Dictionary<long, LfCycDef> m_lifeCycleMap;

        private VDF.Vault.Currency.Connections.Connection m_connection;

		public MainForm(VDF.Vault.Currency.Connections.Connection connection)
		{
			//
			// Required for Windows Form Designer support
			//
			InitializeComponent();

            m_connection = connection;

            // set up the grid
            DataSet dataSet = new DataSet("itemSet");
            m_dataTable = new DataTable("itemTable");
            m_dataTable.Columns.Add("Item Number");
            m_dataTable.Columns.Add("Revision Number");
            m_dataTable.Columns.Add("Title");
            m_dataTable.Columns.Add("Life Cycle State");
            m_dataTable.Columns.Add("Primary File Link");
   
            dataSet.Tables.Add(m_dataTable);
            m_itemsGrid.SetDataBinding(dataSet, "ItemTable");
            m_tableMap = new Dictionary<string,Item>();
            m_selectedItem = null;

            // get all of the life cycle definitions
            LfCycDef [] definitions = m_connection.WebServiceManager.LifeCycleService.GetAllLifeCycleDefinitions();
            m_lifeCycleMap = new Dictionary<long, LfCycDef>();

            // put the life cycle definitions into a hashtable for easy lookup
            foreach (LfCycDef definition in definitions)
            {
                m_lifeCycleMap[definition.Id] = definition;
            }

			RefreshItemList();
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
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.m_importButton = new System.Windows.Forms.Button();
            this.m_exportButton = new System.Windows.Forms.Button();
            this.m_fileLinkButton = new System.Windows.Forms.Button();
            this.m_rollbackButton = new System.Windows.Forms.Button();
            this.m_changeLifeCycleButton = new System.Windows.Forms.Button();
            this.m_changeRevisionButton = new System.Windows.Forms.Button();
            this.m_createItemButton = new System.Windows.Forms.Button();
            this.m_gridLabel = new System.Windows.Forms.Label();
            this.m_itemsGrid = new System.Windows.Forms.DataGrid();
            this.m_tableStyle = new System.Windows.Forms.DataGridTableStyle();
            this.groupBox1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.m_itemsGrid)).BeginInit();
            this.SuspendLayout();
            // 
            // groupBox1
            // 
            this.groupBox1.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left)));
            this.groupBox1.Controls.Add(this.m_importButton);
            this.groupBox1.Controls.Add(this.m_exportButton);
            this.groupBox1.Controls.Add(this.m_fileLinkButton);
            this.groupBox1.Controls.Add(this.m_rollbackButton);
            this.groupBox1.Controls.Add(this.m_changeLifeCycleButton);
            this.groupBox1.Controls.Add(this.m_changeRevisionButton);
            this.groupBox1.Controls.Add(this.m_createItemButton);
            this.groupBox1.Location = new System.Drawing.Point(8, 8);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(136, 330);
            this.groupBox1.TabIndex = 0;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Commands";
            // 
            // m_importButton
            // 
            this.m_importButton.Location = new System.Drawing.Point(8, 210);
            this.m_importButton.Name = "m_importButton";
            this.m_importButton.Size = new System.Drawing.Size(112, 24);
            this.m_importButton.TabIndex = 7;
            this.m_importButton.Text = "Import from CSV";
            this.m_importButton.Click += new System.EventHandler(this.m_importButton_Click);
            // 
            // m_exportButton
            // 
            this.m_exportButton.Location = new System.Drawing.Point(8, 180);
            this.m_exportButton.Name = "m_exportButton";
            this.m_exportButton.Size = new System.Drawing.Size(112, 24);
            this.m_exportButton.TabIndex = 6;
            this.m_exportButton.Text = "Export to CSV";
            this.m_exportButton.Click += new System.EventHandler(this.m_exportButton_Click);
            // 
            // m_fileLinkButton
            // 
            this.m_fileLinkButton.Location = new System.Drawing.Point(8, 150);
            this.m_fileLinkButton.Name = "m_fileLinkButton";
            this.m_fileLinkButton.Size = new System.Drawing.Size(112, 24);
            this.m_fileLinkButton.TabIndex = 5;
            this.m_fileLinkButton.Text = "Link to File";
            this.m_fileLinkButton.Click += new System.EventHandler(this.m_fileLinkButton_Click);
            // 
            // m_rollbackButton
            // 
            this.m_rollbackButton.Location = new System.Drawing.Point(8, 120);
            this.m_rollbackButton.Name = "m_rollbackButton";
            this.m_rollbackButton.Size = new System.Drawing.Size(112, 24);
            this.m_rollbackButton.TabIndex = 3;
            this.m_rollbackButton.Text = "Rollback Life Cycle";
            this.m_rollbackButton.Click += new System.EventHandler(this.m_rollbackButton_Click);
            // 
            // m_changeLifeCycleButton
            // 
            this.m_changeLifeCycleButton.Location = new System.Drawing.Point(8, 88);
            this.m_changeLifeCycleButton.Name = "m_changeLifeCycleButton";
            this.m_changeLifeCycleButton.Size = new System.Drawing.Size(112, 24);
            this.m_changeLifeCycleButton.TabIndex = 2;
            this.m_changeLifeCycleButton.Text = "Change Life Cycle";
            this.m_changeLifeCycleButton.Click += new System.EventHandler(this.m_changeLifeCycleButton_Click);
            // 
            // m_changeRevisionButton
            // 
            this.m_changeRevisionButton.Location = new System.Drawing.Point(8, 56);
            this.m_changeRevisionButton.Name = "m_changeRevisionButton";
            this.m_changeRevisionButton.Size = new System.Drawing.Size(112, 24);
            this.m_changeRevisionButton.TabIndex = 1;
            this.m_changeRevisionButton.Text = "Change Revision";
            this.m_changeRevisionButton.Click += new System.EventHandler(this.m_changeRevisionButton_Click);
            // 
            // m_createItemButton
            // 
            this.m_createItemButton.Location = new System.Drawing.Point(8, 24);
            this.m_createItemButton.Name = "m_createItemButton";
            this.m_createItemButton.Size = new System.Drawing.Size(112, 24);
            this.m_createItemButton.TabIndex = 0;
            this.m_createItemButton.Text = "Create Item";
            this.m_createItemButton.Click += new System.EventHandler(this.m_createItemButton_Click);
            // 
            // m_gridLabel
            // 
            this.m_gridLabel.Location = new System.Drawing.Point(160, 16);
            this.m_gridLabel.Name = "m_gridLabel";
            this.m_gridLabel.Size = new System.Drawing.Size(264, 16);
            this.m_gridLabel.TabIndex = 2;
            this.m_gridLabel.Text = "Item List";
            // 
            // m_itemsGrid
            // 
            this.m_itemsGrid.AllowSorting = false;
            this.m_itemsGrid.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.m_itemsGrid.CaptionVisible = false;
            this.m_itemsGrid.DataMember = "";
            this.m_itemsGrid.HeaderForeColor = System.Drawing.SystemColors.ControlText;
            this.m_itemsGrid.Location = new System.Drawing.Point(160, 40);
            this.m_itemsGrid.Name = "m_itemsGrid";
            this.m_itemsGrid.ReadOnly = true;
            this.m_itemsGrid.RowHeadersVisible = false;
            this.m_itemsGrid.Size = new System.Drawing.Size(384, 290);
            this.m_itemsGrid.TabIndex = 3;
            this.m_itemsGrid.TableStyles.AddRange(new System.Windows.Forms.DataGridTableStyle[] {
            this.m_tableStyle});
            this.m_itemsGrid.MouseUp += new System.Windows.Forms.MouseEventHandler(this.m_itemsGrid_MouseUp);
            // 
            // m_tableStyle
            // 
            this.m_tableStyle.DataGrid = this.m_itemsGrid;
            this.m_tableStyle.HeaderForeColor = System.Drawing.SystemColors.ControlText;
            this.m_tableStyle.ReadOnly = true;
            // 
            // MainForm
            // 
            this.AutoScaleBaseSize = new System.Drawing.Size(5, 13);
            this.ClientSize = new System.Drawing.Size(560, 347);
            this.Controls.Add(this.m_itemsGrid);
            this.Controls.Add(this.m_gridLabel);
            this.Controls.Add(this.groupBox1);
            this.Name = "MainForm";
            this.Text = "Item Editor";
            this.groupBox1.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.m_itemsGrid)).EndInit();
            this.ResumeLayout(false);

        }
		#endregion

		/// <summary>
		/// The main entry point for the application.
		/// </summary>
		[STAThread]
		static void Main() 
		{
            VDF.Vault.Forms.Settings.LoginSettings settings = new VDF.Vault.Forms.Settings.LoginSettings();
            VDF.Vault.Currency.Connections.Connection connection = VDF.Vault.Forms.Library.Login(settings);

            if (connection == null)
                return;

            MainForm mainForm = new MainForm(connection);
            mainForm.ShowDialog();

            VDF.Vault.Library.ConnectionManager.LogOut(connection);
		}

        private void m_createItemButton_Click(object sender, System.EventArgs e)
        {
            CreateItem();
        }

        

        

        private void RefreshItemList()
        {
            m_tableMap.Clear();
            m_dataTable.Clear();
            m_selectedItem = null;
            m_gridLabel.Text = "Refreshing Grid";
            m_itemsGrid.Refresh();

            ItemService itemSvc = m_connection.WebServiceManager.ItemService;

            string bookmark = null;
            List<Item> items = new List<Item>();
            SrchStatus status = null;
            while(status == null || status.TotalHits > items.Count)
            {
                items.AddRange(itemSvc.FindItemRevisionsBySearchConditions(null, null, true, ref bookmark, out status));
            }
           

            if (items != null && items.Count > 0)
            {
                foreach (Item item in items)
                {
                    LfCycDef lifeCycleDef = m_lifeCycleMap[item.LfCyc.LfCycDefId];
                    LfCycState state = lifeCycleDef.StateArray.FirstOrDefault(lifecycleState => lifecycleState.Id == item.LfCycStateId);

                    DataRow newRow = m_dataTable.NewRow();
                    newRow["Item Number"] = item.ItemNum;
                    newRow["Revision Number"] = item.RevNum;
                    newRow["Title"] = item.Title;
                    newRow["Life Cycle State"] = state.Name;
                    
                    ItemFileAssoc [] associations = itemSvc.GetItemFileAssociationsByItemIds(new long [] {item.Id}, ItemFileLnkTypOpt.Primary);
                    if (associations != null)
                    {
                        foreach (ItemFileAssoc assoc in associations)
                        {
                            newRow["Primary File Link"] = assoc.FileName;
                            break;
                        }
                    }

                    m_tableMap[item.ItemNum] = item;
                    m_dataTable.Rows.Add(newRow);
                }
            }

            
            
            m_itemsGrid.Refresh();
            m_gridLabel.Text = "Item List";

        }

        private void m_changeRevisionButton_Click(object sender, System.EventArgs e)
        {
            ChangeRevision();
        }

        
        private void m_itemsGrid_MouseUp(object sender, System.Windows.Forms.MouseEventArgs e)
        {
            DataGrid.HitTestInfo info = m_itemsGrid.HitTest(e.X, e.Y);
            if (info != null && info.Row >= 0)
            {
                m_itemsGrid.Select(info.Row);

                DataRow row = m_dataTable.Rows[info.Row];
                m_selectedItem = m_tableMap[row["Item Number"].ToString()];
            }
        }

        private void m_changeLifeCycleButton_Click(object sender, System.EventArgs e)
        {
            ChangeLifeCycle();
        }

        

        private void m_rollbackButton_Click(object sender, System.EventArgs e)
        {
            RollbackLifeCycle();
        }

        private void ClearItemLock(long revisionId)
        {
            ItemService itemSvc = m_connection.WebServiceManager.ItemService;

            Item editableItem = itemSvc.EditItems(new long[] { revisionId }).First();
            itemSvc.UndoEditItems(new long [] {editableItem.Id});
        }


        

        private void m_fileLinkButton_Click(object sender, System.EventArgs e)
        {
            LinkToFile();
        }

        


        private void CreateItem()
        {
            ItemService itemSvc = m_connection.WebServiceManager.ItemService;

            CreateItemForm form = new CreateItemForm(m_connection);
            form.ShowDialog();

            if (form.DialogResult != DialogResult.OK)
                return;

            try
            {
                // create the initial object
                Item item = itemSvc.AddItemRevision(form.Category.Id);

                // set the data
                item.Title = form.ItemTitle;

                // commit the item, which finalizes the object
                itemSvc.UpdateAndCommitItems(new Item [] {item});
            }
            catch (Exception e)
            {
                ErrorHandler.HandleError(e);
            }

            // MessageBox.Show("Item Created");
            RefreshItemList();
        }

        private void ChangeRevision()
        {
            if (m_selectedItem == null)
            {
                MessageBox.Show("You must select an Item first");
                return;
            }

            ChangeRevisionForm changeRevisionForm = new ChangeRevisionForm(m_selectedItem, m_connection);
            changeRevisionForm.ShowDialog();

            if (changeRevisionForm.DialogResult != DialogResult.OK)
                return;

            string newNumber = changeRevisionForm.SelectedRevisionNumber;
            
            if (newNumber == null || newNumber.Length == 0)
                return;

            try
            {
                ItemService itemSvc = m_connection.WebServiceManager.ItemService;

                // updates and commits the change in a singe call
                itemSvc.UpdateItemRevisionNumbers( new long [] {m_selectedItem.Id}, 
                    new string [] {newNumber}, "");
            }
            catch (Exception e)
            {
                ErrorHandler.HandleError(e);
            }

            RefreshItemList();
        }

        private void ChangeLifeCycle()
        {
            if (m_selectedItem == null)
            {
                MessageBox.Show("You must select an Item first");
                return;
            }

            ChangeLifeCycleForm changeLifeCycleForm = new ChangeLifeCycleForm(m_selectedItem.LfCyc.LfCycDefId, m_selectedItem.LfCycStateId, m_connection);
            
            changeLifeCycleForm.ShowDialog();
            if (changeLifeCycleForm.DialogResult != DialogResult.OK)
                return;
 
            try
            {
                ItemService itemSvc = m_connection.WebServiceManager.ItemService;

                // updates the life cycle state
                Item[] updatedItems = itemSvc.UpdateItemLifeCycleStates(new long[] { m_selectedItem.MasterId }, new long[] { changeLifeCycleForm.SelectedLifeCycleStateId }, null);
            }
            catch (Exception e)
            {
                ErrorHandler.HandleError(e);
            }

            RefreshItemList();
        }

        private void RollbackLifeCycle()
        {
            if (m_selectedItem == null)
            {
                MessageBox.Show("You must select an Item first");
                return;
            }

            try
            {
                ItemService itemSvc = m_connection.WebServiceManager.ItemService;

                // begin the rollback
                Item targetItem = itemSvc.GetLifecycleRollbackTargetItem(m_selectedItem.MasterId);
                itemSvc.ItemRollbackLifeCycleState(targetItem.Id);
            }
            catch (Exception e)
            {
                ErrorHandler.HandleError(e);
            }

            MessageBox.Show("Rollback completed");

            RefreshItemList();
        }

        private void LinkToFile()
        {
            if (m_selectedItem == null)
            {
                MessageBox.Show("You must select an Item first");
                return;
            }

            try
            {
                VDF.Vault.Forms.Settings.SelectEntitySettings settings = new VDF.Vault.Forms.Settings.SelectEntitySettings();
                settings.MultipleSelect = false;
                settings.ActionableEntityClassIds.Add(VDF.Vault.Currency.Entities.EntityClassIds.Files);
                settings.ConfigureActionButtons("Select", null, null, false);
                VDF.Vault.Forms.Results.SelectEntityResults results = VDF.Vault.Forms.Library.SelectEntity(m_connection, settings);

                if (results == null || results.SelectedEntities == null || !results.SelectedEntities.Any())
                    return;

                VDF.Vault.Currency.Entities.FileIteration file = results.SelectedEntities.First() as VDF.Vault.Currency.Entities.FileIteration;
                if (file == null)
                {
                    MessageBox.Show("You must select a file");
                    return;
                }

                ItemService itemSvc = m_connection.WebServiceManager.ItemService;

                // first assign the file to a new item
                itemSvc.AddFilesToPromote(new long[] { file.EntityIterationId }, ItemAssignAll.Default, true);
                DateTime timestamp;
                GetPromoteOrderResults promoteOrderResults = itemSvc.GetPromoteComponentOrder(out timestamp);
                //long[] componentIds = promoteOrderResults.
                if(promoteOrderResults.PrimaryArray != null && promoteOrderResults.PrimaryArray.Any())
                    itemSvc.PromoteComponents(timestamp, promoteOrderResults.PrimaryArray);
                if (promoteOrderResults.NonPrimaryArray != null && promoteOrderResults.NonPrimaryArray.Any())
                    itemSvc.PromoteComponentLinks(promoteOrderResults.NonPrimaryArray);
                ItemsAndFiles promoteResult = itemSvc.GetPromoteComponentsResults(timestamp); 
 
                // find out which item corresponds to the file
                long itemId = -1;
                foreach (ItemFileAssoc assoc in promoteResult.FileAssocArray)
                {
                    if (assoc.CldFileId == file.EntityIterationId)
                    {
                        itemId = assoc.ParItemId;
                    }
                }

                if (itemId < 0)
                {
                    MessageBox.Show("Promote error");
                }
                else
                {
                    // next reassign the file from the new item to the existing item
                    Item[] updatedItems = itemSvc.ReassignComponentsToDifferentItems(
                        new long[] { itemId },
                        new long[] { m_selectedItem.Id });

                    // commit the changes
                    itemSvc.UpdateAndCommitItems(updatedItems);
                }

                // clear out the items from the initial Promote
                long[] itemIds = new long[promoteResult.ItemRevArray.Length];
                long[] itemMasterIds = new long[promoteResult.ItemRevArray.Length];

                for (int i = 0; i < promoteResult.ItemRevArray.Length; i++)
                {
                    itemIds[i] = promoteResult.ItemRevArray[i].Id;
                    itemMasterIds[i] = promoteResult.ItemRevArray[i].MasterId;
                }

                itemSvc.DeleteUnusedItemNumbers(itemMasterIds);
                itemSvc.UndoEditItems(itemIds);
            }
            catch (Exception e)
            {
                ErrorHandler.HandleError(e);
            }

            RefreshItemList();
        }

        private void m_exportButton_Click(object sender, EventArgs e)
        {
            Export();
        }

        private void Export()
        {
            if (m_selectedItem == null)
            {
                MessageBox.Show("You must select an Item first");
                return;
            }

            SaveFileDialog dialog = new SaveFileDialog();
            dialog.Filter = "CSV Files|*.csv";
            DialogResult result = dialog.ShowDialog();
            if (result != DialogResult.OK)
                return;

            string filename = dialog.FileName;

            PackageService packageSvc = m_connection.WebServiceManager.PackageService;

            // export to CSV file
            PkgItemsAndBOM pkgBom = packageSvc.GetLatestPackageDataByItemIds(new long[] { m_selectedItem.Id }, BOMTyp.Latest);

            // Create a mapping between Item properties and columns in the CSV file
            MapPair levelPair = new MapPair();
            levelPair.ToName = "Level";
            levelPair.FromName = "BOMIndicator-41FF056B-8EEF-47E2-8F9E-490BC0C52C71";

            MapPair numberPair = new MapPair();
            numberPair.ToName = "Number";
            numberPair.FromName = "Number";

            MapPair typePair = new MapPair();
            typePair.ToName = "Type";
            typePair.FromName = "ItemClass";

            // NOTE: not everything is being exported in this example.  So if this file is imported,
            // omitted data, such as lifecycle state, may not be the same.

            FileNameAndURL fileNameAndUrl = packageSvc.ExportToPackage(pkgBom, FileFormat.CSV_LEVEL,
                    new MapPair[] { levelPair, numberPair, typePair });

            long currentByte = 0;
            long partSize = m_connection.PartSizeInBytes;
            using (FileStream fs = new FileStream(filename, FileMode.Create))
            {
                while (currentByte < fileNameAndUrl.FileSize)
                {
                    long lastByte = currentByte + partSize < fileNameAndUrl.FileSize ? currentByte + partSize : fileNameAndUrl.FileSize;
                    byte[] contents = packageSvc.DownloadPackagePart(fileNameAndUrl.Name, currentByte, lastByte);
                    fs.Write(contents, 0, (int)(lastByte - currentByte));
                    currentByte += partSize;
                }
            }

            MessageBox.Show("Export completed");
        }

        private void m_importButton_Click(object sender, EventArgs e)
        {
            Import();
        }


        private void Import()
        {
            OpenFileDialog dialog = new OpenFileDialog();
            dialog.Filter = "CSV Files|*.csv";
            DialogResult result = dialog.ShowDialog();
            if (result != DialogResult.OK)
                return;

            string filename = dialog.FileName;

            PackageService packageSvc = m_connection.WebServiceManager.PackageService;

            // Create a mapping between the CVS columns and the item properties.
            // NOTE: It is assumed the we are importing a file created by the
            // export command in this sample.  Otherwise we can't assume the
            // columns being used.
            Map info = new Map();

            //map the first column to level
            MapPair levelPair = new MapPair();
            levelPair.FromName = "Level";
            levelPair.ToName = "BOMIndicator-41FF056B-8EEF-47E2-8F9E-490BC0C52C71";

            //map the second column to number
            MapPair numberPair = new MapPair();
            numberPair.FromName = "Number";
            numberPair.ToName = "Number";

            //map the third column to type
            MapPair typePair = new MapPair();
            typePair.FromName = "Type";
            typePair.ToName = "ItemClass";

            info.PairArray = new MapPair[] { levelPair, numberPair, typePair };

            FileNameAndURL fileNameandURL = null;
            using(FileStream fs = new FileStream(filename, FileMode.Open, FileAccess.Read))
            {
                long partSize = m_connection.PartSizeInBytes;
                byte[] contents = new byte[partSize];
                int bytesRead;
                while ((bytesRead = fs.Read(contents, 0, (int)partSize)) > 0)
                {
                    string existingPackageName = null;
                    if (fileNameandURL != null)
                        existingPackageName = fileNameandURL.Name;
                    fileNameandURL = packageSvc.UploadPackagePart(existingPackageName, ".csv", bytesRead == partSize ? contents : contents.Take(bytesRead).ToArray());
                }
            }

            PkgItemsAndBOM createResult = packageSvc.ImportFromPackage(FileFormat.CSV_LEVEL, info, fileNameandURL.Name);

            foreach (PkgItem item in createResult.PkgItemArray)
            {
                // to keep things simple, this sample only handles cases where we are 
                // importing new items.  It does not handle complex cases like when an item needs updating.
                if (item.Resolution != null && item.Resolution.ResolutionMethod != ResolutionMethod.Create)
                {
                    MessageBox.Show("There are conflicts in the import.  This function can only be used for creating new Items.");
                    return;
                }
            }

            packageSvc.CommitImportedData(createResult);

            MessageBox.Show("Import completed");
            RefreshItemList();
        }
	}
}






