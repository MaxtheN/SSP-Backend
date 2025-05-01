using GenericServices;
using SspUis.DataLayer.EfClasses.Hrm;
namespace SspUis.BizLogicLayer.Hrm;

public class DepartmentListDto : ILinkToEntity<Department>
{
    public int Id { get; set; }
    public string OrderCode { get; set; }
    public long Code { get; set; }
    public string ShortName { get; set; }
    public string FullName { get; set; }
    public int StateId { get; set; }
    public string State { get; set; }
    public string Parent { get; set; }
    public string Organization { get; set; }
    public string IndicatorDepartment { get; set; }
}
