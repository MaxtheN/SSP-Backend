using Microsoft.EntityFrameworkCore;
using SspUis.DataLayer;
using SspUis.DataLayer.EfClasses;
using SspUis.DataLayer.Repositories;
using StatusGeneric;
using System.Linq;
using WEBASE;
using WEBASE.Models;

namespace SspUis.BizLogicLayer
{
    public class RestrictionSendingAppService : StatusGenericHandler, IRestrictionSendingAppService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IRestrictionSendingAppRepository _repository;
        public RestrictionSendingAppService(IUnitOfWork unitOfWork,
            IRestrictionSendingAppRepository repository)
        {
            _unitOfWork = unitOfWork;
            _repository = repository;
        }

        public PagedResult<RestrictionSendingAppListDto> GetList(RestrictionSendingAppDtoSortFilter dto)
        {
            var result = _repository
                .ReadAsNoTracked<RestrictionSendingAppListDto>()
                .SortFilter(dto)
                .AsPagedResult(dto);

            return result;
        }

        public RestrictionSendingAppDto Get()
        {
            return new RestrictionSendingAppDto();
        }

        public RestrictionSendingAppDto Get(long id)
        {
            var dto = _repository.ById<RestrictionSendingAppDto>(id);
            CombineStatuses(_repository);
            return dto;
        }

        public HaveId<long> Create(CreateRestrictionSendingAppDlDto dto)
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

        public void Update(UpdateRestrictionSendingAppDlDto dto)
        {
            _repository.Update(dto, ent => Validation(dto, ent));
            CombineStatuses(_repository);

            if (IsValid) _unitOfWork.Save();
        }

        public void Delete(long id)
        {
            try
            {
                var entity = _unitOfWork.Context
                    .Set<RestrictionOfSendingApplication>()
                    .FirstOrDefault(x => x.Id == id && x.StateId != StateIdConst.PASSIVE);

                if(entity == null)
                {
                    AddError("Бундай модели мавжуд емас.");
                    return;
                }

                _repository.Passive(entity);

                CombineStatuses(_repository);

                if (IsValid) _unitOfWork.Save();
            }
            catch (DbUpdateException)
            {
                AddError("Запись не может быть удален");
            }
        }

        private void Validation<TDto>(RestrictionSendingAppDlDto<TDto> dto, RestrictionOfSendingApplication entity)
            where TDto : RestrictionSendingAppDlDto<TDto>
        {
            var query = _repository.AllAsQueryable;

            if(dto.StartAt > dto.EndAt)
            {
                AddError("дата начала не может быть больше даты окончания");
                return;
            }

            if (entity != null)
                query = query.Where(a => a.Id != entity.Id);
        }
    }
}
