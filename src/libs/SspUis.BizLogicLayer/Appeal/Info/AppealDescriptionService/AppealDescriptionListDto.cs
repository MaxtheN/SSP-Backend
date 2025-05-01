using GenericServices;
using SspUis.DataLayer.EfClasses.Appeal;

namespace SspUis.BizLogicLayer.AppealDescriptionServices;

public class AppealDescriptionListDto : ILinkToEntity<AppealDescription>
{
    public int Id { get; set; }
    public string Code { get; set; } = null!;
    public string ShortName { get; set; } = null!;
    public string FullName { get; set; } = null!;
    public string Details { get; set; } = null!;
    public string State { get; set; } = null!;
    public string Parent { get; set; } = null!;
    public bool HasParent { get; set; }
}
