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

namespace SspUis.BizLogicLayer.Hrm.EmployeeManageServices
{
    public class EmployeeManageService : StatusGenericHandler, IEmployeeManageService
    {
        private readonly IEmployeeManageRepository _repository;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IAuthService _authService;

        public EmployeeManageService(IUnitOfWork unitOfWork, IAuthService authService)
        {
            _repository = unitOfWork.EmployeeManageRepository;
            _unitOfWork = unitOfWork;
            _authService = authService;
        }

        public PagedResult<EmployeeManageListDto> GetList(EmployeeManageSortFilterPageOptions dto)
        {
            var result = _repository.ReadAsNoTracked<EmployeeManageListDto>()
                                    .SortFilter(dto).AsPagedResult(dto);
            return result;
        }

        public EmployeeManageDto Get()
        {
            return new EmployeeManageDto();
        }

        public EmployeeManageDto Get(long id)
        {
            var dto = _repository.ById<EmployeeManageDto>(id);
            CombineStatuses(_repository);
            return dto;
        }

        //public SelectList<long> AsSelectList(SortFilterPageOptions dto)
        //{
        //    return _repository.ReadAsNoTracked<EmployeeLeaveOrderListDto>()
        //     .SortFilter(dto)
        //     .AsSelectList();
        //}
        public EmployeeCheckForSingDto CheckSigners()
        {
            var today = DateOnly.FromDateTime(DateTime.Today);

            var employeeLeaveOrderIds = _unitOfWork.Context.Set<EmployeeLeaveOrder>()
                .Where(a => a.StatusId == StatusIdConst.ACCEPTED && a.Tables.Any(t => t.EndOn > today))
                .SelectMany(b => b.Tables.Select(t => t.EmployeeManageId))
                .ToList();

            var employeeBusinessTripIds = _unitOfWork.Context.Set<OrderToSendBusinessTrip>()
                .Where(a => a.StatusId == StatusIdConst.ACCEPTED && a.Tables.Any(t => t.EndOn > today))
                .SelectMany(b => b.Tables.Select(t => t.EmployeeManageId))
                .ToList();

            var employeeSickLeaveIds = _unitOfWork.Context.Set<EmployeeSickLeave>()
                .Where(a => a.StatusId == StatusIdConst.ACCEPTED && a.Tables.Any(t => t.EndOn > today))
                .SelectMany(b => b.Tables.Select(t => t.EmployeeManageId))
                .ToList();

            if (employeeLeaveOrderIds.Any() || employeeBusinessTripIds.Any() || employeeSickLeaveIds.Any())
            {
                var employeeLeaveDto = new EmployeeCheckForSingDto
                {
                    EmployeeLeaveOrderIds = employeeLeaveOrderIds,
                    EmployeeBusinessTripIds = employeeBusinessTripIds,
                    EmployeeSickLeaveIds = employeeSickLeaveIds
                };

                return employeeLeaveDto;
            }

            return null;
        }

        public HaveId<long> Create(CreateEmployeeManageDlDto dto)
        {
            var entity = _repository.Create(dto, ent => Validation(dto, ent));
            CombineStatuses(_repository);
            if (HasErrors)
                return null;
            if (IsValid)
            {
                _unitOfWork.Save();
                return HaveId.Create(entity.Id);
            }
            return null;
        }

        public void Update(UpdateEmployeeManageDlDto dto)
        {
            _repository.Update(dto, ent => Validation(dto, ent));
            CombineStatuses(_repository);
            if (IsValid)
                _unitOfWork.Save();
        }

        public void Delete(long id)
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

        private void Validation<TDto>(EmployeeManageDlDto<TDto> dto, EmployeeManage entity)
            where TDto : EmployeeManageDlDto<TDto>
        {
            var query = _repository.AllAsQueryable;

            if (entity != null)
                query = query.Where(a => a.Id != entity.Id);

        }
    }
}
