using System;
using System.Collections.Generic;
using WEBASE.Models;

namespace SspUis.BizLogicLayer.Hrm;

public class AppointEmployeeSortFilterOptions : SortFilterPageOptions
{
    public int? EmployeeId { get; set; } = null;
    public DateOnly? StartOn { get; set; } = null;
    public DateOnly? EndOn { get; set; } = null;
    public int? EmpAppointOrderTypeId { get; set; } = null;
    public List<int> StatusIds { get; set; } = new();
}
