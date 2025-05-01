using AutoMapper;
using GenericServices.Configuration;
using SspUis.Core;
using SspUis.Core.Security;
using SspUis.DataLayer;
using SspUis.DataLayer.EfClasses;
using SspUis.DataLayer.EfClasses.Hrm;
using System;
using System.Linq;

namespace SspUis.BizLogicLayer.Hrm;

public class EmployeeMissedDayListDtoConfig : PerDtoConfig<EmployeeMissedDayListDto, EmployeeMissedDay>
{
	public override Action<IMappingExpression<EmployeeMissedDay, EmployeeMissedDayListDto>> AlterReadMapping =>
		cfg => cfg
			.ForMember(x => x.Status, x => x.MapFrom(ent => ent.Status.Translates.AsQueryable()
				.FirstOrDefault(StatusTranslate.GetExpr(TranslateColumn.full_name, ServiceProvider.CultureHelper.CurrentCulture.Id)).TranslateText ?? ent.Status.FullName))
			.ForMember(x => x.Details, x => x.MapFrom(ent => string.Join("\n", ent.Tables.Select(t => $"{t.StartAt} - {t.EndAt}"))))
			.ForMember(x => x.Department, x => x.MapFrom(ent => ent.Department.ShortName))
			.ForMember(x => x.EmployeesId, x => x.MapFrom(ent => ent.Tables.Select(a => a.EmployeeManage.EmployeeId).ToArray()))
			.ForMember(x => x.EmployeeFullNames, x => x.MapFrom(ent => ent.Tables.Select(et => et.EmployeeManage.Employee.Person.FullName)))
		.ForMember(x => x.CanCancel, x => x.MapFrom(ent => ServiceProvider.AuthService
				.HasPermission(ModuleCode.EmployeeMissedDayCancelApprove) && 
					StatusIdConst.CanApplyEmployeeMissedDays(ent.StatusId, StatusIdConst.NOT_ACCEPTED, null) ? true : false))
			  .ForMember(x => x.CanAccept, x => x.MapFrom(ent => ServiceProvider.AuthService
						.HasPermission(ModuleCode.EmployeeMissedDayApprove) && 
							StatusIdConst.CanApplyEmployeeMissedDays(ent.StatusId, StatusIdConst.APPROVED, null) ? true : false))
			 .ForMember(x => x.CanEdit, x => x.MapFrom(ent => ServiceProvider.AuthService
						 .HasPermission(ModuleCode.EmployeeMissedDayEdit) ? StatusIdConst
								.CanApplyEmployeeMissedDays(ent.StatusId, StatusIdConst.MODIFIED, null) : false))
			 .ForMember(x => x.CanDelete, x => x.MapFrom(ent => ServiceProvider.AuthService
						 .HasPermission(ModuleCode.EmployeeMissedDayDelete) ? StatusIdConst
						.CanApplyEmployeeMissedDays(ent.StatusId, StatusIdConst.DELETED, null) : false));
}
