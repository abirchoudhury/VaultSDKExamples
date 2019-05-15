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
using System.IO;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Formatters.Binary;
using System.Reflection;
using System.Threading;

using Autodesk.Connectivity.WebServices;
using System.Threading.Tasks;
using Autodesk.Connectivity.WebServicesTools;

namespace VaultMirror
{
	/// <summary>
	/// Summary description for Form1.
	/// </summary>
	sealed class MainForm : System.Windows.Forms.Form, ICommandReporter
    {
        private System.Windows.Forms.TextBox m_usernameTextBox;
        private System.Windows.Forms.TextBox m_passwordTextBox;
        private System.Windows.Forms.TextBox m_serverTextBox;
        private System.Windows.Forms.TextBox m_vaultTextBox;
        private System.Windows.Forms.Button m_fullMirrorButton;
        private System.Windows.Forms.Button m_partialMirrorButton;
        private System.Windows.Forms.Button m_cancelButton;
        private IContainer components;

        private DateTime m_lastSyncTime;
        private DateTime m_prevSyncTime;
        private System.Windows.Forms.StatusBar m_statusBar;
        private System.Windows.Forms.TextBox m_mirrorFolderTextBox;

        private enum Commands
        {
            UNKNOWN,
            FULL_MIRROR,
            PARTIAL_MIRROR
        }

        private static bool m_silentMode = false;
        private static StreamWriter m_logFileStream = null;
        private System.Windows.Forms.Timer m_statusBarTimer;

        private string m_nextStatusBarMessage = String.Empty;

        private CancellationTokenSource m_cts;

