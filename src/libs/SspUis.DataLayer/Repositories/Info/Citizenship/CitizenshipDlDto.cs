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
    public class CitizenshipDlDto<TDto> : EntityDto<TDto, Citizenship>
        where TDto : CitizenshipDlDto<TDto>
    {
        public string ShortName { get; set; }
        [LocalizedRequired]
        [LocalizedStringLength(250)]
        public string FullName { get; set; }

        public List<CitizenshipTranslateDlDto> Translates { get; set; } = new List<CitizenshipTranslateDlDto>();

        protected override Action<IMappingExpression<TDto, Citizenship>> AlterMapping => cfg => cfg
            .ForMember(x => x.Translates, x => x.Ignore());

        public override Citizenship CreateEntity()
        {
            ShortName = FullName;
            var entity = base.CreateEntity();  
            entity.StateId = StateIdConst.ACTIVE;
            Translates.AddByUniqueFKTo(entity.Translates);
            return entity;
        }
        public override void UpdateEntity(Citizenship entity)
        {
            ShortName = FullName;
            base.UpdateEntity(entity);
            Translates.ApplyChangesByUniqueFKTo(entity.Translates);
        }
    }
}
