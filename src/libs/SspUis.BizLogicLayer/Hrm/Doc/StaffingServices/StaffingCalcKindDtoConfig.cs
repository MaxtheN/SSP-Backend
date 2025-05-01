using System;
using System.Linq;
using GenericServices.Configuration;
using AutoMapper;
using SspUis.DataLayer.EfClasses.Hrm;
using SspUis.DataLayer;

namespace SspUis.BizLogicLayer.Hrm.StaffingServices
{
    public class StaffingCalcKindDtoConfig : PerDtoConfig<StaffingCalcKindDto, StaffingCalcKind>
    {
        public override Action<IMappingExpression<StaffingCalcKind, StaffingCalcKindDto>> AlterReadMapping =>
              cfg => cfg
                .ForMember(x => x.CalculationKindName, x => x.MapFrom(ent => ent.CalculationKind.Translates.AsQueryable().FirstOrDefault(CalculationKindTranslate.GetExpr(TranslateColumn.full_name, ServiceProvider.CultureHelper.CurrentCulture.Id)).TranslateText ?? ent.CalculationKind.FullName))
            ;
    }
}
