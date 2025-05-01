using AutoMapper;
using GenericServices.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SspUis.DataLayer.EfClasses.Hrm;

namespace SspUis.BizLogicLayer.Hrm
{
    public class EmployeeMissedDayTableDtoConfig : PerDtoConfig<EmployeeMissedDayTableDto, EmployeeMissedDayTable>
    {
        public override Action<IMappingExpression<EmployeeMissedDayTable, EmployeeMissedDayTableDto>>
            AlterReadMapping =>
            cfg => cfg
                .ForMember(x => x.Employee, x => x.MapFrom(ent => ent.EmployeeManage.Employee.Person.FullName))
                .ForMember(x => x.MissedDaysType, x => x.MapFrom(ent => ent.MissedDaysType.ShortName));
    }
}
