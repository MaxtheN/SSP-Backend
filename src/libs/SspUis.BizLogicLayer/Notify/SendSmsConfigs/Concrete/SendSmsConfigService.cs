using Microsoft.EntityFrameworkCore;
using SspUis.Core.Security;
using SspUis.DataLayer;
using SspUis.DataLayer.EfClasses;
using SspUis.DataLayer.Repositories;
using StatusGeneric;
using System.Linq;
using WEBASE;
using WEBASE.Models;

namespace SspUis.BizLogicLayer.Notify
{
    public class SendSmsConfigService : StatusGenericHandler, ISendSmsConfigService
    {
        private readonly ISendSmsConfigRepository _repository;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IAuthService _authService;
        public SendSmsConfigService(
            IUnitOfWork unitOfWork,
            IAuthService authService)
        {
            _repository = unitOfWork.SendSmsConfigRepository;
            _unitOfWork = unitOfWork;
            _authService = authService;
        }

        public PagedResult<SendSmsConfigListDto> GetList(SendSmsConfigListDtoSortFilterPageOption dto)
        {
            var result = _repository
                .ReadAsNoTracked<SendSmsConfigListDto>()
                .SortFilter(dto)
                .AsPagedResult(dto);

            return result;
        }

        public HaveId<int> Create(CreateSendSmsConfigDlDto dto)
        {
            var entity = _repository.Create(dto, ent => Validation(dto, ent));
            CombineStatuses(_repository);

            if (HasErrors) return null;

            if (IsValid)
            {
                _unitOfWork.Save();
                return HaveId.Create(entity.Id);
            }
            return null;
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

        public SendSmsConfigDto Get()
        {
            return new SendSmsConfigDto();
        }

        public SendSmsConfigDto Get(int id)
        {
            var dto = _repository.ById<SendSmsConfigDto>(id);
            CombineStatuses(_repository);
            return dto;
        }

        public void Update(UpdateSendSmsConfigDlDto dto)
        {
            _repository.Update(dto, ent => Validation(dto, ent));

            CombineStatuses(_repository);

            if (IsValid)
                _unitOfWork.Save();
        }

        private void Validation<TDto>(SendSmsConfigDlDto<TDto> dto, SendSmsConfig entity)
            where TDto : SendSmsConfigDlDto<TDto>
        {
            var query = _repository.AllAsQueryable;

            if (entity != null)
                query = query.Where(a => a.Id != entity.Id);
        }
    }
}
