using System;
using AutoMapper;
using GenericServices.Configuration;
using SspUis.DataLayer.EfClasses.Hrm;

namespace SspUis.BizLogicLayer.Hrm;

public class OrderToSendBusinessTripTableDtoConfig : PerDtoConfig<OrderToSendBusinessTripTableDto, OrderToSendBusinessTripTable>
{
    public override Action<IMappingExpression<OrderToSendBusinessTripTable, OrderToSendBusinessTripTableDto>> AlterReadMapping =>
        cfg => cfg
            .ForMember(d => d.Country, c => c.MapFrom(e => e.Country.FullName))
            .ForMember(d => d.TableRegion, c => c.MapFrom(e => e.Region.FullName))
            .ForMember(d => d.Department, c => c.MapFrom(e => e.Department.FullName))
            .ForMember(d => d.BusinessTripType, c => c.MapFrom(e => e.BusinessTripType.FullName))
            .ForMember(d => d.Organization, c => c.MapFrom(e => e.Organization.FullName))
            .ForMember(d => d.Employee, c => c.MapFrom(e => e.Employee.Person.FullName))
            .ForMember(d => d.Position, c => c.MapFrom(e => e.EmployeeManage.Position.FullName))
            .ForMember(d => d.DocDetails, c => c.MapFrom(e => e.Details));
}
