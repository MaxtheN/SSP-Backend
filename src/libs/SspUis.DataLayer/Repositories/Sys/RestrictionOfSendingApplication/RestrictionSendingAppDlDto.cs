using SspUis.DataLayer.EfClasses;
using System;
using WEBASE;
using WEBASE.Attributes;
using WEBASE.EF;

namespace SspUis.DataLayer.Repositories
{
    public class RestrictionSendingAppDlDto<TDto> : EntityDto<TDto,
        RestrictionOfSendingApplication> where TDto : RestrictionSendingAppDlDto<TDto>
    {
        [LocalizedRequired]
        public DateTime StartAt { get; set; } = DateTime.Now;

        [LocalizedRequired]
        public DateTime EndAt { get; set; } = DateTime.Now;

        public string Details { get; set; } = string.Empty;

        public string MessageText { get; set; } = string.Empty;

        [LocalizedRequired]
        public int TableId { get; set; }

        [LocalizedRequired]
        public int AppId { get; set; }

        public override RestrictionOfSendingApplication CreateEntity()
        {
            var entity = base.CreateEntity();
            entity.StateId = StateIdConst.ACTIVE;
            return entity;
        }

        public override void UpdateEntity(RestrictionOfSendingApplication entity)
        {
            base.UpdateEntity(entity);
        }
    }
}