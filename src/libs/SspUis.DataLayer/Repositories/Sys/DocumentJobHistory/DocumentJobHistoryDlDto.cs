using System;
using WEBASE.Attributes;
using WEBASE.EF;
using SspUis.DataLayer.EfClasses;
using SspUis.Core;

namespace SspUis.DataLayer.Repositories
{
    public class DocumentJobHistoryDlDto<TDto> : EntityDto<TDto, DocumentJobHistory>
        where TDto : DocumentJobHistoryDlDto<TDto>
    {
        [LocalizedRequired]
        [LocalizedRange(1, int.MaxValue)]
        public int UserId { get; set; }
        [LocalizedRequired]
        [LocalizedStringLength(800)]
        public string Message { get; set; }

        [LocalizedRequired]
        [LocalizedRange(1, int.MaxValue)]
        public int TableId { get; set; }
        [LocalizedRequired]
        [LocalizedRange(1, long.MaxValue)]
        public long DocId { get; set; }
        [LocalizedRequired]
        [LocalizedRange(1, int.MaxValue)]
        public int FromStatusId { get; set; }
        [LocalizedRequired]
        [LocalizedRange(1, int.MaxValue)]
        public int ToStatusId { get; set; }
        [LocalizedRequired]
        [LocalizedRange(1, int.MaxValue)]
        public int StatusId { get; set; } = StatusIdConst.WAITING;
        public int? OrganizationId { get; set; }
        [LocalizedRequired]
        public bool IsSucceed { get; set; }
        public DateTime CreatedAt { get; set; } = DateTime.Now;
        public DateTime? StartAt { get; set; }
        public DateTime? EndAt { get; set; }
        public string? RequestTraceId { get; set; }

        public override DocumentJobHistory CreateEntity()
        {
            var entity = base.CreateEntity();
            return entity;
        }

        public override void UpdateEntity(DocumentJobHistory entity)
        {
            base.UpdateEntity(entity);
        }

    }
}
