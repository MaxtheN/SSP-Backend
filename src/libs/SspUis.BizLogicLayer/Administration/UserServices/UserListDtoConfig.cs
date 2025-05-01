using System;
using System.Linq;
using AutoMapper;
using GenericServices.Configuration;
using SspUis.DataLayer;
using SspUis.DataLayer.EfClasses;
using WEBASE;

namespace SspUis.BizLogicLayer.UserServices
{
    public class UserListDtoConfig : PerDtoConfig<UserListDto, User>
    {
        public override Action<IMappingExpression<User, UserListDto>> AlterReadMapping =>
            cfg => cfg
                .ForMember(x => x.State, x => x.MapFrom(ent => ent.State.Translates.AsQueryable()
                    .FirstOrDefault(StateTranslate.GetExpr(TranslateColumn.full_name, ServiceProvider.CultureHelper.CurrentCulture.Id)).TranslateText ?? ent.State.FullName))
                .ForMember(x => x.Organization, x => x.MapFrom(ent => ent.Organization.Translates.AsQueryable()
                    .FirstOrDefault(OrganizationTranslate.GetExpr(TranslateColumn.full_name, ServiceProvider.CultureHelper.CurrentCulture.Id)).TranslateText ?? ent.Organization.FullName))
                .ForMember(x => x.OrganizationInn, x => x.MapFrom(ent => ent.Organization.Inn))
                .ForMember(x => x.FullName, x => x.MapFrom(ent => ent.Person.FullName))
                .ForMember(x => x.UserRoles, x => x.MapFrom(ent => ent.UserRoles.Where(x => x.StateId == StateIdConst.ACTIVE).Select(x => x.RoleId)))
                .ForMember(x => x.UserModels, x => x.MapFrom(ent => ent.RoleModules.Select(x => x.ModuleId)))
                .ForMember(x => x.Roles, x => x.MapFrom(ent => ent.UserRoles.Where(a => a.StateId == StateIdConst.ACTIVE).Select(a => a.Role.Translates.AsQueryable()
                    .FirstOrDefault(RoleTranslate.GetExpr(TranslateColumn.full_name, ServiceProvider.CultureHelper.CurrentCulture.Id)).TranslateText ?? a.Role.FullName).ToList()))
                ;
    }
}