		public MainForm()
		{
			//
			// Required for Windows Form Designer support
			//
			InitializeComponent();

			LoadSettings();
            SetEnabled(true);
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
            this.components = new System.ComponentModel.Container();
            System.Windows.Forms.GroupBox groupBox1;
            System.Windows.Forms.Label label5;
            System.Windows.Forms.Label label4;
            System.Windows.Forms.Label label3;
            System.Windows.Forms.Label label2;
            System.Windows.Forms.Label label1;
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(MainForm));
            this.m_mirrorFolderTextBox = new System.Windows.Forms.TextBox();
            this.m_vaultTextBox = new System.Windows.Forms.TextBox();
            this.m_serverTextBox = new System.Windows.Forms.TextBox();
            this.m_passwordTextBox = new System.Windows.Forms.TextBox();
            this.m_usernameTextBox = new System.Windows.Forms.TextBox();
            this.m_fullMirrorButton = new System.Windows.Forms.Button();
            this.m_partialMirrorButton = new System.Windows.Forms.Button();
            this.m_cancelButton = new System.Windows.Forms.Button();
            this.m_statusBar = new System.Windows.Forms.StatusBar();
            this.m_statusBarTimer = new System.Windows.Forms.Timer(this.components);
            groupBox1 = new System.Windows.Forms.GroupBox();
            label5 = new System.Windows.Forms.Label();
            label4 = new System.Windows.Forms.Label();
            label3 = new System.Windows.Forms.Label();
            label2 = new System.Windows.Forms.Label();
            label1 = new System.Windows.Forms.Label();
            groupBox1.SuspendLayout();
            this.SuspendLayout();
            // 
            // groupBox1
            // 
            groupBox1.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            groupBox1.Controls.Add(this.m_mirrorFolderTextBox);
            groupBox1.Controls.Add(this.m_vaultTextBox);
            groupBox1.Controls.Add(this.m_serverTextBox);
            groupBox1.Controls.Add(this.m_passwordTextBox);
            groupBox1.Controls.Add(this.m_usernameTextBox);
            groupBox1.Controls.Add(label5);
            groupBox1.Controls.Add(label4);
            groupBox1.Controls.Add(label3);
            groupBox1.Controls.Add(label2);
            groupBox1.Controls.Add(label1);
            groupBox1.Location = new System.Drawing.Point(8, 8);
            groupBox1.Name = "groupBox1";
            groupBox1.Size = new System.Drawing.Size(392, 144);
            groupBox1.TabIndex = 0;
            groupBox1.TabStop = false;
            groupBox1.Text = "Settings";
            // 
            // m_mirrorFolderTextBox
            // 
            this.m_mirrorFolderTextBox.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.m_mirrorFolderTextBox.Location = new System.Drawing.Point(136, 112);
            this.m_mirrorFolderTextBox.Name = "m_mirrorFolderTextBox";
            this.m_mirrorFolderTextBox.Size = new System.Drawing.Size(248, 20);
            this.m_mirrorFolderTextBox.TabIndex = 12;
            // 
            // m_vaultTextBox
            // 
            this.m_vaultTextBox.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.m_vaultTextBox.Location = new System.Drawing.Point(136, 88);
            this.m_vaultTextBox.Name = "m_vaultTextBox";
            this.m_vaultTextBox.Size = new System.Drawing.Size(248, 20);
            this.m_vaultTextBox.TabIndex = 11;
            // 
            // m_serverTextBox
            // 
            this.m_serverTextBox.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.m_serverTextBox.Location = new System.Drawing.Point(136, 64);
            this.m_serverTextBox.Name = "m_serverTextBox";
            this.m_serverTextBox.Size = new System.Drawing.Size(248, 20);
            this.m_serverTextBox.TabIndex = 10;
            // 
            // m_passwordTextBox
            // 
            this.m_passwordTextBox.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.m_passwordTextBox.Location = new System.Drawing.Point(136, 40);
            this.m_passwordTextBox.Name = "m_passwordTextBox";
            this.m_passwordTextBox.PasswordChar = '*';
            this.m_passwordTextBox.Size = new System.Drawing.Size(248, 20);
            this.m_passwordTextBox.TabIndex = 9;
            // 
            // m_usernameTextBox
            // 
            this.m_usernameTextBox.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.m_usernameTextBox.Location = new System.Drawing.Point(136, 16);
            this.m_usernameTextBox.Name = "m_usernameTextBox";
            this.m_usernameTextBox.Size = new System.Drawing.Size(248, 20);
            this.m_usernameTextBox.TabIndex = 8;
            // 
            // label5
            // 
            label5.Location = new System.Drawing.Point(8, 120);
            label5.Name = "label5";
            label5.Size = new System.Drawing.Size(112, 16);
            label5.TabIndex = 7;
            label5.Text = "Mirror Folder:";
            // 
            // label4
            // 
            label4.Location = new System.Drawing.Point(8, 96);
            label4.Name = "label4";
            label4.Size = new System.Drawing.Size(112, 16);
            label4.TabIndex = 6;
            label4.Text = "Vault:";
            // 
            // label3
            // 
            label3.Location = new System.Drawing.Point(8, 72);
            label3.Name = "label3";
            label3.Size = new System.Drawing.Size(112, 16);
            label3.TabIndex = 5;
            label3.Text = "Server:";
            // 
            // label2
            // 
            label2.Location = new System.Drawing.Point(8, 48);
            label2.Name = "label2";
            label2.Size = new System.Drawing.Size(112, 16);
            label2.TabIndex = 4;
            label2.Text = "Password:";
            // 
            // label1
            // 
            label1.Location = new System.Drawing.Point(8, 24);
            label1.Name = "label1";
            label1.Size = new System.Drawing.Size(112, 16);
            label1.TabIndex = 3;
            label1.Text = "Username:";
            // 
            // m_fullMirrorButton
            // 
            this.m_fullMirrorButton.Location = new System.Drawing.Point(8, 160);
            this.m_fullMirrorButton.Name = "m_fullMirrorButton";
            this.m_fullMirrorButton.Size = new System.Drawing.Size(120, 24);
            this.m_fullMirrorButton.TabIndex = 1;
            this.m_fullMirrorButton.Text = "Run Full Mirror";
            this.m_fullMirrorButton.Click += new System.EventHandler(this.m_fullMirrorButton_Click);
            // 
            // m_partialMirrorButton
            // 
            this.m_partialMirrorButton.Location = new System.Drawing.Point(144, 160);
            this.m_partialMirrorButton.Name = "m_partialMirrorButton";
            this.m_partialMirrorButton.Size = new System.Drawing.Size(120, 24);
            this.m_partialMirrorButton.TabIndex = 2;
            this.m_partialMirrorButton.Text = "Run Partial Mirror";
            this.m_partialMirrorButton.Click += new System.EventHandler(this.m_partialMirrorButton_Click);
            // 
            // m_cancelButton
            // 
            this.m_cancelButton.Location = new System.Drawing.Point(280, 160);
            this.m_cancelButton.Name = "m_cancelButton";
            this.m_cancelButton.Size = new System.Drawing.Size(120, 24);
            this.m_cancelButton.TabIndex = 3;
            this.m_cancelButton.Text = "Cancel";
            this.m_cancelButton.Click += new System.EventHandler(this.m_cancelButton_Click);
            // 
            // m_statusBar
            // 
            this.m_statusBar.Location = new System.Drawing.Point(0, 196);
            this.m_statusBar.Name = "m_statusBar";
            this.m_statusBar.Size = new System.Drawing.Size(408, 16);
            this.m_statusBar.TabIndex = 4;
            // 
            // m_statusBarTimer
            // 
            this.m_statusBarTimer.Enabled = true;
            this.m_statusBarTimer.Tick += new System.EventHandler(this.m_statusBarTimer_Tick);
            // 
            // MainForm
            // 
            this.AutoScaleBaseSize = new System.Drawing.Size(5, 13);
            this.ClientSize = new System.Drawing.Size(408, 212);
            this.Controls.Add(this.m_statusBar);
            this.Controls.Add(this.m_cancelButton);
            this.Controls.Add(this.m_partialMirrorButton);
            this.Controls.Add(this.m_fullMirrorButton);
            this.Controls.Add(groupBox1);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MaximumSize = new System.Drawing.Size(1416, 275);
            this.MinimumSize = new System.Drawing.Size(416, 250);
            this.Name = "MainForm";
            this.Text = "Vault Mirror 2017";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.MainForm_FormClosing);
            this.FormClosed += new System.Windows.Forms.FormClosedEventHandler(this.MainForm_FormClosed);
            groupBox1.ResumeLayout(false);
            groupBox1.PerformLayout();
            this.ResumeLayout(false);

        }
		#endregion

		/// <summary>
		/// The main entry point for the application.
		/// </summary>
		[STAThread]
		static void Main() 
		{
            // make sure that there is only 1 instance running
            // use a named mutex to lock out other instances.
            bool lockAcquired = false;
            string myName = Assembly.GetEntryAssembly().GetName().Name;
            using ( Mutex appLock = new Mutex(true, myName, out lockAcquired) )
            {
                if ( !lockAcquired )
                {
                    Environment.Exit(0);
                }

                WebServiceManager.Initialize();
                MainForm mainForm = new MainForm();
            
                string [] args = Environment.GetCommandLineArgs();

                // parse the command line arguments
                Commands command = Commands.UNKNOWN;
                string user = null;
                string pass = null;
                string server = null;
                string vault = null;
                string logfile = null;
                string mirrorFolder = null;
                bool useWorkingFolder = false;
                bool failOnError = true;

                if (args != null && args.Length > 1)
                {
                    m_silentMode = true;

                    for (int i=1; i<args.Length; i++)
                    {
                        if (String.Compare(args[i], "-PartialMirror", true) == 0)
                        {
                            if (command == Commands.UNKNOWN)
                                command = Commands.PARTIAL_MIRROR;
                            else if (command == Commands.FULL_MIRROR)
                                PrintErrorAndExit("Cannot use both -FullMirror and -PartialMirror");
                        }
                        else if (String.Compare(args[i], "-FullMirror", true) == 0)
                        {
                            if (command == Commands.UNKNOWN)
                                command = Commands.FULL_MIRROR;
                            else if (command == Commands.PARTIAL_MIRROR)
                                PrintErrorAndExit("Cannot use both -FullMirror and -PartialMirror");
                        }
                        else if (String.Compare(args[i], "-U", true) == 0)
                        {
                            if (args.Length <= i + 1)
                                PrintErrorAndExit("no username specified");

                            user = args[++i];
                        }
                        else if (String.Compare(args[i], "-P", true) == 0)
                        {
                            if (args.Length <= i + 1)
                                PrintErrorAndExit("no password specified");

                            pass = args[++i];
                        }
                        else if (String.Compare(args[i], "-S", true) == 0)
                        {
                            if (args.Length <= i + 1)
                                PrintErrorAndExit("no server specified");

                            server = args[++i];
                        }
                        else if (String.Compare(args[i], "-V", true) == 0)
                        {
                            if (args.Length <= i + 1)
                                PrintErrorAndExit("no vault specified");

                            vault = args[++i];
                        }
                        else if (String.Compare(args[i], "-L", true) == 0)
                        {
                            if (args.Length <= i + 1)
                                PrintErrorAndExit("no logfile specified");

                            logfile = args[++i];
                        }
                        else if (String.Compare(args[i], "-F", true) == 0)
                        {
                            if (args.Length <= i + 1)
                                PrintErrorAndExit("no mirror folder specified");

                            mirrorFolder = args[++i];
                        }
                        else if (String.Compare(args[i], "-WF", true) == 0)
                        {
                            useWorkingFolder = true;
                        }
                        else if (String.Compare(args[i], "-noFail", true) == 0)
                        {
                            failOnError = false;
                        }
                        else if (String.Compare(args[i], "-?", true) == 0)
                        {
                            PrintUsageAndExit();
                        }
                        else
                        {
                            PrintUsageAndExit();
                        }
                    }

                    try
                    {
                        if (logfile != null)
                        {
                            m_logFileStream = new StreamWriter(logfile, true);
                            m_logFileStream.WriteLine("----------------------");
                            m_logFileStream.WriteLine(DateTime.Now.ToString());
                        }
                    }
                    catch (Exception e)
                    {
                        PrintErrorAndExit(e.Message);
                    }
                        

                    if (command == Commands.UNKNOWN)
                        PrintErrorAndExit("Must use either -FullMirror and -PartialMirror");

                    Settings settings = Settings.LoadSettings();

                    
                    if (server == null)
                    {
                        // the server was not specified on the command line

                        if (vault != null)
                            PrintErrorAndExit("The -V parameter must be used with -S");

                        // if there is only one login stored, use it
                        Login login = settings.GetOnlyLogin();

                        if (login == null)
                            PrintErrorAndExit("Use -S and -V to specify the server and vault name respectively.");

                        server = login.Server;
                        vault = login.Vault;

                        if (user == null)
                            user = login.Username;
                        if (pass == null)
                            pass = login.Password;
                        if (mirrorFolder == null)
                            mirrorFolder = login.MirrorFolder;
                    }
                    else
                    {
                        if (vault == null)
                            PrintErrorAndExit("The -S parameter must be used with -V");

                        if (user == null)
                        {
                            // username was not specified on command line, see if its stored
                            Login login = settings.GetLogin(server, vault);

                            if (login == null)
                                PrintErrorAndExit("Use -U and -P to specify the username and password respectively.");

                            user = login.Username;
                            pass = login.Password;

                            if (mirrorFolder == null)
                                mirrorFolder = login.MirrorFolder;
                        }

                        // if no password is specified, assume a blank password
                        if (pass == null)
                            pass = "";
                    }

                    if (mirrorFolder == null && !useWorkingFolder)
                        PrintErrorAndExit("Use -F to specify the mirror (output) folder or -WF to use your working folder.");

                    try
                    {
                        DateTime now = DateTime.Now;
                        if (command == Commands.PARTIAL_MIRROR)
                        {
                            DateTime lastSyncTime = DateTime.MinValue;

                            Login login = settings.GetLogin(server, vault);
                            if (login != null)
                                lastSyncTime = login.LastSyncTime;

                            PartialMirrorCommand cmd = new PartialMirrorCommand(new SilentCommandReporter(),
                                user, pass, server,
                                vault, mirrorFolder, lastSyncTime,
                                useWorkingFolder, failOnError, CancellationToken.None);
                            cmd.Execute();
                            Print("Partial Mirror complete", "Success");
                        }
                        else if (command == Commands.FULL_MIRROR)
                        {                            
                            FullMirrorCommand cmd = new FullMirrorCommand(new SilentCommandReporter(),
                                user, pass, server,
                                vault, mirrorFolder, 
                                useWorkingFolder, failOnError, CancellationToken.None);
                            cmd.Execute();
                            Print("Full Mirror complete", "Success");
                        }
                        else
                        {
                            // we should never hit this code
                            Print("Error:  Invalid command", "Error", true /* forceShowMessageBox */);
                        }

                        // save the login information, including the updated sync time
                        settings.AddLogin(new Login(user, pass, server, vault, mirrorFolder, now));
                        settings.SaveSettings();
                    }
                    catch (Exception e)
                    {
                        Print(e.ToString(), "Error", true /* forceShowMessageBox */);
                    }
                }
                else
                {
                    Application.Run(new MainForm());
                }

                Cleanup();
            }
		}

        private static void Cleanup()
        {
            if (m_logFileStream != null)
                m_logFileStream.Close();
        }

        private static void PrintUsageAndExit()
        {
            Print("Usage:\nVaultMirror \n\t[-PartialMirror | -FullMirror]\n\t[-U user]\n\t[-P pass]\n\t[-S server]\n\t[-V vault]\n\t[-L logfile]\n\t[-F mirrorFolder]\n\t[-WF]\n\t[-noFail]", "Vault Mirror Usage", true /* forceShowMessageBox */);
            Cleanup();
            Environment.Exit(0);
        }
        
        private static void PrintErrorAndExit(string msg)
        {
            Print("Error: " + msg, "Error", true /* forceShowMessageBox */);
            Cleanup();
            Environment.Exit(0);
        }

        private void LoadSettings()
        {
            Settings settings = Settings.LoadSettings();
            Login login = settings.GetOnlyLogin();

            if (login != null)
            {
                m_usernameTextBox.Text = login.Username;
                m_passwordTextBox.Text = login.Password;
                m_serverTextBox.Text = login.Server;
                m_vaultTextBox.Text = login.Vault;
                m_mirrorFolderTextBox.Text = login.MirrorFolder;
                m_lastSyncTime = login.LastSyncTime;
            }
        }

        private void SaveSettings()
        {
            Settings settings = Settings.LoadSettings();
            settings.AddLogin(new Login(m_usernameTextBox.Text, m_passwordTextBox.Text,
                m_serverTextBox.Text, m_vaultTextBox.Text, m_mirrorFolderTextBox.Text,
                m_lastSyncTime));
            settings.SaveSettings();
        }



        private void MainForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (IsMirrorCommandInProgress())
            {
                e.Cancel = true;
                MessageBox.Show("The application cannot be closed while an operation is in progress.");
                return;
            }
        }
        
        private void MainForm_FormClosed(object sender, FormClosedEventArgs e)
        {
            try
            {
                SaveSettings();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error: " + ex.Message);
            }
        }

        private void PreBeginMirrorCommand()
        {
            SetEnabled(false);
            SaveSettings();

            m_prevSyncTime = m_lastSyncTime;
            m_lastSyncTime = DateTime.Now;
            CreateNewCancellationTokenSourceAndCancelOld();
        }

        private void m_fullMirrorButton_Click(object sender, System.EventArgs e)
        {
            FullMirror();
        }

        private Task FullMirror()
        {
            string folder = m_mirrorFolderTextBox.Text.Trim();
            Func<Task> f = () => FullMirrorAsync(this, m_usernameTextBox.Text, m_passwordTextBox.Text, m_serverTextBox.Text, m_vaultTextBox.Text, folder, m_cts.Token);
            return MirrorCommandHelper(f);
        }

        private static Task FullMirrorAsync(ICommandReporter commandReporter, string username, string password, string servername, string vaultname, string folder, CancellationToken ct)
        {
            return Task.Run(() => FullMirrorWorkerAsync_(commandReporter, username, password, servername, vaultname, folder, ct), ct);
        }

        private static void FullMirrorWorkerAsync_(ICommandReporter commandReporter, string username, string password, string servername, string vaultname, string folder, CancellationToken ct)
        {
            bool useWorkingFolder = string.IsNullOrEmpty(folder);
            bool failOnError = true;
            FullMirrorCommand cmd = new FullMirrorCommand(commandReporter, username, password, servername, vaultname, folder, useWorkingFolder, failOnError, ct);
            cmd.Execute();
        }

        private void m_partialMirrorButton_Click(object sender, System.EventArgs e)
        {
            PartialMirror();
        }

        private Task PartialMirror()
        {
            string folder = m_mirrorFolderTextBox.Text.Trim();
            Func<Task> f = () => PartialMirrorAsync(this, m_usernameTextBox.Text, m_passwordTextBox.Text, m_serverTextBox.Text, m_vaultTextBox.Text, folder, m_prevSyncTime, m_cts.Token);
            return MirrorCommandHelper(f);
        }

        private static Task PartialMirrorAsync(ICommandReporter commandReporter, string username, string password, string servername, string vaultname, string folder, DateTime lastSyncTime, CancellationToken ct)
        {
            return Task.Run(() => PartialMirrorWorkerAsync_(commandReporter, username, password, servername, vaultname, folder, lastSyncTime, ct), ct);
        }

        private static void PartialMirrorWorkerAsync_(ICommandReporter commandReporter, string username, string password, string servername, string vaultname, string folder, DateTime lastSyncTime, CancellationToken ct)
        {
            bool useWorkingFolder = string.IsNullOrEmpty(folder);
            bool failOnError = true;
            PartialMirrorCommand cmd = new PartialMirrorCommand(commandReporter, username, password, servername, vaultname, folder, lastSyncTime, useWorkingFolder, failOnError, ct);
            cmd.Execute();
        }
        
        private async Task MirrorCommandHelper(Func<Task> f)
        {
            PreBeginMirrorCommand();

            Task t = f();
            string resultMessage = "Operation complete";

            try
            {
                await t;
            }
            catch (Exception ex)
            {
                UncompletedOperation();
                resultMessage = ex is OperationCanceledException ? "Operation cancelled" : ex.ToString();
            }
            finally
            {
                SetEnabled(true);
                SetStatusMessageImmediate("");
                MessageBox.Show(resultMessage);
            }
        }

        public void SetNextStatusMessage(string message)
        {
            m_nextStatusBarMessage = message;
        }

        public void SetStatusMessageImmediate(string message)
        {
            SetNextStatusMessage(message);
            UpdateStatusBarText();
        }

        private void UpdateStatusBarText()
        {
            m_statusBar.Text = m_nextStatusBarMessage;
        }

        private void m_statusBarTimer_Tick(object sender, EventArgs e)
        {
            UpdateStatusBarText();   
        }

        public void UncompletedOperation()
        {
            m_lastSyncTime = m_prevSyncTime;
        }

        private void SetEnabled(bool enabled)
        {
            m_fullMirrorButton.Enabled = enabled;
            m_partialMirrorButton.Enabled = enabled;
            m_cancelButton.Enabled = !enabled;
        }

        private bool IsMirrorCommandInProgress()
        {
            return !m_fullMirrorButton.Enabled;
        }
        

        /// <summary>
        /// Write to a message box and log file only
        /// </summary>
        public static void Log(string msg)
        {
            if (m_silentMode)
            {
                if (m_logFileStream != null)
                    m_logFileStream.WriteLine(msg);
            }
        }

        public static void Print(string msg, string title, bool forceShowMessageBox = false)
        {
            Log(msg);
            if (!m_silentMode || forceShowMessageBox)
                MessageBox.Show(msg, title);
        }

        private void m_cancelButton_Click(object sender, EventArgs e)
        {
            m_cts.Cancel();
            m_cancelButton.Enabled = false;
        }

        private void CreateNewCancellationTokenSourceAndCancelOld()
        {
            if (m_cts != null)
                m_cts.Cancel();

            m_cts = new CancellationTokenSource();
        }

        public void ReportStatus(string status)
        {
            SetNextStatusMessage(status);
        }

        void ICommandReporter.Log(string message)
        {
            MainForm.Log(message);
        }
    }

    [Serializable]
    public class Login
    {
        public string Username = null;
        public string Password = null;
        public string Server = null;
        public string Vault = null;
        public string MirrorFolder = null;
        public DateTime LastSyncTime = DateTime.MinValue;

        public Login(string username, string password, string server, string vault, 
            string mirrorFolder, DateTime lastSyncTime)
        {
            this.Username = username;
            this.Password = password;
            this.Server = server;
            this.Vault = vault;
            this.MirrorFolder = mirrorFolder;
            this.LastSyncTime = lastSyncTime;
        }
    }

    /// <summary>
    /// This class stores the user's settings.  It's designed to be easily stored to a file.
    /// </summary>
    [Serializable]
    public class Settings
    {
        // the name of the file that the settings are stored to
        private static string FILENAME = "settings.dat";

        // a store of all the saved logins
        // the key format is "SERVER;VAULT"
        private Dictionary<string, Login> m_logins;
        

        private Settings()
        {
            m_logins = new Dictionary<string, Login>();
        }

        // Adds the login info to the saved settings
        // If the login already exists, the entry is updated.
        public void AddLogin(Login login)
        {
            if (login.Server.Length > 0 && login.Vault.Length > 0)
                m_logins[login.Server.ToLower() + ";" + login.Vault.ToLower()] = login;
        }

        public Login GetLogin(string server, string vault)
        {
            string key = server.ToLower() + ";" + vault.ToLower();

            if (m_logins.ContainsKey(key))
                return m_logins[key];
            else
                return null;
        }

        /// <summary>
        /// If there is only one login, return it.  Otherwise return null.
        /// </summary>
        public Login GetOnlyLogin()
        {
            if (m_logins.Count == 1)
            {
                Dictionary<string, Login>.Enumerator enumerator = m_logins.GetEnumerator();
                enumerator.MoveNext();
                return enumerator.Current.Value;
            }
            else
                return null;
        }

        /// <summary>
        /// Read the object in from the .dat file
        /// </summary>
        public static Settings LoadSettings()
        {
            IFormatter formatter = new BinaryFormatter();
            Stream stream = null;
            try
            {
                stream = new FileStream(FILENAME, FileMode.Open);
                return (Settings) formatter.Deserialize(stream);
            }
            catch 
            {   }
            finally
            {
                if (stream != null)
                    stream.Close();
            }

            return new Settings();
        }

        /// <summary>
        /// Save the settings to the .dat file
        /// </summary>
        public void SaveSettings()
        {
            IFormatter formatter = new BinaryFormatter();
            Stream stream = null;
            try
            {
                stream = new FileStream(FILENAME, FileMode.Create);
                formatter.Serialize(stream, this);
            }
            catch
            {   }
            finally
            {
                if (stream != null)
                    stream.Close();
            }
        }
    }

    class SilentCommandReporter : ICommandReporter
    {
        public void ReportStatus(string status)
        {
        }

        public void Log(string message)
        {
            MainForm.Log(message);
        }
    }
}
