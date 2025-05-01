using AutoMapper;
using GenericServices.Configuration;
using SspUis.DataLayer.EfClasses.Kpi;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SspUis.BizLogicLayer.Kpi;

public class KpiPlanForEmployeeTableDtoConfig
: PerDtoConfig<KpiPlanForEmployeeTableDto, KpiPlanForEmployeeTable>
{
    public override Action<IMappingExpression<KpiPlanForEmployeeTable, KpiPlanForEmployeeTableDto>> AlterReadMapping =>
        cfg => cfg
                     .ForMember(x => x.EmployeeManage, x => x.MapFrom(ent => ent.EmployeeManage.Employee.Person.FullName))
                     .ForMember(x => x.EmployeeManageId, x => x.MapFrom(ent => ent.EmployeeManage.Id))
                     .ForMember(x => x.Creates, x => x.MapFrom(ent => ent.Creates))
                     .ForMember(x => x.DepartmentCode,x => x.MapFrom(ent => ent.EmployeeManage.Department.Code))
                     .ForMember(x => x.DepartmentId, x => x.MapFrom(ent => ent.EmployeeManage.Department.Id))
                     .ForMember(s => s.Department,s => s.MapFrom(ent => ent.EmployeeManage.Department.FullName));
                     
     
}
