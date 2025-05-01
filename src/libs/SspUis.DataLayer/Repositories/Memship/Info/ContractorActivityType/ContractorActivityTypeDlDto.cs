using AutoMapper;
using SspUis.DataLayer.EfClasses;
using SspUis.DataLayer.EfClasses.Hrm;
using System;
using System.Collections.Generic;
using WEBASE.Attributes;
using WEBASE.EF;

namespace SspUis.DataLayer.Repositories
{
    public class ContractorActivityTypeDlDto<TDto> : EntityDto<TDto,ContractorActivityType>
        where TDto : ContractorActivityTypeDlDto<TDto>
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
        [LocalizedRequired]
        [LocalizedRange(1,int.MaxValue)]
        public int ContractorActivityGroupId { get; set; }
        public List<ContractorActivityTypeTranslateDlDto> Translates { get; set; } = new();

        protected override Action<IMappingExpression<TDto, ContractorActivityType>> AlterMapping => cfg => cfg
            .ForMember(x => x.Translates, x => x.Ignore());
        public override ContractorActivityType CreateEntity()
        {
            var entity = base.CreateEntity();
            Translates.AddByUniqueFKTo(entity.Translates);
            return entity;
        }

        public override void UpdateEntity(ContractorActivityType entity)
        {
            base.UpdateEntity(entity);
            Translates.ApplyChangesByUniqueFKTo(entity.Translates);
        }
    }
}
