using GenericServices;
using SspUis.DataLayer.EfClasses.Hrm;
using WEBASE.Models;

namespace SspUis.BizLogicLayer.Hrm.LevelCodeServices;

public class LevelCodeListDto : ILinkToEntity<LevelCode>, IHaveIdProp<int>
{
    public int Id { get; set; }
    public string Code { get; set; }
    public string ShortName { get; set; }
    public string FullName { get; set; }
    public int StateId { get; set; }
    public string State { get; set; }
    public string FirstSignPosition { get; set; }
    public string SecondSignPosition { get; set; }
    public string FirstSignOrganization { get; set; }
    public string SecondSignOrganization { get; set; }
}
