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
    public class OkedDlDto<TDto> : EntityDto<TDto, Oked>
        where TDto : OkedDlDto<TDto>
    {
        [LocalizedRequired]
        [LocalizedStringLength(5)]
        public string Code { get; set; }
        [LocalizedRequired]
        [LocalizedStringLength(500)]
        public string ShortName { get; set; }
        [LocalizedRequired]
        [LocalizedStringLength(500)]
        public string FullName { get; set; }
        public int? OkedTypeId { get; set; }
        public List<OkedTranslateDlDto> Translates { get; set; } = new List<OkedTranslateDlDto>();

        protected override Action<IMappingExpression<TDto, Oked>> AlterMapping => cfg => cfg
            .ForMember(x => x.Translates, x => x.Ignore());

        public override Oked CreateEntity()
        {
            var entity = base.CreateEntity();
            entity.StateId = StateIdConst.ACTIVE;
            if (entity.Level < 5)
            {
                entity.IsGroup = true;
            }
            else
            {
                entity.Level = 5;
                entity.IsGroup = false;
            }
            Translates.AddByUniqueFKTo(entity.Translates);
            return entity;
        }

        public override void UpdateEntity(Oked entity)
        {
            if (entity.Code == "00000")
                Code = entity.Code;
            base.UpdateEntity(entity);
            Translates.ApplyChangesByUniqueFKTo(entity.Translates);
        }

    }
}
