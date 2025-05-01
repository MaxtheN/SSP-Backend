using GenericServices;
using SspUis.DataLayer.EfClasses;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WEBASE.Attributes;
using GenericServices.Configuration;
using AutoMapper;
using WEBASE.Utility;
using SspUis.DataLayer;
using WEBASE;
using SspUis.Core;
using SspUis.DataLayer.EfClasses.Hrm;

namespace SspUis.BizLogicLayer.AccountServices
{
    public class UserDtoConfig : PerDtoConfig<AccountUserDto, User>
    {
        public override Action<IMappingExpression<User, AccountUserDto>> AlterReadMapping =>
            cfg => cfg
                .ForMember(x => x.Organization, x => x.MapFrom(ent => ent.Organization.Translates.AsQueryable()
                    .FirstOrDefault(OrganizationTranslate.GetExpr(TranslateColumn.full_name, ServiceProvider.CultureHelper.CurrentCulture.Id)).TranslateText ?? ent.Organization.FullName))
                .ForMember(x => x.OrganizationRegionId, x => x.MapFrom(ent => ent.Organization.RegionId))
                .ForMember(x => x.OrganizationDistrictId, x => x.MapFrom(ent => ent.Organization.DistrictId))
                .ForMember(x => x.OrganizationGroupId, x => x.MapFrom(ent => ent.Organization.OrganizationGroupId))
                .ForMember(x => x.Pinfl, x => x.MapFrom(ent => ent.Person.Pinfl))
                .ForMember(x => x.Inn, x => x.MapFrom(ent => ent.Person.Inn))
                .ForMember(x => x.PictureId, x => x.MapFrom(ent => ent.Person.PictureId))
                .ForMember(x => x.Language, x => x.MapFrom(ent => ent.Language.FullName))
                .ForMember(x => x.ShortName, x => x.MapFrom(ent => ent.Person.ShortName))
                .ForMember(x => x.FullName, x => x.MapFrom(ent => ent.Person.FullName))
                .ForMember(x => x.Modules, x => x.MapFrom(ent => ent.UserRoles.Where(a => a.StateId == StateIdConst.ACTIVE).SelectMany(a => a.Role.RoleModules.Select(b => b.Module.Code)).Distinct()))
                .ForMember(x => x.Roles, x => x.MapFrom(ent => ent.UserRoles.Where(a => a.StateId == StateIdConst.ACTIVE).Select(a => a.Role.Translates.AsQueryable()
                    .FirstOrDefault(RoleTranslate.GetExpr(TranslateColumn.full_name, ServiceProvider.CultureHelper.CurrentCulture.Id)).TranslateText ?? a.Role.FullName).ToList()))
                .ForMember(x => x.PositionCategoryId, x => x.MapFrom(ent => ent.EmployeeManageId != null
                    ? ent.EmployeeManage.Position.PositionCategoryId ?? null
                    : null))
                .ForMember(x => x.PositionCategory, x => x.MapFrom(ent =>
                    ent.EmployeeManageId != null
                        ? ent.EmployeeManage.Position.PositionCategoryId != null
                            ? ent.EmployeeManage.Position.PositionCategory.Translates.AsQueryable()
                                .FirstOrDefault(PositionCategoryTranslate.GetExpr(TranslateColumn.full_name, ServiceProvider.CultureHelper.CurrentCulture.Id))
                                .TranslateText ?? ent.EmployeeManage.Position.PositionCategory.FullName
                            : string.Empty
                        : string.Empty))
            ;
    }
}
