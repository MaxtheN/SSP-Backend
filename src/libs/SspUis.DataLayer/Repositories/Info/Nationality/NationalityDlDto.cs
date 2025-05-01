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
    public class NationalityDlDto<TDto> : EntityDto<TDto, Nationality>
        where TDto : NationalityDlDto<TDto>
    {
        public string ShortName { get; set; }
        [LocalizedRequired]
        [LocalizedStringLength(250)]
        public string FullName { get; set; }

        public List<NationalityTranslateDlDto> Translates { get; set; } = new List<NationalityTranslateDlDto>();

        protected override Action<IMappingExpression<TDto, Nationality>> AlterMapping => cfg => cfg
            .ForMember(x => x.Translates, x => x.Ignore());

        public override Nationality CreateEntity()
        {
            ShortName = FullName;
            var entity = base.CreateEntity();
            entity.StateId = StateIdConst.ACTIVE;
            Translates.AddByUniqueFKTo(entity.Translates);
            return entity;
        }

        public override void UpdateEntity(Nationality entity)
        {
            ShortName = FullName;
            base.UpdateEntity(entity);
            Translates.AddByUniqueFKTo(entity.Translates);
        }
    }
}
