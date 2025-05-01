using System;
using System.Linq;
using AutoMapper;
using GenericServices.Configuration;
using SspUis.DataLayer.EfClasses;
using SspUis.DataLayer;
using SspUis.DataLayer.EfClasses.Hrm;

namespace SspUis.BizLogicLayer.Hrm;

public class TempCalcKindTableDtoConfig : PerDtoConfig<TempCalcKindTableDto, TempCalcKindTable>
{
    public override Action<IMappingExpression<TempCalcKindTable, TempCalcKindTableDto>> AlterReadMapping =>
        cfg => cfg
            .ForMember(d => d.Department, c => c.MapFrom(e => e.Department.FullName))
            .ForMember(d => d.Organization, c => c.MapFrom(e => e.Owner.Organization.Translates.AsQueryable()
				.FirstOrDefault(OrganizationTranslate.GetExpr(TranslateColumn.full_name, ServiceProvider.CultureHelper.CurrentCulture.Id)).TranslateText ?? e.Owner.Organization.FullName))

			.ForMember(d => d.Employee, c => c.MapFrom(e => e.Employee.Person.FullName))
            .ForMember(d => d.Position, c => c.MapFrom(e => e.EmployeeManage.Position.FullName))
            .ForMember(d => d.ReasonForReceivingAid, c => c.MapFrom(e => e.Owner.CalculationKind.FullName))
            .ForMember(d => d.AmoutOfAidMoney, c => c.MapFrom(e => e.Amount)) ;
}
