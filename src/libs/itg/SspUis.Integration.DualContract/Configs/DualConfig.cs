using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SspUis.Integration.Dual.Configs;

public class DualConfig
{
    public string api { get; set; }
    public string Username { get; set; } = string.Empty;
    public string Password { get; set; } = string.Empty;
    internal string BasicTokenClient { get { return Convert.ToBase64String(System.Text.Encoding.ASCII.GetBytes($"{Username}:{Password}")); } }
}
