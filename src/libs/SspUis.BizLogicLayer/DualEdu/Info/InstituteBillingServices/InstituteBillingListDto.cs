using GenericServices;
using SspUis.DataLayer.EfClasses.DualEdu;
using WEBASE.Models;

namespace SspUis.BizLogicLayer;

public class InstituteBillingListDto : ILinkToEntity<InstituteBilling>, IHaveIdProp<int>
{
    public int Id { get; set; }
    public string OrderCode { get; set; }
    public string ShortName { get; set; }
    public string FullName { get; set; }
    public string Inn { get; set; }
    public string Region { get; set; }
    public string District { get; set; }
    public string HemisExternalCode { get; set; }
    public string? Address { get; set; }
}