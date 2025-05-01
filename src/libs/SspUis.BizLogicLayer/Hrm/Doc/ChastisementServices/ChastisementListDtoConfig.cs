using System;
using System.Linq;
using AutoMapper;
using GenericServices.Configuration;
using SspUis.Core;
using SspUis.Core.Security;
using SspUis.DataLayer;
using SspUis.DataLayer.EfClasses;
using SspUis.DataLayer.EfClasses.Hrm;

namespace SspUis.BizLogicLayer.Hrm;

public class ChastisementListDtoConfig : PerDtoConfig<ChastisementListDto, Chastisement>
{
    public override Action<IMappingExpression<Chastisement, ChastisementListDto>> AlterReadMapping =>
        cfg => cfg
            .ForMember(x => x.Status, x => x.MapFrom(ent => ent.Status.Translates.AsQueryable().FirstOrDefault(StatusTranslate.GetExpr(TranslateColumn.full_name, ServiceProvider.CultureHelper.CurrentCulture.Id)).TranslateText ?? ent.Status.FullName))
            .ForMember(x => x.Organization, x => x.MapFrom(ent => ent.Organization.FullName))
            .ForMember(x => x.Employee, x => x.MapFrom(ent => string.Join(", ", ent.Tables.Select(a => a.Employee.Person.FullName))))
               .ForMember(x => x.CanCancel, x => x.MapFrom(ent => ServiceProvider.AuthService.HasPermission(ModuleCode.ChastisementCancel) != null ? StatusIdConst.CanApplyChastisement(ent.StatusId, StatusIdConst.NOT_ACCEPTED, null) : false))
               .ForMember(x => x.CanSign, x => x.MapFrom(ent => ServiceProvider.AuthService.HasPermission(ModuleCode.ChastisementSign) && StatusIdConst.CanApplyChastisement(ent.StatusId, StatusIdConst.ACCEPTED, null) && ent.StatusId != StatusIdConst.SIGNING ? true : false))
              .ForMember(x => x.CanAccept, x => x.MapFrom(ent => ServiceProvider.AuthService.HasPermission(ModuleCode.ChastisementAccept) && StatusIdConst.CanApplyChastisement(ent.StatusId, StatusIdConst.ACCEPTED, null) ? true : false))
             .ForMember(x => x.CanEdit, x => x.MapFrom(ent => ServiceProvider.AuthService.HasPermission(ModuleCode.ChastisementEdit) != null ? StatusIdConst.CanApplyChastisement(ent.StatusId, StatusIdConst.MODIFIED, null) : false))
             .ForMember(x => x.CanDelete, x => x.MapFrom(ent => ServiceProvider.AuthService.HasPermission(ModuleCode.ChastisementDelete) != null ? StatusIdConst.CanApplyChastisement(ent.StatusId, StatusIdConst.DELETED, null) : false));
}