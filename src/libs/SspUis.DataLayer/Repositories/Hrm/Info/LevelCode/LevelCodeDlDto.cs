using System;
using System.Collections.Generic;
using AutoMapper;
using SspUis.DataLayer.EfClasses.Hrm;
using WEBASE;
using WEBASE.Attributes;
using WEBASE.EF;

namespace SspUis.DataLayer.Repositories
{
    public class LevelCodeDlDto<TDto> : EntityDto<TDto, LevelCode>
        where TDto : LevelCodeDlDto<TDto>
    {
        [LocalizedStringLength(50)]
        public string OrderCode { get; set; }
        [LocalizedRequired]
        [LocalizedStringLength(9)]
        public string Code { get; set; }
        [LocalizedRequired]
        [LocalizedStringLength(250)]
        public string ShortName { get; set; }
        [LocalizedRequired]
        [LocalizedStringLength(500)]
        public string FullName { get; set; }
        [LocalizedRequired]
        [LocalizedStringLength(1024)]
        public string FirstSignPosition { get; set; }
        [LocalizedRequired]
        [LocalizedStringLength(1024)]
        public string SecondSignPosition { get; set; }
        [LocalizedRequired]
        [LocalizedStringLength(1024)]
        public string FirstSignOrganization { get; set; }
        [LocalizedRequired]
        [LocalizedStringLength(1024)]
        public string SecondSignOrganization { get; set; }
        public List<LevelCodeTranslateDlDto> Translates { get; set; }

        protected override Action<IMappingExpression<TDto, LevelCode>> AlterMapping => cfg => cfg
            .ForMember(x => x.Translates, x => x.Ignore());

        public override LevelCode CreateEntity()
        {
            var entity = base.CreateEntity();
            entity.StateId = StateIdConst.ACTIVE;
            Translates.AddByUniqueFKTo(entity.Translates);
            return entity;
        }

        public override void UpdateEntity(LevelCode entity)
        {
            base.UpdateEntity(entity);
            Translates.AddByUniqueFKTo(entity.Translates);
        }
    }
}
