using System;

namespace SspUis.DataLayer.Repositories
{
    public class ContractorOfferDto
    {
        public long OfferId { get; set; }
        public long ContractorId { get; set; }
        public Guid SignData { get; set; }
    }
}
