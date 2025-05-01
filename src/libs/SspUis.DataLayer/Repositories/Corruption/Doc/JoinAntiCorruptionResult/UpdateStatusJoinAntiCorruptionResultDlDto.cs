using SspUis.DataLayer.EfClasses.Corruption;
using WEBASE.Attributes;
using WEBASE.EF;
using WEBASE.Models;

namespace SspUis.DataLayer.Repositories.Corruption
{
    public class UpdateStatusJoinAntiCorruptionResultDlDto
        : EntityDto<UpdateStatusJoinAntiCorruptionResultDlDto, JoinAntiCorruptionResult>, IHaveIdProp<long>
    {
        [LocalizedRequired]
        [LocalizedRange(1, long.MaxValue)]
        public long Id { get; set; }
        public string Message { get; set; } = null!;
        [LocalizedRequired]
        public int StatusId { get; set; }

        public override void UpdateEntity(JoinAntiCorruptionResult entity)
        {
            base.UpdateEntity(entity);
            entity.StatusId = StatusId;
        }
    }
}
