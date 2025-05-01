using System;

namespace SspUis.BizLogicLayer.ReportServices.Main;

public class HrmEmployeeActivityInfoDtoFilter
{
    public int? RegionId { get; set; }
    public int? BirthRegionId { get; set; }
    public int? MinAge { get; set; }
    public int? MaxAge { get; set; }
    public int? GenderId { get; set; }
    public bool? HasLegalEducation { get; set; }
    public DateOnly? StartDate { get; set; }
    public DateOnly? EndDate { get; set; }
}
