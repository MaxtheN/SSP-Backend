using GenericServices;
using SspUis.DataLayer;
using SspUis.DataLayer.EfClasses;
using System;

namespace SspUis.BizLogicLayer.ArbitrationCourtApplicationServices
{
    public class ArbitrationCourtApplicationListDto : DocumentListDto<long>, ILinkToEntity<ArbitrationCourtApplication>
    {
        public ApplicationListDto Application { get; set; }
        public string ECourtNumber { get; set; }
        public string AplicationContractor { get; set; }
        public string ContractorPhonber { get; set; }
        public string ContractorAddress { get; set; }
        public string ResponsiblePhonber { get; set; }
        public int ClaimResponsibleTypeId { get; set; }
        public string ResponsibleAddress { get; set; }
        public DateOnly CreateAt { get; set; }
        public long ResponsibleContractorId { get; set; }
        public decimal Amount { get; set; }
        public decimal ArbitrationAmount { get; set; }
        public int CurrencyId { get; set; }
        public int ArbitrationCourtId { get; set; }
        public int ArbitrationApplicationTypeId { get; set; }
        public int ContractorResponsibleTypeId { get; set; }
        public string ContractorResponsibleType { get; set; }
        public string ClaimResponsibleType { get; set; }
        public string Currency { get; set; }
        public string ResponsibleContractor { get; set; }
        public string ArbitrationCourt { get; set; }
        public string ArbitrationApplicationType { get; set; }
        public int? ArbitrationCourtResultId { get; set; }
        public string ArbitrationCourtResult { get; set; }

        public bool IsForeignContractor { get; set; }
        public bool IsForeignResponsible { get; set; }
        public int TableId { get; } = TableIdConst.CLAIM__DOC_ARBITRATION_APPLICATION;

        #region Actions
        public bool CanChangeStep { get; set; }
        public bool CanSelectJudge { get; set; }
        #endregion
    }
}
