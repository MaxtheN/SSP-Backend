using AutoMapper;
using Humanizer;
using SspUis.Core;
using SspUis.DataLayer.EfClasses.Claim;
using SspUis.DataLayer.Repositories.Doc.BaseApplication;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using WEBASE.Attributes;
using WEBASE.EF;

namespace SspUis.DataLayer.Repositories
{
    public class ClaimApplicationDlDto<TDto> : BaseApplicationDlDto<TDto, ClaimApplication>
        where TDto : ClaimApplicationDlDto<TDto>
    {
        [LocalizedRequired]
        public string SignedData { get; set; }
        public long ApplicationId { get; set; }
        [LocalizedRequired]
        [LocalizedRange(1, long.MaxValue)]
        public long MemshipContractId { get; set; }
        public long? MemshipCertificateId { get; set; }
        public long? PrevApplicationId { get; set; }
        [LocalizedRequired]
        [LocalizedRange(1, int.MaxValue)]
        public int ClaimApplicationTypeId { get; set; }
        [LocalizedRequired]
        [LocalizedRange(1, int.MaxValue)]
        public int ClaimThemeId { get; set; }
        [LocalizedStringLength(600)]
        public string Details { get; set; }
        public decimal? MainDebt { get; set; }
        public decimal? CalculedPenalty { get; set; }
        public decimal? Penalty { get; set; }
        public decimal? Percent { get; set; }
        [LocalizedRequired]
        [LocalizedRange(1, int.MaxValue)]
        public int CurrencyId { get; set; }
        public decimal? CurrentPrincipalInterest { get; set; }
        public decimal? CurrentInterestRate { get; set; }
        public decimal? OtherDebtRepayment { get; set; }
        public int? OrganizationId { get; set; }
        public string? ContractIdentificationNumber { get; set; }
        public string? BankBranchName { get; set; }
        public string? BankResponsiblePerson { get; set; }
        //public new int ApplicationTypeId { get => base.ApplicationTypeId; }
        public string ContractorInn { get; set; }
        public List<ClaimApplicationTableDlDto> Tables { get; set; } = new();
        public List<ClaimApplicationFileDlDto> Files { get; set; } = new();
        protected override Action<IMappingExpression<TDto, ClaimApplication>> AlterMapping =>
           cfg =>
           {
               base.AlterMapping(cfg);
               cfg.ForMember(x => x.Files, x => x.Ignore());
               cfg.ForMember(x => x.Tables, x => x.Ignore());
           };
        
        public override ClaimApplication CreateEntity()
        {
            var entity = base.CreateEntity();
            entity.Application.StatusId = StatusIdConst.CREATED;
            entity.TotalAmount = MainDebt + CalculedPenalty + Penalty
               + Percent + CurrentPrincipalInterest + CurrentInterestRate + OtherDebtRepayment;
            entity.Application.CurrentStepId = StepIdConst.NEW_NOT_SEEN;
            entity.Application.ApplicationTypeId = ApplicationTypeIdConst.CLAIM;
            entity.Application.Id2 = Guid.NewGuid();
            entity.Files.AddFromTempFiles(DocumentStorageConst.DOC_CLAIM_APPLICATION_FILES, Files.Select(a => a.Id).ToList());
            Tables.AddTo(entity.Tables);
            return entity;
        }

        public override void UpdateEntity(ClaimApplication entity)
        {
            base.UpdateEntity(entity);
            entity.Application.StatusId = StatusIdConst.MODIFIED;
            entity.Files.UpdateFromFiles(DocumentStorageConst.DOC_CLAIM_APPLICATION_FILES, entity.Id.ToString(), Files.Select(a => a.Id).ToList());
            Tables.ApplyChangesTo<long, ClaimApplicationTableDlDto, ClaimApplicationTable>(entity.Tables);
        }
    }
}