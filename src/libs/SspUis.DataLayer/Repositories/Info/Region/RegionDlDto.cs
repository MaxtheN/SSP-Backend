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
    public class RegionDlDto<TDto> : EntityDto<TDto, Region>
        where TDto : RegionDlDto<TDto>
    {
        [LocalizedStringLength(50)]
        public string OrderCode { get; set; }
        //[LocalizedRequired]
        [LocalizedStringLength(50)]
        public string Code { get; set; }
        [LocalizedStringLength(50)]
        public string Soato { get; set; }
        [LocalizedStringLength(50)]
        public string RoamingCode { get; set; }
        public string ShortName { get; set; }
        [LocalizedRequired]
        [LocalizedStringLength(250)]
        public string FullName { get; set; }
        public int CountryId { get; set; }
        
        public List<RegionTranslateDlDto> Translates { get; set; } = new List<RegionTranslateDlDto>();

        protected override Action<IMappingExpression<TDto, Region>> AlterMapping => cfg => cfg
            .ForMember(x => x.Translates, x => x.Ignore());

        public override Region CreateEntity()
        {
            ShortName = FullName;
            var entity = base.CreateEntity();
            entity.StateId = StateIdConst.ACTIVE;
            Translates.AddByUniqueFKTo(entity.Translates);
            return entity;
        }

        public override void UpdateEntity(Region entity)
        {
            ShortName = FullName;
            base.UpdateEntity(entity);
            Translates.ApplyChangesByUniqueFKTo(entity.Translates);
        }

    }
}
