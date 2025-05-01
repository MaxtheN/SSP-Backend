using GenericServices;
using Newtonsoft.Json;
using SspUis.DataLayer.EfClasses.Hrm;
using SspUis.DataLayer.Repositories.Hrm;

namespace SspUis.BizLogicLayer.Hrm;

public class EmployeeSendTrainTableDto : EmployeeSendTrainTableDlDto, ILinkToEntity<EmployeeSendTrainTable>
{
    [JsonIgnore]
    [System.Text.Json.Serialization.JsonIgnore]
    public int index { get; set; }  // for pdf print
    public string Department { get; set; }
    public string Employee { get; set; }
    public string Position { get; set; }
    public string? TableOrganization { get; set; }
    public int? TableOrganizationId { get; set; }

    public string? AnotherOrganization { get; set; }
    public string DocDetails { get; set; }
}
