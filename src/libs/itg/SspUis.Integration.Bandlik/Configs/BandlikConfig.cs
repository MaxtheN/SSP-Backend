using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SspUis.Integration.Bandlik.Configs
{
    public class BandlikConfig
    {
        public string Api { get; set; }
        public string Login { get; set; }
        public string Pswd { get; set; }
        public string Basictoken { get { return Convert.ToBase64String(System.Text.Encoding.ASCII.GetBytes($"{Login}:{Pswd}")); } }
        public bool UseProxy { get; set; }

        public string Token { get; set; } = string.Empty;
    }
}
