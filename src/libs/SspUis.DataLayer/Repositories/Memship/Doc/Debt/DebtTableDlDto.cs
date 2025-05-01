using GenericServices;
using SspUis.DataLayer.EfClasses;
using WEBASE.Attributes;
using WEBASE.EF;
using WEBASE.Models;

namespace SspUis.DataLayer.Repositories;

public class DebtTableDlDto : EntityDto<DebtTableDlDto, DebtTable>,
    IHaveIdProp<long>
{
    public long Id { get; set; }
    [LocalizedRequired]
    [LocalizedRange(1, long.MaxValue)]
    public long ContractorId { get; set; }
    public decimal? DebtAmount { get; set; }
    public decimal? EntitlementAmount { get; set; }
    [LocalizedRequired]
    [LocalizedRange(1, int.MaxValue)]
    public int ApplicationTypeId { get; set; }
}
