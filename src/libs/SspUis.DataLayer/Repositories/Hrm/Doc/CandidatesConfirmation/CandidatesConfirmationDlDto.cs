using AutoMapper;
using SspUis.Core;
using SspUis.DataLayer.EfClasses;
using System;
using System.Collections.Generic;
using WEBASE.Attributes;
using WEBASE.EF;

namespace SspUis.DataLayer.Repositories
{
    public class CandidatesConfirmationDlDto<TDto> : EntityDto<TDto, CandidatesConfirmation>
    where TDto : CandidatesConfirmationDlDto<TDto>
    {
        [LocalizedRequired]
        public DateOnly DocOn { get; set; }

        [LocalizedRequired]
        [LocalizedStringLength(50)]
        public string DocNumber { get; set; }

        [LocalizedRequired]
        [LocalizedStringLength(600)]
        public string DocContent { get; set; }

        [LocalizedRequired]
        [LocalizedRange(1, int.MaxValue)]
        public int DepartmentId { get; set; }

        [LocalizedRequired]
        [LocalizedRange(1, int.MaxValue)]
        public int PositionId { get; set; }

        public string GeneralConclusion { get; set; }
        public int? OrganizationId { get; set; }

        public virtual List<CandidatesConfirmationTableDlDto> Tables { get; set; } = new();

        protected override Action<IMappingExpression<TDto, CandidatesConfirmation>> AlterMapping =>
            cfg => cfg.ForMember(x => x.Tables, c => c.Ignore());

        public override CandidatesConfirmation CreateEntity()
        {
            var entity = base.CreateEntity();
            entity.StatusId = StatusIdConst.CREATED;
            Tables.AddTo(entity.Tables);
            return entity;
        }

        public override void UpdateEntity(CandidatesConfirmation entity)
        {
            base.UpdateEntity(entity);
            entity.StatusId = StatusIdConst.MODIFIED;
            Tables.ApplyChangesTo<long, CandidatesConfirmationTableDlDto, CandidatesConfirmationTable>(entity.Tables);
        }
    }
}
