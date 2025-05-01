using System;
using AutoMapper;
using SspUis.DataLayer.EfClasses;
using WEBASE;
using WEBASE.Attributes;
using WEBASE.EF;

namespace SspUis.DataLayer.Repositories
{
    public class ArbitrationJudgeDlDto<TDto> : EntityDto<TDto, ArbitrationJudge>
        where TDto : ArbitrationJudgeDlDto<TDto>
    {
        //[LocalizedRequired]
        //[LocalizedStringLength(100)]
        //public string FirstName { get; set; }
        //[LocalizedRequired]
        //[LocalizedStringLength(100)]
        //public string LastName { get; set; }
        //public string MiddleName { get; set; }
        [LocalizedRequired]
        public string PassportSeria { get; set; }
        [LocalizedRequired]
        public string PassportNumber { get; set; }
        [LocalizedRequired]
        public DateOnly BirthDate { get; set; }
        [LocalizedRequired]
        [LocalizedStringLength(250)]
        public string PositionName { get; set; }
        [LocalizedRequired]
        [LocalizedStringLength(250)]
        public string OrganizationName { get; set; }
        //[LocalizedRequired]
        //[LocalizedRange(1, int.MaxValue)]
        public int? PersonId { get; set; }
        [LocalizedRequired]
        [LocalizedRange(1, int.MaxValue)]
        public int RegionId { get; set; }
        public int? DistrictId { get; set; }
        protected override Action<IMappingExpression<TDto, ArbitrationJudge>> AlterMapping => cfg => cfg
            .ForMember(x => x.BirthDate, c => c.Ignore());

        public override ArbitrationJudge CreateEntity()
        {
            var entity = base.CreateEntity();
            entity.StateId = StateIdConst.ACTIVE;
            //if (BirthDate != null)
            //    entity.BirthDate = BirthDate.Value.AsDateOnly();
            return entity;
        }

        public override void UpdateEntity(ArbitrationJudge entity)
        {
            base.UpdateEntity(entity);
        }
    }
}
