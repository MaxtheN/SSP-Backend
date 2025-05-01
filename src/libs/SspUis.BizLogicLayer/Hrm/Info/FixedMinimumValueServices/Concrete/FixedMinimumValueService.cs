using System.Linq;
using Microsoft.EntityFrameworkCore;
using SspUis.Core.Security;
using SspUis.DataLayer;
using SspUis.DataLayer.EfClasses.Hrm;
using SspUis.DataLayer.Repositories;
using StatusGeneric;
using WEBASE;
using WEBASE.Models;

namespace SspUis.BizLogicLayer.Hrm.FixedMinimumValueServices
{
    public class FixedMinimumValueService : StatusGenericHandler, IFixedMinimumValueService
    {
        private readonly IFixedMinimumValueRepository _repository;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IAuthService _authService;

        public FixedMinimumValueService(IUnitOfWork unitOfWork, IAuthService authService)
        {
            _repository = unitOfWork.FixedMinimumValueRepository;
            _unitOfWork = unitOfWork;
            _authService = authService;
        }

        public PagedResult<FixedMinimumValueListDto> GetList(SortFilterPageOptions dto)
        {
            var result = _repository.ReadAsNoTracked<FixedMinimumValueListDto>(applyFilter: false)
                .SortFilter(dto).AsPagedResult(dto);
            return result;
        }

        public FixedMinimumValueDto Get()
        {
            return new FixedMinimumValueDto();
        }

        public FixedMinimumValueDto Get(long id)
        {
            var dto = _repository.ById<FixedMinimumValueDto>(id, applyFilter: false);
            CombineStatuses(_repository);
            return dto;
        }

        public SelectList<long> AsSelectList()
        {
            return _repository.AllAsQueryable
                .AsSelectList();
        }

        public HaveId<long> Create(CreateFixedMinimumValueDlDto dto)
        {
            var oldValue = _repository.AllAsQueryable.FirstOrDefault(x => x.MinimumValueTypeId == dto.MinimumValueTypeId);
            if (oldValue != null && oldValue.StateId == StateIdConst.ACTIVE)
            {
                oldValue.StateId = StateIdConst.PASSIVE;
            }
            var entity = _repository.Create(dto, ent => Validation(dto, ent));
            CombineStatuses(_repository);
            if (IsValid)
            {
                _unitOfWork.Save();
                return HaveId.Create(entity.Id);
            }
            return null;
        }

        public void Update(UpdateFixedMinimumValueDlDto dto)
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
                var entity = _repository.AllAsQueryable.FirstOrDefault(x => x.Id == id);
                if (entity == null)
                { AddError("Not found"); return; }
                entity.StateId = StateIdConst.PASSIVE;
                _unitOfWork.Save();
                //_repository.Delete(id);
                CombineStatuses(_repository);
                if (IsValid)
                    _unitOfWork.Save();
            }
            catch (DbUpdateException)
            {
                AddError("Запись не может быть удален");
            }
        }

        private void Validation<TDto>(FixedMinimumValueDlDto<TDto> dto, FixedMinimumValue entity)
            where TDto : FixedMinimumValueDlDto<TDto>
        {
            var query = _repository.AllAsQueryable;

            if (entity != null)
                query = query.Where(a => a.Id != entity.Id);

        }
    }
}
