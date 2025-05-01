using System;
using System.Collections.Generic;
using System.Linq;
using AutoMapper;
using SspUis.Core;
using SspUis.DataLayer.EfClasses.DualEdu;
using WEBASE.Attributes;
using WEBASE.EF;

namespace SspUis.DataLayer.Repositories
{
    public class SubsidyRequestDlDto<TDto> : EntityDto<TDto, SubsidyRequest>
        where TDto : SubsidyRequestDlDto<TDto>
    {
        [LocalizedRequired]
        public DateOnly DocOn { get; set; }
        [LocalizedRequired]
        public string DocNumber { get; set; }
        [LocalizedRequired]
        [LocalizedRange(1, int.MaxValue)]
        public int Year { get; set; }
        [LocalizedRequired]
        [LocalizedRange(1, int.MaxValue)]
        public int Month { get; set; }
        [LocalizedRequired]
        [LocalizedRange(1, int.MaxValue)]
        public int RegionId { get; set; }
        [LocalizedRequired]
        [LocalizedRange(1, int.MaxValue)]
        public int DistrictId { get; set; }

        [LocalizedRequired]
        [LocalizedStringLength(500)]
        public string Address { get; set; }
        [LocalizedRequired]
        [LocalizedStringLength(100)]
        public string Email { get; set; }
        [LocalizedRequired]
        [LocalizedStringLength(20)]
        public string Phone { get; set; }
        [LocalizedRequired]
        [LocalizedRange(1, long.MaxValue)]
        public long ContractorSettlementAccountId { get; set; }
        public string Message { get; set; }

        public List<SubsidyRequestTableDlDto> Tables { get; set; } = new();
        public List<SubsidyRequestFileDlDto> Files { get; set; } = new();
        protected override Action<IMappingExpression<TDto, SubsidyRequest>> AlterMapping =>
           cfg =>
           {
               //base.AlterMapping(cfg);
               cfg.ForMember(x => x.Files, x => x.Ignore());
               cfg.ForMember(x => x.Tables, x => x.Ignore());
           };

        public override SubsidyRequest CreateEntity()
        {
            var entity = base.CreateEntity();
            entity.StatusId = StatusIdConst.CREATED;

            Tables.AddTo(entity.Tables);

            entity.Files.AddRange(entity.Tables.SelectMany(x => x.Files));

            entity.Files.AddFromTempFiles(DocumentStorageConst.DOC_DUAL_SUBSIDY_REQUEST_FILES,
                Files.Select(a => a.Id).ToList());

            foreach (var file in entity.Files)
            {
                foreach (var table in Tables)
                {
                    foreach (var tableFile in table.Files)
                    {
                        if (tableFile.Id == file.Id)
                            file.SubsidyRequestTable = table.GetEntity();
                    }
                }
            }
            return entity;
        }

        public override void UpdateEntity(SubsidyRequest entity)
        {
            base.UpdateEntity(entity);
            entity.StatusId = StatusIdConst.MODIFIED;

            Tables.ApplyChangesTo<long, SubsidyRequestTableDlDto, SubsidyRequestTable>(entity.Tables);

            entity.Files.AddRange(entity.Tables.SelectMany(x => x.Files));

            entity.Files.UpdateFromFiles(DocumentStorageConst.DOC_DUAL_SUBSIDY_REQUEST_FILES,
                entity.Id.ToString(),
                Files.Select(a => a.Id).ToList());

            foreach (var file in entity.Files)
            {
                foreach (var table in Tables)
                {
                    foreach (var tableFile in table.Files)
                    {
                        if (tableFile.Id == file.Id)
                            file.SubsidyRequestTable = table.GetEntity();
                    }
                }
            }
        }
    }
}