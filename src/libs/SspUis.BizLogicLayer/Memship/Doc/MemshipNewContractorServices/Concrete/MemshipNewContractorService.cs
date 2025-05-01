using Microsoft.EntityFrameworkCore;
using SspUis.BizLogicLayer.DocumentChangeLogServices;
using SspUis.BizLogicLayer.ManualServices;
using SspUis.Core.Security;
using SspUis.Core;
using SspUis.DataLayer.EfClasses.Memship;
using SspUis.DataLayer.EfClasses;
using SspUis.DataLayer.Repositories.Memship;
using SspUis.DataLayer;
using SspUis.ServiceLayer.NumberServices;
using System;
using System.Collections.Generic;
using System.Linq;

using WEBASE.Models;
using WEBASE.Storage;
using SspUis.DataLayer.Repositories;
using WEBASE;

namespace SspUis.BizLogicLayer.Memship;




public class MemshipNewContractorService
    : BaseEntityService<long, MemshipNewContractor, MemshipNewContractorListDto, MemshipNewContractorDto, CreateMemshipNewContractorDlDto, UpdateMemshipNewContractorDlDto, IMemshipNewContractorRepository, MemshipNewContractorSortFilterOptions>
    , IMemshipNewContractorService
{
    IAuthService _authService;
    IUnitOfWork _unitOfWork;
    IDocumentChangeLogService _documentChangeLogService;
    private readonly IMemshipNewContractorRepository _repository;
    private readonly INumberService _numberService;
    private readonly IStorageService _storageService;
    private readonly IManualService _manualService;
    public MemshipNewContractorService(
        IUnitOfWork unitOfWork,
        INumberService numberService,
        IDocumentChangeLogService documentChangeLogService,
        IAuthService authService,
        IManualService manualService,
        IStorageService storageService)
        : base(unitOfWork)
    {
        this._repository = unitOfWork.MemshipNewContractorRepository;
        this._authService = authService;
        this._unitOfWork = unitOfWork;
        this._documentChangeLogService = documentChangeLogService;
        this._numberService = numberService;
        _storageService = storageService;
        _manualService = manualService;
    }
    public PagedResult<MemshipNewContractorListDto> GetList(MemshipNewContractorSortFilterOptions options)
    {
        return Repository.ReadAsNoTracked<MemshipNewContractorListDto>()
            .SortFilter(options)
            .AsPagedResult(options);
    }
    public SelectList<long> AsSelectList()
    {
        return _repository.AllAsQueryable.Where(a => a.StatusId == StatusIdConst.ACCEPTED)
                        .AsSelectList();
    }
    public MemshipNewContractorDto Get()
    {
        return new MemshipNewContractorDto
        {
            DocOn = DateOnly.FromDateTime(DateTime.Now),
            FromDate = DateOnly.FromDateTime(DateTime.Now),
            ToDate = DateOnly.FromDateTime(DateTime.Now),
            DocNumber = _numberService.GetNext(NumberTemplateDocumentConst.DOC_MEMSHIP_NEW_CONTRACTOR, 1).Item2,
            OrganizationId = _authService.User.OrganizationId
        };
    }
    

    public HaveId<long> Create(CreateMemshipNewContractorDlDto dto)
    {
        using (var transaction = _unitOfWork.BeginTransaction())
        {
            
            var entity = _repository.Create(dto, ent => Validation(dto, ent));
            CombineStatuses(_repository);
            if (HasErrors)
                return null;
            _unitOfWork.Save();

            //var res = CreateDocumentChangeLog(entity.Id, entity.StatusId);
            if (IsValid)
            {
                transaction.Commit();
                return HaveId.Create(entity.Id);
            }
        }
        return null;
    }
    public override MemshipNewContractorDto Get(long id)
    {
        var dto = _repository.ById<MemshipNewContractorDto>(id);
        CombineStatuses(_repository);
        if (dto is not null)
        {
            //dto.CellTables = ConvertToCellTables(id);
            dto.CanModify = StatusIdConst.CanApplyHrmDocStatus(dto.StatusId, StatusIdConst.MODIFIED) && _authService.HasPermission(ModuleCode.MemshipNewContractorEdit);
            dto.CanAccept = StatusIdConst.CanApplyHrmDocStatus(dto.StatusId, StatusIdConst.ACCEPTED) && _authService.HasPermission(ModuleCode.MemshipNewContractorAccept);
            dto.CanCancel = StatusIdConst.CanApplyHrmDocStatus(dto.StatusId, StatusIdConst.NOT_ACCEPTED) && _authService.HasPermission(ModuleCode.MemshipNewContractorCancel);
            dto.CanDelete = StatusIdConst.CanApplyHrmDocStatus(dto.StatusId, StatusIdConst.DELETED) && _authService.HasPermission(ModuleCode.MemshipNewContractorDelete);
        }
        return dto;
    }
    public override void Update(UpdateMemshipNewContractorDlDto dto)
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
    public void Accept(UpdateStatusMemshipNewContractorDto dTo)
    {
        var dto = new UpdateStatusMemshipNewContractorDlDto { Id = dTo.Id, StatusId = StatusIdConst.ACCEPTED };
        UpdateStatus(dto, ent =>
        {
            if (!StatusIdConst.CanApplyStatus(ent.StatusId, dto.StatusId))
                _repository.AddError("Нет доступа");
        });
    }
    public void Cancel(UpdateStatusMemshipNewContractorDto dTo)
    {
        var dto = new UpdateStatusMemshipNewContractorDlDto { Id = dTo.Id, StatusId = StatusIdConst.NOT_ACCEPTED };
        UpdateStatus(dto, ent =>
        {
            if (!StatusIdConst.CanApplyStatus(ent.StatusId, dto.StatusId))
                _repository.AddError("Нет доступа");
        });
    }
    public override void Delete(long id)
    {
        var dto = new UpdateStatusMemshipNewContractorDlDto { Id = id, StatusId = StatusIdConst.DELETED };
        UpdateStatus(dto, ent =>
        {
            if (!StatusIdConst.CanApplyStatus(ent.StatusId, dto.StatusId))
                _repository.AddError("Нет доступа");
        });
    }
    private HaveId<long> UpdateStatus(UpdateStatusMemshipNewContractorDlDto dto, Action<MemshipNewContractor> validation)
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
                //var res = CreateDocumentChangeLog(entity.Id, dto.StatusId);
                if (IsValid)
                    transaction.Commit();
                //return res;
            }
            catch (Exception ex)
            {
                AddError(ex.Message);
                transaction.Rollback();
            }
        }
        return null;
    }
    private HaveId<long> CreateDocumentChangeLog(long id, int statusId, string message = null)
    {
        var moveDto = _repository.ById<MemshipNewContractorDto>(id, applyFilter: false);
        _documentChangeLogService.Create(
            dto: moveDto,
            tableId: TableIdConst.DOC_MEMSHIP_YEARLY_PLAN,
            organizationId: null,
            statusId: statusId,
            message: message);
        CombineStatuses(_documentChangeLogService);

        if (HasErrors)
            return null;

        return HaveId.Create(id);
    }
    private void Validation<TDto>(MemshipNewContractorDlDto<TDto> dto, MemshipNewContractor entity)
          where TDto : MemshipNewContractorDlDto<TDto>
    {
        var query = _repository.AllAsQueryable;

        if (entity != null)
        {
            query = query.Where(a => a.Id != entity.Id);

            if (!StatusIdConst.CanModifyStatus.Contains(entity.StatusId))
                AddError("Таҳрирлаш мумкин эмас / Невозможно редактировать");
        }

        if (query.ByDocNumber(dto.DocNumber).Any())
            _repository.AddError($"Документ с этим кодом {dto.DocNumber} уже существует", nameof(dto.DocNumber));
    }
    public IEnumerable<MemshipNewContractorFileDto> UploadFiles(params StorageFile[] files)
    {
        var result = _storageService.SaveTemp(DocumentStorageConst.DOC_MEMSHIP_NEW_CONTRACTOR, files).Select(a => new MemshipNewContractorFileDto
        {
            Id = a.FileId,
            FileName = a.FileName
        });
        CombineStatuses(_storageService);
        return IsValid ? result : null;
    }
    public StorageFile DownloadFile(Guid fileId)
    {
        var entity = _unitOfWork.Context.Set<MemshipNewContractorsFile>().FirstOrDefault(a => a.Id == fileId);
        return Download(fileId, entity, DocumentStorageConst.DOC_MEMSHIP_NEW_CONTRACTOR);
    }
    private StorageFile Download(Guid fileId, FileEntity<long> entity, string storageDocument)
    {
        StorageFile file;

        if (entity == null)
        {
            file = _storageService.GetTempFile(storageDocument, fileId);
            CombineStatuses(_storageService);
        }
        else
        {
            file = _storageService.GetFile(storageDocument, entity.OwnerId.ToString(), fileId);
            CombineStatuses(_storageService);

            if (IsValid)
                file.FileName = entity.FileName;
        }

        return file;
    }
    public void DeleteFile(Guid fileId)
    {
        var entity = _unitOfWork.Context
            .Set<MemshipNewContractorsFile>()
            .FirstOrDefault(a => a.Id == fileId);

        Delete(fileId, entity, DocumentStorageConst.DOC_MEMSHIP_NEW_CONTRACTOR);
    }
    private void Delete(Guid fileId, FileEntity<long> entity, string storageDocument)
    {
        if (entity != null)
        {
            _storageService.DeleteTemp(storageDocument, fileId);
            CombineStatuses(_storageService);
        }
    }

    public List<MemshipNewContractorTableDto> FillTable(int regionId)
    {
        var districts = _manualService.DistrictSelectList(regionId);
        var data = new List<MemshipNewContractorTableDto>();
        foreach(var district in districts)
        {
            data.Add(new MemshipNewContractorTableDto()
            {
                District = district.Text,
                DistrictId = district.Value,
                
                

            });
        }

        return data;
    }
}
