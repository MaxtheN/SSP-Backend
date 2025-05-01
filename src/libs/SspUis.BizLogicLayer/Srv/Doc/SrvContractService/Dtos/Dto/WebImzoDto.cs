using System;

namespace SspUis.BizLogicLayer.Srv;

public class WebImzoDto
{
    public long Id { get; set; }
    public Guid Id2 { get; set; }
    public string DocNumber { get; set; }
    public DateOnly DocOn { get; set; }
    public int? OrganizationId { get; set; }
    public int StatusId { get; set; }
    public Guid WebImzoRequestId { get; set; }
    public string WebImzoSecretKey { get; set; }
}
