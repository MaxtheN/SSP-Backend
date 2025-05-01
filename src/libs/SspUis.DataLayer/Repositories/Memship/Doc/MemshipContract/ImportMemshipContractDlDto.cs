using Newtonsoft.Json;

namespace SspUis.DataLayer.Repositories.Memship;

public class ImportMemshipContractDlDto
{
    [JsonProperty("INN")]
    public string ContractorInn { get; set; }
    [JsonProperty("Email")]
    public string ContractorEmail { get; set; }
    [JsonProperty("PhoneNumber")]
    public string ContractorPhoneNumber { get; set; }
    [JsonProperty("AdditionalPhoneNumber")]
    public string ContractorAdditionalPhoneNumber { get; set; }
    [JsonProperty("ContractNumber")]
    public string DocNumber { get; set; }
    [JsonProperty("day")]
    public int Day { get; set; }
    [JsonProperty("month")]
    public int Month { get; set; }
    [JsonProperty("year")]
    public int Year { get; set; }
}

public class ImportYuridikMemshipContractDlDto
{
    [JsonProperty("tin")]
    public int ContractorInn { get; set; }

    [JsonProperty("name-2")]
    public string Email { get; set; }

    [JsonProperty("name-3")]
    public string Phone { get; set; }

    [JsonProperty("telegram")]
    public string Telegram { get; set; }

    [JsonProperty("contract_number")]
    public string DocNumber { get; set; }

    [JsonProperty("contract_date")]
    public string ContractDate { get; set; }

    [JsonProperty("Day")]
    public string Day { get; set; }

    [JsonProperty("Month")]
    public string Month { get; set; }

    [JsonProperty("Year")]
    public string Year { get; set; }
}

public class ImportJismoniyMemshipContractDlDto
{
    [JsonProperty("pinfl")]
    public string Pinfl { get; set; }

    [JsonProperty("email")]
    public string Email { get; set; }

    [JsonProperty("phone_number")]
    public string PhoneNumber { get; set; }

    [JsonProperty("telegram")]
    public string Telegram { get; set; }

    [JsonProperty("doc_number")]
    public string DocNumber { get; set; }

    [JsonProperty("contract_date")]
    public string ContractDate { get; set; }

    [JsonProperty("day")]
    public string Day { get; set; }

    [JsonProperty("month")]
    public string Month { get; set; }

    [JsonProperty("year")]
    public string Year { get; set; }
}
