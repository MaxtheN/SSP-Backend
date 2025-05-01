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

namespace SspUis.BizLogicLayer.AccountServices
{
    public static class UserAuthModelSelect
    {
        public static IQueryable<UserAuthModel> MapToAuthModel(this IQueryable<User> users)
        {
            return users.Select(a => new UserAuthModel
                        {
                            Id = a.Id,
                            Inn = a.Person.Inn,
                            Pinfl = a.Person.Pinfl,
                            PersonId = a.PersonId,
                            UserName = a.UserName,
                            FullName = a.Person.FullName,
                            IsAdmin = a.UserRoles.Where(a => a.StateId == StateIdConst.ACTIVE).Any(a => a.Role.IsAdmin),
                            LanguageId = a.LanguageId,
                            LanguageCode = a.Language.Code,
                            OrganizationId = a.OrganizationId,
                            EmployeeManageId = a.EmployeeManageId,
                            Modules = a.UserRoles
                           .Where(a => a.StateId == StateIdConst.ACTIVE)
                           .SelectMany(b => b.Role.RoleModules.Select(c => c.Module.Code))
                           .Distinct(),
                        });
        }
    }
}
