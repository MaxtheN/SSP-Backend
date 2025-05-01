using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SspUis.Integration.DavAktiv
{
    public class DavAktivConfig
    {
        public string Api { get; set; } = string.Empty;
        public string Login { get; set; } = string.Empty;
        public string Password { get; set; } = string.Empty;
        public string ApplicationApi { get; set; } = string.Empty;
        public string ApplicationLogin { get; set; } = string.Empty;
        public string ApplicationPassword { get; set; } = string.Empty;
        public bool UseProxy { get; set; }
        internal string BasicToken { get { return Convert.ToBase64String(System.Text.Encoding.ASCII.GetBytes($"{Login}:{Password}")); } }
        internal string ApplicationBasicToken { get { return Convert.ToBase64String(System.Text.Encoding.ASCII.GetBytes($"{ApplicationLogin}:{ApplicationPassword}")); } }
    }
}
