VAULT MIRROR


INTRODUCTION:
---------------------------------
Vault Mirror creates a read-only mirror of the files within a Vault.  The mirror will contain the same files and directory structure as seen from Vault Explorer.  It's similar to running the "Get Entire Folder" command on the root folder, but Vault Mirror is smart enough to pull down only the needed files.

When used with Windows Task Scheduler, Vault Mirror can be used to create remote copy of the latest versions of the Vault file tree.


FEATURES:
---------------------------------
- Full Mirror command which fully arranges the mirror folder to be identical to the Vault.  This command may take a while to run depending on the size of the Vault.
- Partial Mirror command which pulls down files added since the last command was run.  This command is a quick alternative to performing a Full Mirror.
- Can be run from UI or command line


TO USE:
---------------------------------
- Run VaultMirror.exe
- Fill in the appropriate data.  Make sure that your mirror folder is not being used for something else.  Otherwise you run the risk of files getting deleted or overwritten. 
- Click on the "Full Mirror" button


COMMAND LINE USE:
---------------------------------
Usage:  VaultMirror [-PartialMirror | -FullMirror] [-U user] [-P pass] [-S server] [-V vault] [-L logfile] [-F mirrorFolder] [-WF] [-noFail]

-F Specifices where to download the files.
-WF Causes Vault Mirror to download based on the working folder settings.
Either -F or -WF must be used, but not both.

-noFail If this parameter is set, Vault Mirror will continue to the next file if there is a download error.  By default Vault Mirror aborts on the first failure.


KEEPING YOUR FILES IN SYNC:
---------------------------------
Vault Mirror does not enable a "real time" view of the Vault.  However it can be run frequently to insure that the files are mostly current. 

After the Full Mirror is run from the UI, set up a scheduled task from the Control Panel.  A Partial Mirror is a fast operation so it can run every 10 minutes or so.  A Full Mirror takes much longer, so a separate task can be scheduled to run every week or so. 


NOTES:
---------------------------------

- Modification of files in the mirror will NOT propagate back to the Vault itself.

- Partial Mirror will only pick up added files and new check-ins.  It will not detect deleted files or folders.  If a file or folder is renamed, the new name will be pulled down, but the old name will not be deleted.  Performing a Full Mirror will correctly remove deleted and renamed files or folders.

- Login data is automatically saved in a file called settings.dat.  This file is unencrypted, so it is a potential security hole.

- BUG: When running from the command line, messages cannot be written to the console.  To see error or success messages, use the -L option to write messages to the logfile then view the file.


REVISION HISTORY:
---------------------------------

7.0.4
- Bugfix: Force overwrite of local CAD files.

7.0.3
- Re-branded for App Store
- File resolution leaves a _V folder behind.
- Bugfix: Failed downloads no longer leave behind a 0 byte file.

7.0.1
- Update to work with Vault 2014
- New command line parameters -WF and -noFail
- Cancel button now works
- No longer requires Vault Client to be installed.  However, broken CAD references will not be resolved if Vault Client is not present.

6.0.1
- Updated to work with Vault 2013

5.0.2
- Updated to work with Vault 2012 
- Added requirement: Vault Client must be installed 

4.0.2
- Updated to work with Vault 2011

3.0.2
- Updated to work with Vault 2010

2.0.2
- Updated to work with ADMS 6
- Application can now remember information from multiple Vaults.
- Login information can now be specified on the command line
- Logfile option added to command line options
- BUG FIX: Files in the root folder can now be downloaded during a Partial Mirror 

1.0.4
- BUG FIX: Large files can now be downloaded

1.0.3 
- BUG FIX: Read-only files can now be deleted during a Full Mirror

1.0.2 
- Initial release


SUPPORT:
---------------------------------
No official support is available for this tool.  However you can post questions or comments to the Vault newsgroup http://forums.autodesk.com/t5/Autodesk-Vault/bd-p/101