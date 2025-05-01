using System;
using GenericServices;
using SspUis.DataLayer.EfClasses.Hrm;
using WEBASE.Models;

namespace SspUis.BizLogicLayer.Hrm;

public class WorkDayOffListDto : ILinkToEntity<WorkDayOff>, IHaveIdProp<long>
{
    public long Id { get; set; }
    public string DocNumber { get; set; }
    public DateOnly DocOn { get; set; }
    public string Details { get; set; }
    public string Status { get; set; }
    public int StatusId { get; set; }
    public int EmployeeId { get; set; }
    public string Employee { get; set; }
}
