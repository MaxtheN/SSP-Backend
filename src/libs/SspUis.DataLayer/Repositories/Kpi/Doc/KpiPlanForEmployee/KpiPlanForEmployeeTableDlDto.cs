using AutoMapper;
using SspUis.DataLayer.EfClasses.Kpi;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WEBASE.EF;
using WEBASE.Models;

namespace SspUis.DataLayer.Repositories.Kpi;

public class KpiPlanForEmployeeTableDlDto : EntityDto<KpiPlanForEmployeeTableDlDto, KpiPlanForEmployeeTable>, IHaveIdProp<long>
{
    public long Id { get; set; }
    public int EmployeeManageId { get; set; }
    public List<KpiPlanEmployeeIndicatorCreateDlDto> Creates { get; set; }
    protected override Action<IMappingExpression<KpiPlanForEmployeeTableDlDto, KpiPlanForEmployeeTable>> AlterMapping =>
     cfg => cfg.ForMember(x => x.Creates, c => c.Ignore());
}
