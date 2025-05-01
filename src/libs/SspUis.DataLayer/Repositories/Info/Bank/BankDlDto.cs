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
    public class BankDlDto<TDto> : EntityDto<TDto, Bank>
        where TDto : BankDlDto<TDto>
    {
        [LocalizedStringLength(50)]
        public string? OrderCode { get; set; }
        [LocalizedRequired]
        [LocalizedStringLength(5)]
        public string Code { get; set; }
        [LocalizedRequired]
        [LocalizedStringLength(300)]
        public string BankName { get; set; }
        public int? BankCodeId { get; set; }
        public List<BankTranslateDlDto> Translates { get; set; } = new List<BankTranslateDlDto>();

        protected override Action<IMappingExpression<TDto, Bank>> AlterMapping => cfg => cfg
            .ForMember(x => x.Translates, x => x.Ignore());

        public override Bank CreateEntity()
        {
            var entity = base.CreateEntity();
            entity.StateId = StateIdConst.ACTIVE;
            Translates.AddByUniqueFKTo(entity.Translates);
            return entity;
        }

        public override void UpdateEntity(Bank entity)
        {
            base.UpdateEntity(entity);
            Translates.ApplyChangesByUniqueFKTo(entity.Translates);
        }

    }
}
