using GenericServices;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Newtonsoft.Json;
using System.Threading.Tasks;
using WEBASE.Attributes;
using WEBASE.Models;
using SspUis.DataLayer.Repositories;
using SspUis.DataLayer.EfClasses;
using SspUis.DataLayer.EfClasses.Hrm;

namespace SspUis.BizLogicLayer.Hrm.WorkScheduleServices
{
    public class WorkScheduleDto : UpdateWorkScheduleDlDto, ILinkToEntity<WorkSchedule>
    {
        public string State { get; internal set; }
        public string WorkScheduleKind { get; internal set; }
        public string FullName { get; internal set; }
        public string ShortName { get; internal set; }
        //new public List<WorkScheduleTranslateDto> Translates { get; set; } = new();
        new public List<WorkScheduleDayHourDto> DayHours { get; set; } = new();
        new public List<WorkScheduleWorkHourDto> WorkHours { get; set; } = new();
    }
}
