using System;
using System.Collections.Generic;
using AutoMapper;
using SspUis.DataLayer.EfClasses.Appeal;
using WEBASE.Attributes;
using WEBASE.EF;

namespace SspUis.DataLayer.Repositories
{
    public class AppealDescriptionDlDto<TDto> : EntityDto<TDto, AppealDescription>
        where TDto : AppealDescriptionDlDto<TDto>
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
        public int? ParentId { get; set; }
        [LocalizedRequired]
        public bool HasParent { get; set; }

        public List<AppealDescriptionTranslateDlDto> Translates { get; set; } = new();

        protected override Action<IMappingExpression<TDto, AppealDescription>> AlterMapping => cfg => cfg
            .ForMember(x => x.Translates, x => x.Ignore());
        public override AppealDescription CreateEntity()
        {
            var entity = base.CreateEntity();
            Translates.AddByUniqueFKTo(entity.Translates);
            return entity;
        }

        public override void UpdateEntity(AppealDescription entity)
        {
            base.UpdateEntity(entity);
            Translates.ApplyChangesByUniqueFKTo(entity.Translates);
        }
    }
}
