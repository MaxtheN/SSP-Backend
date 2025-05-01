using System;
using System.Collections.Generic;
using AutoMapper;
using SspUis.DataLayer.EfClasses.Hrm;
using WEBASE;
using WEBASE.Attributes;
using WEBASE.EF;

namespace SspUis.DataLayer.Repositories
{
    public class StateAwardDlDto<TDto> : EntityDto<TDto, StateAward>
        where TDto : StateAwardDlDto<TDto>
    {
        [LocalizedRequired]
        [LocalizedStringLength(9)]
        public string Code { get; set; }
        [LocalizedRequired]
        [LocalizedStringLength(250)]
        public string ShortName { get; set; }
        [LocalizedRequired]
        [LocalizedStringLength(500)]
        public string FullName { get; set; }
        public List<StateAwardTranslateDlDto> Translates { get; set; } = new List<StateAwardTranslateDlDto>();

        protected override Action<IMappingExpression<TDto, StateAward>> AlterMapping => cfg => cfg
            .ForMember(x => x.Translates, x => x.Ignore());

        public override StateAward CreateEntity()
        {
            var entity = base.CreateEntity();
            entity.StateId = StateIdConst.ACTIVE;
            Translates.AddByUniqueFKTo(entity.Translates);
            return entity;
        }

        public override void UpdateEntity(StateAward entity)
        {
            base.UpdateEntity(entity);
            Translates.AddByUniqueFKTo(entity.Translates);
        }
    }
}
