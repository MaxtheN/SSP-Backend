using GenericServices;
using SspUis.DataLayer.EfClasses;
using SspUis.DataLayer.EfClasses.Claim;
using System;

namespace SspUis.BizLogicLayer;

public class ArbitrationJudgeListDto :  ILinkToEntity<ArbitrationJudge>
{
    public int Id { get; set; }
    public string FirstName { get; set; }
    public string LastName { get; set; }
    public string MiddleName { get; set; }
    public int StateId { get; set; }
    public string State { get; set; } = null!;
    public string PositionName { get; set; }
    public string OrganizationName { get; set; }
    public int RegionId { get; set; }
    public string Region { get; set; }
    public int? DistrictId { get; set; }
    public string District { get; set; }


}
