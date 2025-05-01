using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SspUis.DataLayer
{
    public enum UserLogAction
    {
        ValidatePassword,
        IncorrectPasswordEntered,
        LoginByPassword,
        Logout,
        RestorePassword,
        RestoredPassword,
        LoginBySms,
        LoginByEImzo,
        LoginByOneId
    }
}
