using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SspUis.Integration.MarkaziyBank
{
    public class MarkaziyBankConfig
    {
        public string Api { get; set; } = string.Empty;
        public string Login { get; set; } = string.Empty;
        public string Password { get; set; } = string.Empty;
        public bool UseProxy { get; set; }
        internal string BasicToken { get { return Convert.ToBase64String(System.Text.Encoding.ASCII.GetBytes($"{Login}:{Password}")); } }
    }
}
