Overview:
JobProcessorApiSamples are samples that illustrate how to queue and process a Job. 
 
JobProcessorApiSamples contains the following features/concepts:
	- Custom Command
	- Adding a Job to the queue
	- Adding a Handler to the Job Processor application

Requirements:
	Vault Workgroup, Vault Collaboration or Vault Professional
	The Job Server feature must be enabled

To Queue a job:
Open JobProcessorApiSamples.sln in Visual Studio.  The project should open with no errors.

Deploy the built files to %programData%\Autodesk\Vault 2018\Extensions\JobProcessorApiSamples
NOTE: The %programData% variable may not be set for all operating systems.  In Windows XP and 2003, this is usually "C:\Documents and Settings\All Users\Application Data".  In Vista and above, this is usually "C:\ProgramData"

Run Vault Explorer.  There should be a "Queue Publish Job" command when right-clicking on a file.  Execute the command and you should see a job in the Job Queue of type "MyCompany.File.Publish".


To Process a job:
Run JobProcessor.exe.  It should automatically process the job and copy the file to your C:\PublishFolder directory.

Known issues:
- There is almost no error handling code. 


 
