using System;
using SspUis.DataLayer.EfClasses.Corruption;
using WEBASE.Attributes;
using WEBASE.EF;
using WEBASE.Models;

namespace SspUis.DataLayer.Repositories.Corruption;

public class JoinAntiCorruptionResultTableDlDto : EntityDto<JoinAntiCorruptionResultTableDlDto, JoinAntiCorruptionResultTable> ,IHaveIdProp<long>
{
    public long Id { get; set; }
    [LocalizedRequired]
    [LocalizedRange(1, long.MaxValue)]
    public long ApplicationId { get; set; }
    [LocalizedRequired]
    [LocalizedRange(1, int.MaxValue)]
    public int JoinAntiCorruptionResultTypeId { get; set; }
    [LocalizedStringLength(50)]
    public string CorruptionCertificateNumber { get; set; }
    public DateOnly? CorruptionCertificateOn { get; set; }
    public DateOnly? CorruptionCertificateExpireOn { get; set; }
}
