using GenericServices;
using SspUis.DataLayer.EfClasses.Hrm;
using SspUis.DataLayer.Repositories;

namespace SspUis.BizLogicLayer.Hrm;

public class DepartmentDto : UpdateDepartmentDlDto, ILinkToEntity<Department>
{
    public string State { get; set; }
    public string Parent { get; set; }
    public string Organization { get; set; }
    public string IndicatorDepartment { get; set; }
}
