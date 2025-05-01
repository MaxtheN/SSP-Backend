using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SspUis.Integration.AgroBank.Configs;

public class AgroBankConfig
{
    public string Api { get; set; } = null!;
    public string Login { get; set; }
    public string Pswd { get; set; }
    public string BasicToken { get { return Convert.ToBase64String(System.Text.Encoding.ASCII.GetBytes($"{Login}:{Pswd}")); } }

    public string Username { get; set; }
    public string Password { get; set; }
    public string AccessToken { get { return Convert.ToBase64String(System.Text.Encoding.ASCII.GetBytes($"{Username}:{Password}")); } }
    public bool UseProxy { get; set; }
}
