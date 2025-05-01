using SspUis.DataLayer.EfClasses.Kpi;
using SspUis.DataLayer.EfClasses;
using SspUis.DataLayer.Repositories.Kpi;
using SspUis.DataLayer;
using SspUis.ServiceLayer.NumberServices;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WEBASE.Models;
using SspUis.Core.Security;
using SspUis.Core;

using SspUis.DataLayer.EfClasses.Hrm;
using WEBASE;
using SspUis.BizLogicLayer.Hrm.EmployeeManageServices;
using Microsoft.EntityFrameworkCore;
using SspUis.DataLayer.Repositories;
using Microsoft.AspNetCore.Http;
using System.IO;
using OfficeOpenXml;
using WEBASE.Storage;
using WEBASE.i18n;
using OfficeOpenXml.Style;

namespace SspUis.BizLogicLayer.Kpi;

public class KpiPlanForEmployeeService : BaseEntityService<long, KpiPlanForEmployee, KpiPlanForEmployeeListDto, KpiPlanForEmployeeDto, CreateKpiPlanForEmployeeDlDto, UpdateKpiPlanForEmployeeDlDto, IKpiPlanForEmployeeRepository, KpiPlanForEmployeeSortFilterOptions>, IKpiPlanForEmployeeService
{
    IAuthService _authService;
    IUnitOfWork _unitOfWork;
    private readonly INumberService _numberService;
    private readonly IKpiPlanForEmployeeRepository _repository;
    private readonly IStorageService _storageService;
    private readonly ICultureHelper _cultureHelper;


    public KpiPlanForEmployeeService(
        IUnitOfWork unitOfWork,
        INumberService numberService,

        IAuthService authService
,
        IStorageService storageService,
        ICultureHelper cultureHelper)
        : base(unitOfWork)
    {
        this._repository = unitOfWork.KpiPlanForEmployeeRepository;
        this._authService = authService;
        this._unitOfWork = unitOfWork;
        _numberService = numberService;
        _storageService = storageService;
        _cultureHelper = cultureHelper;
    }

    public PagedResult<KpiPlanForEmployeeListDto> GetList(KpiPlanForEmployeeSortFilterOptions options)
    {
        return Repository.ReadAsNoTracked<KpiPlanForEmployeeListDto>()
            .SortFilter(options)
            .AsPagedResult(options);
    }


    public KpiPlanForEmployeeDto Get()
    {
        return new KpiPlanForEmployeeDto
        {
            DocOn = DateOnly.FromDateTime(DateTime.Now),
            DocNumber = _numberService.GetNext(NumberTemplateDocumentConst.DOC_KPI_PLAN_FOR_EMPLOYEE, 1).Item2,
            OrganizationId = _authService.User.OrganizationId
        };
    }

    public KpiPlanForEmployeeDto Get(long id)
    {
        var dto = _repository.ById<KpiPlanForEmployeeDto>(id);
        CombineStatuses(_repository);
        if (dto is not null)
        {
            dto.CanModify = StatusIdConst.CanKpiPlanForEmployee(dto.StatusId, StatusIdConst.MODIFIED) && _authService.HasPermission(ModuleCode.KpiPlanForEmployeeEdit);
            dto.CanAccept = StatusIdConst.CanKpiPlanForEmployee(dto.StatusId, StatusIdConst.ACCEPTED) && _authService.HasPermission(ModuleCode.KpiPlanForEmployeeAccept);
            dto.CanCancel = StatusIdConst.CanKpiPlanForEmployee(dto.StatusId, StatusIdConst.NOT_ACCEPTED) && _authService.HasPermission(ModuleCode.KpiPlanForEmployeeCancel);
            dto.CanDelete = StatusIdConst.CanKpiPlanForEmployee(dto.StatusId, StatusIdConst.DELETED) && _authService.HasPermission(ModuleCode.KpiPlanForEmployeeDelete);
        }
        return dto;
    }
    public SelectList<long> AsSelectList()
    {
        return _repository.AllAsQueryable.Where(a => a.StatusId == StatusIdConst.ACCEPTED)
                        .AsSelectList();
    }

