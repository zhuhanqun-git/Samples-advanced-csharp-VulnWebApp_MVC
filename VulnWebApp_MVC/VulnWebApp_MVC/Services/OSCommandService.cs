using Microsoft.AspNetCore.Hosting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Diagnostics;
using System.IO;

namespace VulnWebApp.Services
{
    public class OSCommandService : JsonFileUserService
    {
        public OSCommandService(IWebHostEnvironment webHostEnvironment) : base(webHostEnvironment)
        { }
        
        public string CommandResult { get; set; }

        //Func<int, int, bool> isAdmin = (_, _) => { return GetUsers()["role"] == "admin"; };

        public bool UserIsAdmin()
        {
            var users = GetUsers();
            var role = users["role"];

            return role == "admin";
        }

        public void ExecuteCommand(string command)
        {
            if (UserIsAdmin())
            {
                ProcessStartInfo startinfo = new ProcessStartInfo();
                startinfo.FileName = @"cmd.exe";
                startinfo.Arguments = " /C ping -n 2 " + command;
                Process process = new Process();
                process.StartInfo = startinfo;
                process.StartInfo.UseShellExecute = false;
                process.StartInfo.RedirectStandardInput = true;
                process.StartInfo.RedirectStandardOutput = true;
                process.Start();

                string output = process.StandardOutput.ReadToEnd();
                process.WaitForExit();
                Console.WriteLine(output);

                // Feature from C# 9
                Func<int, int, string> Result = (_, _) => { return output; };
                CommandResult = Result(1, 2);
            }
            else
            {
                CommandResult = "You're not worthy!\nYou must have 'admin' role.";
            }
        }
    }
}
