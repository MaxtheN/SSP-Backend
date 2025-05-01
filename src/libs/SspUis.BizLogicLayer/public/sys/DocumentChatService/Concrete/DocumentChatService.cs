using SspUis.DataLayer;
using SspUis.DataLayer.EfClasses;
using SspUis.DataLayer.Repositories;
using StatusGeneric;
using System;
using System.Linq;
using WEBASE;
using WEBASE.Models;

namespace SspUis.BizLogicLayer
{
    public class DocumentChatService : StatusGenericHandler, IDocumentChatService
    {
        private readonly IDocumentChatRepository _repository;
        private readonly IUnitOfWork _unitOfWork;
        public DocumentChatService(IDocumentChatRepository repository, IUnitOfWork unitOfWork)
        {
            _repository = repository;
            _unitOfWork = unitOfWork;
        }

        public PagedResult<DocumentChatListDto> GetList(DocumentChatListDtoSortFilterPageOption dto)
            => _repository.ReadAsNoTracked<DocumentChatListDto>()
                .SortFilter(dto)
                .AsPagedResult(dto);
        public HaveId<long> Create(CreateDocumentChatDlDto dto)
        {
            try
            {
                var entity = _repository.Create(dto, ent => Validation(dto, ent));

                CombineStatuses(_repository);
                if (HasErrors) return null;

                if (IsValid)
                {
                    _unitOfWork.Save();
                    return HaveId.Create(entity.Id);
                }
            }
            catch (Exception ex)
            {
                AddError(ex.Message);
                if (ex.InnerException is not null) AddError(ex.InnerException.Message);
            }

            return null;
        }
        public void Delete(long id)
        {
            try
            {
                var chat = _repository.ById(id);
                chat.Delete(ref chat);

                _unitOfWork.Save();
            }
            catch (Exception ex)
            {
                AddError($"Запись не может быть удален: {ex.Message}");
            }
        }
        private void Validation<TDto>(DocumentChatDlDto<TDto> dto, DocumentChat entity)
            where TDto : DocumentChatDlDto<TDto>
        {
            var query = _repository.AllAsQueryable;

            if (entity != null)
                query = query.Where(a => a.Id != entity.Id);
        }
    }
}
