using GenericServices;
using SspUis.DataLayer.EfClasses.Hrm;
using SspUis.DataLayer.Repositories;

namespace SspUis.BizLogicLayer.Hrm;

public class EmployeePartisanshipDto : EmployeePartisanshipDlDto, ILinkToEntity<EmployeePartisanship>
{
    public string Partisanship { get; set; }
}
