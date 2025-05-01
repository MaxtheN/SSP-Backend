using GenericServices;
using SspUis.DataLayer.EfClasses;
using SspUis.DataLayer.Repositories;

namespace SspUis.BizLogicLayer;

public class SrvYearlyPlanTableDistrictDto : SrvYearlyPlanTableDistrictDlDto, ILinkToEntity<SrvYearlyPlanTableDistrict>
{
    public string District { get; set; }
}
