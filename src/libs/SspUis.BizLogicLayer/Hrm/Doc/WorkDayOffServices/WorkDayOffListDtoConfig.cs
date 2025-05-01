using System;
using System.Linq;
using AutoMapper;
using GenericServices.Configuration;
using SspUis.DataLayer.EfClasses;
using SspUis.DataLayer.EfClasses.Hrm;

namespace SspUis.BizLogicLayer.Hrm;

public class WorkDayOffListDtoConfig : PerDtoConfig<WorkDayOffListDto, WorkDayOff>
{
    public override Action<IMappingExpression<WorkDayOff, WorkDayOffListDto>> AlterReadMapping =>
        cfg => cfg
            .ForMember(d => d.Status, c => c.MapFrom(e => e.Status.Translates.AsQueryable()
               .FirstOrDefault(StatusTranslate.GetExpr(DataLayer.TranslateColumn.full_name, ServiceProvider.CultureHelper.CurrentCulture.Id)).TranslateText ?? e.Status.FullName))
            .ForMember(d => d.Employee, c => c.MapFrom(e => e.Tables.Select(t => t.Employee.Person.FullName).FirstOrDefault()));
}
