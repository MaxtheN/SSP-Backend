using SspUis.BizLogicLayer.AccountServices;
using SspUis.Core.Security;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SspUis.BizLogicLayer
{
    public static class UserExtensions
    {
        public static string ToTextForDocumentLog(this UserAuthModel user) => $"{user.UserName} - {user.FullName}";
    }
}
