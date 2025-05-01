using GenericServices;
using SspUis.DataLayer.EfClasses.Hrm;
using SspUis.DataLayer.Repositories.Hrm;
using Newtonsoft.Json;

namespace SspUis.BizLogicLayer.Hrm;

public class RecallLeaveTableDto : RecallLeaveTableDlDto, ILinkToEntity<RecallLeaveTable>
{
    [JsonIgnore]
    [System.Text.Json.Serialization.JsonIgnore]
    public int index { get; set; }
    public string Department { get; set; }
    public string Employee { get; set; }
    public int EmployeeId { get; set; }
    public EmployeeLeaveOrderTableDto EmployeeLeaveOrderTable { get; set; }
}
