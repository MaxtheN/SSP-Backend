using GenericServices;
using StatusGeneric;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WEBASE;
using WEBASE.AspNet;
using WEBASE.Models;
using SspUis.Core.Security;
using SspUis.DataLayer.EfClasses;
using SspUis.DataLayer;
using SspUis.DataLayer.Repositories;
using Microsoft.EntityFrameworkCore;
using SspUis.Core;
using SspUis.DataLayer.EfClasses.Hrm;

namespace SspUis.BizLogicLayer.Hrm.WorkScheduleServices
{
    public class WorkScheduleService : StatusGenericHandler, IWorkScheduleService
    {
        private readonly IWorkScheduleRepository _repository;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IAuthService _authService;

        public WorkScheduleService(IUnitOfWork unitOfWork, IAuthService authService)
        {
            _repository = unitOfWork.WorkScheduleRepository;
            _unitOfWork = unitOfWork;
            _authService = authService;
        }

        public PagedResult<WorkScheduleListDto> GetList(SortFilterPageOptions dto)
        {
            var result = _repository.ReadAsNoTracked<WorkScheduleListDto>().SortFilter(dto).AsPagedResult(dto);
            return result;
        }

        public WorkScheduleDto Get()
        {
            return new WorkScheduleDto();
        }

        public WorkScheduleDto Get(int id)
        {
            //var dto = _repository.ById<WorkScheduleDto>(id);
            //CombineStatuses(_repository);
            //return dto;
            var dto = _repository.ById<WorkScheduleDto>(id);

            if (dto != null && dto.WorkHours != null)
            {
                dto.WorkHours = dto.WorkHours.OrderBy(wh => wh.DateOn.Month).ThenBy(wh => wh.DateOn.Day).ToList();
                dto.Translates.Clear();
            }

            CombineStatuses(_repository);
            return dto;
        }

        public SelectList<int> AsSelectList(int? workScheduleKindId = null)
        {
            return _repository.AllAsQueryable
                .AsSelectList(workScheduleKindId);
        }

        public HaveId<int> Create(CreateWorkScheduleDlDto dto)
        {
            var entity = _repository.Create(dto, ent => Validation(dto, ent));
            CombineStatuses(_repository);
            if (IsValid)
            {
                _unitOfWork.Save();
                return HaveId.Create(entity.Id);
            }
            return null;
        }

        public void Update(UpdateWorkScheduleDlDto dto)
        {
            _repository.Update(dto, ent => Validation(dto, ent));
            CombineStatuses(_repository);
            if (IsValid)
                _unitOfWork.Save();
        }

        public void Delete(int id)
        {
            try
            {
                _repository.Delete(id);
                CombineStatuses(_repository);
                if (IsValid)
                    _unitOfWork.Save();
            }
            catch (DbUpdateException)
            {
                AddError("Запись не может быть удален");
            }
        }

        private void Validation<TDto>(WorkScheduleDlDto<TDto> dto, WorkSchedule entity)
            where TDto : WorkScheduleDlDto<TDto>
        {
            var query = _repository.AllAsQueryable;

            if (entity != null)
                query = query.Where(a => a.Id != entity.Id);

        }

        //GetWorkSscheduleWorkHours in EduErp
        public List<WorkScheduleWorkHourDto> GetWorkSscheduleWorkHours(int workScheduleId, DateOnly startDate, DateOnly endDate)
        {
            List<WorkScheduleWorkHourDto> workSscheduleWorkHours = new();
            var workScheduleWorkHours = _unitOfWork.Context.Set<WorkScheduleWorkHour>().Where(a => a.OwnerId == workScheduleId && a.DateOn >= startDate && a.DateOn <= endDate);

            foreach (var table in workScheduleWorkHours)
            {
                var workscheduleworkhour = new WorkScheduleWorkHourDto()
                {
                    Id = table.Id,
                    OwnerId = table.OwnerId,
                    DateOn = table.DateOn,
                    Days = table.Days,
                    Hours = table.Hours
                };
                workSscheduleWorkHours.Add(workscheduleworkhour);
            }
            return workSscheduleWorkHours;
        }
    }
}
