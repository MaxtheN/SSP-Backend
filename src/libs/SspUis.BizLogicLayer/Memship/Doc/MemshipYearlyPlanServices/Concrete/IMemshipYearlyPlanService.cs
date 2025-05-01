using SspUis.DataLayer.EfClasses.Memship;
using SspUis.DataLayer.Repositories.Memship;
using System;
using System.Collections.Generic;
using WEBASE.Models;
using WEBASE.Storage;

namespace SspUis.BizLogicLayer.Memship;

public interface IMemshipYearlyPlanService
    : IBaseEntityService<long, MemshipYearlyPlan, MemshipYearlyPlanListDto, MemshipYearlyPlanDto, CreateMemshipYearlyPlanDlDto, UpdateMemshipYearlyPlanDlDto,MemshipYearlyPlanSortFilterOptions>
{
    PagedResult<MemshipYearlyPlanListDto> GetList(MemshipYearlyPlanSortFilterOptions options);
    MemshipYearlyPlanDto Get();
    List<MemshipYearlyPlanDto> FillTable();
    MemshipYearlyPlanDto Get(long id);
    SelectList<long> AsSelectList();
    HaveId<long> Create(CreateMemshipYearlyPlanDlDto dto);
    void Accept(UpdateStatusMemshipYearlyPlanDto dTo);
    void Cancel(UpdateStatusMemshipYearlyPlanDto dTo);
    void Update(UpdateMemshipYearlyPlanDlDto dto);
    void Delete(long id);
    IEnumerable<MemshipYearlyPlanFileDto> UploadFiles(params StorageFile[] files);
    StorageFile DownloadFile(Guid fileId);
    void DeleteFile(Guid fileId);
}
