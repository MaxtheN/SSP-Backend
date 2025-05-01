using GenericServices;
using SspUis.DataLayer.EfClasses.Hrm;
using SspUis.DataLayer.Repositories;

namespace SspUis.BizLogicLayer.Hrm;

public class EmployeeScientificDegreeDto : EmployeeScientificDegreeDlDto, ILinkToEntity<EmployeeScientificDegree>
{
    public string ScientificDegree { get; set; }
}
