using AutoMapper;
using SspUis.Core;
using SspUis.DataLayer.EfClasses;
using System;
using System.Collections.Generic;
using System.Linq;
using WEBASE.Attributes;
using WEBASE.EF;
using WEBASE.Models;

namespace SspUis.DataLayer.Repositories
{
    public class CandidatesConfirmationTableDlDto
        : EntityDto<CandidatesConfirmationTableDlDto, CandidatesConfirmationTable>,
        IHaveIdProp<long>
    {
        public long Id { get; set; }

        [LocalizedRequired]
        [LocalizedRange(1, long.MaxValue)]
        public int EmployeeId { get; set; }

        public string Details { get; set; }

        public List<CandidatesConfirmationTableFileDlDto> Files { get; set; } = new();

        protected override Action<IMappingExpression<CandidatesConfirmationTableDlDto, CandidatesConfirmationTable>> AlterMapping =>
            cfg => cfg.ForMember(x => x.Files, c => c.Ignore());

        public override CandidatesConfirmationTable CreateEntity()
        {
            var entity = base.CreateEntity();
            entity.StatusId = StatusIdConst.CREATED;
            entity.Files.AddFromTempFiles(DocumentStorageConst.DOC_CANDITATES_CONFIRMATION_TABLE_FILES, Files.Select(a => a.Id).ToList());
            return entity;
        }

        public override void UpdateEntity(CandidatesConfirmationTable entity)
        {
            base.UpdateEntity(entity);
            entity.StatusId = StatusIdConst.MODIFIED;
            entity.Files.UpdateFromFiles(
                DocumentStorageConst.DOC_CANDITATES_CONFIRMATION_TABLE_FILES,
                entity.Id.ToString(),
                Files.Select(a => a.Id).ToList());
        }
    }
}
