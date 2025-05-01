using System;
using GenericServices;
using SspUis.DataLayer.EfClasses;
using WEBASE.Models;

namespace SspUis.BizLogicLayer.Memship;

public class DebtListDto : ILinkToEntity<Debt>, IHaveIdProp<long>
{
    public long Id { get; set; }
    public DateOnly DocOn { get; set; }
    public string DocNumber { get; set; }
    public int OrganizationId { get; set; }
    public int StatusId { get; set; }
    public string Organization { get; set; }
    public string Status { get; set; }
    public decimal? TotalDebtAmount { get; set; }
    public decimal? TotalEntitlementAmount { get; set; }
}
