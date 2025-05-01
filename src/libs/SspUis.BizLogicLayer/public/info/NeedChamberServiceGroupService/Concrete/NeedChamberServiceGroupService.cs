using Microsoft.EntityFrameworkCore;
using SspUis.Core.Security;
using SspUis.DataLayer;
using SspUis.DataLayer.EfClasses;
using SspUis.DataLayer.Repositories;
using StatusGeneric;
using System.Linq;
using WEBASE;
using WEBASE.Models;

namespace SspUis.BizLogicLayer
{
    public class NeedChamberServiceGroupService : StatusGenericHandler, INeedChamberServiceGroupService
    {
        private readonly INeedChamberServiceGroupRepository _repository;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IAuthService _authService;
        public NeedChamberServiceGroupService(
            INeedChamberServiceGroupRepository repository,
            IUnitOfWork unitOfWork,
            IAuthService authService)
        {
            _repository = repository;
            _unitOfWork = unitOfWork;
            _authService = authService;
        }

        public PagedResult<NeedChamberServiceGroupListDto> GetList(SortFilterPageOptions dto)
        {
            var result = _repository.ReadAsNoTracked<NeedChamberServiceGroupListDto>()
                .SortFilter(dto)
                .AsPagedResult(dto);

            return result;
        }

        public NeedChamberServiceGroupDto Get()
        {
            return new NeedChamberServiceGroupDto();
        }

        public NeedChamberServiceGroupDto Get(int id)
        {
            var res = _repository.ById<NeedChamberServiceGroupDto>(id);
            CombineStatuses(_repository);
            return res;
        }

        public SelectList<int> AsSelectList()
            => _repository.AllAsQueryable.AsSelectList();

        public HaveId<int> Create(CreateNeedChamberServiceGroupDlDto dto)
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

        public void Update(UpdateNeedChamberServiceGroupDlDto dto)
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

        private void Validation<TDto>(NeedChamberServiceGroupDlDto<TDto> dto, NeedChamberServiceGroup entity)
            where TDto : NeedChamberServiceGroupDlDto<TDto>
        {
            var query = _repository.AllAsQueryable;

            if (entity != null)
                query = query.Where(a => a.Id != entity.Id);
        }
    }
}
