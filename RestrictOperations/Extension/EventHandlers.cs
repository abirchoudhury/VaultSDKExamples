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

using Autodesk.Connectivity.Extensibility.Framework;
using Autodesk.Connectivity.WebServices;

[assembly:ApiVersion("11.0")]
[assembly:ExtensionId("7D12A22D-F4D9-4436-B2CA-CE7FB710EC71")]

namespace RestrictOperations
{
    public class EventHandlers : IWebServiceExtension
    {

        #region IWebServiceExtension Members

        public void OnLoad()
        {
            // register for events here
            // in this case, we want to register for the GetRestrictions event for all operations

            // File Events
            DocumentService.AddFileEvents.GetRestrictions += new EventHandler<AddFileCommandEventArgs>(AddFileEvents_GetRestrictions);
            DocumentService.CheckinFileEvents.GetRestrictions += new EventHandler<CheckinFileCommandEventArgs>(CheckinFileEvents_GetRestrictions);
            DocumentService.CheckoutFileEvents.GetRestrictions += new EventHandler<CheckoutFileCommandEventArgs>(CheckoutFileEvents_GetRestrictions);
            DocumentService.DeleteFileEvents.GetRestrictions += new EventHandler<DeleteFileCommandEventArgs>(DeleteFileEvents_GetRestrictions);
            DocumentService.DownloadFileEvents.GetRestrictions += new EventHandler<DownloadFileCommandEventArgs>(DownloadFileEvents_GetRestrictions);
            DocumentServiceExtensions.UpdateFileLifecycleStateEvents.GetRestrictions += new EventHandler<UpdateFileLifeCycleStateCommandEventArgs>(UpdateFileLifecycleStateEvents_GetRestrictions);

            // Folder Events
            DocumentService.AddFolderEvents.GetRestrictions += new EventHandler<AddFolderCommandEventArgs>(AddFolderEvents_GetRestrictions);
            DocumentService.DeleteFolderEvents.GetRestrictions += new EventHandler<DeleteFolderCommandEventArgs>(DeleteFolderEvents_GetRestrictions);

            // Item Events
            ItemService.AddItemEvents.GetRestrictions += new EventHandler<AddItemCommandEventArgs>(AddItemEvents_GetRestrictions);
            ItemService.CommitItemEvents.GetRestrictions += new EventHandler<CommitItemCommandEventArgs>(CommitItemEvents_GetRestrictions);
            ItemService.ItemRollbackLifeCycleStatesEvents.GetRestrictions += new EventHandler<ItemRollbackLifeCycleStateCommandEventArgs>(ItemRollbackLifeCycleStatesEvents_GetRestrictions);
            ItemService.DeleteItemEvents.GetRestrictions += new EventHandler<DeleteItemCommandEventArgs>(DeleteItemEvents_GetRestrictions);
            ItemService.EditItemEvents.GetRestrictions += new EventHandler<EditItemCommandEventArgs>(EditItemEvents_GetRestrictions);
            ItemService.PromoteItemEvents.GetRestrictions += new EventHandler<PromoteItemCommandEventArgs>(PromoteItemEvents_GetRestrictions);
            ItemService.UpdateItemLifecycleStateEvents.GetRestrictions += new EventHandler<UpdateItemLifeCycleStateCommandEventArgs>(UpdateItemLifecycleStateEvents_GetRestrictions);

            // Change Order Events
            ChangeOrderService.AddChangeOrderEvents.GetRestrictions += new EventHandler<AddChangeOrderCommandEventArgs>(AddChangeOrderEvents_GetRestrictions);
            ChangeOrderService.CommitChangeOrderEvents.GetRestrictions += new EventHandler<CommitChangeOrderCommandEventArgs>(CommitChangeOrderEvents_GetRestrictions);
            ChangeOrderService.DeleteChangeOrderEvents.GetRestrictions += new EventHandler<DeleteChangeOrderCommandEventArgs>(DeleteChangeOrderEvents_GetRestrictions);
            ChangeOrderService.EditChangeOrderEvents.GetRestrictions += new EventHandler<EditChangeOrderCommandEventArgs>(EditChangeOrderEvents_GetRestrictions);
            ChangeOrderService.UpdateChangeOrderLifecycleStateEvents.GetRestrictions += new EventHandler<UpdateChangeOrderLifeCycleStateCommandEventArgs>(UpdateChangeOrderLifecycleStateEvents_GetRestrictions);

            // Custom Entity Events
            CustomEntityService.UpdateCustomEntityLifecycleStateEvents.GetRestrictions +=new EventHandler<UpdateCustomEntityLifeCycleStateCommandEventArgs>(UpdateCustomEntityLifecycleStateEvents_GetRestrictions);
        }
        #endregion

