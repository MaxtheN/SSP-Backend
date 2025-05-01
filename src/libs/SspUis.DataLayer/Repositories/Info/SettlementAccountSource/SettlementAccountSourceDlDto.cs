using System;
using System.Collections.Generic;
using AutoMapper;
using SspUis.DataLayer.EfClasses;
using WEBASE;
using WEBASE.Attributes;
using WEBASE.EF;

namespace SspUis.DataLayer.Repositories
{
    public class SettlementAccountSourceDlDto<TDto> : EntityDto<TDto, SettlementAccountSource>
        where TDto : SettlementAccountSourceDlDto<TDto>
    {
        [LocalizedRequired]
        [LocalizedStringLength(1)]
        public string Code1 { get; set; }
        [LocalizedRequired]
        [LocalizedStringLength(3)]
        public string Code2 { get; set; }
        [LocalizedRequired]
        [LocalizedStringLength(1)]
        public string Code3 { get; set; }
        [LocalizedRequired]
        [LocalizedStringLength(1)]
        public string Code4 { get; set; }
        [LocalizedRequired]
        [LocalizedStringLength(250)]
        public string ShortName { get; set; }
        [LocalizedRequired]
        [LocalizedStringLength(500)]
        public string FullName { get; set; }
        public int? ParentId { get; set; }
        public List<SettlementAccountSourceTranslateDlDto> Translates { get; set; }

        protected override Action<IMappingExpression<TDto, SettlementAccountSource>> AlterMapping => cfg => cfg
            .ForMember(x => x.Translates, x => x.Ignore());

        public override SettlementAccountSource CreateEntity()
        {
            var entity = base.CreateEntity();
            entity.StateId = StateIdConst.ACTIVE;
            Translates.AddByUniqueFKTo(entity.Translates);
            return entity;
        }

        public override void UpdateEntity(SettlementAccountSource entity)
        {
            base.UpdateEntity(entity);
            Translates.AddByUniqueFKTo(entity.Translates);
        }
    }
}
