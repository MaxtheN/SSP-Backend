using Microsoft.AspNetCore.Http;
using SspUis.DataLayer.EfClasses.Kpi;
using SspUis.DataLayer.Repositories.Kpi;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WEBASE.Models;

namespace SspUis.BizLogicLayer.Kpi;

public interface IKpiPlanForEmployeeService : IBaseEntityService<long, KpiPlanForEmployee, KpiPlanForEmployeeListDto, KpiPlanForEmployeeDto, CreateKpiPlanForEmployeeDlDto, UpdateKpiPlanForEmployeeDlDto, KpiPlanForEmployeeSortFilterOptions>

{
    PagedResult<KpiPlanForEmployeeListDto> GetList(KpiPlanForEmployeeSortFilterOptions options);
    KpiPlanForEmployeeDto Get();
    List<KpiPlanForEmployeeTableDto> FillTable(int organizationId);
    List<KpiPlanForEmployeeTableDto> FillTableByDepartment(int organizationId);
    KpiPlanForEmployeeDto Get(long id);
    SelectList<long> AsSelectList();
    HaveId<long> Create(CreateKpiPlanForEmployeeDlDto dto);
    void Accept(UpdateStatusKpiPlanForEmployeeDlDto dTo);
    void Cancel(UpdateStatusKpiPlanForEmployeeDlDto dTo);
    void Update(UpdateKpiPlanForEmployeeDlDto dto);
    void Delete(long id);
    void ReadFromExcelFile(IFormFile file);
    Stream DownloadExcelTemplate();
}