        /// <summary>
        /// Check the settings file and restrict the operation if needed.
        /// </summary>
        /// <param name="eventArgs"></param>
        private void RestrictOperation(WebServiceCommandEventArgs eventArgs, string eventName)
        {
            RestrictSettings settings = RestrictSettings.Load();
            if (settings.RestrictedOperations.Contains(eventName))
                eventArgs.AddRestriction(new ExtensionRestriction("Unknown", "Test restriction"));
        }


        void UpdateChangeOrderLifecycleStateEvents_GetRestrictions(object sender, UpdateChangeOrderLifeCycleStateCommandEventArgs e)
        {
            RestrictOperation(e, "UpdateChangeOrderLifecycleState");
        }

        void EditChangeOrderEvents_GetRestrictions(object sender, EditChangeOrderCommandEventArgs e)
        {
            RestrictOperation(e, "EditChangeOrder");
        }

        void DeleteChangeOrderEvents_GetRestrictions(object sender, DeleteChangeOrderCommandEventArgs e)
        {
            RestrictOperation(e, "DeleteChangeOrder");
        }

        void CommitChangeOrderEvents_GetRestrictions(object sender, CommitChangeOrderCommandEventArgs e)
        {
            RestrictOperation(e, "CommitChangeOrder");
        }

        void AddChangeOrderEvents_GetRestrictions(object sender, AddChangeOrderCommandEventArgs e)
        {
            RestrictOperation(e, "AddChangeOrder");
        }

        void UpdateItemLifecycleStateEvents_GetRestrictions(object sender, UpdateItemLifeCycleStateCommandEventArgs e)
        {
            RestrictOperation(e, "UpdateItemLifecycleState");
        }

        void PromoteItemEvents_GetRestrictions(object sender, PromoteItemCommandEventArgs e)
        {
            RestrictOperation(e, "PromoteItem");
        }

        void EditItemEvents_GetRestrictions(object sender, EditItemCommandEventArgs e)
        {
            RestrictOperation(e, "EditItem");
        }

        void DeleteItemEvents_GetRestrictions(object sender, DeleteItemCommandEventArgs e)
        {
            RestrictOperation(e, "DeleteItem");
        }

        void ItemRollbackLifeCycleStatesEvents_GetRestrictions(object sender, ItemRollbackLifeCycleStateCommandEventArgs e)
        {
            RestrictOperation(e, "ItemRollbackLifeCycleStates");
        }

        void CommitItemEvents_GetRestrictions(object sender, CommitItemCommandEventArgs e)
        {
            RestrictOperation(e, "CommitItem");
        }

        void AddItemEvents_GetRestrictions(object sender, AddItemCommandEventArgs e)
        {
            RestrictOperation(e, "AddItem");
        }

        void UpdateFileLifecycleStateEvents_GetRestrictions(object sender, UpdateFileLifeCycleStateCommandEventArgs e)
        {
            RestrictOperation(e, "UpdateFileLifecycleState");
        }

        void DownloadFileEvents_GetRestrictions(object sender, DownloadFileCommandEventArgs e)
        {
            RestrictOperation(e, "DownloadFile");
        }

        void DeleteFolderEvents_GetRestrictions(object sender, DeleteFolderCommandEventArgs e)
        {
            RestrictOperation(e, "DeleteFolder");
        }

        void DeleteFileEvents_GetRestrictions(object sender, DeleteFileCommandEventArgs e)
        {
            RestrictOperation(e, "DeleteFile");
        }

        void CheckoutFileEvents_GetRestrictions(object sender, CheckoutFileCommandEventArgs e)
        {
            RestrictOperation(e, "CheckoutFile");
        }

        void CheckinFileEvents_GetRestrictions(object sender, CheckinFileCommandEventArgs e)
        {
            RestrictOperation(e, "CheckinFile");
        }

        void AddFolderEvents_GetRestrictions(object sender, AddFolderCommandEventArgs e)
        {
            RestrictOperation(e, "AddFolder");
        }

        void AddFileEvents_GetRestrictions(object sender, AddFileCommandEventArgs e)
        {
            RestrictOperation(e, "AddFile");
        }

        void UpdateCustomEntityLifecycleStateEvents_GetRestrictions(object sender, UpdateCustomEntityLifeCycleStateCommandEventArgs e)
        {
            RestrictOperation(e, "UpdateCustomEntityLifeCycleState");
        }
    }
}
