using GenericServices;
using SspUis.DataLayer.EfClasses;
using SspUis.DataLayer.Repositories;
using System.Collections.Generic;

namespace SspUis.BizLogicLayer;

public class SrvYearlyPlanTableRegionDto : SrvYearlyPlanTableRegionDlDto, ILinkToEntity<SrvYearlyPlanTableRegion>
{
    public string Region { get; set; }
    public List<SrvYearlyPlanTableDistrictDto> Districts { get; set; } = new();
}
