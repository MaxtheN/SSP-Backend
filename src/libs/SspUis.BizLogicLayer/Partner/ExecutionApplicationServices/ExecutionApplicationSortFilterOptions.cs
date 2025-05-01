using System;
using System.Collections.Generic;
using WEBASE.Models;

namespace SspUis.BizLogicLayer.ExecutionApplicationServices;

public class ExecutionApplicationSortFilterOptions : SortFilterPageOptions
{
    public DateOnly? StartOn { get; set; } = null;
    public DateOnly? EndOn { get; set; } = null;
}
