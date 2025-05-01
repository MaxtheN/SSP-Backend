using SspUis.DataLayer.EfClasses;
using SspUis.DataLayer.Repositories;
using System;
using System.Collections.Generic;
using WEBASE.Models;
using WEBASE.Storage;

namespace SspUis.BizLogicLayer;

public interface ISrvYearlyPlanService
    : IBaseEntityService<long, SrvYearlyPlan, SrvYearlyPlanListDto, SrvYearlyPlanDto, CreateSrvYearlyPlanDlDto, UpdateSrvYearlyPlanDlDto,SrvYearlyPlanSortFilterOptions>
{
    PagedResult<SrvYearlyPlanListDto> GetList(SrvYearlyPlanSortFilterOptions options);
    SrvYearlyPlanDto Get();
    List<SrvYearlyPlanDto> FillTable(int? regionId);
    SrvYearlyPlanDto Get(long id);
    SelectList<long> AsSelectList();
    HaveId<long> Create(CreateSrvYearlyPlanDlDto dto);
    void Accept(UpdateStatusSrvYearlyPlanDto dTo);
    void Cancel(UpdateStatusSrvYearlyPlanDto dTo);
    void Update(UpdateSrvYearlyPlanDlDto dto);
    void Delete(long id);
    IEnumerable<SrvYearlyPlanFileDto> UploadFiles(params StorageFile[] files);
    StorageFile DownloadFile(Guid fileId);
    void DeleteFile(Guid fileId);
}
