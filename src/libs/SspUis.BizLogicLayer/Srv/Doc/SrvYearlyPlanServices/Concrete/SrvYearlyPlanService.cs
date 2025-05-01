using Microsoft.EntityFrameworkCore;
using SspUis.BizLogicLayer.DocumentChangeLogServices;
using SspUis.BizLogicLayer.ManualServices;
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
using WEBASE.Integration.Manuals.Models;
using WEBASE.Models;
using WEBASE.Storage;

namespace SspUis.BizLogicLayer;

public class SrvYearlyPlanService
    : BaseEntityService<long, SrvYearlyPlan, SrvYearlyPlanListDto, SrvYearlyPlanDto, CreateSrvYearlyPlanDlDto, UpdateSrvYearlyPlanDlDto, ISrvYearlyPlanRepository, SrvYearlyPlanSortFilterOptions>
    , ISrvYearlyPlanService
{
    IAuthService _authService;
    IUnitOfWork _unitOfWork;
    IDocumentChangeLogService _documentChangeLogService;
    private readonly ISrvYearlyPlanRepository _repository;
    private readonly INumberService _numberService;
    private readonly IStorageService _storageService;
    private readonly IManualService _manualService;
    public SrvYearlyPlanService(
        IUnitOfWork unitOfWork,
        INumberService numberService,
        IDocumentChangeLogService documentChangeLogService,
        IAuthService authService,
        IManualService manualService,
        IStorageService storageService)
        : base(unitOfWork)
    {
        this._repository = unitOfWork.SrvYearlyPlanRepository;
        this._authService = authService;
        this._unitOfWork = unitOfWork;
        this._documentChangeLogService = documentChangeLogService;
        this._numberService = numberService;
        _storageService = storageService;
        _manualService = manualService;
    }
    public PagedResult<SrvYearlyPlanListDto> GetList(SrvYearlyPlanSortFilterOptions options)
    {
        return Repository.ReadAsNoTracked<SrvYearlyPlanListDto>()
            .SortFilter(options)
            .AsPagedResult(options);
    }
    public SelectList<long> AsSelectList()
    {
        return _repository.AllAsQueryable.Where(a => a.StatusId == StatusIdConst.ACCEPTED)
                        .AsSelectList();
    }
    public SrvYearlyPlanDto Get()
    {
        return new SrvYearlyPlanDto
        {
            DocOn = DateOnly.FromDateTime(DateTime.Now),
            DocNumber = _numberService.GetNext(NumberTemplateDocumentConst.DOC_MEMSHIP_YEARLY_PLAN, 1).Item2,
            OrganizationId = _authService.User.OrganizationId
        };
    }
    public List<SrvYearlyPlanDto> FillTable(int? regionId)
    {
        return new List<SrvYearlyPlanDto>
        {
            new SrvYearlyPlanDto
            {
               CellTables = ConvertToCellTables(null, regionId.Value),
            }
        };
    }
    public List<SrvYearlyPlanTableCellDto> ConvertToCellTables(long? id, int? regionId)
    {
        var monthColumns = GetForMonth();
        var monthVsValues = GetRegionsOrDistricts(id, regionId);
        var headerColumns = GetRegionForDistrict();
        return new List<SrvYearlyPlanTableCellDto>()
        {
            new SrvYearlyPlanTableCellDto
            {
                 MonthColumns = monthColumns.Select(a => new MonthColumn
                 {
                     MonthOn = a.MonthOn,
                     MonthName = a.MonthName,
                 }).ToList(),
                 ColumnForRegions = headerColumns.Select(a => new ColumnForRegion
                 {
                     Region = a.Region,
                     RegionId = a.RegionId,
                     RegionValues = a.RegionValues,
                 }).ToList(),
                 MonthVsValues = monthVsValues.Select(b => new MonthVsValue
                 {
                     RegionId = b.RegionId,
                     Region = b.Region,
                     RegionValues = b.RegionValues,
                     ColumnForDistricts = b.ColumnForDistricts,
                 }).ToList()
            }
        };
    }
    public List<MonthVsValue> GetRegionsOrDistricts(long? id, int? regionId)
    {
        var result = new List<MonthVsValue>();
        var months = GetForMonth();
        if (id != null)
        {
            var regions = _unitOfWork.Context.Set<SrvYearlyPlanTableRegion>()
                         .Include(a => a.Region)
                         .ThenInclude(a => a.Districts)
                         .Where(a => a.OwnerId == id)
                         .OrderBy(a => a.Region.OrderCode)
                         .ToList();

            foreach (var gr in regions.GroupBy(a => new
            {
                a.RegionId,
                RegionName = a.Region.FullName,
            }).ToList())
            {
                var values = gr.Select(a => new RegionValue
                {
                    MonthOn = a.MonthOn,
                    Amount = a.Amount,
                }).ToList();

                var regionItem = new MonthVsValue
                {
                    RegionId = gr.Key.RegionId,
                    Region = gr.Key.RegionName,
                    RegionValues = values,
                };

                var districts = _unitOfWork.Context.Set<SrvYearlyPlanTableDistrict>()
                    .Where(a => a.OwnerId == regions.FirstOrDefault().Id)
                    .ToList();

                foreach (var tr in districts.GroupBy(a => new
                {
                    a.DistrictId,
                    DistrictName = a.District.FullName,
                }).ToList())
                {
                    var values2 = tr.Select(a => new DistrictValue
                    {
                        MonthOn = a.MonthOn,
                        Amount = a.Amount,
                    }).ToList();

                    regionItem.ColumnForDistricts.Add(new ColumnForDistrict
                    {
                        DistrictId = tr.Key.DistrictId,
                        District = tr.Key.DistrictName,
                        DistrictValues = values2,
                    });
                }

                result.Add(regionItem);
            }
        }
        else if (id == null)
        {

            var regions = _unitOfWork.RegionRepository.AllAsQueryable.Where(a => a.Id == regionId)
                         .IsActive()
               .Select(a => new
               {
                   Id = a.Id,
                   OrderCode = a.OrderCode,
                   FullName = a.Translates.AsQueryable().FirstOrDefault(RegionTranslate.GetExpr(TranslateColumn.full_name,           ServiceProvider.CultureHelper.CurrentCulture.Id)).TranslateText ?? a.FullName
                 })
               .OrderBy(a => a.OrderCode)
               .ToArray();

            foreach (var region in regions)
            {
                var regionItem = new MonthVsValue
                {
                    Region = region.FullName,
                    RegionId = region.Id,
                    RegionValues = months.Select(a => new RegionValue
                    {
                        MonthOn = a.MonthOn,
                        Amount = 0
                    })
                    .ToList(),
                    ColumnForDistricts = new List<ColumnForDistrict>()
                };

                var districts = _unitOfWork.DistrictRepository.AllAsQueryable
                    .IsActive()
                    .Where(a => a.RegionId == regionId)
                    .Select(a => new
                    {
                        DistrictId = a.Id,
                        District = a.Translates.AsQueryable().FirstOrDefault(DistrictTranslate.GetExpr(TranslateColumn.full_name, ServiceProvider.CultureHelper.CurrentCulture.Id)).TranslateText ?? a.FullName
                    })
                    .OrderByDescending(a => a.DistrictId)
                    .ToArray();

                foreach (var district in districts)
                {
                    regionItem.ColumnForDistricts.Add(new ColumnForDistrict
                    {
                        DistrictId = district.DistrictId,
                        District = district.District,
                        DistrictValues = months.Select(a => new DistrictValue
                        {
                            MonthOn = a.MonthOn,
                            Amount = 0
                        })
                        .ToList()
                    });
                }

                result.Add(regionItem);
            }
            result = result.OrderByDescending(a => a.RegionId).ToList();
        }

        return result;
    }
    public List<MonthColumn> GetForMonth()
    {
        var months = _manualService.GetMonthSelectList();
        var result = new List<MonthColumn>();

        foreach (var item in months)
        {
            result.Add(new MonthColumn
            {
                MonthOn = item.Value,
                MonthName = item.Text
            });
        }
        return result;
    }
    public List<ColumnForRegion> GetRegionForDistrict()
    {
        var result = new List<ColumnForRegion>();
        var tables = _unitOfWork.Context.Set<SrvYearlyPlanTableRegion>()
                    .Include(a => a.Owner)
                   .Include(a => a.Region)
                   .Where(a => a.OwnerId == a.Owner.Id && a.RegionId == _authService.Organization.RegionId && a.Owner.StatusId == StatusIdConst.ACCEPTED).OrderByDescending(a => a.RegionId).ToList();

        foreach (var gr in tables.GroupBy(a => new
        {
            a.RegionId,
            RegionName = a.Region.FullName,
        }).ToList())
        {
            var values = gr.Select(a => new RegionValue
            {
                MonthOn = a.MonthOn,
                Amount = a.Amount,
            }).ToList();

            //for (int kvartal = 1; kvartal <= 4; kvartal++)
            //    values.Insert(3 * kvartal + kvartal - 1, new Value
            //    {
            //        MonthOn = 0,
            //        MembersCount = values.Where(a => !a.IsKvartal && kvartal * 3 - 2 <= a.MonthOn && kvartal * 3 >= a.MonthOn).Sum(a => a.MembersCount),
            //        IsKvartal = true
            //    });
            result.Add(new ColumnForRegion
            {
                Region = gr.Key.RegionName,
                RegionId = gr.Key.RegionId,
                RegionValues = values,
            });
        }
        return result;
    }
    public HaveId<long> Create(CreateSrvYearlyPlanDlDto dto)
    {
        using (var transaction = _unitOfWork.BeginTransaction())
        {
            dto.BeforeSave();
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
    public SrvYearlyPlanDto Get(long id)
    {
        var dto = _repository.ById<SrvYearlyPlanDto>(id);
        CombineStatuses(_repository);
        if (dto is not null)
        {
            dto.CellTables = ConvertToCellTables(id, null);
            dto.CanModify = StatusIdConst.CanApplyHrmDocStatus(dto.StatusId, StatusIdConst.MODIFIED) && _authService.HasPermission(ModuleCode.SrvYearlyPlanEdit);
            dto.CanAccept = StatusIdConst.CanApplyHrmDocStatus(dto.StatusId, StatusIdConst.ACCEPTED) && _authService.HasPermission(ModuleCode.SrvYearlyPlanAccept);
            dto.CanCancel = StatusIdConst.CanApplyHrmDocStatus(dto.StatusId, StatusIdConst.NOT_ACCEPTED) && _authService.HasPermission(ModuleCode.SrvYearlyPlanCancel);
            dto.CanDelete = StatusIdConst.CanApplyHrmDocStatus(dto.StatusId, StatusIdConst.DELETED) && _authService.HasPermission(ModuleCode.SrvYearlyPlanDelete);
        }
        return dto;
    }
    public override void Update(UpdateSrvYearlyPlanDlDto dto)
    {
        using (var transaction = UnitOfWork.BeginTransaction())
        {
            try
            {
                dto.BeforeSave();
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
    public void Accept(UpdateStatusSrvYearlyPlanDto dTo)
    {
        var dto = new UpdateStatusSrvYearlyPlanDlDto { Id = dTo.Id, StatusId = StatusIdConst.ACCEPTED };
        UpdateStatus(dto, ent =>
        {
            if (!StatusIdConst.CanApplyStatus(ent.StatusId, dto.StatusId))
                _repository.AddError("Нет доступа");
        });
    }
    public void Cancel(UpdateStatusSrvYearlyPlanDto dTo)
    {
        var dto = new UpdateStatusSrvYearlyPlanDlDto { Id = dTo.Id, StatusId = StatusIdConst.NOT_ACCEPTED };
        UpdateStatus(dto, ent =>
        {
            if (!StatusIdConst.CanApplyStatus(ent.StatusId, dto.StatusId))
                _repository.AddError("Нет доступа");
        });
    }
    public override void Delete(long id)
    {
        var dto = new UpdateStatusSrvYearlyPlanDlDto { Id = id, StatusId = StatusIdConst.DELETED };
        UpdateStatus(dto, ent =>
        {
            if (!StatusIdConst.CanApplyStatus(ent.StatusId, dto.StatusId))
                _repository.AddError("Нет доступа");
        });
    }
    private HaveId<long> UpdateStatus(UpdateStatusSrvYearlyPlanDlDto dto, Action<SrvYearlyPlan> validation)
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
        var moveDto = _repository.ById<SrvYearlyPlanDto>(id, applyFilter: false);
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
    private void Validation<TDto>(SrvYearlyPlanDlDto<TDto> dto, SrvYearlyPlan entity)
          where TDto : SrvYearlyPlanDlDto<TDto>
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
    public IEnumerable<SrvYearlyPlanFileDto> UploadFiles(params StorageFile[] files)
    {
        var result = _storageService.SaveTemp(DocumentStorageConst.DOC_SRV_YEARLY_PLAN_FILES, files).Select(a => new SrvYearlyPlanFileDto
        {
            Id = a.FileId,
            FileName = a.FileName
        });
        CombineStatuses(_storageService);
        return IsValid ? result : null;
    }
    public StorageFile DownloadFile(Guid fileId)
    {
        var entity = _unitOfWork.Context.Set<SrvYearlyPlanFile>().FirstOrDefault(a => a.Id == fileId);
        return Download(fileId, entity, DocumentStorageConst.DOC_SRV_YEARLY_PLAN_FILES);
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
            .Set<SrvYearlyPlanFile>()
            .FirstOrDefault(a => a.Id == fileId);

        Delete(fileId, entity, DocumentStorageConst.DOC_SRV_YEARLY_PLAN_FILES);
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
