using System;
using System.Linq;

using AutoMapper;
using GenericServices.Configuration;
using SspUis.DataLayer.EfClasses;

using SspUis.DataLayer;
using SspUis.DataLayer.EfClasses.Hrm;
using System.Linq.Dynamic.Core;

namespace SspUis.BizLogicLayer.Hrm;

public class AppointEmployeeTableDtoConfig : PerDtoConfig<AppointEmployeeTableDto, AppointEmployeeTable>
{
    public override Action<IMappingExpression<AppointEmployeeTable, AppointEmployeeTableDto>> AlterReadMapping =>
        cfg => cfg
            .ForMember(x => x.EmpAppointOrderType, x => x.MapFrom(ent => ent.EmpAppointOrderType.Translates.AsQueryable().FirstOrDefault(EmpAppointOrderTypeTranslate.GetExpr(TranslateColumn.full_name, ServiceProvider.CultureHelper.CurrentCulture.Id)).TranslateText ?? ent.EmpAppointOrderType.FullName))
                .ForMember(x => x.Organization, x => x.MapFrom(ent => ent.Owner.Organization != null ? ent.Owner.Organization.Translates.AsQueryable()
			                    	.FirstOrDefault(OrganizationTranslate.GetExpr(TranslateColumn.full_name, ServiceProvider.CultureHelper.CurrentCulture.Id)).TranslateText ?? ent.Owner.Organization.FullName: ""))

				.ForMember(x => x.Department, x => x.MapFrom(ent => ent.Department != null ? ent.Department.Translates.AsQueryable().FirstOrDefault(DepartmentTranslate.GetExpr(TranslateColumn.full_name, ServiceProvider.CultureHelper.CurrentCulture.Id)).TranslateText ?? ent.Department.FullName : ""))
                .ForMember(x => x.Position, x => x.MapFrom(ent => ent.Position != null ? ent.Position.Translates.AsQueryable().FirstOrDefault(PositionTranslate.GetExpr(TranslateColumn.full_name, ServiceProvider.CultureHelper.CurrentCulture.Id)).TranslateText ?? ent.Position.FullName : ""))
                .ForMember(x => x.EmployeeFull, x => x.MapFrom(ent => ent.Employee.Person.FullName))
                .ForMember(x => x.MessageFromDoc, x => x.MapFrom(ent => ent.Owner.Details))
                .ForMember(x => x.ChoosenEmployee, x => x.MapFrom(ent => ent.ChoosenEmployeeManage.Employee.Person.FullName))
                .ForMember(x => x.DetailForPrint, x => x.MapFrom(ent => ent.Details))
                .ForMember(x => x.EmploymentType, x => x.MapFrom(ent => ent.EmploymentType != null ? ent.EmploymentType.Translates.AsQueryable().FirstOrDefault(EmploymentTypeTranslate.GetExpr(TranslateColumn.full_name, ServiceProvider.CultureHelper.CurrentCulture.Id)).TranslateText ?? ent.EmploymentType.FullName : ""))
                .ForMember(x => x.WorkSchedule, x => x.MapFrom(ent => ent.WorkSchedule != null ? ent.WorkSchedule.Translates.AsQueryable().FirstOrDefault(WorkScheduleTranslate.GetExpr(TranslateColumn.full_name, ServiceProvider.CultureHelper.CurrentCulture.Id)).TranslateText ?? ent.WorkSchedule.FullName : ""))
                .ForMember(x => x.FromDepartment, x => x.MapFrom(ent => ent.FromDepartment != null ? ent.FromDepartment.FullName : ""))
                .ForMember(x => x.FromPosition, x => x.MapFrom(ent => ent.FromPosition != null ? ent.FromPosition.Translates.AsQueryable().FirstOrDefault(PositionTranslate.GetExpr(TranslateColumn.full_name, ServiceProvider.CultureHelper.CurrentCulture.Id)).TranslateText ?? ent.FromPosition.FullName : ""));
}
