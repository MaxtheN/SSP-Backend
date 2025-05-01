using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SspUis.Integration.DigitizationCenter
{
    public class DigitizationCenterConfig
    {
        public string Api { get; set; } = null!;
        public string LoginApi { get; set; } = null!;

        public string Login { get; set; } = null!; 
        public string Pswd { get; set; } = null!;
        
        public string UserName { get; set; } = null!;
        public string Password { get; set; } = null!;

        public string ConsumerKey { get; set; } = null!;
        public string ConsumerSecret { get; set; } = null!;
        
        internal string Token { get { return Convert.ToBase64String(System.Text.Encoding.ASCII.GetBytes($"{ConsumerKey}:{ConsumerSecret}")); } }
        internal string AccessToken { get; set; } = null!;

        internal string BasicToken { get { return Convert.ToBase64String(System.Text.Encoding.ASCII.GetBytes($"{Login}:{Pswd}")); } }
        public bool UseProxy { get; set; }
    }
}
