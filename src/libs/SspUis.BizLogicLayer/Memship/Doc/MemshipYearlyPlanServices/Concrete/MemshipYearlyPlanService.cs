using Microsoft.EntityFrameworkCore;
using SspUis.BizLogicLayer.DocumentChangeLogServices;
using SspUis.BizLogicLayer.ManualServices;
using SspUis.Core;
using SspUis.Core.Security;
using SspUis.DataLayer;
using SspUis.DataLayer.EfClasses;
using SspUis.DataLayer.EfClasses.Memship;
using SspUis.DataLayer.Repositories.Memship;
using SspUis.ServiceLayer.NumberServices;
using System;
using System.Collections.Generic;
using System.Linq;
using WEBASE;
using WEBASE.Integration.Manuals.Models;
using WEBASE.Models;
using WEBASE.Storage;

namespace SspUis.BizLogicLayer.Memship;

public class MemshipYearlyPlanService
    : BaseEntityService<long, MemshipYearlyPlan, MemshipYearlyPlanListDto, MemshipYearlyPlanDto, CreateMemshipYearlyPlanDlDto, UpdateMemshipYearlyPlanDlDto, IMemshipYearlyPlanRepository, MemshipYearlyPlanSortFilterOptions>
    , IMemshipYearlyPlanService
{
    IAuthService _authService;
    IUnitOfWork _unitOfWork;
    IDocumentChangeLogService _documentChangeLogService;
    private readonly IMemshipYearlyPlanRepository _repository;
    private readonly INumberService _numberService;
    private readonly IStorageService _storageService;
    private readonly IManualService _manualService;
    public MemshipYearlyPlanService(
        IUnitOfWork unitOfWork,
        INumberService numberService,
        IDocumentChangeLogService documentChangeLogService,
        IAuthService authService,
        IManualService manualService,
        IStorageService storageService)
        : base(unitOfWork)
    {
        this._repository = unitOfWork.MemshipYearlyPlanRepository;
        this._authService = authService;
        this._unitOfWork = unitOfWork;
        this._documentChangeLogService = documentChangeLogService;
        this._numberService = numberService;
        _storageService = storageService;
        _manualService = manualService;
    }
    public PagedResult<MemshipYearlyPlanListDto> GetList(MemshipYearlyPlanSortFilterOptions options)
    {
        return Repository.ReadAsNoTracked<MemshipYearlyPlanListDto>()
            .SortFilter(options)
            .AsPagedResult(options);
    }
    public SelectList<long> AsSelectList()
    {
        return _repository.AllAsQueryable.Where(a => a.StatusId == StatusIdConst.ACCEPTED)
                        .AsSelectList();
    }
    public MemshipYearlyPlanDto Get()
    {
        return new MemshipYearlyPlanDto
        {
            DocOn = DateOnly.FromDateTime(DateTime.Now),
            DocNumber = _numberService.GetNext(NumberTemplateDocumentConst.DOC_MEMSHIP_YEARLY_PLAN, 1).Item2,
            OrganizationId = _authService.User.OrganizationId
        };
    }
    public List<MemshipYearlyPlanDto> FillTable()
    {
        return new List<MemshipYearlyPlanDto>
        {
            new MemshipYearlyPlanDto
            {
               CellTables = ConvertToCellTables(null),
            }
        };
    }
    public List<MemshipYearlyPlanTableCellDto> ConvertToCellTables(long? id)
    {
        var monthColumns = GetForMonth();
        var monthVsValues = GetRegionsOrDistricts(id);
        var headerColumns = GetRegionForDistrict();
        return new List<MemshipYearlyPlanTableCellDto>()
        {
            new MemshipYearlyPlanTableCellDto
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
                Values = a.Values,
            }).ToList(),
            MonthVsValues = monthVsValues.Select(b => new MonthVsValue
            {
                RegionId = b.RegionId,
                Region = b.Region,
                DistrictId = b.DistrictId,
                District = b.District,
                TotalForYear = b.TotalForYear,
                Values = b.Values
            }).ToList()
            }
        };
    }
    public List<MonthVsValue> GetRegionsOrDistricts(long? id)
    {
        var result = new List<MonthVsValue>();
        var months = GetForMonth();
        if (id != null)
        {
            if (_authService.User.OrganizationId == 1)
            {
                var tables = _unitOfWork.Context.Set<MemshipYearlyPlanTable>()
                    .Include(a => a.Region)
                    .Where(a => a.OwnerId == id).OrderBy(a => a.Region.OrderCode).ToList();

                foreach (var gr in tables.GroupBy(a => new
                {
                    a.RegionId,
                    RegionName = a.Region.FullName,
                }).ToList())
                {
                    var values = gr.Select(a => new Value
                    {
                        MonthOn = a.MonthOn,
                        MembersCount = a.MembersCount,
                        IsKvartal = false
                    }).ToList();

                    for (int kvartal = 1; kvartal <= 4; kvartal++)
                        values.Insert(3 * kvartal + kvartal - 1, new Value
                        {
                            MonthOn = 0,
                            MembersCount = values.Where(a => !a.IsKvartal && kvartal * 3 - 2 <= a.MonthOn && kvartal * 3 >= a.MonthOn).Sum(a => a.MembersCount),
                            IsKvartal = true
                        });

                    result.Add(new MonthVsValue
                    {
                        Region = gr.Key.RegionName,
                        RegionId = gr.Key.RegionId,
                        TotalForYear = gr.Sum(a => a.MembersCount),
                        Values = values
                    });
                }
            }
            else
            {
                var tables = _unitOfWork.Context.Set<MemshipYearlyPlanTable>()
                  .Include(a => a.Region)
                  .Include(a => a.District)
                  .Where(a => a.OwnerId == id && a.RegionId == _authService.Organization.RegionId).OrderBy(a => a.Region.OrderCode).ToList();

                foreach (var gr in tables.GroupBy(a => new
                {
                    a.RegionId,
                    RegionName = a.Region.FullName,
                }).ToList())
                {
                    var values = gr.Select(a => new Value
                    {
                        MonthOn = a.MonthOn,
                        MembersCount = a.MembersCount,
                        IsKvartal = false
                    }).ToList();

                    for (int kvartal = 1; kvartal <= 4; kvartal++)
                        values.Insert(3 * kvartal + kvartal - 1, new Value
                        {
                            MonthOn = 0,
                            MembersCount = values.Where(a => !a.IsKvartal && kvartal * 3 - 2 <= a.MonthOn && kvartal * 3 >= a.MonthOn).Sum(a => a.MembersCount),
                            IsKvartal = true
                        });
                    result.Add(new MonthVsValue
                    {
                        Region = gr.Key.RegionName,
                        RegionId = gr.Key.RegionId,
                        Values = values,
                        TotalForYear = gr.Sum(a => a.MembersCount)
                    });
                }

                foreach (var gr in tables.GroupBy(a => new
                {
                    a.DistrictId,
                    DistrictName = a.District.FullName,
                }).ToList())
                {
                    var values = gr.Select(a => new Value
                    {
                        MonthOn = a.MonthOn,
                        MembersCount = a.MembersCount,
                        IsKvartal = false
                    }).ToList();

                    for (int kvartal = 1; kvartal <= 4; kvartal++)
                        values.Insert(3 * kvartal + kvartal - 1, new Value
                        {
                            MonthOn = 0,
                            MembersCount = values.Where(a => !a.IsKvartal && kvartal * 3 - 2 <= a.MonthOn && kvartal * 3 >= a.MonthOn).Sum(a => a.MembersCount),
                            IsKvartal = true
                        });
                    result.Add(new MonthVsValue
                    {
                        District = gr.Key.DistrictName,
                        Values = values,
                        TotalForYear = gr.Sum(a => a.MembersCount)
                    });
                }
                result = result.OrderBy(a => a.RegionId).ToList();
            }
        }
        else if (id == null)
        {
            if (_authService.User.OrganizationId == 1)
            {
                var regions = _unitOfWork.RegionRepository.AllAsQueryable
                                   .IsActive()
                                   .Select(a => new
                                   {
                                       Id = a.Id,
                                       OrderCode = a.OrderCode,
                                       FullName = a.Translates.AsQueryable().FirstOrDefault(RegionTranslate.GetExpr(TranslateColumn.full_name, ServiceProvider.CultureHelper.CurrentCulture.Id)).TranslateText ?? a.FullName
                                   })
                                   .OrderBy(a => a.OrderCode)
                                   .ToArray();

                foreach (var region in regions)
                {
                    result.Add(new MonthVsValue
                    {
                        Region = region.FullName,
                        RegionId = region.Id,
                        Values = months.Select(a => new Value
                        {
                            MonthOn = a.MonthOn,
                            MembersCount = 0
                        })
                        .ToList()
                    });
                }
            }
            else
            {
                //var tables = _unitOfWork.Context.Set<MemshipYearlyPlanTable>()
                //    .Include(a => a.Owner)
                //   .Include(a => a.Region)
                //   .Where(a => a.OwnerId == a.Owner.Id && a.RegionId == _authService.Organization.RegionId && a.Owner.StatusId == StatusIdConst.ACCEPTED).OrderByDescending(a => a.RegionId).ToList();

                //foreach (var gr in tables.GroupBy(a => new
                //{
                //    a.RegionId,
                //    RegionName = a.Region.FullName,
                //}).ToList())
                //{
                //    var values = gr.Select(a => new Value
                //    {
                //        MonthOn = a.MonthOn,
                //        MembersCount = a.MembersCount,
                //        IsKvartal = false
                //    }).ToList();

                //    for (int kvartal = 1; kvartal <= 4; kvartal++)
                //        values.Insert(3 * kvartal + kvartal - 1, new Value
                //        {
                //            MonthOn = 0,
                //            MembersCount = values.Where(a => !a.IsKvartal && kvartal * 3 - 2 <= a.MonthOn && kvartal * 3 >= a.MonthOn).Sum(a => a.MembersCount),
                //            IsKvartal = true
                //        });
                //    result.Add(new MonthVsValue
                //    {
                //        Region = gr.Key.RegionName,
                //        RegionId = gr.Key.RegionId,
                //        Values = values,
                //        TotalForYear = gr.Sum(a => a.MembersCount)
                //    });
                //}

                var districts = _unitOfWork.DistrictRepository.AllAsQueryable
                     .IsActive()
                     .Where(a => _authService.Organization.RegionId == a.RegionId)
                     .Select(a => new
                     {
                         RegionId = a.RegionId,
                         Region = a.Region.Translates.AsQueryable().FirstOrDefault(RegionTranslate.GetExpr(TranslateColumn.full_name, ServiceProvider.CultureHelper.CurrentCulture.Id)).TranslateText ?? a.Region.FullName,
                         District = a.Translates.AsQueryable().FirstOrDefault(DistrictTranslate.GetExpr(TranslateColumn.full_name, ServiceProvider.CultureHelper.CurrentCulture.Id)).TranslateText ?? a.FullName
                     }
                     ).OrderByDescending(a => a.RegionId).ToArray();
                foreach (var district in districts)
                {
                    //if (result.Select(a => a.DistrictId).Contains(district.Key))
                    //    continue;

                    result.Add(new MonthVsValue
                    {
                        //Region = district.Region,
                        //RegionId = district.RegionId,
                        District = district.District,
                        Values = months.Select(a => new Value
                        {
                            MonthOn = a.MonthOn,
                            MembersCount = 0
                        })
                        .ToList()
                    });
                }
                result = result.OrderByDescending(a => a.RegionId).ToList();
            }
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
        var tables = _unitOfWork.Context.Set<MemshipYearlyPlanTable>()
                    .Include(a => a.Owner)
                   .Include(a => a.Region)
                   .Where(a => a.OwnerId == a.Owner.Id && a.RegionId == _authService.Organization.RegionId && a.Owner.StatusId == StatusIdConst.ACCEPTED).OrderByDescending(a => a.RegionId).ToList();

        foreach (var gr in tables.GroupBy(a => new
        {
            a.RegionId,
            RegionName = a.Region.FullName,
        }).ToList())
        {
            var values = gr.Select(a => new Value
            {
                MonthOn = a.MonthOn,
                MembersCount = a.MembersCount,
                IsKvartal = false
            }).ToList();

            for (int kvartal = 1; kvartal <= 4; kvartal++)
                values.Insert(3 * kvartal + kvartal - 1, new Value
                {
                    MonthOn = 0,
                    MembersCount = values.Where(a => !a.IsKvartal && kvartal * 3 - 2 <= a.MonthOn && kvartal * 3 >= a.MonthOn).Sum(a => a.MembersCount),
                    IsKvartal = true
                });
            result.Add(new ColumnForRegion
            {
                Region = gr.Key.RegionName,
                RegionId = gr.Key.RegionId,
                Values = values,
                TotalForYear = gr.Sum(a => a.MembersCount)
            });
        }
        return result;
    }
    public HaveId<long> Create(CreateMemshipYearlyPlanDlDto dto)
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
    public override MemshipYearlyPlanDto Get(long id)
    {
        var dto = _repository.ById<MemshipYearlyPlanDto>(id);
        CombineStatuses(_repository);
        if (dto is not null)
        {
            dto.CellTables = ConvertToCellTables(id);
            dto.CanModify = StatusIdConst.CanApplyHrmDocStatus(dto.StatusId, StatusIdConst.MODIFIED) && _authService.HasPermission(ModuleCode.MemshipYearlyPlanEdit);
            dto.CanAccept = StatusIdConst.CanApplyHrmDocStatus(dto.StatusId, StatusIdConst.ACCEPTED) && _authService.HasPermission(ModuleCode.MemshipYearlyPlanAccept);
            dto.CanCancel = StatusIdConst.CanApplyHrmDocStatus(dto.StatusId, StatusIdConst.NOT_ACCEPTED) && _authService.HasPermission(ModuleCode.MemshipYearlyPlanCancel);
            dto.CanDelete = StatusIdConst.CanApplyHrmDocStatus(dto.StatusId, StatusIdConst.DELETED) && _authService.HasPermission(ModuleCode.MemshipYearlyPlanDelete);
        }
        return dto;
    }
    public override void Update(UpdateMemshipYearlyPlanDlDto dto)
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
    public void Accept(UpdateStatusMemshipYearlyPlanDto dTo)
    {
        var dto = new UpdateStatusMemshipYearlyPlanDlDto { Id = dTo.Id, StatusId = StatusIdConst.ACCEPTED };
        UpdateStatus(dto, ent =>
        {
            if (!StatusIdConst.CanApplyStatus(ent.StatusId, dto.StatusId))
                _repository.AddError("Нет доступа");
        });
    }
    public void Cancel(UpdateStatusMemshipYearlyPlanDto dTo)
    {
        var dto = new UpdateStatusMemshipYearlyPlanDlDto { Id = dTo.Id, StatusId = StatusIdConst.NOT_ACCEPTED };
        UpdateStatus(dto, ent =>
        {
            if (!StatusIdConst.CanApplyStatus(ent.StatusId, dto.StatusId))
                _repository.AddError("Нет доступа");
        });
    }
    public override void Delete(long id)
    {
        var dto = new UpdateStatusMemshipYearlyPlanDlDto { Id = id, StatusId = StatusIdConst.DELETED };
        UpdateStatus(dto, ent =>
        {
            if (!StatusIdConst.CanApplyStatus(ent.StatusId, dto.StatusId))
                _repository.AddError("Нет доступа");
        });
    }
    private HaveId<long> UpdateStatus(UpdateStatusMemshipYearlyPlanDlDto dto, Action<MemshipYearlyPlan> validation)
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
        var moveDto = _repository.ById<MemshipYearlyPlanDto>(id, applyFilter: false);
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
    private void Validation<TDto>(MemshipYearlyPlanDlDto<TDto> dto, MemshipYearlyPlan entity)
          where TDto : MemshipYearlyPlanDlDto<TDto>
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
    public IEnumerable<MemshipYearlyPlanFileDto> UploadFiles(params StorageFile[] files)
    {
        var result = _storageService.SaveTemp(DocumentStorageConst.DOC_MEMSHIP_YEARLY_PLAN_FILES, files).Select(a => new MemshipYearlyPlanFileDto
        {
            Id = a.FileId,
            FileName = a.FileName
        });
        CombineStatuses(_storageService);
        return IsValid ? result : null;
    }
    public StorageFile DownloadFile(Guid fileId)
    {
        var entity = _unitOfWork.Context.Set<MemshipYearlyPlanFile>().FirstOrDefault(a => a.Id == fileId);
        return Download(fileId, entity, DocumentStorageConst.DOC_MEMSHIP_YEARLY_PLAN_FILES);
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
            .Set<MemshipYearlyPlanFile>()
            .FirstOrDefault(a => a.Id == fileId);

        Delete(fileId, entity, DocumentStorageConst.DOC_MEMSHIP_YEARLY_PLAN_FILES);
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
