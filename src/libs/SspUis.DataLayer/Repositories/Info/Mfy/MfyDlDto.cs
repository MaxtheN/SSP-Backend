using SspUis.DataLayer.EfClasses;
using WEBASE;
using WEBASE.Attributes;
using WEBASE.EF;

namespace SspUis.DataLayer.Repositories
{
    public class MfyDlDto<TDto> : EntityDto<TDto, Mfy>
        where TDto : MfyDlDto<TDto>
    {
        public long ExternalId { get; set; }
        public string OrderCode { get; set; }
        [LocalizedRequired]
        [LocalizedStringLength(250)]
        public string ShortName { get; set; }
        [LocalizedRequired]
        [LocalizedStringLength(500)]
        public string FullName { get; set; }
        public int RegionId { get; set; }
        public int DistrictId { get; set; }
        public long ExternalLastUpdatedId { get; set; }

        public override Mfy CreateEntity()
        {
            ShortName = FullName;
            var entity = base.CreateEntity();
            entity.StateId = StateIdConst.ACTIVE;
            return entity;
        }

        public override void UpdateEntity(Mfy entity)
        {
            ShortName = FullName;
            base.UpdateEntity(entity);
        }

    }
}
