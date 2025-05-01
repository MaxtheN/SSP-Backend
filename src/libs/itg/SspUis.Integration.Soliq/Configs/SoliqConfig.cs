using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SspUis.Integration.Soliq
{
    public class SoliqConfig
    {
        public string api { get; set; }

        public string login { get; set; }
        public string pswd { get; set; }
        public string basictoken { get { return Convert.ToBase64String(System.Text.Encoding.ASCII.GetBytes($"{login}:{pswd}")); } }
        public string Username { get; set; } = string.Empty;
        public string Password { get; set; } = string.Empty;
        public bool UseProxy { get; set; }
        internal string BasicTokenClient { get { return Convert.ToBase64String(System.Text.Encoding.ASCII.GetBytes($"{Username}:{Password}")); } }
    }
}
