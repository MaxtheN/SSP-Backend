using GenericServices;
using SspUis.DataLayer.EfClasses;
using SspUis.DataLayer.EfClasses.Claim;
using SspUis.DataLayer.Interfaces;
using SspUis.DataLayer.Repositories;
using System.Collections.Generic;

namespace SspUis.BizLogicLayer;

public class ArbitrationJudgeDto : UpdateArbitrationJudgeDlDto, ILinkToEntity<ArbitrationJudge>, IInfoHl
{
    public string LastName { get; set; }
    public string MiddleName { get; set; }
    public string FirstName { get; set; }
    public string State { get; set; } = null!;
    public string Region { get; set; }
    public string District { get; set; }

}
