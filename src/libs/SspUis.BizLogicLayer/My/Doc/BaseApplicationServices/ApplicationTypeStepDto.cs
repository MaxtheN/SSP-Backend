using GenericServices;
using SspUis.DataLayer.EfClasses;

namespace SspUis.BizLogicLayer;

public class ApplicationTypeStepDto : ILinkToEntity<ApplicationTypeStep>
{
    public int Id { get; set; }
    public string OrderCode { get; set; }
    public string Code { get; set; }
    public string FullName { get; set; }
    public string ShortName { get; set; }
    public int ApplicationTypeId { get; set; }
    public string ApplicationType { get; set; }
    public int StateId { get; set; }
    public string State { get; set; }
}
