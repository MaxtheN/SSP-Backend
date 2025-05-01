using System;
using System.Linq;
using AutoMapper;
using GenericServices.Configuration;
using SspUis.DataLayer;
using SspUis.DataLayer.EfClasses;
using SspUis.DataLayer.EfClasses.Hrm;

namespace SspUis.BizLogicLayer.Hrm;

public class RecallLeaveDtoConfig :PerDtoConfig<RecallLeaveDto, RecallLeave>
{
    public override Action<IMappingExpression<RecallLeave, RecallLeaveDto>> AlterReadMapping =>
        cfg => cfg
            .ForMember(d => d.Status, c => c.MapFrom(e => e.Status.Translates.AsQueryable()
                .FirstOrDefault(StatusTranslate.GetExpr(TranslateColumn.full_name, ServiceProvider.CultureHelper.CurrentCulture.Id)).TranslateText ?? e.Status.FullName))
         .ForMember(d => d.Region, c => c.MapFrom(e => e.Organization.Region.Translates.AsQueryable()
                .FirstOrDefault(RegionTranslate.GetExpr(TranslateColumn.full_name, 3)).TranslateText ?? e.Organization.Region.FullName))
            .ForMember(d => d.Organization, c => c.MapFrom(e => e.Organization.Translates.AsQueryable()
                .FirstOrDefault(OrganizationTranslate.GetExpr(TranslateColumn.full_name, ServiceProvider.CultureHelper.CurrentCulture.Id)).TranslateText ?? e.Organization.FullName));
}
