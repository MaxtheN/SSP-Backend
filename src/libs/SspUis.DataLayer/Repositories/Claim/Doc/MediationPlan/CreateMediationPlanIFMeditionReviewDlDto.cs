using SspUis.Core;
using SspUis.DataLayer.EfClasses.Claim;
using System;
using WEBASE.Attributes;
using WEBASE.EF;

namespace SspUis.DataLayer.Repositories
{
    public class CreateMediationPlanIFMeditionReviewDlDto
        : EntityDto<CreateMediationPlanIFMeditionReviewDlDto, MediationPlan>
    {
        [LocalizedRequired]
        [LocalizedRange(1, long.MaxValue)]
        public long Id { get; set; }

        public DateOnly? DocOn { get; set; }
        public int? MeetingTypeId { get; set; }
        public string? AddressOrUrl { get; set; }
        public DateTime? MeditionAt { get; set; }
        public string? ChamberPerson { get; set; }

        public override MediationPlan CreateEntity()
        {
            var entity = base.CreateEntity();
            entity.StatusId = StatusIdConst.CREATED;
            return entity;
        }
        public override void UpdateEntity(MediationPlan entity)
        {
            base.UpdateEntity(entity);
            entity.StatusId = StatusIdConst.MODIFIED;
        }
    }
}
