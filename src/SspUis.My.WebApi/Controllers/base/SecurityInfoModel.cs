using SspUis.Core.Security;
using System.Collections.Generic;

namespace SspUis.My.WebApi.Controllers
{
    public class SecurityInfoModel
    {
        public SecurityInfoModel()
        {
            ModuleCodes = new();
        }

        public string Action { get; set; }
        public string Method { get; set; }
        public bool UnauthorizedAccess { get; set; }
        public HashSet<string> ModuleCodes { get; set; }
    }
}
