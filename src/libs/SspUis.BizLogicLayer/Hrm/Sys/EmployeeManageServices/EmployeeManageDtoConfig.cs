using AutoMapper;
using GenericServices.Configuration;
using SspUis.DataLayer;
using SspUis.DataLayer.EfClasses;
using SspUis.DataLayer.EfClasses.Hrm;
using System;
using System.Linq;

namespace SspUis.BizLogicLayer.Hrm.EmployeeManageServices;

public class EmployeeManageDtoConfig : PerDtoConfig<EmployeeManageDto, EmployeeManage>
{
    public override Action<IMappingExpression<EmployeeManage, EmployeeManageDto>> AlterReadMapping => cfg => cfg
        .ForMember(x => x.Employee, x => x.MapFrom(ent => ent.Employee.Person.FullName))
        .ForMember(x => x.Department, x => x.MapFrom(ent => ent.Department.FullName))
        .ForMember(x => x.PositionClassificationId, x => x.MapFrom(ent => ent.Position.PositionClassificationId))
        .ForMember(x => x.Organization, x => x.MapFrom(ent => ent.Organization.FullName))
        .ForMember(x => x.Position, x => x.MapFrom(ent => ent.Position.Translates.AsQueryable()
           .FirstOrDefault(PositionTranslate.GetExpr(TranslateColumn.full_name, ServiceProvider.CultureHelper.CurrentCulture.Id)).TranslateText ?? ent.Position.FullName))
        .ForMember(x => x.Employee, x => x.MapFrom(ent => ent.Employee.Person.FullName))
        .ForMember(x => x.EmployeePinfl, x => x.MapFrom(ent => ent.Employee.Person.Pinfl))
        .ForMember(x => x.EmployeeBirthOn, x => x.MapFrom(ent => ent.Employee.Person.BirthDate))
        .ForMember(x => x.EmployeePhoneNumber, x => x.MapFrom(ent => ent.Employee.PhoneNumber))
        .ForMember(x => x.EmploymentTypeName, x => x.MapFrom(ent => ent.EmploymentType.Translates.AsQueryable()
           .FirstOrDefault(EmploymentTypeTranslate.GetExpr(TranslateColumn.full_name, ServiceProvider.CultureHelper.CurrentCulture.Id)).TranslateText ?? ent.EmploymentType.FullName))
        ;
}
