using System.Diagnostics;

namespace SspUis.Integration.IntegrationCertificate
{
    public class IntegrationCertificateConfig
    {
        public bool UseProxy { get; set; }
        //-----Soliq-----//
        public string Api { get; set; } = null!;
        public string Token { get; set; } = null!;
        public string UserNameSoliq { get; set; } = null!;
        public string PasswordSoliq { get; set; } = null!;
        internal string BasicTokenSoliq { get { return Convert.ToBase64String(System.Text.Encoding.ASCII.GetBytes($"{UserNameSoliq}:{PasswordSoliq}")); } }
        //-----MB-----//
        public string ApiMB { get; set; } = null!;
        public string UserNameMB { get; set; } = null!;
        public string PasswordMB { get; set; } = null!;
        internal string BasicTokenMB { get { return Convert.ToBase64String(System.Text.Encoding.ASCII.GetBytes($"{UserNameMB}:{PasswordMB}")); } }
        //-----Bojxona-----//
        public string ApiBojxona { get; set; } = null!;
        public string UserNameBojxona { get; set; } = null!;
        public string PasswordBojxona { get; set; } = null!;
        internal string BasicTokenBojxona { get { return Convert.ToBase64String(System.Text.Encoding.ASCII.GetBytes($"{UserNameBojxona}:{PasswordBojxona}")); } }
        //-----XalqBank-----//
        public string ApiXalqBank{ get; set; } = null!;
        public string UserNameXalqBank { get; set; } = null!;
        public string PasswordXalqBank { get; set; } = null!;
        internal string BasicTokenXalqBank { get { return Convert.ToBase64String(System.Text.Encoding.ASCII.GetBytes($"{UserNameMB}:{PasswordMB}")); } }
        //-----Moliya-----//
        public string ApiMoliya { get; set; } = null!;
        public string UserNameMoliya { get; set; } = null!;
        public string PasswordMoliya { get; set; } = null!;
        public string TokenMoliya { get; set; } = null!;
        internal string BasicTokenMoliya { get { return Convert.ToBase64String(System.Text.Encoding.ASCII.GetBytes($"{UserNameMB}:{PasswordMB}")); } }
    }
}
