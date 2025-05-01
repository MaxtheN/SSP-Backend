using GenericServices;
using SspUis.DataLayer.EfClasses.Kpi;
using SspUis.DataLayer.Repositories;
using System.Collections.Generic;

namespace SspUis.BizLogicLayer;

public class KpiRatingEmployeeTableDto : KpiRatingEmployeeTableDlDto, ILinkToEntity<KpiRatingEmployeeTable>
{
    public long Id { get; set; }
    public long EmployeeManageId { get; set; }
    public string EmployeeManage { get; set; }
    public List<KpiRatingEmployeePointDto> Points { get; set; } = new();
}
