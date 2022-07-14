This is simple web application that contains JSON Injection and Command Injection vulnerabilities. The application is written using ASP.NET Web Core.
You must have .NET 5.0 and MSBuild 16.10 or Visual Studio 2019 installed (and optionally the Fortify extension for Visual Studio 2019).

There are two folders in VulnWebApp_MVC: 
 - VulnWebApp_MVC: contains source code and solution file
 - Published: folder used to run the application in IIS

This application represents a simple web application. You have user 'guest' with password 'guest' and role 'default'. 
User data is stored in VulnWebApp_MVC\wwwroot\data\users.json. 
NOTE: Do not change the structure of this file. To restore this file, click the "Restore data" link on the site.

To run the application:
I) Using Visual Studio
Open the application in Visual Studio 2019 or later, and then click "Debug -> Start debugging" or ("Debug -> Start without debugging").

II) Using IIS
1. Turn IIS on: Control Panel -> Programs and Features -> Turn Windows features on or off -> Select Internet Information Service.
	1.1. Open Internet Information Service -> World wide web service -> Application Development Features and select all check boxes.
2. Download and install https://dotnet.microsoft.com/permalink/dotnetcore-current-windows-runtime-bundle-installer
3. Open "Internet Information Service Manager".
4. Right-click "Sites" in the left menu.
5. Add Website.
6. Fill in the open form:
- Site name - Enter any site name
- Physical path - Path to folder with published site (VS2019\VulnWebApp_MVC\Published)
- Host name - localhost
- Port (if necessary) - provide any free port
Click OK.
7. In a browser, navigate to "localhost:<port>".


The goal is to change the user's role to 'admin', log in, and then you can execute any OS (Windows) command.

To achieve the goal, exploit the JSON Injection vulnerability.
1. Navigate to "Change password".
2. Enter: <password>","role":"admin
replace <password> with any password you want.
3. Navigate to the "Login" page and enter your credentials.
4. In the authorized zone, you can ping any host. Try to chain commands.
For example, try to execute something like this: 8.8.8.8 & dir
=> After the ping is finished, the second command (in our case 'dir') is executed.

~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~

Translate and scan the solution from the Developer Command Prompt.
Change to this directory (VS2019\VulnWebApp_MVC\VulnWebApp_MVC), and then run the following commands:
  $ sourceanalyzer -b VulnWebApp -clean
  $ sourceanalyzer -b VulnWebApp msbuild /t:restore /t:rebuild VulnWebApp_MVC.sln
  $ sourceanalyzer -b VulnWebApp -scan


If you have the Fortify Extension for Visual Studio 2019 installed, you can translate and scan the solution from Visual Studio:
1. In Visual Studio 2019, open VS2019\VulnWebApp_MVC\VulnWebApp_MVC.sln.
2. Select Extensions > Fortify > Analyze Solution.

After successful completion of the scan, you should see:
- JSON Injection vulnerability 
- Command Injection vulnerability
- Other vulnerability categories might also be present depending on the Fortify Rulepacks used in the scan.
The JSON Injection vulnerability is reported for a method WriteRawValue from method ModifyUser in JsonFileUserService object. 
By exploiting the vulnerability, an attacker can change the role.

The Command Injection vulnerability shows that tainted data flows through the users' request to execution of an OS command.
By exploiting this vulnerability, an attacker can chain commands and execute arbitrary code on a server.
