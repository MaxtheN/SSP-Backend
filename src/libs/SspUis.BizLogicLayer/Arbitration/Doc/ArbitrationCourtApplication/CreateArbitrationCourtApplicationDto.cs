using System;

namespace SspUis.BizLogicLayer.ArbitrationCourtApplicationServices;

public class CreateArbitrationCourtApplicationDto
{
    public long Id { get; set; }
    public Guid Id2 { get; set; }
    public long ContractId { get; set; }
    public Guid ContractId2 { get; set; }
}
