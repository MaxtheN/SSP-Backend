using AutoMapper;
using GenericServices;
using GenericServices.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SspUis.DataLayer.EfClasses;
using SspUis.DataLayer.Repositories;
using SspUis.DataLayer.EfClasses.Hrm;

namespace SspUis.BizLogicLayer.Hrm.WorkScheduleServices
{
    public class WorkScheduleTranslateDto : WorkScheduleTranslateDlDto, ILinkToEntity<WorkScheduleTranslate>
    {
        public string Language { get; set; }
    }
    public class WorkScheduleTranslateDtoConfig : PerDtoConfig<WorkScheduleTranslateDto, WorkScheduleTranslate>
    {
        public override Action<IMappingExpression<WorkScheduleTranslate, WorkScheduleTranslateDto>> AlterReadMapping => cfg => cfg
                .IncludeBase<WorkScheduleTranslate, WorkScheduleTranslateDlDto>()
                .ForMember(x => x.Language, x => x.MapFrom(ent => ent.Language.FullName));
    }
}
