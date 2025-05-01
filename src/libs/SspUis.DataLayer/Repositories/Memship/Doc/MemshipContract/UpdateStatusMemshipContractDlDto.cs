using SspUis.DataLayer.EfClasses;
using System;
using WEBASE.Attributes;
using WEBASE.EF;
using WEBASE.Models;

namespace SspUis.DataLayer.Repositories.Memship;

public class UpdateStatusMemshipContractDlDto : EntityDto<UpdateStatusMemshipContractDlDto, MemshipContract>, IHaveIdProp<long>
{
    [LocalizedRequired]
    [LocalizedRange(1, long.MaxValue)]
    public long Id { get; set; }
    [LocalizedRequired]
    public int StatusId { get; set; }
    public bool IsRead { get; set; }
    public string Message { get; set; }
    public string RejectMessage { get; set; }
    public DateTime RejectDate { get; set; } = DateTime.Now;
}
