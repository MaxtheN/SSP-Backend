using GenericServices;
using SspUis.DataLayer.EfClasses.Hrm;
using SspUis.DataLayer.Repositories.Hrm;
using Newtonsoft.Json;

namespace SspUis.BizLogicLayer.Hrm;

public class TempCalcKindTableDto : TempCalcKindTableDlDto, ILinkToEntity<TempCalcKindTable>
{
    [JsonIgnore]
    [System.Text.Json.Serialization.JsonIgnore]
    public int index { get; set; }
    public string Department { get; set; }
    public string Organization { get; set; }
    public string Employee { get; set; }
    public string Position { get; set; }
    public string DocDetails { get; set; }
    public decimal AmoutOfAidMoney { get; set; }
    public string ReasonForReceivingAid { get; set; }
}
