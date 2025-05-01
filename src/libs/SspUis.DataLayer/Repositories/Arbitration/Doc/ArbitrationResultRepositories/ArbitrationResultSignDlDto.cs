using SspUis.DataLayer.EfClasses;
using WEBASE.Attributes;
using WEBASE.EF;
using WEBASE.Models;

namespace SspUis.DataLayer.Repositories;

public class ArbitrationResultSignDlDto :
    EntityDto<ArbitrationResultSignDlDto,
        ArbitrationResultSign>
    , IHaveIdProp<long>
{
    public long Id { get; set; }

    [LocalizedRequired]
    public int ArbitrationJudgeId { get; set; }
}
