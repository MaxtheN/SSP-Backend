using System;
using Newtonsoft.Json;

namespace SspUis.BizLogicLayer.ReportServices;

public class StaffCountDtoFilter
{
    public int? RegionId { get; set; }
    public bool ByRegion { get; set; } = false;
    public int? OrganizationId { get; set; }
    public bool ByOrganization { get; set; } = false;

    public int? DepartmentId { get; set; }
    public bool ByDepartment { get; set; } = false;

    public int? PositionId { get; set; }
    public bool ByPosition { get; set; } = false;

    public DateOnly? FromDate { get; set; }
    public DateOnly? ToDate { get; set; }

}
