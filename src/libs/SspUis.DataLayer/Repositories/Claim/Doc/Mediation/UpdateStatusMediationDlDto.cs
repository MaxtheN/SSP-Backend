using SspUis.DataLayer.EfClasses.Claim;
using WEBASE.Attributes;
using WEBASE.EF;
using WEBASE.Models;

namespace SspUis.DataLayer.Repositories.Claim;

public class UpdateStatusMediationDlDto : EntityDto<UpdateStatusMediationDlDto, Mediation>, IHaveIdProp<long>, IHaveStatusId
{
    [LocalizedRange(1, long.MaxValue)]
    public long Id { get; set; }


    [LocalizedRange(1, int.MaxValue)]
    public int StatusId { get; set; }
}
