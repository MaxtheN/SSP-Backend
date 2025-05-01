using System;
using System.Linq;
using AutoMapper;
using GenericServices.Configuration;
using OpenXmlPowerTools;
using SspUis.DataLayer.EfClasses;
using SspUis.DataLayer;
using SspUis.DataLayer.EfClasses.Hrm;

namespace SspUis.BizLogicLayer.Hrm;

public class EmployeeSendTrainTableDtoConfig : PerDtoConfig<EmployeeSendTrainTableDto, EmployeeSendTrainTable>
{
    public override Action<IMappingExpression<EmployeeSendTrainTable, EmployeeSendTrainTableDto>> AlterReadMapping =>
        cfg => cfg
            .ForMember(d => d.Department, c => c.MapFrom(e => e.Department.FullName))
            .ForMember(d => d.TableOrganization, c => c.MapFrom(e => e.Organization.Translates.AsQueryable()
				.FirstOrDefault(OrganizationTranslate.GetExpr(TranslateColumn.full_name, ServiceProvider.CultureHelper.CurrentCulture.Id)).TranslateText ?? e.Organization.FullName))
			.ForMember(d => d.Employee, c => c.MapFrom(e => e.Employee.Person.FullName))
            .ForMember(d => d.Position, c => c.MapFrom(e => e.Employee.EmployeeManage.Position.FullName))
            .ForMember(d => d.DocDetails, c => c.MapFrom(e => e.Details))
            .ForMember(d => d.AnotherOrganization, c => c.MapFrom(e => e.AnotherOrganization))
            .ForMember(d => d.TableOrganizationId, c => c.MapFrom(e => e.OrganizationId));
}
