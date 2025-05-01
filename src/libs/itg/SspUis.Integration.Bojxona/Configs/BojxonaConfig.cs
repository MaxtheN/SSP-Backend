using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SspUis.Integration.Bojxona.Configs
{
    public class BojxonaConfig
    {
        public string Api { get; set; }
        public string Login { get; set; }
        public string Pswd { get; set; }
        public string Basictoken { get { return Convert.ToBase64String(System.Text.Encoding.ASCII.GetBytes($"{Login}:{Pswd}")); } }
        public bool UseProxy { get; set; }

        public string UserName { get; set; } = string.Empty;
        public string Password { get; set; } = string.Empty;
        internal string BasicTokenClient { get { return Convert.ToBase64String(System.Text.Encoding.ASCII.GetBytes($"{UserName}:{Password}")); } }
    }
}
