using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SspUis.Integration.OneId;

public class OneIdUserDataDto
{
    [JsonProperty("legal_info")]
    public List<LegalInfo> LegalInfo { get; set; }

    [JsonProperty("birth_date")]
    public string BirthDate { get; set; }

    [JsonProperty("ctzn")]
    public string Ctzn { get; set; }

    [JsonProperty("per_adr")]
    public string PerAdr { get; set; }

    [JsonProperty("tin")]
    public string Tin { get; set; }

    [JsonProperty("pport_issue_place")]
    public string PportIssuePlace { get; set; }

    [JsonProperty("sur_name")]
    public string SurName { get; set; }

    [JsonProperty("gd")]
    public string Gd { get; set; }

    [JsonProperty("natn")]
    public string Natn { get; set; }

    [JsonProperty("pport_issue_date")]
    public string PportIssueDate { get; set; }

    [JsonProperty("_pport_issue_date")]
    public string _PportIssueDate { get; set; }

    [JsonProperty("pport_expr_date")]
    public string PportExprDate { get; set; }

    [JsonProperty("_pport_expr_date")]
    public string _PportExprDate { get; set; }

    [JsonProperty("pport_no")]
    public string PportNo { get; set; }

    [JsonProperty("pin")]
    public string Pin { get; set; }

    [JsonProperty("mob_phone_no")]
    public string MobPhoneNo { get; set; }

    [JsonProperty("user_id")]
    public string UserId { get; set; }

    [JsonProperty("email")]
    public string Email { get; set; }

    [JsonProperty("birth_place")]
    public string BirthPlace { get; set; }

    [JsonProperty("mid_name")]
    public string MidName { get; set; }

    [JsonProperty("valid")]
    public string Valid { get; set; }

    [JsonProperty("user_type")]
    public string UserType { get; set; }

    [JsonProperty("sess_id")]
    public string SessId { get; set; }

    [JsonProperty("ret_cd")]
    public string RetCd { get; set; }

    [JsonProperty("first_name")]
    public string FirstName { get; set; }

    [JsonProperty("full_name")]
    public string FullName { get; set; }

}
public class LegalInfo
{
    [JsonProperty("le_tin")]
public string LeTin { get; set; }

    [JsonProperty("tin")]
    public string Tin { get; set; }

    [JsonProperty("le_name")]
    public string LeName { get; set; }

    [JsonProperty("acron_UZ")]
    public string AcronUZ { get; set; }

    [JsonProperty("is_basic")]
    public bool IsBasic { get; set; }

}
