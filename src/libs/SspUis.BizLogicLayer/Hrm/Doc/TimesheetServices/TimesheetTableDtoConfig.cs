using System;
using System.Linq;
using AutoMapper;
using GenericServices.Configuration;
using SspUis.DataLayer.EfClasses;

namespace SspUis.BizLogicLayer.Hrm;

public class TimesheetTableDtoConfig : PerDtoConfig<TimesheetTableDto, TimesheetTable>
{
    public override Action<IMappingExpression<TimesheetTable, TimesheetTableDto>> AlterReadMapping =>
        cfg => cfg
            .ForMember(d => d.Department, c => c.MapFrom(e => e.Department.FullName))
            .ForMember(d => d.Employee, c => c.MapFrom(e => e.Employee.Person.FullName))
            .ForMember(d => d.EmploymentType, c => c.MapFrom(e => e.EmploymentType.Translates.AsQueryable()
                .FirstOrDefault(DataLayer.EfClasses.Hrm.EmploymentTypeTranslate.GetExpr(
                    DataLayer.TranslateColumn.full_name,ServiceProvider.CultureHelper.CurrentCulture.Id))
                .TranslateText??e.EmploymentType.FullName))
            .ForMember(d => d.Position, c => c.MapFrom(e => e.Position.Translates.AsQueryable()
                .FirstOrDefault(DataLayer.EfClasses.PositionTranslate.GetExpr(
                    DataLayer.TranslateColumn.full_name,ServiceProvider.CultureHelper.CurrentCulture.Id))
                .TranslateText??e.Position.FullName))
            .ForMember(d => d.WorkSchedule, c => c.MapFrom(e => e.WorkSchedule.Translates.AsQueryable()
                .FirstOrDefault(DataLayer.EfClasses.Hrm.WorkScheduleTranslate.GetExpr(
                    DataLayer.TranslateColumn.full_name,ServiceProvider.CultureHelper.CurrentCulture.Id))
                .TranslateText??e.WorkSchedule.FullName))
        ;
}
