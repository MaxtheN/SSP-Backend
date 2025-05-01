using System;
using System.Linq;
using AutoMapper;
using GenericServices.Configuration;
using SspUis.DataLayer.EfClasses;
using SspUis.DataLayer.EfClasses.Kpi;
using SspUis.DataLayer;
using System.Linq.Dynamic.Core;

namespace SspUis.BizLogicLayer;

public class KpiRatingEmployeeTableDtoConfig : PerDtoConfig<KpiRatingEmployeeTableDto, KpiRatingEmployeeTable>
{
    public override Action<IMappingExpression<KpiRatingEmployeeTable, KpiRatingEmployeeTableDto>> AlterReadMapping =>
        cfg => cfg
                     .ForMember(x => x.EmployeeManage, x => x.MapFrom(ent => ent.EmployeeManage.Employee.Person.FullName));
}
