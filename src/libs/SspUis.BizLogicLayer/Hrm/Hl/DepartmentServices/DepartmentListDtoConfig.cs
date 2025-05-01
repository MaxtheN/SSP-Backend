using System;
using System.Linq;
using AutoMapper;
using GenericServices.Configuration;
using SspUis.DataLayer.EfClasses;
using SspUis.DataLayer;
using SspUis.DataLayer.EfClasses.Hrm;

namespace SspUis.BizLogicLayer.Hrm;
public class DepartmentListDtoConfig :PerDtoConfig<DepartmentListDto,Department>
{
    public override Action<IMappingExpression<Department, DepartmentListDto>> AlterReadMapping =>
        cfg => cfg
            .ForMember(x => x.State, x => x.MapFrom(ent => ent.State.Translates.AsQueryable()
                .FirstOrDefault(StateTranslate.GetExpr(TranslateColumn.full_name, ServiceProvider.CultureHelper.CurrentCulture.Id)).TranslateText ?? ent.State.FullName))
            .ForMember(d => d.Parent, c => c.MapFrom(d => d.Parent != null ? d.Parent.FullName : ""))
        .ForMember(d => d.IndicatorDepartment, c => c.MapFrom(d => d.IndicatorDepartment != null ? d.IndicatorDepartment.FullName : ""))
            .ForMember(x => x.Organization, x => x.MapFrom(ent => ent.Organization.Translates.AsQueryable()
                .FirstOrDefault(OrganizationTranslate.GetExpr(TranslateColumn.full_name, ServiceProvider.CultureHelper.CurrentCulture.Id)).TranslateText ?? ent.Organization.FullName))
            ;
}
