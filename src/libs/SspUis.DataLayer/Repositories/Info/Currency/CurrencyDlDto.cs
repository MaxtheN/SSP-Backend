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
    public class CurrencyDlDto<TDto> : EntityDto<TDto, Currency>
        where TDto : CurrencyDlDto<TDto>
    {
        [LocalizedRequired]
        [LocalizedStringLength(3)]
        public string Code { get; set; }
        [LocalizedStringLength(3)]
        public string TextCode { get; set; }
        [LocalizedStringLength(50)]
        public string OrderCode { get; set; }
        [LocalizedRequired]
        [LocalizedStringLength(500)]
        public string FullName { get; set; }
        [LocalizedRequired]
        [LocalizedStringLength(250)]
        public string ShortName { get; set; }

        public List<CurrencyTranslateDlDto> Translates { get; set; } = new List<CurrencyTranslateDlDto>();

        protected override Action<IMappingExpression<TDto, Currency>> AlterMapping => cfg => cfg
            .ForMember(x => x.Translates, x => x.Ignore());

        public override Currency CreateEntity()
        {
            ShortName = FullName;
            var entity = base.CreateEntity();
            entity.StateId = StateIdConst.ACTIVE;
            Translates.AddByUniqueFKTo(entity.Translates);
            return entity;
        }

        public override void UpdateEntity(Currency entity)
        {
            ShortName = FullName;
            base.UpdateEntity(entity);
            Translates.AddByUniqueFKTo(entity.Translates);
        }
    }
}
