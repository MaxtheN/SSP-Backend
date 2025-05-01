using GenericServices;
using SspUis.DataLayer.EfClasses.Hrm;
using SspUis.DataLayer.Repositories;

namespace SspUis.BizLogicLayer.Hrm.CalculationKindServices
{
    public class CalculationKindUsedTableDto : CalculationKindUsedTableDlDto, ILinkToEntity<CalculationKindUsedTable>
    {
        public string FormedCalculationKind { get; set; } = null!;
        public string MinimumValueType { get; set; } = null!;
    }
}
