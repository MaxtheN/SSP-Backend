using AutoMapper;
using SspUis.DataLayer.EfClasses;
using System;
using System.Collections.Generic;
using WEBASE.Attributes;
using WEBASE.EF;
using SspUis.Core;
using System.Linq;

namespace SspUis.DataLayer.Repositories
{
    public class PrtnContractDlDto<TDto> : EntityDto<TDto, PrtnContract>
        where TDto : PrtnContractDlDto<TDto>
    {
        [LocalizedRequired]
        public DateOnly DocOn { get; set; }
        [LocalizedRequired]
        [LocalizedStringLength(50)]
        public string DocNumber { get; set; }
        [LocalizedRequired]
        [LocalizedRange(1, long.MaxValue)]
        public long ContractorId { get; set; }
        [LocalizedRequired]
        [LocalizedRange(1, int.MaxValue)]
        public int PrtnContractTypeId { get; set; }
        [LocalizedRequired]
        [LocalizedRange(1, int.MaxValue)]
        public int NewVacanciesCount { get; set; }
        [LocalizedRequired]
        [LocalizedRange(1, long.MaxValue)]
        public long ApplicationId { get; set; }
        public List<PrtnContractFileDlDto> Files { get; set; } = new();

        protected override Action<IMappingExpression<TDto, PrtnContract>> AlterMapping => cfg => cfg
              .ForMember(x => x.Files, x => x.Ignore());

        public override PrtnContract CreateEntity()
        {
            var entity = base.CreateEntity();
            entity.IsRead = false;
            entity.Id2 = Guid.NewGuid();
            entity.StatusId = StatusIdConst.SENT_FOR_EXPERTISE;
            entity.Files.AddFromTempFiles(DocumentStorageConst.DOC_PRTN_CONTRACT_FILES, entity.Files.Select(a => a.Id).ToList());

            return entity;
        }

        public override void UpdateEntity(PrtnContract entity)
        {
            base.UpdateEntity(entity);
            entity.Files.UpdateFromFiles(DocumentStorageConst.DOC_PRTN_CONTRACT_FILES, entity.Id.ToString(), entity.Files.Select(a => a.Id).ToList());
        }
    }
}
