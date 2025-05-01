using Microsoft.EntityFrameworkCore;
using SspUis.BizLogicLayer.DocumentChangeLogServices;
using SspUis.Core;
using SspUis.Core.Security;
using SspUis.DataLayer;
using SspUis.DataLayer.EfClasses;
using SspUis.DataLayer.Repositories;
using SspUis.ServiceLayer.NumberServices;
using System;
using System.Collections.Generic;
using System.Linq;
using WEBASE;
using WEBASE.Models;

namespace SspUis.BizLogicLayer
{
    public class ServicePriceService
        : BaseEntityService<long, ServicePrice, ServicePriceListDto, ServicePriceDto, CreateServicePriceDlDto, UpdateServicePriceDlDto,
        IServicePriceRepository, ServicePriceSortFilterOption>,
        IServicePriceService
    {
        private readonly IAuthService _authService;
        private readonly IServicePriceRepository _repository;
        private readonly IDocumentChangeLogService _documentChangeLogService;
        private readonly IUnitOfWork _unitOfWork;
        private readonly INumberService _numberService;
        public ServicePriceService(
            IUnitOfWork unitOfWork,
            IAuthService authService,
            IDocumentChangeLogService documentChangeLogService,
            IServicePriceRepository repository,
            INumberService numberService)
            : base(unitOfWork)
        {
            _documentChangeLogService = documentChangeLogService;
            _authService = authService;
            _repository = repository;
            _numberService = numberService;
            _unitOfWork = unitOfWork;
        }

        #region C O R E
        public SelectList<long> AsSelectList(ServicePriceSortFilterOption options)
        {
            return Repository.ReadAsNoTracked<ServicePriceListDto>()
                .SortFilter(options)
                .AsSelectList();
        }
        public override PagedResult<ServicePriceListDto> GetList(ServicePriceSortFilterOption options)
        {
            return Repository.ReadAsNoTracked<ServicePriceListDto>()
                .SortFilter(options)
                .AsPagedResult(options);
        }
        public override ServicePriceDto Get()
        {
            return new ServicePriceDto()
            {
                DocOn = DateOnly.FromDateTime(DateTime.Now),
                DocNumber = _numberService.GetNext(NumberTemplateDocumentConst.DOC_SERVICE_PRICE, 1).Item2
            };
        }
        public override ServicePriceDto Get(long id)
        {
            var dto = Repository.ById<ServicePriceDto>(id);
            CombineStatuses(Repository);
            if (IsValid)
            {
                dto.CanEdit = StatusIdConst.CanApplySrvDocStatus(dto.StatusId, StatusIdConst.MODIFIED);
                dto.CanAccept = StatusIdConst.CanApplySrvDocStatus(dto.StatusId, StatusIdConst.ACCEPTED);
                dto.CanCancel = StatusIdConst.CanApplySrvDocStatus(dto.StatusId, StatusIdConst.NOT_ACCEPTED);
                dto.CanDelete = StatusIdConst.CanApplySrvDocStatus(dto.StatusId, StatusIdConst.DELETED);
            }
            return dto;
        }
        public override HaveId<long> Create(CreateServicePriceDlDto dto)
        {
            using (var transaction = UnitOfWork.BeginTransaction())
            {
                try
                {
                    var entity = Repository.Create(dto, ent => Validation(dto, ent));

                    UnitOfWork.Save();
                    CombineStatuses(Repository);

                    if (IsValid) transaction.Commit();

                    return HaveId.Create(entity.Id);
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
                return null;
            }
        }
        public override void Update(UpdateServicePriceDlDto dto)
        {
            using (var transaction = UnitOfWork.BeginTransaction())
            {
                try
                {
                    var entity = Repository.Update(dto, ent => Validation(dto, ent));

                    UnitOfWork.Save();

                    CombineStatuses(Repository);
                    if (IsValid) transaction.Commit();
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
        public override void Delete(long id)
        {
            try
            {
                var entity = Repository.ById(id);

                var statusDto = new UpdateStatusServicePriceDlDto()
                {
                    Id = id,
                    StatusId = StatusIdConst.DELETED
                };

                this.UpdateStatus(statusDto, ent =>
                {
                    if (!StatusIdConst.CanApplySrvDocStatus(ent.StatusId, statusDto.StatusId))
                        Repository.AddError("Нет доступа");
                });

                if (HasErrors) return;

                UnitOfWork.Save();
            }
            catch (DbUpdateException ex)
            {
                AddError(ex.Message);
                return;
            }
        }
        public HaveId<long> Cancel(CancelStatusSrvPriceDto dto)
        {
            using (var transaction = _unitOfWork.BeginTransaction())
            {
                try
                {
                    var entity = _repository.UpdateStatus(dto, ent =>
                    {
                        if (ent == null) AddError("Not found");
                    });

                    CombineStatuses(_repository);
                    if (HasErrors) return null;

                    _unitOfWork.Save();

                    //var res = CreateDocumentChangeLog(entity.Id, entity.StatusId);

                    if (IsValid) transaction.Commit();

                    return new HaveId<long>(entity.Id);
                }
                catch (Exception ex)
                {
                    AddError($"{ex.Message} -- {ex.InnerException}");
                    transaction.Rollback();
                }
            }
            return null;
        }
        public HaveId<long> Accept(AcceptStatusSrvPriceDto dto)
        {
            using (var transaction = _unitOfWork.BeginTransaction())
            {
                try
                {
                    var entity = _repository.UpdateStatus(dto, ent =>
                    {
                        if (ent == null) AddError("Not found");
                    });

                    CombineStatuses(_repository);
                    if (HasErrors) return null;

                    _unitOfWork.Save();
                    //var res = CreateDocumentChangeLog(entity.Id, entity.StatusId);

                    if (IsValid) transaction.Commit();

                    return new HaveId<long>(entity.Id);
                }
                catch (Exception ex)
                {
                    AddError($"{ex.Message} -- {ex.InnerException}");
                    transaction.Rollback();
                }
            }
            return null;
        }
        #endregion

        #region H E L P E R
        private HaveId<long> UpdateStatus(UpdateStatusServicePriceDlDto dto, Action<ServicePrice> validation)
        {
            using (var transaction = UnitOfWork.BeginTransaction())
            {
                var entity = Repository.UpdateStatus(dto, validation);

                CombineStatuses(Repository);
                if (HasErrors) return null;

                UnitOfWork.Save();

                //var res = CreateDocumentChangeLog(entity.Id, entity.StatusId, "Service Price");

                if (IsValid)
                    transaction.Commit();

                return new HaveId<long>(entity.Id);
            }
        }
        private void Validation<TDto>(ServicePriceDlDtoo<TDto> dto, ServicePrice entity)
            where TDto : ServicePriceDlDtoo<TDto>
        {
            var query = Repository.AllAsQueryable;

            if (entity != null)
                query = query.Where(a => a.Id != entity.Id);

            var paidServices = _unitOfWork.Context.Set<NeedChamberService>()
                .Where(n => n.ServicePriceTypeId != ServicePriceTypeIdConst.FREE && n.ServicePriceTypeId != ServicePriceTypeIdConst.BY_AGREEMENT)
                .ToDictionary(c => c.Id);

            foreach (var group in dto.Groups)
            {
                foreach (var table in group.Tables.Where(t => paidServices.ContainsKey(t.NeedChamberServiceId)).ToList())
                {
                    if (table.BeginCoef != 0 && table.EndCoef != 0)
                    {
                        if (table.BeginCoef > table.EndCoef) { AddError("Бошланғич сумма охирги суммадан катта бўла олмайди !"); return; }

                        if (table.ConcreteCoef != 0) { AddError("Оралиқдаги суммани танласангиз аниқ сумма кирита опмайсиз !"); return; }
                    }
                    else if (table.ConcreteCoef == 0) { AddError(""); return; }
                    else { table.BeginCoef = 0; table.EndCoef = 0; }
                }
            }
        }
        private HaveId<long> CreateDocumentChangeLog(long id, int statusId, string message = null)
        {
            var moveDto = Repository.ById<ServicePriceDto>(id, applyFilter: false);

            //_documentChangeLogService.CreateApplication(
            //    dto: moveDto,
            //    organizationId: null,
            //    message: message);

            CombineStatuses(_documentChangeLogService);

            if (HasErrors)
                return null;

            return null;
        }
        public List<ServicePriceGroupListDto> GroupingByServicePrice(GroupingByServicesPriceDtoFilter dto)
        {
            var doc = _unitOfWork.Context.Set<ServicePrice>()
                .Where(c => (!dto.DocOn.HasValue || c.DocOn <= dto.DocOn) && c.StatusId == StatusIdConst.ACCEPTED);

            var res = doc
                .SelectMany(c => c.Groups)
                .Where(x => !dto.NeedChamberServiceGroupId.HasValue || (x.GroupId != null ? x.GroupId == dto.NeedChamberServiceGroupId : true))
                .Where(x => x.Tables.Any(s => dto.IsPaid.HasValue
                        ? dto.IsPaid.Value
                            ? (s.NeedChamberService.ServicePriceTypeId == ServicePriceTypeIdConst.BXM ||
                                s.NeedChamberService.ServicePriceTypeId == ServicePriceTypeIdConst.BY_AGREEMENT ||
                                s.NeedChamberService.ServicePriceTypeId == ServicePriceTypeIdConst.PERCENTAGE_CONTRACT_SIZE)
                            : s.NeedChamberService.ServicePriceTypeId == ServicePriceTypeIdConst.FREE
                        : true))
                .Select(g => new ServicePriceGroupListDto
                {
                    Id = g.Id,
                    GroupId = g.GroupId,
                    Group = g.NeedChamberServiceGroup != null
                       ? g.NeedChamberServiceGroup.Translates.AsQueryable()
                           .FirstOrDefault(NeedChamberServiceGroupTranslate.GetExpr(
                               TranslateColumn.full_name, ServiceProvider.CultureHelper.CurrentCulture.Id))
                           .TranslateText ?? g.NeedChamberServiceGroup.FullName
                       : string.Empty,
                    CreatedAt = DateTime.Now,
                    Tables = g.Tables
                    .Select(t => new ServicePriceGroupTableListDto
                    {
                        Id = t.Id,
                        NeedChamberServiceId = t.NeedChamberServiceId,
                        NeedChamberService = t.NeedChamberService.Translates.AsQueryable()
                           .FirstOrDefault(NeedChamberServiceTranslate.GetExpr(
                               TranslateColumn.full_name, ServiceProvider.CultureHelper.CurrentCulture.Id))
                           .TranslateText ?? t.NeedChamberService.FullName,
                        ServicePriceTypeId = t.NeedChamberService.ServicePriceTypeId,
                        ServicePriceType = t.NeedChamberService.ServicePriceType.Translates.AsQueryable()
                           .FirstOrDefault(ServicePriceTypeTranslate.GetExpr(
                               TranslateColumn.full_name, ServiceProvider.CultureHelper.CurrentCulture.Id))
                           .TranslateText ?? t.NeedChamberService.ServicePriceType.FullName,
                        BeginCoef = t.BeginCoef,
                        EndCoef = t.EndCoef,
                        ConcreteCoef = t.ConcreteCoef,
                        IsConcrete = t.IsConcrete
                    }).ToList()
                }).ToList();

            return res;
        }
        public ServicePriceDto CloneServicePrice(long id)
        {
            var data = Repository.ById<ServicePriceDto>(id);

            if(data is null) { AddError($"Бу {id} ид ли хизматлар нархи мавжуд емас !"); return null; }

            return data.Clone(ref data);
        }
        #endregion
    }
}