    public HaveId<long> Create(CreateKpiPlanForEmployeeDlDto dto)
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

    public override void Update(UpdateKpiPlanForEmployeeDlDto dto)
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

    public void Accept(UpdateStatusKpiPlanForEmployeeDlDto dTo)
    {
        var dto = new UpdateStatusKpiPlanForEmployeeDto { Id = dTo.Id, StatusId = StatusIdConst.ACCEPTED };
        UpdateStatus(dto, ent =>
        {
            if (!StatusIdConst.CanApplyStatus(ent.StatusId, dto.StatusId))
                _repository.AddError("Нет доступа");
        });
    }

    public void Cancel(UpdateStatusKpiPlanForEmployeeDlDto dTo)
    {
        var dto = new UpdateStatusKpiPlanForEmployeeDto { Id = dTo.Id, StatusId = StatusIdConst.CANCELED };
        UpdateStatus(dto, ent =>
        {
            if (!StatusIdConst.CanApplyStatus(ent.StatusId, dto.StatusId))
                _repository.AddError("Нет доступа");
        });
    }

    private HaveId<long> UpdateStatus(UpdateStatusKpiPlanForEmployeeDlDto dto, Action<KpiPlanForEmployee> validation)
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

    public List<KpiPlanForEmployeeTableDto> FillTable(int organizationId)
    {
       var data = _unitOfWork.Context.EmployeeManages.Where(s => s.OrganizationId == organizationId && s.EndOn == null && !s.IsDeleted);
       var indicators = _unitOfWork.IndicatorRepository.Context.Set<Indicator>().Where(s => s.StateId ==StateIdConst.ACTIVE).OrderBy(s => s.OrderCode);
        var result = data.Select(a => new KpiPlanForEmployeeTableDto
        {

            EmployeeManage = a.Employee.Person.FullName,
            EmployeeManageId = a.Id,
            Department = a.Department.FullName,
            DepartmentCode = a.Department.OrderCode,
            DepartmentId = a.Department.Id,
            Creates = indicators.Select(d => new KpiPlanEmployeeIndicatorCreateDto()
            {
                Indicator = d.FullName,
                IndicatorId = d.Id,
                IsAble = d.DepartmentId == a.Department.IndicatorDepartmentId //*|| d.DepartmentId == a.Department.Parent.IndicatorDepartmentId*/) ,

            }).ToList(),


        }).OrderBy(s => s.DepartmentCode).ToList();

       
        return result;



    }
    public List<KpiPlanForEmployeeTableDto> FillTableByDepartment(int organizationId)
    {
        // Get distinct department information first
        var distinctDepartments = _unitOfWork.Context.EmployeeManages
            .Where(s => s.OrganizationId == organizationId && s.EndOn == null && !s.IsDeleted)
            .Select(a => new
            {
                DepartmentId = a.Department.Id,
                DepartmentFullName = a.Department.FullName,
                DepartmentOrderCode = a.Department.OrderCode,
                IndicatorDepartmentId = a.Department.IndicatorDepartmentId
            })
            .Distinct() // Apply distinct on the department fields (without the Creates collection)
            .ToList();

        // Get active indicators
        var indicators = _unitOfWork.IndicatorRepository.Context.Set<Indicator>()
            .Where(s => s.StateId == StateIdConst.ACTIVE)
            .OrderBy(s => s.OrderCode)
            .ToList();

        // Build the result with distinct departments and corresponding indicator Creates
        var result = distinctDepartments.Select(dept => new KpiPlanForEmployeeTableDto
        {
            Department = dept.DepartmentFullName,
            DepartmentCode = dept.DepartmentOrderCode,
            DepartmentId = dept.DepartmentId,
            // Creates is projected after the distinct filtering
            Creates = indicators.Select(d => new KpiPlanEmployeeIndicatorCreateDto
            {
                Indicator = d.FullName,
                IndicatorId = d.Id,
                IsAble = (d.DepartmentId == dept.IndicatorDepartmentId /* || d.DepartmentId == dept.Parent.IndicatorDepartmentId*/)
            }).ToList()
        }).ToList();

        return result;
    }

