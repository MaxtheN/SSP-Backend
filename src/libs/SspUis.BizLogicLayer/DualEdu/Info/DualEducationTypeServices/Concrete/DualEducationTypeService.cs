using Microsoft.EntityFrameworkCore;
using SspUis.Core.Security;
using SspUis.DataLayer;
using SspUis.DataLayer.EfClasses.DualEdu;
using SspUis.DataLayer.Repositories;
using StatusGeneric;
using System.Linq;
using WEBASE;
using WEBASE.Models;

namespace SspUis.BizLogicLayer.Hrm.DualEducationTypeServices
{
    public class DualEducationTypeService : StatusGenericHandler, IDualEducationTypeService
    {
        private readonly IDualEducationTypeRepository _repository;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IAuthService _authService;
        public DualEducationTypeService(IUnitOfWork unitOfWork, IAuthService authService)
        {
            _repository = unitOfWork.DualEducationTypeRepository;
            _unitOfWork = unitOfWork;
            _authService = authService;
        }

        public PagedResult<DualEducationTypeListDto> GetList(SortFilterPageOptions dto)
        {
            var result = _repository.ReadAsNoTracked<DualEducationTypeListDto>().SortFilter(dto).AsPagedResult(dto);
            return result;
        }
        public DualEducationTypeDto Get()
        {
            return new DualEducationTypeDto();
        }
        public DualEducationTypeDto Get(int id)
        {
            var dto = _repository.ById<DualEducationTypeDto>(id);
            CombineStatuses(_repository);
            return dto;
        }
        public SelectList<int> AsSelectList()
        {
            return _repository.AllAsQueryable
                .AsSelectList();
        }
        public HaveId<int> Create(CreateDualEducationTypeDlDto dto)
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
        public void Update(UpdateDualEducationTypeDlDto dto)
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
        private void Validation<TDto>(DualEducationTypeDlDto<TDto> dto, DualEducationType entity)
            where TDto : DualEducationTypeDlDto<TDto>
        {
            var query = _repository.AllAsQueryable;

            if (entity != null)
                query = query.Where(a => a.Id != entity.Id);

        }
    }
}