using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SspUis.Integration.Finance.Configs;

public class FinanceConfig
{
    public string Api { get; set; }
    public bool UseProxy { get; set; }
    public string LoginApi {  get; set; }
    public string UserName { get; set; } = string.Empty;
    public string Password { get; set; } = string.Empty;
    internal string TokenClient { get { return Convert.ToBase64String(System.Text.Encoding.ASCII.GetBytes($"{UserName}:{Password}")); } }
    public string AccessToken { get; set; }
}
