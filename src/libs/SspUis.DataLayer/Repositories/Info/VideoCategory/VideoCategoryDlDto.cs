using AutoMapper;
using SspUis.DataLayer.EfClasses;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WEBASE;
using WEBASE.Attributes;
using WEBASE.EF;

namespace SspUis.DataLayer.Repositories
{
    public class VideoCategoryDlDto<TDto> : EntityDto<TDto, VideoCategory>
        where TDto : VideoCategoryDlDto<TDto>
    {
        [LocalizedRequired]
        [LocalizedStringLength(50)]
        public string Code { get; set; }

        [LocalizedStringLength(50)]
        public string OrderCode { get; set; }
        public string ShortName { get; set; }
        [LocalizedRequired]
        [LocalizedStringLength(500)]
        public string FullName { get; set; }

        public List<VideoCategoryTranslateDlDto> Translates { get; set; } = new List<VideoCategoryTranslateDlDto>();

        protected override Action<IMappingExpression<TDto, VideoCategory>> AlterMapping => cfg => cfg
            .ForMember(x => x.Translates, x => x.Ignore());

        public override VideoCategory CreateEntity()
        {
            ShortName = FullName;
            var entity = base.CreateEntity();
            entity.StateId = StateIdConst.ACTIVE;
            Translates.AddByUniqueFKTo(entity.Translates);
            return entity;
        }
        public override void UpdateEntity(VideoCategory entity)
        {
            ShortName = FullName;
            base.UpdateEntity(entity);
            Translates.ApplyChangesByUniqueFKTo(entity.Translates);
        }

    }
}
