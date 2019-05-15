Overview:
RestrictOperations is a simple Vault extension which illustrates how to restrict operations by responding to Web Service Command Events.  The sample contains two parts.  The RestrictOperations.dll, which hooks to the Vault framework, and Configuratior.exe, which allows you to configure which operations are restricted.
 
RestrictOperation contains the following features/concepts:
	- Web Service Command Events
	- Restrictions


To Use:
Open RestrictOperations.sln in Visual Studio.  The project should open and compile with no errors.

Deploy the built files from the Configurator project to %programData%\Autodesk\Vault 2018\Extensions\RestrictOperations
NOTE: The %programData% variable may not be set for all operating systems.  In Windows XP and 2003, this is usually "C:\Documents and Settings\All Users\Application Data".  In Vista and above, this is usually "C:\ProgramData"

Run Configuratior.exe.  Check the boxes of the operations you want to restrict.  Launch any Vault client application on that computer and run the restricted command.  You should see the operation blocked in the client.
When boxes are checked or unchecked in Configurator, the result is immediate.  There is no need to restart the Vault client application.


Known issues:
- There is almost no error handling code.  
