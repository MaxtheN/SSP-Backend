using GenericServices;
using SspUis.DataLayer.EfClasses.DualEdu;
using WEBASE.Models;

namespace SspUis.BizLogicLayer;

public class SpecialtyBillingListDto : ILinkToEntity<SpecialtyBilling>, IHaveIdProp<int>
{
    public int Id { get; set; }
    public string Organization { get; set; }
    public string Code { get; set; }
    public string ShortName { get; set; }
    public string FullName { get; set; }
    public string Faculty { get; set; }
    public string FacultyCode { get; set; }
}