using SspUis.BizLogicLayer.Kpi;
using SspUis.DataLayer.EfClasses.Kpi;
using SspUis.DataLayer.Repositories;
using System;
using System.Collections.Generic;
using WEBASE.Models;
using WEBASE.Storage;

namespace SspUis.BizLogicLayer;

public interface IKpiRatingEmployeeService
    : IBaseEntityService<long, KpiRatingEmployee, KpiRatingEmployeeListDto, KpiRatingEmployeeDto, CreateKpiRatingEmployeeDlDto, UpdateKpiRatingEmployeeDlDto,KpiRatingEmployeeSortFilterOptions>
{
    PagedResult<KpiRatingEmployeeListDto> GetList(KpiRatingEmployeeSortFilterOptions options);
    KpiRatingEmployeeDto Get();
    KpiRatingEmployeeDto Get(long id);
    List<KpiRatingEmployeeTableDto> FillTable(int kpiPlanId);
   
    SelectList<long> AsSelectList();
    HaveId<long> Create(CreateKpiRatingEmployeeDlDto dto);
    void Accept(UpdateStatusKpiRatingEmployeeDto dTo);
    void Cancel(UpdateStatusKpiRatingEmployeeDto dTo);
    void Update(UpdateKpiRatingEmployeeDlDto dto);
    void Delete(long id);
}
