using System;

namespace SspUis.BizLogicLayer;
public class MemshipCertificateForPdf
{
    public string? DocNumber { get; set; } = string.Empty;
    public int? ContractorId { get; set; }
    public int? MemshipContractId { get; set; }
    public DateOnly? DocOn { get; set; } = DateTime.Now.AsDateOnly();

    //[JsonIgnore]
    public string? Region { get; set; } = string.Empty;
    //[JsonIgnore]
    public string? OrgName { get; set; } = string.Empty;
    //[JsonIgnore]
    public string? ActivityType { get; set; } = string.Empty;
    //[JsonIgnore]
    public string? Inn { get; set; } = string.Empty;
}
