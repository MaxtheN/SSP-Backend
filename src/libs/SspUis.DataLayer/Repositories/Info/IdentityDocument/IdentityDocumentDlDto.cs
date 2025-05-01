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
    public class IdentityDocumentDlDto<TDto> : EntityDto<TDto, IdentityDocument>
        where TDto : IdentityDocumentDlDto<TDto>
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
        [LocalizedStringLength(500)]
        public string FullName { get; set; }

        public List<IdentityDocumentTranslateDlDto> Translates { get; set; } = new List<IdentityDocumentTranslateDlDto>();

        protected override Action<IMappingExpression<TDto, IdentityDocument>> AlterMapping => cfg => cfg
            .ForMember(x => x.Translates, x => x.Ignore());

        public override IdentityDocument CreateEntity()
        {
            var entity = base.CreateEntity();
            entity.StateId = StateIdConst.ACTIVE;
            Translates.AddByUniqueFKTo(entity.Translates);
            return entity;
        }

        public override void UpdateEntity(IdentityDocument entity)
        {
            base.UpdateEntity(entity);
            Translates.AddByUniqueFKTo(entity.Translates);
        }
    }
}
