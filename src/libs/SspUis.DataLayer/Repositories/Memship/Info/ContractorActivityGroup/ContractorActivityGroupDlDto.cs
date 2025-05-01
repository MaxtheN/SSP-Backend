using AutoMapper;
using SspUis.DataLayer.EfClasses;
using SspUis.DataLayer.EfClasses.Hrm;
using System;
using System.Collections.Generic;
using WEBASE.Attributes;
using WEBASE.EF;

namespace SspUis.DataLayer.Repositories
{
    public class ContractorActivityGroupDlDto<TDto> : EntityDto<TDto,ContractorActivityGroup>
        where TDto : ContractorActivityGroupDlDto<TDto>
    {
        [LocalizedRequired]
        [LocalizedStringLength(9)]
        public string Code { get; set; } = null!;
        [LocalizedRequired]
        [LocalizedStringLength(50)]
        public string OrderCode { get; set; }
        [LocalizedRequired]
        [LocalizedStringLength(250)]
        public string ShortName { get; set; } = null!;
        [LocalizedStringLength(500)]
        [LocalizedRequired]
        public string FullName { get; set; } = null!;

        public List<ContractorActivityGroupTranslateDlDto> Translates { get; set; } = new();

        protected override Action<IMappingExpression<TDto, ContractorActivityGroup>> AlterMapping => cfg => cfg
            .ForMember(x => x.Translates, x => x.Ignore());
        public override ContractorActivityGroup CreateEntity()
        {
            var entity = base.CreateEntity();
            Translates.AddByUniqueFKTo(entity.Translates);
            return entity;
        }

        public override void UpdateEntity(ContractorActivityGroup entity)
        {
            base.UpdateEntity(entity);
            Translates.ApplyChangesByUniqueFKTo(entity.Translates);
        }
    }
}
