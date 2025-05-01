using System;
using System.Collections.Generic;
using GenericServices;
using SspUis.DataLayer.EfClasses;
using SspUis.DataLayer.Repositories;

namespace SspUis.BizLogicLayer.Memship
{
    public class ChangeContractToPayedDto
    {
        public long Id { get; set; }
        public string DocNumber { get; set; }
        public string? Details { get; set; }
        public decimal BaseFixedMinimumValue { get; set; }
        public long? OrganizationSettlementAccountId { get; set; }
        public int? RegionalOrganizationId { get; set; }
    }
}
