

using Newtonsoft.Json;

namespace SspUis.Integration.Finance.Models;

public class FinanceLoginResponseDto
{

    [JsonProperty("filial")]
    public string Filial { get; set; }

    [JsonProperty("id")]
    public int Id { get; set; }

    [JsonProperty("deptlevel")]
    public int Deptlevel { get; set; }

    [JsonProperty("struct")]
    public string Struct { get; set; }

    [JsonProperty("staff")]
    public string Staff { get; set; }

    [JsonProperty("fio")]
    public string Fio { get; set; }

    [JsonProperty("login")]
    public string Login { get; set; }

    [JsonProperty("password")]
    public string Password { get; set; }

    [JsonProperty("phonenumber")]
    public string Phonenumber { get; set; }

    [JsonProperty("mobilenumber")]
    public string Mobilenumber { get; set; }

    [JsonProperty("dateopen")]
    public DateTime Dateopen { get; set; }

    [JsonProperty("dateexpire")]
    public DateTime? Dateexpire { get; set; }

    [JsonProperty("cnttries")]
    public int Cnttries { get; set; }

    [JsonProperty("action")]
    public int Action { get; set; }

    [JsonProperty("state")]
    public int State { get; set; }

    [JsonProperty("createdDate")]
    public DateTime CreatedDate { get; set; }

    [JsonProperty("createdBy")]
    public int CreatedBy { get; set; }

    [JsonProperty("modified_date")]
    public DateTime ModifiedDate { get; set; }

    [JsonProperty("modifiedBy")]
    public int ModifiedBy { get; set; }

    [JsonProperty("bankClientId")]
    public object BankClientId { get; set; }

    [JsonProperty("reason")]
    public string Reason { get; set; }

    [JsonProperty("tin")]
    public string Tin { get; set; }

    [JsonProperty("pinfl")]
    public string Pinfl { get; set; }

    [JsonProperty("authorities")]
    public object[] Authorities { get; set; }

    [JsonProperty("accountNonExpired")]
    public bool AccountNonExpired { get; set; }

    [JsonProperty("credentialsNonExpired")]
    public bool CredentialsNonExpired { get; set; }

    [JsonProperty("accountNonLocked")]
    public bool AccountNonLocked { get; set; }

    [JsonProperty("enabled")]
    public bool Enabled { get; set; }

    [JsonProperty("username")]
    public string Username { get; set; }
}
