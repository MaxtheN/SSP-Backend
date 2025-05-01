using Microsoft.EntityFrameworkCore;
using SspUis.BizLogicLayer.Hrm.StaffingIndicatorServices;
using SspUis.Core.Security;
using SspUis.DataLayer;
using SspUis.DataLayer.EfClasses;
using SspUis.DataLayer.EfClasses.Hrm;
using SspUis.DataLayer.Repositories;
using StatusGeneric;
using System.Linq;
using WEBASE;
using WEBASE.Models;

namespace SspUis.BizLogicLayer.Hrm.StaffingIndicatorServices
{
    public class StaffingIndicatorService : StatusGenericHandler, IStaffingIndicatorService
    {
        private readonly IStaffingIndicatorRepository _repository;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IAuthService _authService;

        public StaffingIndicatorService(IUnitOfWork unitOfWork, IAuthService authService)
        {
            _repository = unitOfWork.StaffingIndicatorRepository;
            _unitOfWork = unitOfWork;
            _authService = authService;
        }

        public PagedResult<StaffingIndicatorListDto> GetList(SortFilterPageOptions dto)
        {
            var result = _repository.ReadAsNoTracked<StaffingIndicatorListDto>().SortFilter(dto).AsPagedResult(dto);
            return result;
        }

        public StaffingIndicatorDto Get()
        {
            return new StaffingIndicatorDto();
        }

        public StaffingIndicatorDto Get(int id)
        {
            var dto = _repository.ById<StaffingIndicatorDto>(id);
            CombineStatuses(_repository);
            return dto;
        }

        public SelectList<int> AsSelectList(bool filterByOrganizationalStructure = true)
        {
            if (!filterByOrganizationalStructure)
                return _repository.AllAsQueryable.AsSelectList();

            if (_authService.Organization.OrganizationalStructureId.HasValue)
            {
                var organizationalStructure = _repository.Context.Set<OrganizationalStructure>()
                    .Include(a => a.StructureStaffingIndicator)
                    .FirstOrDefault(a => a.Id == _authService.Organization.OrganizationalStructureId);

                if (organizationalStructure != null)
                {
                    var staffingIndicatorIds = organizationalStructure.StructureStaffingIndicator.Select(b => b.StaffingIndicatorId);

                    return _repository.AllAsQueryable
                        .Where(a => staffingIndicatorIds.Contains(a.Id))
                        .AsSelectList();
                }
            }

            return null;
        }

        public HaveId<int> Create(CreateStaffingIndicatorDlDto dto)
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

        public void Update(UpdateStaffingIndicatorDlDto dto)
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

        private void Validation<TDto>(StaffingIndicatorDlDto<TDto> dto, StaffingIndicator entity)
            where TDto : StaffingIndicatorDlDto<TDto>
        {
            var query = _repository.AllAsQueryable;

            if (entity != null)
                query = query.Where(a => a.Id != entity.Id);

        }
    }
}
