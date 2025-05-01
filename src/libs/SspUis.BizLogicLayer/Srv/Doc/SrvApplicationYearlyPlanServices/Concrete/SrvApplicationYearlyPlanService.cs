using Microsoft.EntityFrameworkCore;
using SspUis.BizLogicLayer.DocumentChangeLogServices;
using SspUis.BizLogicLayer.ManualServices;
using SspUis.Core;
using SspUis.Core.Security;
using SspUis.DataLayer;
using SspUis.DataLayer.EfClasses;
using SspUis.DataLayer.EfClasses.Hrm;
using SspUis.DataLayer.Repositories;
using SspUis.ServiceLayer.NumberServices;
using System;
using System.Collections.Generic;
using System.Linq;
using WEBASE;
using WEBASE.Integration.Manuals.Models;
using WEBASE.Models;
using WEBASE.Storage;

namespace SspUis.BizLogicLayer;

public class SrvApplicationYearlyPlanService
    : BaseEntityService<long, SrvApplicationYearlyPlan, SrvApplicationYearlyPlanListDto, SrvApplicationYearlyPlanDto, CreateSrvApplicationYearlyPlanDlDto, UpdateSrvApplicationYearlyPlanDlDto, ISrvApplicationYearlyPlanRepository, SrvApplicationYearlyPlanSortFilterOptions>
    , ISrvApplicationYearlyPlanService
{
    IAuthService _authService;
    IUnitOfWork _unitOfWork;
    IDocumentChangeLogService _documentChangeLogService;
    private readonly ISrvApplicationYearlyPlanRepository _repository;
    private readonly INumberService _numberService;
    private readonly IStorageService _storageService;
    private readonly IManualService _manualService;
    public SrvApplicationYearlyPlanService(
        IUnitOfWork unitOfWork,
        INumberService numberService,
        IDocumentChangeLogService documentChangeLogService,
        IAuthService authService,
        IManualService manualService,
        IStorageService storageService)
        : base(unitOfWork)
    {
        this._repository = unitOfWork.SrvApplicationYearlyPlanRepository;
        this._authService = authService;
        this._unitOfWork = unitOfWork;
        this._documentChangeLogService = documentChangeLogService;
        this._numberService = numberService;
        _storageService = storageService;
        _manualService = manualService;
    }
    public PagedResult<SrvApplicationYearlyPlanListDto> GetList(SrvApplicationYearlyPlanSortFilterOptions options)
    {
        return Repository.ReadAsNoTracked<SrvApplicationYearlyPlanListDto>()
            .SortFilter(options)
            .AsPagedResult(options);
    }
    public SelectList<long> AsSelectList()
    {
        return _repository.AllAsQueryable.Where(a => a.StatusId == StatusIdConst.ACCEPTED)
                        .AsSelectList();
    }
    public SrvApplicationYearlyPlanDto Get()
    {
        return new SrvApplicationYearlyPlanDto
        {
            DocOn = DateOnly.FromDateTime(DateTime.Now),
            DocNumber = _numberService.GetNext(NumberTemplateDocumentConst.DOC_SRV_APPLICATION_YEARLY_PLAN, 1).Item2,
            OrganizationId = _authService.User.OrganizationId,
        };
    }
    public List<SrvApplicationYearlyPlanTableCellDto> ConvertToCellTables(long? id, int? regionId)
    {
        var tableVsValues = GetRegionsOrDistricts(id, regionId);

        if (tableVsValues.Any())
        {
            var firstRegionItem = tableVsValues.First();

            var cellDto = new List<SrvApplicationYearlyPlanTableCellDto>
        {
            new SrvApplicationYearlyPlanTableCellDto
            {
                RegionId = firstRegionItem.RegionId,
                Region = firstRegionItem.Region,
                RegionFreeCount = firstRegionItem.RegionFreeCount,
                RegionPaidCount = firstRegionItem.RegionPaidCount,
                RegionAmount = firstRegionItem.RegionAmount,
                RegionEmployeeCount = firstRegionItem.RegionEmployeeCount,
                RegionEconomyAmount = firstRegionItem.RegionEconomyAmount,
                RegionLegalAmount = firstRegionItem.RegionLegalAmount,
                ValueForDistricts = firstRegionItem.ValueForDistricts
                    .Select(districtItem => new ValueForDistrict
                    {
                        DistrictId = districtItem.DistrictId,
                        District = districtItem.District,
                        FreeCount = districtItem.FreeCount,
                        PaidCount = districtItem.PaidCount,
                        Amount = districtItem.Amount,
                        EmployeeCount = districtItem.EmployeeCount,
                        EconomyAmount= districtItem.EconomyAmount,
                        LegalAmount = districtItem.LegalAmount,
                    })
                    .ToList()
            }
        };
            return cellDto;
        }
        return null;
    }

    public List<SrvApplicationYearlyPlanTableCellDto> GetRegionsOrDistricts(long? id, int? regionId)
    {
        var result = new List<SrvApplicationYearlyPlanTableCellDto>();
        if (id != null)
        {
            var query = _unitOfWork.Context.Set<SrvApplicationYearlyPlan>()
                    .Include(a => a.Tables)
                    .ThenInclude(a => a.District)
                    .ThenInclude(c => c.Translates)
                    .Include(a => a.Region)
                    .ThenInclude(b => b.Translates)
                    .Where(a => a.Id == id)
                    .OrderBy(a => a.Region.OrderCode)
                    .FirstOrDefault();


            var regionItem = new SrvApplicationYearlyPlanTableCellDto
            {
                Region = query.Region.Translates.AsQueryable().FirstOrDefault(RegionTranslate.GetExpr(TranslateColumn.full_name, ServiceProvider.CultureHelper.CurrentCulture.Id)).TranslateText ?? query.Region.FullName,
                RegionId = query.RegionId,
                RegionFreeCount = query.RegionFreeCount,
                RegionPaidCount = query.RegionPaidCount,
                RegionAmount = query.RegionAmount,
                RegionEmployeeCount = query.RegionEmployeeCount,
                RegionEconomyAmount = query.RegionEconomyAmount,
                RegionLegalAmount = query.RegionLegalAmount,
            };

            var districts = _unitOfWork.Context.Set<SrvApplicationYearlyPlanTable>()
                               .Include(a => a.District)
                               .Where(a => a.OwnerId == query.Id)
                               .OrderByDescending(a => a.DistrictId)
                               .ToArray();

            foreach (var district in districts)
            {
                var valueForDistrict = new ValueForDistrict
                {
                    DistrictId = district.DistrictId,
                    District = district.District.Translates.AsQueryable().FirstOrDefault(DistrictTranslate.GetExpr(TranslateColumn.full_name, ServiceProvider.CultureHelper.CurrentCulture.Id)).TranslateText ?? district.District.FullName,
                    FreeCount = district.FreeCount,
                    PaidCount = district.PaidCount,
                    Amount = district.Amount,
                    EmployeeCount = district.EmployeeCount,
                    EconomyAmount = district.EconomyAmount,
                    LegalAmount = district.LegalAmount
                };
                regionItem.ValueForDistricts.Add(valueForDistrict);
            }
            result.Add(regionItem);

            result = result.OrderByDescending(a => a.RegionId).ToList();
        }
        else if (id == null)
        {
            var regions = _unitOfWork.RegionRepository.AllAsQueryable.Where(a => a.Id == regionId)
                         .IsActive()
               .Select(a => new
               {
                   Id = a.Id,
                   OrderCode = a.OrderCode,
                   FullName = a.Translates.AsQueryable().FirstOrDefault(RegionTranslate.GetExpr(TranslateColumn.full_name, ServiceProvider.CultureHelper.CurrentCulture.Id)).TranslateText ?? a.FullName
               })
               .OrderBy(a => a.OrderCode)
               .FirstOrDefault();

            var emplyeeCountForRegion = _unitOfWork.Context.Set<AppointEmployeeTable>().Include(a => a.Owner).ThenInclude(a => a.Organization).Where(a => a.Owner.Organization.RegionId == regions.Id && a.Owner.StatusId == StatusIdConst.ACCEPTED).Count();

            var regionItem = new SrvApplicationYearlyPlanTableCellDto
            {
                Region = regions.FullName,
                RegionId = regions.Id,
                RegionFreeCount = 0,
                RegionPaidCount = 0,
                RegionAmount = 0,
                RegionEmployeeCount = emplyeeCountForRegion,
                RegionLegalAmount = 0,
                RegionEconomyAmount = 0,
            };

            var districts = _unitOfWork.DistrictRepository.AllAsQueryable
                               .IsActive()
                               .Where(a => a.RegionId == regionId)
                               .Select(a => new
                               {
                                   DistrictId = a.Id,
                                   District = a.Translates
                                       .AsQueryable()
                                       .FirstOrDefault(DistrictTranslate.GetExpr(TranslateColumn.full_name, ServiceProvider.CultureHelper.CurrentCulture.Id))
                                       .TranslateText ?? a.FullName
                               })
                               .OrderByDescending(a => a.DistrictId)
                               .ToArray();

            foreach (var district in districts)
            {
                var emplyeeCountForDistrict = _unitOfWork.Context.Set<AppointEmployeeTable>()
                    .Include(a => a.Owner)
                    .ThenInclude(a => a.Organization)
                    .Where(a => a.Owner.Organization.DistrictId == district.DistrictId && a.Owner.StatusId == StatusIdConst.ACCEPTED)
                    .Count();

                var valueForDistrict = new ValueForDistrict
                {
                    DistrictId = district.DistrictId,
                    District = district.District,
                    FreeCount = 0,
                    PaidCount = 0,
                    Amount = 0,
                    EmployeeCount = emplyeeCountForDistrict,
                    EconomyAmount = 0,
                    LegalAmount = 0,
                };

                regionItem.ValueForDistricts.Add(valueForDistrict);
            }
            result.Add(regionItem);

            result = result.OrderByDescending(a => a.RegionId).ToList();
        }
        return result;
    }

    public HaveId<long> Create(CreateSrvApplicationYearlyPlanDlDto dto)
    {
        using (var transaction = _unitOfWork.BeginTransaction())
        {
            dto.BeforeSave();
            var entity = _repository.Create(dto, ent => Validation(dto, ent));
            CombineStatuses(_repository);
            UnitOfWork.Save();
            _storageService.MoveToPersistent(DocumentStorageConst.DOC_SRV_APPLICATION_YEARLY_PLAN_FILES, $"{entity.Id}", dto.Files.Select(a => a.Id).ToArray());
            CombineStatuses(_storageService);
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
    public SrvApplicationYearlyPlanDto Get(long id)
    {
        var dto = _repository.ById<SrvApplicationYearlyPlanDto>(id);
        CombineStatuses(_repository);
        if (dto is not null)
        {
            dto.CellTables = ConvertToCellTables(id, null);
            dto.CanModify = StatusIdConst.CanApplyHrmDocStatus(dto.StatusId, StatusIdConst.MODIFIED) && _authService.HasPermission(ModuleCode.SrvApplicationYearlyPlanEdit);
            dto.CanAccept = StatusIdConst.CanApplyHrmDocStatus(dto.StatusId, StatusIdConst.ACCEPTED) && _authService.HasPermission(ModuleCode.SrvApplicationYearlyPlanAccept);
            dto.CanCancel = StatusIdConst.CanApplyHrmDocStatus(dto.StatusId, StatusIdConst.NOT_ACCEPTED) && _authService.HasPermission(ModuleCode.SrvApplicationYearlyPlanCancel);
            dto.CanDelete = StatusIdConst.CanApplyHrmDocStatus(dto.StatusId, StatusIdConst.DELETED) && _authService.HasPermission(ModuleCode.SrvApplicationYearlyPlanDelete);
        }
        return dto;
    }
    public override void Update(UpdateSrvApplicationYearlyPlanDlDto dto)
    {
        using (var transaction = UnitOfWork.BeginTransaction())
        {
            try
            {
                dto.BeforeSave();
                var entity = Repository.Update(dto, ent => Validation(dto, ent));
                CombineStatuses(Repository);
				UnitOfWork.Save();
				_storageService.ResolveMarkedFiles(DocumentStorageConst.DOC_SRV_APPLICATION_YEARLY_PLAN_FILES, $"{dto.Id}");
                CombineStatuses(_storageService);

                UnitOfWork.Save();
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
    public void Accept(UpdateStatusSrvApplicationYearlyPlanDto dTo)
    {
        var dto = new UpdateStatusSrvApplicationYearlyPlanDlDto { Id = dTo.Id, StatusId = StatusIdConst.ACCEPTED };
        UpdateStatus(dto, ent =>
        {
            if (!StatusIdConst.CanApplyStatus(ent.StatusId, dto.StatusId))
                _repository.AddError("Нет доступа");
        });
    }
    public void Cancel(UpdateStatusSrvApplicationYearlyPlanDto dTo)
    {
        var dto = new UpdateStatusSrvApplicationYearlyPlanDlDto { Id = dTo.Id, StatusId = StatusIdConst.NOT_ACCEPTED };
        UpdateStatus(dto, ent =>
        {
            if (!StatusIdConst.CanApplyStatus(ent.StatusId, dto.StatusId))
                _repository.AddError("Нет доступа");
        });
    }
    public override void Delete(long id)
    {
        var dto = new UpdateStatusSrvApplicationYearlyPlanDlDto { Id = id, StatusId = StatusIdConst.DELETED };
        UpdateStatus(dto, ent =>
        {
            if (!StatusIdConst.CanApplyStatus(ent.StatusId, dto.StatusId))
                _repository.AddError("Нет доступа");
        });
    }
    private HaveId<long> UpdateStatus(UpdateStatusSrvApplicationYearlyPlanDlDto dto, Action<SrvApplicationYearlyPlan> validation)
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
        var moveDto = _repository.ById<SrvApplicationYearlyPlanDto>(id, applyFilter: false);
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
    private void Validation<TDto>(SrvApplicationYearlyPlanDlDto<TDto> dto, SrvApplicationYearlyPlan entity)
          where TDto : SrvApplicationYearlyPlanDlDto<TDto>
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
    public IEnumerable<SrvApplicationYearlyPlanFileDto> UploadFiles(params StorageFile[] files)
    {
        var result = _storageService.SaveTemp(DocumentStorageConst.DOC_SRV_APPLICATION_YEARLY_PLAN_FILES, files).Select(a => new SrvApplicationYearlyPlanFileDto
        {
            Id = a.FileId,
            FileName = a.FileName
        });
        CombineStatuses(_storageService);
        return IsValid ? result : null;
    }
    public StorageFile DownloadFile(Guid fileId)
    {
        var entity = _unitOfWork.Context.Set<SrvApplicationYearlyPlanFile>().FirstOrDefault(a => a.Id == fileId);
        return Download(fileId, entity, DocumentStorageConst.DOC_SRV_APPLICATION_YEARLY_PLAN_FILES);
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
            .Set<SrvApplicationYearlyPlanFile>()
            .FirstOrDefault(a => a.Id == fileId);

        Delete(fileId, entity, DocumentStorageConst.DOC_SRV_APPLICATION_YEARLY_PLAN_FILES);
    }
    private void Delete(Guid fileId, FileEntity<long> entity, string storageDocument)
    {
        if (entity != null)
        {
            _storageService.DeleteTemp(storageDocument, fileId);
            CombineStatuses(_storageService);
        }
    }
}
