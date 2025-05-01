using GenericServices;
using SspUis.DataLayer;
using SspUis.DataLayer.EfClasses.Claim;
using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace SspUis.BizLogicLayer.ClaimApplicationServices
{
    public class ClaimApplicationListDto : DocumentListDto<long>, ILinkToEntity<ClaimApplication>
    {
        public ApplicationListDto Application { get; set; }
        public string Details { get; set; }
        public decimal? TotalAmount { get; set; }
        public decimal? MainDebt { get; set; }
        public decimal? CalculedPenalty { get; set; }
        public string? ContractIdentificationNumber { get; set; }
        public decimal? Penalty { get; set; }
        public decimal? Percent { get; set; }
        public int? CurrencyId { get; set; }
        public string Currency { get; set; }
        public int? ClaimApplicationTypeId { get; set; }
        public int? ClaimResponsibleTypeId { get; set; }
        public string ClaimApplicationType { get; set; }
        public string ClaimTheme { get; set; }
        public int? ClaimThemeId { get; set; }
        public long? EmployeeManageId { get; set; }
        public string EmployeeManage { get; set; }
        public DateTime? DurationGivenPerformer { get; set; }
        public int? OrganizationId { get; set; }
        public string OrganizationName { get; set; }
        public string InnOrPinfl { get; set; }
        public int TableId { get; } = TableIdConst.CLAIM__DOC_CLAIM_APPLICATION;
    }
}
