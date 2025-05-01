using GenericServices;
using SspUis.DataLayer.EfClasses;
using WEBASE.Models;

namespace SspUis.BizLogicLayer.Hrm.SettlementAccountSourceServices;

public class SettlementAccountSourceListDto : ILinkToEntity<SettlementAccountSource>, IHaveIdProp<int>
{
    public int Id { get; set; }
    public string Code { get; set; }
    public string ShortName { get; set; }
    public string FullName { get; set; }
    public int StateId { get; set; }
    public string State { get; set; }
    public string Code1 { get; set; }
    public string Code2 { get; set; }
    public string Code3 { get; set; }
    public string Code4 { get; set; }
    public int? ParentId { get; set; }
    public string? Parent { get; set; }
}
