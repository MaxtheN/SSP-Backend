using System.Linq;
using WEBASE.Models;

namespace SspUis.BizLogicLayer;

public static class EmployeeSelectList
{
    public static PagedSelectList<int> AsSelectList(this PagedResult<EmployeeListDto> pagedResult)
    {
        return new PagedSelectList<int>(
            pagedResult,
            pagedResult
                .Rows
                .Select(a => new EmployeeSelectListItem<int>
                {
                    Value = a.Id,
                    Text = a.FullName,
                    PassportInfo = a.PassportInfo,
                    OrganizatonId = a.OrganizationId,
                    Organization = a.Organization,
                    Region = a.Region,
                    District = a.District,
                }));
    }

}

public class EmployeeSelectListItem<T> : SelectListItem<T>
{
    public string PassportInfo { get; set; }
    public int? OrganizatonId { get; set; }
    public string Organization { get; set; }
    public string Position { get; set; }
    public string Region { get; set; }
    public string District { get; set; }
}
