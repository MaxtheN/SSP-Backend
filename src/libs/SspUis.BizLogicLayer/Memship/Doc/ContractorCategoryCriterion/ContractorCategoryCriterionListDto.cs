using System;
using GenericServices;
using SspUis.DataLayer.EfClasses;
using WEBASE.Models;

namespace SspUis.BizLogicLayer;

public class ContractorCategoryCriterionListDto
    : DocumentListDto<long>,
    ILinkToEntity<ContractorCategoryCriterion>,
    IHaveIdProp<long>,
    IHaveStatusId
{
    public int ContractorCategoryId { get; set; }
    public decimal? MinAmount { get; set; }
    public decimal? MaxAmount { get; set; }
    public DateOnly? ExpirationDate { get; set; }
    public string DocNumber { get; set; }
    public string Status { get; set; }
    public string ContractorCategory { get; set; }
}
