using SspUis.DataLayer.EfClasses;
using SspUis.DataLayer.Repositories;
using System;
using System.Collections.Generic;
using WEBASE.Models;
using WEBASE.Storage;

namespace SspUis.BizLogicLayer;

public interface ISrvApplicationYearlyPlanService
    : IBaseEntityService<long, SrvApplicationYearlyPlan, SrvApplicationYearlyPlanListDto, SrvApplicationYearlyPlanDto, CreateSrvApplicationYearlyPlanDlDto, UpdateSrvApplicationYearlyPlanDlDto,SrvApplicationYearlyPlanSortFilterOptions>
{
    PagedResult<SrvApplicationYearlyPlanListDto> GetList(SrvApplicationYearlyPlanSortFilterOptions options);
    SrvApplicationYearlyPlanDto Get();
    List<SrvApplicationYearlyPlanTableCellDto> ConvertToCellTables(long? id, int? regionId);
    SrvApplicationYearlyPlanDto Get(long id);
    SelectList<long> AsSelectList();
    HaveId<long> Create(CreateSrvApplicationYearlyPlanDlDto dto);
    void Accept(UpdateStatusSrvApplicationYearlyPlanDto dTo);
    void Cancel(UpdateStatusSrvApplicationYearlyPlanDto dTo);
    void Update(UpdateSrvApplicationYearlyPlanDlDto dto);
    void Delete(long id);
    IEnumerable<SrvApplicationYearlyPlanFileDto> UploadFiles(params StorageFile[] files);
    StorageFile DownloadFile(Guid fileId);
    void DeleteFile(Guid fileId);
}
