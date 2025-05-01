using SspUis.DataLayer.EfClasses;
using System;
using WEBASE.Attributes;
using WEBASE.EF;

namespace SspUis.DataLayer.Repositories;

public class DualContractSignDlDto<TDto> : EntityDto<TDto, DualContractSign>
    where TDto : DualContractSignDlDto<TDto>
{
    public long OwnerId { get; set; }
    public Guid SignFile { get; set; }
    public Guid DataFile { get; set; }
    [LocalizedRequired]
    [LocalizedStringLength(500)]
    public string SignedUserInfo { get; set; }
    public DateTime? SignedAt { get; set; }
}