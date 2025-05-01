using SspUis.DataLayer.EfClasses;
using WEBASE.Attributes;
using WEBASE.EF;
using WEBASE.Models;

namespace SspUis.DataLayer.Repositories;

public class ArbitrationCourtApplicationSignerDlDto : EntityDto<ArbitrationCourtApplicationSignerDlDto, ArbitrationCourtApplicationSigner>, IHaveIdProp<long>
{
    public long Id { get; set; }
    public int? SignOrder { get; set; }
    [LocalizedRequired]
    [LocalizedRange(1, int.MaxValue)]
    public int ArbitrationJudgeId { get; set; }
    //public int StepId { get; set; }
}
