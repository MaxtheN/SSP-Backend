using SspUis.BizLogicLayer;
using WEBASE;

namespace WbCrm.BizLogicLayer.ContractorSurveyServices;

public class ContractorSurveySortFilterOptions : DocumentSortFilterOptions
{
    public int FromYear { get; set; }
    public int ToYear { get; set; }
}

public class ContractorSurveySortFilterPageOptions : TableSortFilterPageOptions
{

}
