Overview:
The ItemEditor is a tool that allows users to perform common Item related activities such as chaing the revision number or life cycle state.  

 
ProductstreamItemEditor contains the following features/concepts:
	- Log in/security
	- Item viewing
	- Item creating
	- Item revision numbers
	- Item life cycle states
	- Item restoring
	- Linking an existing Item to a File.
	- Parsing error data
	- Automatic re-authentication


Known issues:
- If the user chooses the "Remember this information" option on the login screen, a login.dat file is created in the same directory as ProductstreamItemEditor.exe.  This file is unencrypted and can be used to get password information.
- Errors are dispalyed but not properly handled (ie. rollbacks/undos are not done).  