    //public List<KpiPlanForEmployeeTableDto> FillTableByDepartment(int organizationId)
    //{
    //    var data = _unitOfWork.Context.EmployeeManages.Where(s => s.OrganizationId == organizationId && s.EndOn == null && !s.IsDeleted);
    //    var indicators = _unitOfWork.IndicatorRepository.Context.Set<Indicator>().Where(s => s.StateId == StateIdConst.ACTIVE).OrderBy(s => s.OrderCode);
    //    var result = data.Select(a => new KpiPlanForEmployeeTableDto
    //    {

    //        Department = a.Department.FullName,
    //        DepartmentCode = a.Department.OrderCode,
    //        DepartmentId = a.Department.Id,
    //        Creates = indicators.Select(d => new KpiPlanEmployeeIndicatorCreateDto()
    //        {
    //            Indicator = d.FullName,
    //            IndicatorId = d.Id,
    //            IsAble = (d.DepartmentId == a.Department.IndicatorDepartmentId /*|| d.DepartmentId == a.Department.Parent.IndicatorDepartmentId*/),

    //        }).ToList(),


    //    }).ToList();


    //    return result;
    //}

    private void Validation<TDto>(KpiPlanForEmployeeDlDto<TDto> dto, KpiPlanForEmployee entity)
         where TDto : KpiPlanForEmployeeDlDto<TDto>
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

    static bool IsTrailingFinalRow(int row, int column, ExcelWorksheet worksheet) =>
            String.IsNullOrEmpty(worksheet.Cells[row, column].Value?.ToString());

    public void ReadFromExcelFile(IFormFile file)
    {
        var memoryStream = ValidateSpreadSheetNotNull(file);

        int row = 2;
        int column = 1;
        using var excelPackage = new ExcelPackage(memoryStream);
        ExcelWorksheet worksheet =
            excelPackage.Workbook.Worksheets[0];

    }

    private static MemoryStream ValidateSpreadSheetNotNull(IFormFile file)
    {
        if (file is null)
        {
            throw new NullReferenceException();
        }

        MemoryStream memoryStream = new MemoryStream();
        file.CopyTo(memoryStream);
        memoryStream.Position = 1;

        return memoryStream;
    }

    //public Stream DownloadExcelTemplate()
    //{
    //    var data = UnitOfWork.IndicatorRepository.ReadAsNoTracked<IndicatorDto>().ToList();

    //    MemoryStream result = new MemoryStream();
    //    MemoryStream template = _storageService.GetStaticFile(StaticFileConst.ExcelTemplate.GetFileName(_cultureHelper.CurrentCulture.Code, StaticFileConst.Report.XududiyKPI));

    //    if (IsValid && data != null)
    //    {
    //        ExcelPackage.LicenseContext = LicenseContext.NonCommercial;
    //        using (ExcelPackage excelPackage = new ExcelPackage(template))
    //        {
    //            var ws = excelPackage.Workbook.Worksheets[0];
    //            var insertRow = excelPackage.Workbook.Names["insertRow"];
    //            int templateRow = insertRow.Start.Row;
    //            int currentRow = insertRow.Start.Row;
    //            int index = 1;

    //            foreach (var item in data)
    //            {
    //                // Copy the template row
    //                ws.Cells[templateRow, 1, templateRow, ws.Dimension.End.Column].Copy(ws.Cells[currentRow, 1]);

