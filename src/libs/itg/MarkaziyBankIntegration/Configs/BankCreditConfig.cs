using System.Diagnostics;

namespace SspUis.Integration.BankCredit
{
    public class BankCreditConfig
    {
        public string Api { get; set; } = null!;
        public string Token { get; set; } = null!;
        public string UserName { get; set; } = null!;
        public string Password { get; set; } = null!;
        internal string BasicToken { get { return Convert.ToBase64String(System.Text.Encoding.ASCII.GetBytes($"{UserName}:{Password}")); } }
        public bool UseProxy { get; set; }
       
    }
}
