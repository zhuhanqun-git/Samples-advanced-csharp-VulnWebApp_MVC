using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json;
using System.Threading.Tasks;

namespace VulnWebApp.Models
{
    public class User
    {
        public string Login { get; set; }
        public string Role { get; set; }
        public string Password { get; set; }

        public override string ToString() => JsonSerializer.Serialize<User>(this);
    }
}
