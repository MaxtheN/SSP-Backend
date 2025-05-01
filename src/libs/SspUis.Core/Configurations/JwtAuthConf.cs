using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WEBASE.Utility;
using WEBASE;
using WEBASE.AspNet.Security;

namespace SspUis.Core.Configurations
{
    public class IntegrationJwtConfig : JwtConfig
    {

    }

    public class IntegrationAuthConfig
    {
        public IntegrationJwtConfig Jwt { get; set; } = new();
        public IntegrationUserConfig[] IntegrationUsers { get; set; } = new IntegrationUserConfig[] { }; 
    }

    public class IntegrationUserConfig
    {
        public string UserName { get; set; }
        public string Password { get; set; }
        public string Inn { get; set; }
        public bool IsValidPassword(string password)
        {
            return !(password.NullOrEmpty() || this.Password != password);
        }
    }
}
