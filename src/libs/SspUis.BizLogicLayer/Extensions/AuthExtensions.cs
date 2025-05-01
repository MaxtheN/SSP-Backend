using SspUis.Core;
using SspUis.Core.Security;
using SspUis.DataLayer;
using SspUis.DataLayer.EfClasses;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WEBASE;
using WEBASE.Utility;

namespace SspUis.BizLogicLayer.Extensions
{
    public static class AuthExtensions
    {

        public static string GetSignerPosition(this IAuthService authService, IUnitOfWork unitOfWork)
        {
            var positionName = "";
            if (authService.User.PositionId.HasValue)
                positionName = unitOfWork.PositionRepository.ById(authService.User.PositionId.Value).FullName;
            return positionName;
        }

        public static string GetSignerName(this IAuthService authService)
        {
            if (authService.User.FullName.NullOrEmpty() || authService.User.FullName == "[null]")
                return "";

            string[] fio = authService.User.FullName.Split(new string[] { " " }, StringSplitOptions.None);

            return StringUtility.GetFIO(string.Join(" ", fio[0][..1].ToUpper() + fio[0][1..].ToLower(), fio[1]));
        }
    }
}
