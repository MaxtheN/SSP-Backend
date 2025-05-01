using SspUis.Core;
using SspUis.DataLayer.EfClasses;
using SspUis.DataLayer.EfClasses.Claim;
using System;
using WEBASE.Attributes;
using WEBASE.EF;

namespace SspUis.DataLayer.Repositories
{
    public class UpdateEmployeeAttechmentDlDto : EntityDto<UpdateEmployeeAttechmentDlDto, ClaimApplication>
    {
        [LocalizedRequired]
        [LocalizedRange(1, long.MaxValue)]
        public long Id { get; set; }

        [LocalizedRequired]
        [LocalizedRange(1, long.MaxValue)]
        public long EmployeeManageId { get; set; }

        [LocalizedRequired]
        public DateTime DurationGivenPerformer { get; set; }

        public override void UpdateEntity(ClaimApplication entity)
        {
            base.UpdateEntity(entity);
            entity.EmployeeManageId = EmployeeManageId;
            entity.Application.CurrentStepId = StepIdConst.EXECUTING;
            entity.DurationGivenPerformer = DurationGivenPerformer;
        }
    }

    public class UpdateStepDlDto : EntityDto<UpdateStepDlDto, Application>
    {
        [LocalizedRequired]
        [LocalizedRange(1, int.MaxValue)]
        public long Id { get; set; }

        public int? StepId { get; set; }

        public override void UpdateEntity(Application entity)
        {
            base.UpdateEntity(entity);
            entity.CurrentStepId = StepId;
        }
    }
}
