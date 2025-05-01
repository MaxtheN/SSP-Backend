using AutoMapper;
using SspUis.DataLayer.EfClasses;
using SspUis.DataLayer.EfClasses.Hrm;
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
    public class WorkScheduleDlDto<TDto> : EntityDto<TDto, WorkSchedule>
        where TDto : WorkScheduleDlDto<TDto>
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
        [LocalizedRequired]
        public int WorkScheduleKindId { get; set; }

        public List<WorkScheduleTranslateDlDto> Translates { get; set; } = new();
        public List<WorkScheduleDayHourDlDto> DayHours { get; set; } = new List<WorkScheduleDayHourDlDto>();
        public List<WorkScheduleWorkHourDlDto> WorkHours { get; set; } = new List<WorkScheduleWorkHourDlDto>();

        protected override Action<IMappingExpression<TDto, WorkSchedule>> AlterMapping => cfg => cfg
            .ForMember(x => x.Translates, x => x.Ignore())
            .ForMember(x => x.DayHours, x => x.Ignore())
            .ForMember(x => x.WorkHours, x => x.Ignore());

        public override WorkSchedule CreateEntity()
        {
            var entity = base.CreateEntity();
            entity.StateId = StateIdConst.ACTIVE;
            DayHours.AddTo(entity.DayHours);
            WorkHours.AddTo(entity.WorkHours);
            Translates.AddByUniqueFKTo(entity.Translates);
            return entity;
        }

        public override void UpdateEntity(WorkSchedule entity)
        {
            base.UpdateEntity(entity);
            DayHours.ApplyChangesTo<long, WorkScheduleDayHourDlDto, WorkScheduleDayHour>(entity.DayHours);
            WorkHours.ApplyChangesTo<long, WorkScheduleWorkHourDlDto, WorkScheduleWorkHour>(entity.WorkHours);
            Translates.AddByUniqueFKTo(entity.Translates);
        }
    }
}
