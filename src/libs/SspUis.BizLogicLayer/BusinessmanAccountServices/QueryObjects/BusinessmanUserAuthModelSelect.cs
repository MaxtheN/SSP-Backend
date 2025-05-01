using SspUis.Core.Security;
using SspUis.DataLayer;
using SspUis.DataLayer.EfClasses;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WEBASE;
using WEBASE.Models;

namespace SspUis.BizLogicLayer.BusinessmanAccountServices
{
    public static class BusinessmanUserAuthModelSelect
    {
        public static IQueryable<UserAuthModel> MapToAuthModel(this IQueryable<BusinessmanUser> users)
        {
            return users.Select(a => new UserAuthModel
            {
                Id = a.Id,
                Inn = a.Inn,
                Pinfl = a.Pinfl,
                UserName = a.UserName,
                FullName = a.FullName,
                LanguageId = a.LanguageId,
                LanguageCode = a.Language.Code
            });
        }
    }
}
