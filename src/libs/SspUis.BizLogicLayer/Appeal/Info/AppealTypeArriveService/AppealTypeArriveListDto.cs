using GenericServices;
using SspUis.DataLayer.EfClasses.Appeal;

namespace SspUis.BizLogicLayer.AppealTypeArriveServices;

public class AppealTypeArriveListDto : ILinkToEntity<AppealTypeArrive>
{
    public int Id { get; set; }
    public string Code { get; set; } = null!;
    public string ShortName { get; set; } = null!;
    public string FullName { get; set; } = null!;
    public string Details { get; set; } = null!;
    public string State { get; set; } = null!;
}
