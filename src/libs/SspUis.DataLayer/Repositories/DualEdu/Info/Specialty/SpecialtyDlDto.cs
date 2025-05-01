using AutoMapper;
using SspUis.DataLayer.EfClasses;
using SspUis.DataLayer.EfClasses.DualEdu;
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
    public class SpecialtyDlDto<TDto> : EntityDto<TDto, Specialty>
        where TDto : SpecialtyDlDto<TDto>
    {
        [LocalizedRequired]
        [LocalizedRange(1, int.MaxValue)]
        public int OwnerId { get; set; }
        [LocalizedRequired]
        [LocalizedStringLength(50)]
        public string OrderCode { get; set; }
        [LocalizedRequired]
        [LocalizedStringLength(250)]
        public string ShortName { get; set; }
        [LocalizedRequired]
        [LocalizedStringLength(500)]
        public string FullName { get; set; }
        [LocalizedRequired]
        [LocalizedStringLength(50)]
        public string Code { get; set; }
        [LocalizedRequired]
        [LocalizedStringLength(50)]
        public string TextCode { get; set; }
		[LocalizedRequired]
		public int ExternalId { get; set; }

		public List<SpecialtyTranslateDlDto> Translates { get; set; } = new List<SpecialtyTranslateDlDto>();

        protected override Action<IMappingExpression<TDto, Specialty>> AlterMapping => cfg => cfg
            .ForMember(x => x.Translates, x => x.Ignore());

        public override Specialty CreateEntity()
        {
            var entity = base.CreateEntity();
            entity.StateId = StateIdConst.ACTIVE;
            Translates.AddByUniqueFKTo(entity.Translates);
            return entity;
        }

        public override void UpdateEntity(Specialty entity)
        {
            base.UpdateEntity(entity);
            Translates.AddByUniqueFKTo(entity.Translates);
        }
    }
}
