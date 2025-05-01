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
    public class PrtnRejectReasonDlDto<TDto> : EntityDto<TDto, PrtnRejectReason>
        where TDto : PrtnRejectReasonDlDto<TDto>
    {
        [LocalizedStringLength(50)]
        public string OrderCode { get; set; }
        [LocalizedRequired]
        [LocalizedStringLength(250)]
        public string ShortName { get; set; }
        [LocalizedRequired]
        [LocalizedStringLength(500)]
        public string FullName { get; set; }
        public int? PrtnContractTypeId { get; set; }
        public int? PrtnContractTypeTableId { get; set; }

        public List<PrtnRejectReasonTranslateDlDto> Translates { get; set; } = new List<PrtnRejectReasonTranslateDlDto>();

        protected override Action<IMappingExpression<TDto, PrtnRejectReason>> AlterMapping => cfg => cfg
            .ForMember(x => x.Translates, x => x.Ignore());

        public override PrtnRejectReason CreateEntity()
        {
            var entity = base.CreateEntity();  
            entity.StateId = StateIdConst.ACTIVE;
            Translates.AddByUniqueFKTo(entity.Translates);
            return entity;
        }
        public override void UpdateEntity(PrtnRejectReason entity)
        {
            base.UpdateEntity(entity);
            Translates.ApplyChangesByUniqueFKTo(entity.Translates);
        }
    }
}
