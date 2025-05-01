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
    public class PositionDlDto<TDto> : EntityDto<TDto, Position>
        where TDto : PositionDlDto<TDto>
    {
        [LocalizedStringLength(50)]
        public string OrderCode { get; set; }
        [LocalizedStringLength(250)]
        public string ShortName { get; set; }
        [LocalizedRequired]
        [LocalizedStringLength(250)]
        public string FullName { get; set; }
        public string IndexCode { get; set; }
        public int? PositionClassificationId { get; set; }
        public int? PositionCategoryId { get; set; }
        public int? TariffScaleTypeId { get; set; }
        public int? StaffTypeBasicTariffId { get; set; }
        public List<PositionTranslateDlDto> Translates { get; set; } = new List<PositionTranslateDlDto>();

        protected override Action<IMappingExpression<TDto, Position>> AlterMapping => cfg => cfg
            .ForMember(x => x.Translates, x => x.Ignore());

        public override Position CreateEntity()
        {
            var entity = base.CreateEntity();
            entity.StateId = StateIdConst.ACTIVE;
            Translates.AddByUniqueFKTo(entity.Translates);
            return entity;
        }

        public override void UpdateEntity(Position entity)
        {
            base.UpdateEntity(entity);
            Translates.ApplyChangesByUniqueFKTo(entity.Translates);
        }

    }
}
