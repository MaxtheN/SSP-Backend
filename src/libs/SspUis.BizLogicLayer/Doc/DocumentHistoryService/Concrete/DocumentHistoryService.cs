using SspUis.DataLayer.EfClasses;
using SspUis.DataLayer.Repositories;
using StatusGeneric;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WEBASE;
using WEBASE.Models;

namespace SspUis.BizLogicLayer.DocumentHistoryService
{
    public class DocumentHistoryService : StatusGenericHandler, IDocumentHistoryService
    {

        private readonly IDocumentHistoryRepository _repository;

        public DocumentHistoryService(
            IDocumentHistoryRepository repository
            )
        {
            _repository = repository;
        }

        public DocumentHistoryCompareDto CompareContents(int tableId, long previouesId, long currentId)
        {
            var previous = _repository.AllAsQueryable.FirstOrDefault(a => a.TableId == tableId && a.ChangeLogId == previouesId);
            var current = _repository.AllAsQueryable.FirstOrDefault(a => a.TableId == tableId && a.ChangeLogId == currentId);

            return new DocumentHistoryCompareDto
            {
                TableId = tableId,
                PreviousDocContent = previous?.DocContent,
                CurrentDocContent = current?.DocContent
            };
        }

        public PagedResult<DocumentHistoryListDto> GetList(DocumentSortFilterDto dto)
        {
            return _repository.ReadAsNoTracked<DocumentHistoryListDto>(a => a.TableId == dto.TableId && a.DocId == dto.DocId)
                              .OrderBy(a => a.Id)
                              .AsPagedResult(dto);
        }

        public DocLastMessageResponseDto GetLastMessage(DocLastMessageRequestDto dto)
        {
            var contractLastHistory = _repository.Context.Set<DocumentChangeLog>()
                .Where(a => a.TableId == dto.TableId && a.DocId == dto.DocumentId)
                .OrderByDescending(a => a.Id)
                .FirstOrDefault();

            if (contractLastHistory == null)
                return new DocLastMessageResponseDto();

            var previousContractLastHistory = _repository.Context.Set<DocumentChangeLog>()
                .Where(a => a.TableId == dto.TableId && a.DocId == dto.DocumentId && a.Id < contractLastHistory.Id)
                .OrderByDescending(a => a.Id)
                .FirstOrDefault();

            return new DocLastMessageResponseDto()
            {
                PreviaousStatusId = previousContractLastHistory != null ? previousContractLastHistory.StatusId : null,
                CurrenctStatusId = contractLastHistory.StatusId,
                Message = contractLastHistory.Message
            };
        }
    }
}
