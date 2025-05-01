
using Newtonsoft.Json;

namespace SspUis.DataLayer.Repositories.Memship;

public class ImportMemshipCertificateDlDto
{
    [JsonProperty("doc_on")]
    public string? DocOn { get; set; }

    [JsonProperty("doc_day")]
    public string? DocDay { get; set; }

    [JsonProperty("doc_month")]
    public string? DocMonth { get; set; }

    [JsonProperty("doc_year")]
    public string? DocYear { get; set; }

    [JsonProperty("doc_number")]
    public string? DocNumber { get; set; }

    [JsonProperty("expire_on")]
    public string? ExpireOn { get; set; }

    [JsonProperty("expire_day")]
    public string? ExpireDay { get; set; }

    [JsonProperty("expire_month")]
    public string? ExpireMonth { get; set; }

    [JsonProperty("expire_year")]
    public string? ExpireYear { get; set; }
}
public class ImportYurMemshipCertificateDlDto : ImportMemshipCertificateDlDto
{
    [JsonProperty("inn")]
    public long? Inn { get; set; }
}
public class ImportJISMemshipCertificateDlDto : ImportMemshipCertificateDlDto
{
    [JsonProperty("pinfl")]
    public long? Pinfl { get; set; }
}