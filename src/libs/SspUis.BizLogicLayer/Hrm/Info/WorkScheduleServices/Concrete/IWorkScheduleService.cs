using SspUis.DataLayer.Repositories;
using StatusGeneric;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WEBASE.AspNet;
using WEBASE.Models;

namespace SspUis.BizLogicLayer.Hrm.WorkScheduleServices
{
    public interface IWorkScheduleService : IStatusGeneric
    {
        PagedResult<WorkScheduleListDto> GetList(SortFilterPageOptions dto);
        WorkScheduleDto Get();
        WorkScheduleDto Get(int id);
        SelectList<int> AsSelectList(int? workScheduleKindId = null);
        HaveId<int> Create(CreateWorkScheduleDlDto dto);
        void Update(UpdateWorkScheduleDlDto dto);
        void Delete(int id);
        List<WorkScheduleWorkHourDto> GetWorkSscheduleWorkHours(int workScheduleId, DateOnly startDate, DateOnly endDate);
    }
}
