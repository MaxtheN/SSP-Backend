using WEBASE.Models;
using Newtonsoft.Json;
using StatusGeneric;
using Microsoft.AspNetCore.Http;
using SspUis.DataLayer.Repositories;
using SspUis.Core.Security;
using SspUis.DataLayer.EfClasses;
using System.Collections.Generic;
using System.Linq;
using System;
using SspUis.DataLayer;
using Microsoft.EntityFrameworkCore;

namespace SspUis.BizLogicLayer.DocumentChangeLogServices
{
    public class DocumentChangeLogService : StatusGenericHandler, IDocumentChangeLogService
    {
        private readonly IDocumentChangeLogRepository _repository;
        private readonly IDocumentHistoryRepository _documentHistoryRepository;
        private readonly IAuthService _authService;
        private readonly IUnitOfWork _uniteOfWork;

        public DocumentChangeLogService(
            IDocumentChangeLogRepository repository,
            IAuthService authService,
			IUnitOfWork uniteOfWork,
            IDocumentHistoryRepository documentHistoryRepository
            )
        {
			_uniteOfWork = uniteOfWork;
            _repository = repository;
            _authService = authService;
            _documentHistoryRepository = documentHistoryRepository;
        }

        public HaveId<long> Create<TId, TDocumentDto>(TDocumentDto dto, int tableId, int? organizationId, int statusId, string message = null, string userIp = null, string userAgent = null)
           where TDocumentDto : class, IDocument<TId>
        {
            var log = new CreateDocumentChangeLogDlDto
            {
                DocId = long.Parse(dto.Id.ToString()),
                DateAt = DateTime.Now,
                StatusId = statusId,
                TableId = tableId,
                IpAddress = string.IsNullOrEmpty(userIp) ? _authService?.UserIp : userIp,
                UserAgent = string.IsNullOrEmpty(userAgent) ? _authService?.UserAgent : userAgent,
                UserId = (_authService?.UserId != null) ? (int)_authService?.UserId : 0,
				UserInfo = (_authService?.UserId != null) ? _authService?.User?.ToString() : "{}", // todo: need to clearify format
                Message = message,
            };

            var entity = _repository.Create(log);

            CombineStatuses(_repository);
            if (HasErrors)
                return null;
            _repository.Context.SaveChanges();

            _documentHistoryRepository.Create(new DocumentHistoryDlDto
            {
                ChangeLogId = entity.Id,
                DateAt = DateTime.Now,
                DocId = long.Parse(dto.Id.ToString()),
                OrganizationId = organizationId ?? 0,
                StatusId = statusId,
                TableId = tableId,
                IpAddress = string.IsNullOrEmpty(userIp) ? _authService.UserIp : userIp,
                UserAgent = string.IsNullOrEmpty(userAgent) ? _authService?.UserAgent : userAgent,
                UserId = (_authService?.UserId != null) ? (int)_authService?.UserId : 0,
                UserInfo = (_authService?.UserId != null) ? _authService?.User?.ToString() : "{}", // todo: need to clearify format
                Message = message,
                DocContent = JsonConvert.SerializeObject(dto),
            });

            CombineStatuses(_documentHistoryRepository);
            if (HasErrors)
                return null;
            _documentHistoryRepository.Context.SaveChanges();

            return HaveId.Create(entity.Id);
        }

        public HaveId<long> Create<TDocumentDto>(TDocumentDto dto, int tableId, int? organizationId, int statusId, string message = null, string userIp = null, string userAgent = null)
            where TDocumentDto : class, IDocument
        {
            return Create<long, TDocumentDto>(dto, tableId, organizationId, statusId, message, userIp, userAgent);
        }
        HaveId<long> IDocumentChangeLogService.CreateApplication<TApplicationDto>(TApplicationDto dto, int? organizationId, string message)
        {
            var log = new CreateDocumentChangeLogDlDto
            {
                DocId = long.Parse(dto.Id.ToString()),
                DateAt = DateTime.Now,
                StatusId = dto.Application.StatusId,
                TableId = dto.Application.TableId,
                IpAddress = _authService?.UserIp ?? "anonymous",
                UserAgent = _authService?.UserAgent ?? "anonymous",
                UserId = (int?)_authService?.UserId ?? 0,
                UserInfo = _authService?.User?.ToString() ?? "{}", // todo: need to clearify format
                Message = message,
            };

            var entity = _repository.Create(log);

            CombineStatuses(_repository);
            if (HasErrors)
                return null;
            _repository.Context.SaveChanges();

            _documentHistoryRepository.Create(new DocumentHistoryDlDto
            {
                ChangeLogId = entity.Id,
                DateAt = DateTime.Now,
                DocId = long.Parse(dto.Id.ToString()),
                OrganizationId = organizationId ?? 0,
                StatusId = dto.Application.StatusId,
                TableId = dto.Application.TableId,
                IpAddress = _authService?.UserIp ?? "anonymous",
                UserAgent = _authService?.UserAgent ?? "anonymous",
                UserId = (int?)_authService?.UserId ?? 0,
                UserInfo = _authService?.User?.ToString() ?? "{}", // todo: need to clearify format
                Message = message,
                DocContent = JsonConvert.SerializeObject(dto),
            });

            CombineStatuses(_documentHistoryRepository);
            if (HasErrors)
                return null;
            _documentHistoryRepository.Context.SaveChanges();

            return HaveId.Create(entity.Id);
        }
        public List<DocumentChangeLogListDto> GetListByDocumentId(int tableId, long docId)
        {
            var result = _repository.ReadAsNoTracked<DocumentChangeLogListDto>(
                a => a.TableId == tableId &&
                a.DocId == docId)
                              .OrderByDescending(a => a.Id)
                              .ToList();
            var ttttt = _uniteOfWork.Context.Set<DocumentChangeLog>().Include(a => a.Status).ThenInclude(b=> b.Translates)
                 .Where(x => x.TableId == tableId && x.DocId == docId)
                                .OrderByDescending(a => a.Id)
                               .ToList();   

            if(result.Count == 0)
            {
                result = ttttt.Select(x => new DocumentChangeLogListDto 
                { 
                   UserInfo = x.UserInfo,
                   Message = x.Message,
                   DocId = x.DocId,
                   UserAgent = x.UserAgent,
                   DateAt = x.DateAt,
                   Id = x.Id,
                   IpAddress = x.IpAddress,
                   TableId = tableId,
                   StatusId = x.StatusId,
                   UserId = x.UserId,
                   //Status = x.Status.FullName??"",
                   Status = x.Status?.Translates.AsQueryable().FirstOrDefault(StatusTranslate
                                    .GetExpr(TranslateColumn.full_name, ServiceProvider.CultureHelper.CurrentCulture.Id))
                                    ?.TranslateText ?? x.Status.FullName?? " "
                  
                }).ToList();
			}
            return result;
        }
    }
}