    //                // Update specific cells
    //                ws.InsertRow(currentRow, 1, insertRow.Start.Row);
    //                ws.Cells[currentRow, 1].Value = index++;
    //                ws.Cells[currentRow, 2].Value = item.FullName;

    //                // Adjust formulas in the copied row
    //                for (int col = 1; col <= ws.Dimension.End.Column; col++)
    //                {
    //                    var cell = ws.Cells[currentRow, col];
    //                    if (cell.Formula.StartsWith("="))
    //                    {
    //                        cell.Formula = AdjustFormula(cell.Formula, currentRow - templateRow);
    //                    }
    //                }

    //                currentRow++;
    //            }



    //            ws.Workbook.CalcMode = ExcelCalcMode.Automatic;
    //            ws.Workbook.Calculate();

    //            ws.DeleteRow(insertRow.Start.Row);
    //            result = new MemoryStream(excelPackage.GetAsByteArray());
    //            excelPackage.Dispose();
    //        }

    //        result.Position = 0;
    //        return result;
    //    }
    //    return result;
    //}

    //private string AdjustFormula(string formula, int rowDifference)
    //{
    //    // This is a simple adjustment. You might need to make it more sophisticated
    //    // depending on the complexity of your formulas.
    //    return formula.Replace("$", "").Replace("#ССЫЛКА!", "");
    //}

    public Stream DownloadExcelTemplate()
    {
        var data = UnitOfWork.IndicatorRepository.ReadAsNoTracked<IndicatorDto>().ToList();
       
        var regions = _unitOfWork.Context.Set<Region>().OrderBy(a=>a.OrderCode).ToList();
        MemoryStream result = new MemoryStream();
        MemoryStream template = _storageService.GetStaticFile(StaticFileConst.ExcelTemplate.GetFileName(_cultureHelper.CurrentCulture.Code, StaticFileConst.Report.XududiyKPI));
        regions.RemoveAt(0);
        if (IsValid && data != null)
        {
            ExcelPackage.LicenseContext = LicenseContext.NonCommercial;
            ExcelPackage excelPackage = new ExcelPackage(template);
            var ws = excelPackage.Workbook.Worksheets[0];
            var insertRow = excelPackage.Workbook.Names["insertRow"];

            var reg = excelPackage.Workbook.Names["regionsArea"];

            int currentRow = insertRow.Start.Row + 1;
            int reColumn = reg.Start.Column;
            int index = 1;


            foreach (var item in regions)
            {
                ws.Cells[reg.Start.Row, reColumn].Value = item.FullName;
                ws.Cells[reg.Start.Row, reColumn, reg.Start.Row, reColumn + 3].Merge = true;
                ws.Cells.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                ws.Cells.Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                reColumn += 4;
            }
            foreach (var item in data)
            {
                var KpiIndicator = _unitOfWork.Context.Set<KpiGratingIndicator>().Include(a=>a.Indicator).Include(t=>t.Tables).Where(a=>a.Indicator.Id == item.Id);
                var column = 1;
              
                ws.InsertRow(currentRow, 1, insertRow.Start.Row);

                ws.Cells[currentRow, column++].Value = index++;
                ws.Cells[currentRow, column++].Value = item.FullName;
                int section1 = column; 
                ws.Cells[currentRow, column+=2].Formula = $"({ws.Cells[currentRow, column-1]}/{ws.Cells[currentRow, column - 2]}* 100)";  //formula %
                while(reColumn >= column +4)
                {
                     ws.Cells[currentRow, column+=4].Formula = $"({ws.Cells[currentRow, column - 1]}/{ws.Cells[currentRow, column - 2]}* 100)"; //formula %
                }

                currentRow++;

            }
            ws.DeleteRow(insertRow.Start.Row);
            result = new MemoryStream(excelPackage.GetAsByteArray());
            excelPackage.Dispose();
            result.Position = 0;
            return result;

        }
        return result;
    }
}
