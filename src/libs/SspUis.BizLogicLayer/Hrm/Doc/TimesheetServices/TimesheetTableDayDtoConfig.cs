using System;
using System.Linq;
using AutoMapper;
using GenericServices.Configuration;
using SspUis.DataLayer.EfClasses;

namespace SspUis.BizLogicLayer.Hrm;

public class TimesheetTableDayDtoConfig : PerDtoConfig<TimesheetTableDayDto, TimesheetTableDay>
{
    public override Action<IMappingExpression<TimesheetTableDay, TimesheetTableDayDto>> AlterReadMapping =>
        cfg => cfg
            .ForMember(d => d.TimesheetIndicator, c => c.MapFrom(d => d.TimesheetIndicator.Translates.AsQueryable()
            .FirstOrDefault(TimesheetIndicatorTranslate.GetExpr(
                    DataLayer.TranslateColumn.full_name,
                    ServiceProvider.CultureHelper.CurrentCulture.Id))
            .TranslateText ?? d.TimesheetIndicator.FullName));
}
