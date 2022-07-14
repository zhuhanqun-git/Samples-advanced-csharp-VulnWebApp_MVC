using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.Json;
using VulnWebApp.Models;
using Microsoft.AspNetCore.Hosting;
using System.Text;
using Newtonsoft.Json;
using System;

namespace VulnWebApp.Services
{
    public class JsonFileUserService
    {
        public JsonFileUserService(IWebHostEnvironment webHostEnvironment)
        {
            WebHostEnvironment = webHostEnvironment;
        }

        public IWebHostEnvironment WebHostEnvironment { get; }

        private string JsonFileName
        {
            get { return Path.Combine(WebHostEnvironment.WebRootPath, "data", "users.json"); }
        }

        public Dictionary<string, string> GetUsers()
        {
            string jsonString = File.ReadAllText(JsonFileName);
            try
            {
                Dictionary<string, string> userInfo = JsonConvert.DeserializeObject<Dictionary<string, string>>(jsonString);
                return userInfo;
            }
            catch (Exception e)
            {
                RestoreUser();
                return GetUsers();
            }
        }

        // Changing password
        public void ModifyUser(string password)
        {
            var users = GetUsers();
            var query = users.Values;

            StringBuilder sb = new StringBuilder();
            StringWriter sw = new StringWriter(sb);

            using (JsonWriter writer = new JsonTextWriter(sw))
            {
                writer.Formatting = Formatting.Indented;

                writer.WriteStartObject();

                writer.WritePropertyName("role");
                writer.WriteRawValue("\"default\"");

                writer.WritePropertyName("login");
                //writer.WriteRawValue("\"" + login + "\"");
                writer.WriteRawValue("\"guest\"");

                writer.WritePropertyName("password");
                writer.WriteRawValue("\"" + password + "\"");

                writer.WriteEndObject();
            }

            File.WriteAllText(JsonFileName, sb.ToString());
        }

        // Restoring default user
        public void RestoreUser()
        {
            StringBuilder sb = new StringBuilder();
            StringWriter sw = new StringWriter(sb);

            using (JsonWriter writer = new JsonTextWriter(sw))
            {
                writer.Formatting = Formatting.Indented;

                writer.WriteStartObject();

                writer.WritePropertyName("role");
                writer.WriteRawValue("\"default\"");

                writer.WritePropertyName("login");
                writer.WriteRawValue("\"guest\"");

                writer.WritePropertyName("password");
                writer.WriteRawValue("\"guest\"");

                writer.WriteEndObject();
            }

            File.WriteAllText(JsonFileName, sb.ToString());
        }
    }
}
