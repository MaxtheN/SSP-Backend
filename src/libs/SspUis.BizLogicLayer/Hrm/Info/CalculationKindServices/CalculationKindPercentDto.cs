using GenericServices;
using SspUis.DataLayer.EfClasses.Hrm;
using SspUis.DataLayer.Repositories;

namespace SspUis.BizLogicLayer.Hrm.CalculationKindServices
{
    public class CalculationKindPercentDto : CalculationKindPercentDlDto, ILinkToEntity<CalculationKindPercent>
    {
        public string? LimitOperType { get; set; } = null;

    }
}
