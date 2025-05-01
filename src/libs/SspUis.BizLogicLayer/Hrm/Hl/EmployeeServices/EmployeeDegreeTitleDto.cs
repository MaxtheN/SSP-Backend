using GenericServices;
using SspUis.DataLayer.EfClasses.Hrm;
using SspUis.DataLayer.Repositories;

namespace SspUis.BizLogicLayer.Hrm;

public class EmployeeDegreeTitleDto : EmployeeDegreeTitleDlDto, ILinkToEntity<EmployeeDegreeTitle>
{
    public string DegreeTitle { get; set; }
}
