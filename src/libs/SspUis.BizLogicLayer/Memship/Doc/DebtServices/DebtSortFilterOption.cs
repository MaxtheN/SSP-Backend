using System;
using WEBASE.Models;

namespace SspUis.BizLogicLayer.Memship;

public class DebtSortFilterOption : SortFilterPageOptions
{
    public int? OrganizationId { get; set; }
    public int? StatusId { get; set; }
}
