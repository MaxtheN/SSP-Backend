using System;
using System.Linq;
using AutoMapper;
using GenericServices.Configuration;
using SspUis.DataLayer;
using SspUis.DataLayer.EfClasses;
using WEBASE;

namespace SspUis.BizLogicLayer.UserServices
{
    public class UserDtoConfig : PerDtoConfig<UserDto, User>
    {
        public override Action<IMappingExpression<User, UserDto>> AlterReadMapping =>
            cfg => cfg
                .ForMember(x => x.State, x => x.MapFrom(ent => ent.State.Translates.AsQueryable()
                    .FirstOrDefault(StateTranslate.GetExpr(TranslateColumn.full_name, ServiceProvider.CultureHelper.CurrentCulture.Id)).TranslateText ?? ent.State.FullName))
                .ForMember(x => x.Organization, x => x.MapFrom(ent => ent.Organization.Translates.AsQueryable()
                    .FirstOrDefault(OrganizationTranslate.GetExpr(TranslateColumn.full_name, ServiceProvider.CultureHelper.CurrentCulture.Id)).TranslateText ?? ent.Organization.FullName))
                .ForMember(x => x.EmployeeManage, x => x.MapFrom(ent => ent.EmployeeManageId != null ? ent.EmployeeManage.Employee.Person.FullName : ""))
                .ForMember(x => x.Roles, x => x.MapFrom(ent => ent.UserRoles.Where(a => a.StateId == StateIdConst.ACTIVE).Select(a => a.RoleId)));
    }
}
