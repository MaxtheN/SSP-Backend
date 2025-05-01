using GenericServices;
using SspUis.DataLayer.EfClasses.Hrm;
using SspUis.DataLayer.Repositories;

namespace SspUis.BizLogicLayer.Hrm;

public class EmployeeRelativeDto : EmployeeRelativeDlDto, ILinkToEntity<EmployeeRelative>
{
    public virtual string Nationality { get; set; }
    public virtual string RelativeDegree { get; set; }
    public virtual string Citizenship { get; set; }
    public virtual string Country { get; set; }
    public virtual string Region { get; set; }
    public virtual string District { get; set; }
}
