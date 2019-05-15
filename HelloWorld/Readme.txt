Overview:
HelloWorld is a simple command extension which illustrates how to extend Vault Explorer. 
 
HelloWorld contains the following features/concepts:
	- Custom Command
	- Custom Tab View


To Use:
Open HelloWorld.sln in Visual Studio.  The project should open and compile with no errors.

Deploy the built files to %programData%\Autodesk\Vault 2018\Extensions\HelloWorld
NOTE: The %programData% variable may not be set for all operating systems.  In Windows XP and 2003, this is usually "C:\Documents and Settings\All Users\Application Data".  In Vista and above, this is usually "C:\ProgramData"

Run Vault Explorer.  There should be a "Hello World" command when right-clicking on a file.  There should also be a new file tab view called "Selection Info"


Known issues:
- There is almost no error handling code.  
