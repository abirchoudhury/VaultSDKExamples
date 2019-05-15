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
using System.Reflection;

using Autodesk.Connectivity.Extensibility.Framework;
using Autodesk.Connectivity.Explorer.Extensibility;
using Autodesk.Connectivity.WebServices;
using Autodesk.Connectivity.WebServicesTools;

[assembly: AssemblyCompany("Autodesk")]
[assembly: AssemblyProduct("JobProcessorApiSamples")]
[assembly: AssemblyDescription("A sample that queues and handles a job")]
[assembly: ApiVersion("11.0")]
[assembly: ExtensionId("50A18DD5-892A-477e-911E-1D6E3C647036")]

namespace JobProcessorApiSamples
{
    public class QueuePublishJobCommandExtension : IExplorerExtension
    {

        public void QueuePublishJobCommandHandler(object s, CommandItemEventArgs e)
        {
            // Queue a ShareDwf job
            //
            const string PublishJobTypeName = "MyCompany.File.Publish";
            const string PublishJob_FileMasterId = "FileMasterId";
            const string PublishJob_FileName     = "FileName";

            foreach (ISelection vaultObj in e.Context.CurrentSelectionSet)
            {
                JobParam[] paramList = new JobParam[2];
                JobParam masterIdParam = new JobParam();
                masterIdParam.Name = PublishJob_FileMasterId;
                masterIdParam.Val = vaultObj.Id.ToString();
                paramList[0] = masterIdParam;

                JobParam fileNameParam = new JobParam();
                fileNameParam.Name = PublishJob_FileName;
                fileNameParam.Val = vaultObj.Label;
                paramList[1] = fileNameParam;

                // Add the job to the queue
                //
                e.Context.Application.Connection.WebServiceManager.JobService.AddJob(
                    PublishJobTypeName, String.Format("Publish File - {0}", fileNameParam.Val),
                    paramList, 10);
            }
        }

        public string ResourceCollectionName()
        {
            return String.Empty;
        }

        public IEnumerable<CommandSite> CommandSites()
        {
            List<CommandSite> sites = new List<CommandSite>();

            // Describe user history command item
            //
            CommandItem queuePublishJobCmdItem = new CommandItem("Command.QueuePublishJob", "Queue Publish Job");
            queuePublishJobCmdItem.NavigationTypes = new SelectionTypeId[] { SelectionTypeId.File };
            queuePublishJobCmdItem.MultiSelectEnabled = false;
            queuePublishJobCmdItem.Execute += QueuePublishJobCommandHandler;

            // deploy user history command on file context menu
            //
            CommandSite queuePublishContextMenu = new CommandSite("Menu.FileContextMenu", "Queue Publish Job");
            queuePublishContextMenu.Location = CommandSiteLocation.FileContextMenu;
            queuePublishContextMenu.DeployAsPulldownMenu = false;
            queuePublishContextMenu.AddCommand(queuePublishJobCmdItem);
            sites.Add(queuePublishContextMenu);
            
            return sites;
        }


        public IEnumerable<DetailPaneTab> DetailTabs()
        {
            return null;
        }
       
        public void OnLogOn(IApplication application)
        {
            // NoOp;
        }

        public void OnLogOff(IApplication application)
        {
            // NoOp;
        }

        public void OnStartup(IApplication application)
        {
            // NoOp;
        }

        public void OnShutdown(IApplication application)
        {
            // NoOp;
        }


        public IEnumerable<string> HiddenCommands()
        {
            return null;
        }


        public IEnumerable<CustomEntityHandler> CustomEntityHandlers()
        {
            return null;
        }
    }
}
