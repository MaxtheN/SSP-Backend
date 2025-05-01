using GenericServices;
using SspUis.DataLayer.EfClasses.Hrm;
using WEBASE.Models;

namespace SspUis.BizLogicLayer.Hrm.PartisanshipServices;

public class PartisanshipListDto : ILinkToEntity<Partisanship>, IHaveIdProp<int>
{
    public int Id { get; set; }
    public string Code { get; set; }
    public string ShortName { get; set; }
    public string FullName { get; set; }
    public int StateId { get; set; }
    public string State { get; set; }
}
