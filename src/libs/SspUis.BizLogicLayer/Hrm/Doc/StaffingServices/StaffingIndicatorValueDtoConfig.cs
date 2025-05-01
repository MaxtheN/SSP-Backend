using System;
using System.Linq;
using GenericServices.Configuration;
using AutoMapper;
using SspUis.DataLayer.EfClasses.Hrm;
using SspUis.DataLayer;

namespace SspUis.BizLogicLayer.Hrm.StaffingServices
{
    public class StaffingIndicatorValueDtoConfig : PerDtoConfig<StaffingIndicatorValueDto, StaffingIndicatorValue>
    {
        public override Action<IMappingExpression<StaffingIndicatorValue, StaffingIndicatorValueDto>> AlterReadMapping
            => cfg => cfg
                .ForMember(x => x.StaffingIndicatorCode, x => x.MapFrom(ent => ent.StaffingIndicator.Code))
                .ForMember(x => x.StaffingIndicatorName, x => x.MapFrom(ent => ent.StaffingIndicator.Translates.AsQueryable()
                    .FirstOrDefault(StaffingIndicatorTranslate.GetExpr(TranslateColumn.full_name, ServiceProvider.CultureHelper.CurrentCulture.Id)).TranslateText ?? ent.StaffingIndicator.FullName));
    }
}
