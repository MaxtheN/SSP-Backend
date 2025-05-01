using AutoMapper;
using SspUis.DataLayer.EfClasses.Appeal;
using System;
using System.Collections.Generic;
using WEBASE.Attributes;
using WEBASE.EF;

namespace SspUis.DataLayer.Repositories
{
    public class AppealTypeArriveDlDto<TDto> : EntityDto<TDto,AppealTypeArrive>
        where TDto : AppealTypeArriveDlDto<TDto>
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

        public List<AppealTypeArriveTranslateDlDto> Translates { get; set; } = new();

        protected override Action<IMappingExpression<TDto, AppealTypeArrive>> AlterMapping => cfg => cfg
            .ForMember(x => x.Translates, x => x.Ignore());
        public override AppealTypeArrive CreateEntity()
        {
            var entity = base.CreateEntity();
            Translates.AddByUniqueFKTo(entity.Translates);
            return entity;
        }

        public override void UpdateEntity(AppealTypeArrive entity)
        {
            base.UpdateEntity(entity);
            Translates.ApplyChangesByUniqueFKTo(entity.Translates);
        }
    }
}
