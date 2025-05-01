using System.Collections.Generic;
using WEBASE.Models;

namespace SspUis.BizLogicLayer.Corruption;

public class JoinAntiCorruptionResultSortFilterOptions : DocumentSortFilterOptions
{
    public int? EmployeeId { get; set; } = null;
    public List<int> StatusIds { get; set; } = new();
}
