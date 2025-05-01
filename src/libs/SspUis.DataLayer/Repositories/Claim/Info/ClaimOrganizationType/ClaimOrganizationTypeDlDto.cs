using AutoMapper;
using SspUis.DataLayer.EfClasses.Claim;
using System;
using System.Collections.Generic;
using WEBASE.Attributes;
using WEBASE.EF;

namespace SspUis.DataLayer.Repositories
{
    public class ClaimOrganizationTypeDlDto<TDto> : EntityDto<TDto,ClaimOrganizationType>
        where TDto : ClaimOrganizationTypeDlDto<TDto>
    {
        [LocalizedRequired]
        [LocalizedStringLength(9)]
        public string Code { get; set; } = null!;
        [LocalizedStringLength(50)]
        public string OrderCode { get; set; }
        [LocalizedRequired]
        [LocalizedStringLength(250)]
        public string ShortName { get; set; } = null!;
        [LocalizedStringLength(500)]
        [LocalizedRequired]
        public string FullName { get; set; } = null!;

        public List<ClaimOrganizationTypeTranslateDlDto> Translates { get; set; } = new();

        protected override Action<IMappingExpression<TDto, ClaimOrganizationType>> AlterMapping =>
            cfg => cfg
                .ForMember(x => x.Translates, x => x.Ignore());
        public override ClaimOrganizationType CreateEntity()
        {
            var entity = base.CreateEntity();
            Translates.AddByUniqueFKTo(entity.Translates);
            return entity;
        }

        public override void UpdateEntity(ClaimOrganizationType entity)
        {
            base.UpdateEntity(entity);
            Translates.ApplyChangesByUniqueFKTo(entity.Translates);
        }
    }
}
