using AutoMapper;
using GenericServices.Configuration;
using SspUis.DataLayer;
using SspUis.DataLayer.EfClasses;
using SspUis.DataLayer.EfClasses.Hrm;
using System;
using System.Linq;

namespace SspUis.BizLogicLayer.Hrm.EmployeeManageServices
{
    public class EmployeeManageListDtoConfig : PerDtoConfig<EmployeeManageListDto, EmployeeManage>
    {
        public override Action<IMappingExpression<EmployeeManage, EmployeeManageListDto>> AlterReadMapping => cfg => cfg
            .ForMember(x => x.Department, x => x.MapFrom(ent => ent.Department.FullName))
            .ForMember(x => x.EmpAppointOrderType, x => x.MapFrom(ent => ent.EmpAppointOrderType.Translates.AsQueryable()
                .FirstOrDefault(EmpAppointOrderTypeTranslate.GetExpr(TranslateColumn.full_name, ServiceProvider.CultureHelper.CurrentCulture.Id)).TranslateText ?? ent.EmpAppointOrderType.FullName))
            .ForMember(x => x.Employee, x => x.MapFrom(ent => ent.Employee.Person.FullName))
            .ForMember(x => x.EmployeePhoneNumber, x => x.MapFrom(ent => ent.Employee.PhoneNumber))
            .ForMember(x => x.EmployeePinfl, x => x.MapFrom(ent => ent.Employee.Person.Pinfl))
            .ForMember(x => x.EmploymentType, x => x.MapFrom(ent => ent.EmploymentType.Translates.AsQueryable()
                .FirstOrDefault(EmploymentTypeTranslate.GetExpr(TranslateColumn.full_name, ServiceProvider.CultureHelper.CurrentCulture.Id)).TranslateText ?? ent.EmploymentType.FullName))
            .ForMember(x => x.Organization, x => x.MapFrom(ent => ent.Organization.Translates.AsQueryable()
                .FirstOrDefault(OrganizationTranslate.GetExpr(TranslateColumn.full_name, ServiceProvider.CultureHelper.CurrentCulture.Id)).TranslateText ?? ent.Organization.FullName))
            .ForMember(x => x.Position, x => x.MapFrom(ent => ent.Position.Translates.AsQueryable()
                .FirstOrDefault(PositionTranslate.GetExpr(TranslateColumn.full_name, ServiceProvider.CultureHelper.CurrentCulture.Id)).TranslateText ?? ent.Position.FullName))
            .ForMember(x => x.WorkSchedule, x => x.MapFrom(ent => ent.WorkSchedule.Translates.AsQueryable()
                .FirstOrDefault(WorkScheduleTranslate.GetExpr(TranslateColumn.full_name, ServiceProvider.CultureHelper.CurrentCulture.Id)).TranslateText ?? ent.WorkSchedule.FullName))
            .ForMember(x => x.OrganizationId, x => x.MapFrom(ent => ent.OrganizationId))
            .ForMember(x => x.GenderId, x => x.MapFrom(ent => ent.Employee.Person.GenderId))
            .ForMember(x => x.Gender, x => x.MapFrom(ent => ent.Employee.Person.Gender.Translates.AsQueryable()
                .FirstOrDefault(GenderTranslate.GetExpr(TranslateColumn.full_name, ServiceProvider.CultureHelper.CurrentCulture.Id)).TranslateText ?? ent.Employee.Person.Gender.FullName));
    }
}
