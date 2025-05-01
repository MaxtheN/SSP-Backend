using SspUis.DataLayer;
using SspUis.DataLayer.EfClasses;
using SspUis.DataLayer.Repositories;
using StatusGeneric;
using System.Collections.Generic;
using System.Linq;
using WEBASE.Models;

namespace SspUis.BizLogicLayer.Notify
{
    public class SendSmsLogService : StatusGenericHandler, ISendSmsLogService
    {
        private readonly ISendSmsLogRepository _repository;
        private readonly IUnitOfWork _unitOfWork;
        public SendSmsLogService(ISendSmsLogRepository repository, IUnitOfWork unitOfWork)
        {
            _repository = repository;
            _unitOfWork = unitOfWork;
        }
        public List<SendSmsLogListDto> GetList()
        {
            var errorsInLog = _repository
                .ReadAsNoTracked<SendSmsLogListDto>()
                .Where(x => x.ErrorText != null)
                .ToList();

            return errorsInLog;
        }
        public HaveId<long> Create(CreateSendSmsLogDlDto dto)
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
        private void Validation<TDto>(SendSmsLogDlDto<TDto> dto, SendSmsLog entity)
            where TDto : SendSmsLogDlDto<TDto>
        {
            var query = _repository.AllAsQueryable;

            if (entity != null)
                query = query.Where(a => a.Id != entity.Id);
        }
    }
}
