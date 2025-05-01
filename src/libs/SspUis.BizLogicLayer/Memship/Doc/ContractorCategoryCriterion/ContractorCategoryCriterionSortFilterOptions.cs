using System;
using WEBASE.Models;
namespace SspUis.BizLogicLayer;

public class ContractorCategoryCriterionSortFilterOptions : SortFilterPageOptions
{
    public int? StatusId { get; set; }
    public int? ContractorCategoryId { get; set; }
    public DateOnly? ExpirationDate { get; set; }
}
