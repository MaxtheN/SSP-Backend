using System;

namespace SspUis.BizLogicLayer.Memship;

public class MemshipContractWebImzoDto
{
    public long Id { get; set; }
    public Guid Id2 { get; set; }
    public string DocNumber { get; set; }
    public DateOnly DocOn { get; set; }
    public int OrganizationId { get; set; }
    public int StatusId { get; set; }
    public int EduYearId { get; set; }
    public int? ProtocolStepId { get; set; }
    public Guid WebImzoRequestId { get; set; }
    public string WebImzoSecretKey { get; set; }
}
