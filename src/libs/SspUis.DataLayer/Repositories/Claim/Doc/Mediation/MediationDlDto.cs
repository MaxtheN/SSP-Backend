using System;
using System.Collections.Generic;
using System.Linq;
using AutoMapper;
using SspUis.Core;
using SspUis.DataLayer.EfClasses.Claim;
using WEBASE.Attributes;
using WEBASE.EF;

namespace SspUis.DataLayer.Repositories
{
    public class MediationDlDto<TDto> : EntityDto<TDto, Mediation>
        where TDto : MediationDlDto<TDto>
    {
        [LocalizedRequired]
        public DateOnly DocOn { get; set; } = DateOnly.FromDateTime(DateTime.Now);
        [LocalizedRequired]
        [LocalizedStringLength(50)]
        public string DocNumber { get; set; }
        [LocalizedRequired]
        [LocalizedRange(1,long.MaxValue)]
        public long MediationPlanId { get; set; }
        [LocalizedRequired]
        public string ContractorDetails { get; set; }
        [LocalizedRequired]
        public string ResponsibleDetails { get; set; }
        [LocalizedRequired]
        [LocalizedRange(1,int.MaxValue)]
        public int MediationResultId { get; set; }
        [LocalizedRequired]
        [LocalizedStringLength(100)]
        public string ResponsiblePersonName { get; set; }
        [LocalizedRequired]
        [LocalizedStringLength(100)]
        public string ClaimantPersonName { get; set; }

        [LocalizedRequired]
        [LocalizedRange(1,int.MaxValue)]
        public int ClaimNeedCourtId { get; set; }

        public DateTime? CourtAt { get; set; }

        [LocalizedRequired]
        [LocalizedRange(1,long.MaxValue)]
        public long ContractorId { get; set; }

        [LocalizedRequired]
        [LocalizedStringLength(200)]
        public string ChamberPerson { get; set; }

        public List<MediationFileDlDto>? Files { get; set; } = new();

        protected override Action<IMappingExpression<TDto, Mediation>> AlterMapping =>
            cfg => cfg
                .ForMember(x => x.Files, opt => opt.Ignore());

        public override Mediation CreateEntity()
        {
            var entity = base.CreateEntity();
            entity.StatusId = StatusIdConst.CREATED;
            entity.Files.AddFromTempFiles(DocumentStorageConst.DOC_MEDIATION_FILE, Files.Select(a => a.Id).ToList());
            return entity;
        }

        public override void UpdateEntity(Mediation entity)
        {
            base.UpdateEntity(entity);
        }

    }
}