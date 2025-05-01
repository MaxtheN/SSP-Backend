using GenericServices;
using SspUis.DataLayer.EfClasses.Hrm;
using SspUis.DataLayer.Repositories.Hrm;

namespace SspUis.BizLogicLayer.Hrm;

public class ChastisementTableDto : ChastisementTableDlDto, ILinkToEntity<ChastisementTable>
{
    public int index { get; set; }
    public string Department { get; set; } = null!;
    public string Employee { get; set; } = null!;
    public string EmployeeManage { get; set; } = null!;
    public int DepartmentId { get; set; }
    public int EmployeeId { get; set; }
    public string DetailPrint { get; set; }
    public string Position { get; set; }
    public string DetailForPrint { get; set; }
    public decimal EmployeeRate { get; set; }
    public bool IsBlocked { get; set; }
}
