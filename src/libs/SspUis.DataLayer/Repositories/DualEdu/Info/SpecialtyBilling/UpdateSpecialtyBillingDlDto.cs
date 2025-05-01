using WEBASE.Attributes;
using WEBASE.Models;

namespace SspUis.DataLayer.Repositories;

public class UpdateSpecialtyBillingDlDto : SpecialtyBillingDlDto<UpdateSpecialtyBillingDlDto>, IHaveIdProp<int>
{
    [LocalizedRequired]
    [LocalizedRange(1, int.MaxValue)]
    public int Id { get; set; }
}