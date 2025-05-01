using System;
using SspUis.Core;
using SspUis.DataLayer.EfClasses;
using WEBASE.EF;

namespace SspUis.DataLayer.Repositories;

public class ArbitrationDelaySignDlDto :
    EntityDto<ArbitrationDelaySignDlDto,
        ArbitrationDelaySign>
{
    public int? ArbitrationJudgeId { get; set; }
}
