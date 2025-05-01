using System;
using Microsoft.EntityFrameworkCore;
using SspUis.BizLogicLayer.ContractorServices;
using SspUis.BizLogicLayer.DocumentChangeLogServices;
using SspUis.Core;
using SspUis.Core.Configurations;
using SspUis.Core.Security;
using SspUis.DataLayer;
using SspUis.DataLayer.EfClasses;
using SspUis.DataLayer.Repositories;
using SspUis.Integration.DocxToPdf;
using SspUis.ServiceLayer.NumberServices;
using WEBASE;
using WEBASE.i18n;
using WEBASE.Models;
using WEBASE.Storage;

namespace SspUis.BizLogicLayer;

public class ContractorCategoryCriterionService
    : BaseEntityService<long, ContractorCategoryCriterion, ContractorCategoryCriterionListDto, ContractorCategoryCriterionDto, CreateContractorCategoryCriterionDlDto, UpdateContractorCategoryCriterionDlDto, IContractorCategoryCriterionRepository, ContractorCategoryCriterionSortFilterOptions>
    , IContractorCategoryCriterionService
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IContractorCategoryCriterionRepository _repository;

    public ContractorCategoryCriterionService(
        IUnitOfWork unitOfWork)
        : base(unitOfWork)
    {
        _repository = unitOfWork.ContractorCategoryCriterionRepository;
        _unitOfWork = unitOfWork;
    }
    public PagedResult<ContractorCategoryCriterionListDto> GetList(ContractorCategoryCriterionSortFilterOptions options)
    {
        return Repository.ReadAsNoTracked<ContractorCategoryCriterionListDto>()
            .SortFilter(options)
            .AsPagedResult(options);
    }
    public SelectList<long> AsSelectList()
    {
        return _repository.AllAsQueryable.AsSelectList();
    }
    public ContractorCategoryCriterionDto Get()
    {
        return new ContractorCategoryCriterionDto
        {
            DocOn = DateOnly.FromDateTime(DateTime.Now)
            //DocNumber = _numberService.GetNext(NumberTemplateDocumentConst.DOC_MEMSHIP_CERTIFICATE, 1).Item2
        };
    }
    public override ContractorCategoryCriterionDto Get(long id)
    {
        var dto = _repository.ById<ContractorCategoryCriterionDto>(id);
        CombineStatuses(_repository);

        return dto;
    }
    public override HaveId<long> Create(CreateContractorCategoryCriterionDlDto dto)
    {
        using (var transaction = UnitOfWork.BeginTransaction())
        {
            try
            {
                var entity = Repository.Create(dto, ent => Validation(dto, ent));
                UnitOfWork.Save();
                CombineStatuses(Repository);
                if (IsValid)
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
    }
    public override void Update(UpdateContractorCategoryCriterionDlDto dto)
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
        }
    }
    public void Accept(UpdateStatusContractorCategoryCriterionDto dTo)
    {
        var dto = new UpdateStatusContractorCategoryCriterionDlDto { Id = dTo.Id, StatusId = StatusIdConst.ACCEPTED };
        UpdateStatus(dto, ent =>
        {
            if (!StatusIdConst.CanApplyStatus(ent.StatusId, dto.StatusId))
                _repository.AddError("Нет доступа");
        });
    }
    public void Cancel(UpdateStatusContractorCategoryCriterionDto dTo)
    {
        var dto = new UpdateStatusContractorCategoryCriterionDlDto
        { Id = dTo.Id, StatusId = StatusIdConst.CANCELED };
        UpdateStatus(dto, ent =>
        {
            if (!StatusIdConst.CanApplyStatus(ent.StatusId, dto.StatusId))
                _repository.AddError("Нет доступа");
        });
    }
    public override void Delete(long id)
    {
        using (var transaction = UnitOfWork.BeginTransaction())
        {
            try
            {
                var entity = Repository.ById(id);
                var statusDto = new UpdateStatusContractorCategoryCriterionDlDto()
                {
                    Id = id,
                    StatusId = StatusIdConst.DELETED
                };
                Repository.UpdateStatus(statusDto);
                UnitOfWork.Save();

                if (IsValid)
                {
                    transaction.Commit();
                }
            }
            catch (DbUpdateException ex)
            {
                AddError(ex.Message);
                transaction.Rollback();
            }
        }
    }
    private HaveId<long> UpdateStatus(UpdateStatusContractorCategoryCriterionDlDto dto, Action<ContractorCategoryCriterion> validation)
    {
        using (var transaction = _unitOfWork.BeginTransaction())
        {
            try
            {
                var entity = _repository.UpdateStatus(dto);
                CombineStatuses(_repository);
                if (HasErrors)
                    return null;
                _unitOfWork.Save();
                //var res = CreateDocumentChangeLog(entity.Id, "ContractorCategoryCriterion");
                if (IsValid)
                    transaction.Commit();
                return HaveId.Create(entity.Id);
            }
            catch (Exception ex)
            {
                AddError(ex.Message);
                transaction.Rollback();
            }
        }
        return null;
    }
    //private HaveId<long> CreateDocumentChangeLog(long id, string message = null, string userIp = null, string userAgent = null)
    //{
    //    var entityDto = Repository.ById<ContractorCategoryCriterionDto>(id, applyFilter: false);
    //    _documentChangeLogService.Create(
    //        dto: entityDto,
    //        tableId: TableIdConst.MEMSHIP__DOC_MEMSHIP_CERTIFICATE,
    //        organizationId: null,
    //        statusId: entityDto.StatusId,
    //        message: message,
    //        userIp: userIp,
    //        userAgent: userAgent);
    //    CombineStatuses(_documentChangeLogService);

    //    if (HasErrors)
    //        return null;

    //    return HaveId.Create(id);
    //}
    private void Validation<TDto>(ContractorCategoryCriterionDlDto<TDto> dto, ContractorCategoryCriterion entity)
          where TDto : ContractorCategoryCriterionDlDto<TDto>
    {
        var query = _repository.AllAsQueryable;

        //if (entity != null)
        //{
        //    query = query.Where(a => a.Id != entity.Id);

        //    if (!StatusIdConst.CanModifyStatus.Contains(entity.StatusId))
        //        AddError("Таҳрирлаш мумкин эмас / Невозможно редактировать");
        //}

        //if (query.ByDocNumber(dto.DocNumber).Any())
        //    _repository.AddError($"Документ с этим кодом {dto.DocNumber} уже существует", nameof(dto.DocNumber));
    }

}
