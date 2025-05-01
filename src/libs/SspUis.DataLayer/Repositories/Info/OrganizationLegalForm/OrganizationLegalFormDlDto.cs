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
    public class OrganizationLegalFormDlDto<TDto> : EntityDto<TDto, OrganizationLegalForm>
        where TDto : OrganizationLegalFormDlDto<TDto>
    {

        [LocalizedStringLength(50)]
        public string OrderCode { get; set; }
        [LocalizedRequired]
        [LocalizedStringLength(50)]
        public string Code { get; set; }
        [LocalizedRequired]
        [LocalizedStringLength(250)]
        public string ShortName { get; set; }
        [LocalizedRequired]
        [LocalizedStringLength(300)]
        public string FullName { get; set; }

        public List<OrganizationLegalFormTranslateDlDto> Translates { get; set; } = new List<OrganizationLegalFormTranslateDlDto>();

        protected override Action<IMappingExpression<TDto, OrganizationLegalForm>> AlterMapping => cfg => cfg
            .ForMember(x => x.Translates, x => x.Ignore());

        public override OrganizationLegalForm CreateEntity()
        {
            var entity = base.CreateEntity();
            entity.StateId = StateIdConst.ACTIVE;
            Translates.AddByUniqueFKTo(entity.Translates);
            return entity;
        }
        public override void UpdateEntity(OrganizationLegalForm entity)
        {
            base.UpdateEntity(entity);
            Translates.ApplyChangesByUniqueFKTo(entity.Translates);
        }

    }
}
