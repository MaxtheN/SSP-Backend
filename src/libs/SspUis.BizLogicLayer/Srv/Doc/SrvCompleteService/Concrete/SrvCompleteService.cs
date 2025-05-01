using System;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using SspUis.BizLogicLayer.DocumentChangeLogServices;
using SspUis.Core;
using SspUis.Core.Security;
using SspUis.DataLayer;
using SspUis.DataLayer.EfClasses;
using SspUis.DataLayer.Repositories;
using SspUis.ServiceLayer.NumberServices;
using WEBASE;
using WEBASE.Models;
using static SspUis.BizLogicLayer.UpdateStatusCompleteService;

namespace SspUis.BizLogicLayer
{
    public class SrvCompleteService
         : BaseEntityService<long, CompletedService, CompletedServiceListDto, CompletedServiceDto, CreateCompletedServiceDlDto, UpdateCompletedServiceDlDto,
            ICompletedServiceRepository, SrvCompleteSortFilterOption>,
        ISrvCompleteService
    {
        private readonly IAuthService _authService;
        private readonly ICompletedServiceRepository _repository;
        private readonly IDocumentChangeLogService _documentChangeLogService;
        private readonly IUnitOfWork unitOfWork;
        private readonly INumberService _numberService;
        public SrvCompleteService(
            IUnitOfWork unitOfWork,
            IAuthService authService,
            IDocumentChangeLogService documentChangeLogService,
            ICompletedServiceRepository repository,
            INumberService numberService)
            : base(unitOfWork)
        {
            _documentChangeLogService = documentChangeLogService;
            _authService = authService;
            _repository = repository;
            _numberService = numberService;
        }

        public override PagedResult<CompletedServiceListDto> GetList(SrvCompleteSortFilterOption options)
        {
            return Repository.ReadAsNoTracked<CompletedServiceListDto>()
                .SortFilter(options)
                .AsPagedResult(options);
        }
        public CompletedServiceDto Get()
        {
            return new CompletedServiceDto()
            {
                DocOn = DateOnly.FromDateTime(DateTime.Now),
                DocNumber = _numberService.GetNext(NumberTemplateDocumentConst.DOC_COMPLETE_SERVICE, 1).Item2
            };
        }
        public CompletedServiceDto Get(long id) => Repository.ById<CompletedServiceDto>(id);
        public HaveId<long> Create(CreateCompletedServiceDlDto dto)
        {
            var canCommit = UnitOfWork.CurrentTransaction == null;
            var transaction = UnitOfWork.CurrentTransaction ?? UnitOfWork.BeginTransaction();
            try
            {
                var entity = Repository.Create(dto, ent => Validation(dto, ent));

                UnitOfWork.Save();
                CombineStatuses(Repository);

                if (IsValid && canCommit)
                    transaction.Commit();

                return HaveId.Create(entity.Id);
            }
            catch (DbUpdateException e)
            {
                AddError(e.Message);
                transaction.Rollback();
            }

            return null;

        }
        public void Update(UpdateCompletedServiceDlDto dto)
        {
            using (var transaction = UnitOfWork.BeginTransaction())
            {
                try
                {
                    var entity = Repository.Update(dto, ent => Validation(dto, ent));

                    UnitOfWork.Save();

                    CombineStatuses(Repository);
                    if (IsValid)
                        transaction.Commit();
                }
                catch (DbUpdateException e)
                {
                    AddError(e.Message);
                    transaction.Rollback();
                }
                finally
                {
                    transaction.Dispose();
                }
            }
        }
        public void Delete(long id)
        {
            try
            {
                var entity = Repository.ById(id);

                var statusDto = new UpdateStatusCompletedServiceDlDto()
                {
                    Id = id,
                    StatusId = StatusIdConst.DELETED
                };

                this.UpdateStatus(statusDto, ent =>
                {
                    if (!StatusIdConst.CanApplySrvDocStatus(ent.StatusId, statusDto.StatusId))
                        Repository.AddError("Нет доступа");
                });

                if (HasErrors)
                    return;

                UnitOfWork.Save();
            }
            catch (DbUpdateException ex)
            {
                AddError(ex.Message);
                return;
            }
        }
        public void Cancel(CancelStatusCompletedSrvDto dto)
        {
            using (var transaction = unitOfWork.BeginTransaction())
            {
                try
                {
                    Repository.AllAsQueryable.Lock(dto.Id);
                    var contract = Repository.UpdateStatus(dto, ent =>
                    {
                        if (!StatusIdConst.CanApplySrvDocStatus(ent.StatusId, dto.StatusId))
                            AddError("Имкони йўқ / Нет доступа");
                    });

                    CombineStatuses(Repository);
                    if (HasErrors)
                        return;

                    UnitOfWork.Save();

                    var log = CreateDocumentChangeLog(dto.Id, StatusIdConst.CANCELED, dto.Details);
                    if (IsValid)
                        transaction.Commit();
                }
                catch (Exception ex)
                {
                    AddError($"{ex.Message} - {ex.InnerException}");
                    return;
                }
                finally
                {
                    transaction.Dispose();
                }
            }
        }
        public void Accept(AcceptStatusCompletedSrvDto dto)
        {
            var completeService = unitOfWork.Context.Set<CompletedService>()
                .Where(x => x.StatusId != StatusIdConst.CANCELED && x.StatusId != StatusIdConst.DELETED)
                .FirstOrDefault(x => x.Id == dto.Id);

            if (completeService == null)
            { AddError("Бундай тўлиқ хизмат мавжуд емас !"); return; }

            var payment = unitOfWork.Context.Set<MemshipPaymentOrder>()
                .Where(x => x.StatusId != StatusIdConst.DELETED
                    && x.StatusId != StatusIdConst.CANCELED
                    && x.ApplicationTypeId == ApplicationTypeIdConst.SERVICE)
                .FirstOrDefault(x => x.ServiceContractId == completeService.ServiceContractId);

            if (payment is null || payment.Amount < completeService.ServiceContract.Groups.SelectMany(x => x.Tables).Sum(x => x.Price))
            {
                AddError($"Тўлов ҳужжат мавжуд емас ёки нарх тўлиқ тўланмаган !");
                return;
            }

            using (var transaction = unitOfWork.BeginTransaction())
            {
                try
                {
                    Repository.AllAsQueryable.Lock(dto.Id);
                    var contract = Repository.UpdateStatus(dto, ent =>
                    {
                        if (!StatusIdConst.CanApplySrvDocStatus(ent.StatusId, dto.StatusId))
                            AddError("Имкони йўқ / Нет доступа");
                    });

                    CombineStatuses(Repository);
                    if (HasErrors)
                        return;

                    UnitOfWork.Save();

                    var log = CreateDocumentChangeLog(dto.Id, StatusIdConst.CANCELED, dto.Details);
                    if (IsValid)
                        transaction.Commit();
                }
                catch (Exception ex)
                {
                    AddError($"{ex.Message} - {ex.InnerException}");
                    return;
                }
                finally
                {
                    transaction.Dispose();
                }
            }
        }

        #region H E L P E R
        private HaveId<long> UpdateStatus(UpdateStatusCompletedServiceDlDto dto, Action<CompletedService> validation)
        {
            using (var transaction = UnitOfWork.BeginTransaction())
            {
                var entity = Repository.UpdateStatus(dto, validation);

                CombineStatuses(Repository);
                if (HasErrors)
                    return null;

                UnitOfWork.Save();

                //var res = CreateDocumentChangeLog(entity.Id, entity.StatusId, "Service Price");

                if (IsValid)
                    transaction.Commit();

                return new HaveId<long>(entity.Id);
            }
        }
        private HaveId<long> CreateDocumentChangeLog(long id, int statusId, string message = null)
        {
            var moveDto = Repository.ById<CompletedServiceDto>(id, applyFilter: false);

            _documentChangeLogService.Create(
                dto: moveDto,
                tableId: TableIdConst.DOC_COMPLETED_SERVICE,
                organizationId: null,
                statusId: statusId,
                message: message);

            CombineStatuses(_documentChangeLogService);
            if (HasErrors)
                return null;

            return HaveId.Create(id);
        }
        private void Validation<TDto>(CompletedServiceDlDto<TDto> dto, CompletedService entity)
        where TDto : CompletedServiceDlDto<TDto>
        {
            var query = Repository.AllAsQueryable;

            if (entity != null)
                query = query.Where(a => a.Id != entity.Id);

        }
        #endregion

    }
}
