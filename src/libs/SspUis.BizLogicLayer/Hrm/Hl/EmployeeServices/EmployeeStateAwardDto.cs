using GenericServices;
using SspUis.DataLayer.EfClasses.Hrm;
using SspUis.DataLayer.Repositories;

namespace SspUis.BizLogicLayer.Hrm;

public class EmployeeStateAwardDto : EmployeeStateAwardDlDto, ILinkToEntity<EmployeeStateAward>
{
    public string StateAwards { get; set; }
}
