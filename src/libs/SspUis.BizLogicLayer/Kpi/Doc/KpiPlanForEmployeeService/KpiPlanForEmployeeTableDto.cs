using GenericServices;
using SspUis.DataLayer.EfClasses.Kpi;
using SspUis.DataLayer.Repositories.Kpi;
using System.Collections.Generic;


namespace SspUis.BizLogicLayer.Kpi;

public class KpiPlanForEmployeeTableDto : KpiPlanForEmployeeTableDlDto, ILinkToEntity<KpiPlanForEmployeeTable>
{
    public long Id { get; set; }
    public long EmployeeManageId { get; set; }
    public string EmployeeManage { get; set; }
    public string DepartmentCode { get; set; }
    public string Department { get; set; }
    public int DepartmentId { get; set; }
    public List<KpiPlanEmployeeIndicatorCreateDto> Creates { get; set; }
}
