using GenericServices;
using SspUis.DataLayer.EfClasses.Hrm;
using SspUis.DataLayer.Repositories.Hrm;
using System.Collections.Generic;

namespace SspUis.BizLogicLayer.Hrm.StaffingServices
{
    public class StaffingCalcKindDto : StaffingCalcKindDlDto, ILinkToEntity<StaffingCalcKind>
    {
        public int? OrgSettlementAccountId { get; set; }
        public string? orgSettlementAccountCode { get; set; }
        public string CalculationKindName { get; set; }
        public decimal? CalcCoef { get; set; }
        public decimal? CalcSum { get; set; }
    }
}
