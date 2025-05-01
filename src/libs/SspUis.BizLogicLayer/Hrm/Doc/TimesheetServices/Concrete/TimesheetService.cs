using GenericServices;
using Microsoft.EntityFrameworkCore;
using OfficeOpenXml;
using OfficeOpenXml.Style;
using SspUis.BizLogicLayer.DocumentChangeLogServices;
using SspUis.BizLogicLayer.Hrm.EmployeeManageServices;
using SspUis.BizLogicLayer.Hrm.WorkScheduleServices;
using SspUis.BizLogicLayer.ManualServices;
using SspUis.Core;
using SspUis.Core.Security;
using SspUis.DataLayer;
using SspUis.DataLayer.EfClasses;
using SspUis.DataLayer.EfClasses.Hrm;
using SspUis.DataLayer.Repositories;
using SspUis.DataLayer.Repositories.Hrm;
using SspUis.ServiceLayer.NumberServices;
using StatusGeneric;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using WEBASE;
using WEBASE.i18n;
using WEBASE.Models;
using WEBASE.Storage;
using WEBASE.Utility;

namespace SspUis.BizLogicLayer.Hrm;

public class TimesheetService : StatusGenericHandler, ITimesheetService
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IAuthService _authService;
    private readonly IDocumentChangeLogService _documentChangeLogService;
    private readonly ITimesheetRepository _repository;
    private readonly IEmployeeManageRepository _employeeManageRepository;
    private readonly IWorkScheduleService _workScheduleService;
    private readonly ICrudServices _crudServices;
    private readonly INumberService _numberService;
    private readonly IEmployeeMissedDayService _employeeMissedDayService;
    private readonly IStorageService _storageService;
    private readonly ICultureHelper _cultureHelper;
    private readonly IEmployeeSickLeaveService _employeeSickLeaveService;
    private readonly IEmployeeLeaveOrderService _employeeLeaveOrderService;
    private readonly IOrderToSendBusinessTripService _orderToSendBusinessTripService;
    private readonly IEmployeeSendStudyService _employeeSendStudyService;
    private readonly IManualService _manualService;

    public TimesheetService(
        IUnitOfWork unitOfWork,
        IAuthService authService,
        IEmployeeManageRepository employeeManageRepository,
        IWorkScheduleService workScheduleService,
        ICrudServices crudServices,
        IDocumentChangeLogService documentChangeLogService,
        ITimesheetRepository repository,
        INumberService numberService,
        IEmployeeMissedDayService employeeMissedDayService,
        IStorageService storageService,
        ICultureHelper cultureHelper,
        IEmployeeSickLeaveService employeeSickLeaveService,
        IEmployeeLeaveOrderService employeeLeaveOrderService,
        IOrderToSendBusinessTripService orderToSendBusinessTripService,
        IEmployeeSendStudyService employeeSendStudyService,
        IManualService manualService)
    {
        _unitOfWork = unitOfWork;
        _authService = authService;
        _documentChangeLogService = documentChangeLogService;
        _repository = unitOfWork.TimesheetRepository;
        _employeeManageRepository = employeeManageRepository;
        _workScheduleService = workScheduleService;
        _crudServices = crudServices;
        _numberService = numberService;
        _employeeMissedDayService = employeeMissedDayService;
        _storageService = storageService;
        _cultureHelper = cultureHelper;
        _employeeSickLeaveService = employeeSickLeaveService;
        _employeeLeaveOrderService = employeeLeaveOrderService;
        _orderToSendBusinessTripService = orderToSendBusinessTripService;
        _employeeSendStudyService = employeeSendStudyService;
        _manualService = manualService;
    }

    #region Public CRUD Methods
    public PagedResult<TimesheetListDto> GetList(TimesheetSortFilterOptions options)
    {
        var result = _repository
            .ReadAsNoTracked<TimesheetListDto>()
            .SortFilter(options)
            .AsPagedResult(options);
        return result;
    }

    public TimesheetDto Get()
    {
        var today = DateTime.Today;

        int year = today.Year;
        int month = today.Day > 10 ? today.Month : today.Month - 1;
        int organizationId = _authService.User.OrganizationId;

        if (today.Month == 1 && today.Day <= 10)
            year--;

        return new TimesheetDto
        {
            DocOn = DateOnly.FromDateTime(DateTime.Today),
            Month = month,
            Year = year,
            DocNumber = _numberService.GetNext(NumberTemplateDocumentConst.DOC_TIMESHEET, organizationId).Item2
        };
    }

    public TimesheetDto Get(long id)
    {
        return GetMethod(id);
    }

    private TimesheetDto GetMethod(long id)
    {
        var dto = _repository.ById<TimesheetDto>(id);
        CombineStatuses(_repository);

        if (IsValid)
        {
            dto.CanModify = StatusIdConst.CanApplyTimesheet(dto.StatusId, StatusIdConst.MODIFIED) && _authService.HasPermission(ModuleCode.TimesheetEdit);
            dto.CanAccept = StatusIdConst.CanApplyTimesheet(dto.StatusId, StatusIdConst.ACCEPTED) && _authService.HasPermission(ModuleCode.TimesheetAccept);
            dto.CanCancel = StatusIdConst.CanApplyTimesheet(dto.StatusId, StatusIdConst.NOT_ACCEPTED) && _authService.HasPermission(ModuleCode.TimesheetCancel);
            dto.CanDelete = StatusIdConst.CanApplyTimesheet(dto.StatusId, StatusIdConst.DELETED) && _authService.HasPermission(ModuleCode.TimesheetDelete);
            dto.Tables = dto.Tables.OrderBy(a => a.Employee).ToList();
        }
        return dto;
    }

    public SelectList<long> AsSelectList(int? employeeId = null)
    {
        return _repository.AllAsQueryable
            .Include(a => a.Tables)
            .Where(a => employeeId.HasValue ? a.Tables.Any(a => a.EmployeeId == employeeId.Value) : true && a.StatusId == StatusIdConst.ACCEPTED)
                        .AsSelectList();
    }

    public HaveId<long> Create(CreateTimesheetDlDto dto)
    {
        try
        {
            using (var transaction = _unitOfWork.BeginTransaction())
            {
                var entity = _repository.Create(dto, ent =>
                {
                    Validation(dto, ent);
                });

                CombineStatuses(_repository);
                if (HasErrors)
                {
                    transaction.Rollback();
                    return null;
                }

                _unitOfWork.Save();
                var res = CreateDocumentChangeLog(entity.Id, entity.StatusId);

                if (IsValid) transaction.Commit();
                return res;
            }
        }
        catch (Exception ex)
        {
            AddError(ex.Message + ex.InnerException?.Message);
            return null;
        }
    }

    public TimesheetTableDto GetTable(long id)
    {
        return GetTableMethod(id);
    }

    public TimesheetTableWithDaysDlDto UpdateTable(TimesheetTableWithDaysDlDto dto)
    {
        using (var transaction = _unitOfWork.BeginTransaction())
        {
            var entity = _repository.UpdateTable(dto);

            CombineStatuses(_repository);
            if (HasErrors)
            {
                transaction.Rollback();
                return null;
            }

            _repository.Context.SaveChanges();
            CreateDocumentChangeLog(entity.OwnerId, StatusIdConst.MODIFIED);

            if (IsValid)
            {
                transaction.Commit();
                return entity;
            }

            transaction.Rollback();
            return null;
        }
    }

    public HaveId<long> FillTimeSheet(TimesheetFillDto dto)
    {
        var validationEntity = _repository.AllAsQueryable.FirstOrDefault(a => a.Id == dto.Id);
        Validation(dto, validationEntity);

        CombineStatuses(_repository);
        if (HasErrors)
            return null;

        try
        {
            if (dto.Id > 0)
            {
                ClearTimeSheetTable(new ClearTableDto
                {
                    Id = dto.Id,
                    CreateLog = false
                });
            }

            dto.Tables = new List<TimesheetTableWithDaysDlDto>();

            var startDate = new DateTime(dto.Year, dto.Month, 1);
            var endDate = startDate.LastDayOfMonth();
            var tableResult = new List<TimesheetTableDto>();

            var employeeManageList = GetEmployeeManageInfo(DateOnly.FromDateTime(startDate), DateOnly.FromDateTime(endDate), dto.DepartmentId, null,
                dto.TimesheetTypeId, dto.Year, dto.Month).ToArray();

            if (employeeManageList == null)
            {
                AddError("Ишга қабул қилинган ҳодимлар рўйхати топилмади");
                return null;
            }

            var excludedPeriods = GetExcludePeriods(DateOnly.FromDateTime(startDate), DateOnly.FromDateTime(endDate));

            CalculateWorkingTime(dto, startDate, endDate, employeeManageList, excludedPeriods);

            if (IsValid)
            {
                var result = Save(dto);
                if (IsValid) return result;
            }

            AddError("Произошла ошибка при заполнении");
            return null;
        }
        catch (Exception ex)
        {
            AddError(ex.Message + ex.InnerException?.Message);
            return null;
        }
    }

    public void Update(UpdateTimesheetDlDto dto)
    {
        using (var transaction = _unitOfWork.BeginTransaction())
        {
            try
            {
                var entity = _repository.Update(dto, ent =>
                {
                    if (!StatusIdConst.CanApplyTimesheet(ent.StatusId, StatusIdConst.MODIFIED))
                        _repository.AddError("Нет доступа");
                    else
                        Validation(dto, ent);
                });

                CombineStatuses(_repository);
                if (HasErrors)
                {
                    transaction.Rollback();
                    return;
                }

                _unitOfWork.Save();
                CreateDocumentChangeLog(entity.Id, entity.StatusId);

                if (IsValid)
                    transaction.Commit();
                else
                    transaction.Rollback();
            }
            catch (Exception ex)
            {
                AddError(ex.Message);
                transaction.Rollback();
            }
        }
    }

    public void Accept(UpdateStatusTimesheetDto dTo)
    {
        var dto = new UpdateStatusTimesheetDlDto { Id = dTo.Id, StatusId = StatusIdConst.ACCEPTED };
        UpdateStatus(dto, ent =>
        {
            if (!StatusIdConst.CanApplyTimesheet(ent.StatusId, dto.StatusId))
            {
                _repository.AddError("Нет доступа");
                return;
            }

            bool isDeleted = _unitOfWork.Context.Set<EmployeeManage>()
                .Where(a => ent.Tables.Select(a => a.EmployeeManageId).Contains(a.Id))
                .Any(a => a.IsDeleted);

            if (isDeleted)
            {
                _repository.AddError("Сотрудник был удален");
                return;
            }
        });
    }

    public void Cancel(UpdateStatusTimesheetDto dTo)
    {
        var dto = new UpdateStatusTimesheetDlDto { Id = dTo.Id, StatusId = StatusIdConst.NOT_ACCEPTED };
        UpdateStatus(dto, ent =>
        {
            if (!StatusIdConst.CanApplyTimesheet(ent.StatusId, dto.StatusId))
                _repository.AddError("Нет доступа");
        });
    }

    public void Delete(long id)
    {
        var dto = new UpdateStatusTimesheetDlDto { Id = id, StatusId = StatusIdConst.DELETED };
        UpdateStatus(dto, ent =>
        {
            if (!StatusIdConst.CanApplyTimesheet(ent.StatusId, dto.StatusId))
                _repository.AddError("Нет доступа");
        });
    }

    public HaveId<long> ClearTimeSheetTable(ClearTableDto dto)
    {
        using (var transaction = _unitOfWork.BeginTransaction())
        {
            var entity = _repository.AllAsQueryable
                            .Include(a => a.Tables.Where(a => !a.IsDeleted))
                            .FirstOrDefault(a => a.Id == dto.Id);

            CombineStatuses(_repository);
            if (HasErrors)
                return null;

            if (!StatusIdConst.CanApplyTimesheet(entity.StatusId, StatusIdConst.MODIFIED))
            {
                _repository.AddError("Нет доступа");
                return null;
            }

            if (entity.Tables?.Any() == true)
            {
                if (dto.TableIds?.Any() == true)
                {
                    foreach (var entityTable in entity.Tables.Where(a => dto.TableIds.Contains(a.Id)))
                        entityTable.MarkAsDeleted();
                }
                else
                {
                    foreach (var entityTable in entity.Tables)
                        entityTable.MarkAsDeleted();
                }

                CombineStatuses(_repository);
                if (HasErrors)
                {
                    transaction.Rollback();
                    return null;
                }

                _unitOfWork.Save();
                if (dto.CreateLog.HasValue && dto.CreateLog.Value)
                {
                    var res = CreateDocumentChangeLog(entity.Id, StatusIdConst.MODIFIED);
                    if (IsValid)
                        transaction.Commit();
                    return res;
                }
                else
                {
                    transaction.Commit();
                }
            }
            return HaveId.Create(entity.Id);
        }
    }

    public TimesheetFillDto CalculateWorkingTime(
                TimesheetFillDto dto,
                DateTime startDate,
                DateTime endDate,
                EmployeeManageDto[] employeeManageList,
                Dictionary<int, List<ExcludePeriodDto>> excludePeriods)
    {
        try
        {
            if (dto.TimesheetTypeId == TimeSheetTypeIdConst.HalfMonth)
                endDate = new DateTime(endDate.Year, endDate.Month, 15);

            if (endDate < startDate)
                return null;

            Dictionary<int, List<WorkScheduleWorkHourDto>> dicWorkHours = new();
            foreach (var manage in employeeManageList)
            {
                if (!dicWorkHours.ContainsKey(manage.WorkScheduleId))
                    dicWorkHours.Add(manage.WorkScheduleId, _workScheduleService
                        .GetWorkSscheduleWorkHours(manage.WorkScheduleId, DateOnly.FromDateTime(startDate), DateOnly.FromDateTime(endDate)));

                IEnumerable<WorkScheduleWorkHourDto> workHours = dicWorkHours[manage.WorkScheduleId];

                IEnumerable<ExcludePeriodDto> excludePeriodsByEmployee =
                    excludePeriods.ContainsKey(manage.EmployeeId) ? excludePeriods[manage.EmployeeId] : new List<ExcludePeriodDto>();

                //DateTime startEmpDate = manage.StartOn > DateOnly.FromDateTime(startDate) ? manage.StartOn.AsDateTime() : startDate;

                //DateTime? endEmpDate = (DateTimeUtility.ToNullable(manage.EndOn) > endDate ||
                //                        DateTimeUtility.ToNullable(manage.EndOn) == null) ? endDate : manage.EndOn?.AsDateTime();

                DateOnly startEmpDate = manage.StartOn > DateOnly.FromDateTime(startDate) ? manage.StartOn : DateOnly.FromDateTime(startDate);

                DateOnly? endEmpDate = (manage.EndOn > DateOnly.FromDateTime(endDate) || manage.EndOn == null)
                    ? DateOnly.FromDateTime(endDate)
                    : manage.EndOn;

                var timeSheetManage = new TimesheetTableWithDaysDlDto()
                {
                    DepartmentId = manage.DepartmentId,
                    EmployeeId = manage.EmployeeId,
                    PositionId = manage.PositionId,
                    EmployeeManageId = manage.Id,
                    WorkScheduleId = manage.WorkScheduleId,
                    EmploymentRate = manage.EmploymentRate.Value,
                    PlanDays = workHours.Sum(x => x.Days),
                    PlanHours = workHours.Sum(x => x.Hours),
                    FactDays = 0,
                    FactHours = 0,
                    DayOffHours = 0,
                    NightHours = 0,
                    //StartOn = DateOnly.FromDateTime(startEmpDate),
                    //EndOn = DateOnly.FromDateTime((DateTime)endEmpDate),
                    StartOn = startEmpDate,
                    EndOn = endEmpDate,
                    EmploymentTypeId = manage.EmploymentTypeId,
                    DocumentTableId = manage.DocTableId,
                    DocumentId = manage.DocId,
                    DocumentInfo = manage.DocumentInfo
                };

                Dictionary<long, TimesheetTableDayDlDto> tableDaysDic = new Dictionary<long, TimesheetTableDayDlDto>();
                for (DateTime date = startDate; date <= endDate;)
                {
                    int plandays = workHours
                                    .Where(x => x.DateOn == DateOnly.FromDateTime(date))
                                        .Sum(x => x.Days);

                    decimal planhours = workHours
                                    .Where(x => x.DateOn == DateOnly.FromDateTime(date))
                                        .Sum(x => x.Hours);

                    var tableDay = new TimesheetTableDayDlDto()
                    {
                        TimesheetIndicatorId = 22,
                        DateOn = DateOnly.FromDateTime(date),
                        PlanDays = plandays,
                        PlanHours = planhours,
                        FactDays = 0,
                        FactHours = 0,
                        DayOffHours = 0,
                        NightHours = 0
                    };

                    timeSheetManage.TableDays.Add(tableDay);
                    tableDaysDic.Add(date.Ticks, tableDay);
                    date = date.AddDays(1);
                }

                for (DateTime date = startEmpDate.AsDateTime(); date <= endEmpDate.Value.AsDateTime();)
                {
                    var tableDay = tableDaysDic[date.Ticks];
                    int timesheetindicatorid = 0;

                    int factdays = 0;
                    decimal facthours = 0m;

                    if (tableDay.PlanDays == 0)
                    {
                        timesheetindicatorid = 20;
                        factdays = 0;
                        facthours = 0;
                    }
                    else
                    {
                        var excludePeriod = excludePeriodsByEmployee
                            .FirstOrDefault(x => x.StartDate <= DateOnly.FromDateTime(date) &&
                                                 x.EndDate >= DateOnly.FromDateTime(date) && (x.ExcludePeriodType == ExcludePerionType.SickLeave || x.ExcludePeriodType == ExcludePerionType.LeaveOrder || x.ExcludePeriodType == ExcludePerionType.LeaveOrderWithoutPay));
                        var sendtrainPriod = excludePeriodsByEmployee
                           .FirstOrDefault(x => x.StartDate <= DateOnly.FromDateTime(date) &&
                                                x.EndDate >= DateOnly.FromDateTime(date) && !(x.ExcludePeriodType == ExcludePerionType.SickLeave || x.ExcludePeriodType == ExcludePerionType.LeaveOrder || x.ExcludePeriodType == ExcludePerionType.LeaveOrderWithoutPay));

                        if (sendtrainPriod != null)
                        {
                            if (sendtrainPriod.ExcludePeriodType == ExcludePerionType.SickLeaveIsMaternityLeave)
                                timesheetindicatorid = 11;

                            if (sendtrainPriod.ExcludePeriodType == ExcludePerionType.SendTrain)
                                timesheetindicatorid = 8;

                            factdays = tableDay.PlanDays;
                            facthours = tableDay.PlanHours;

                            if (sendtrainPriod.ExcludePeriodType == ExcludePerionType.LeaveOrderWithoutPay)
                            {
                                timesheetindicatorid = 14;
                                factdays = 0;
                                facthours = 0;
                            }
                        }
                        else if (excludePeriod == null)
                        {
                            timesheetindicatorid = 1;
                            factdays = tableDay.PlanDays;
                            facthours = tableDay.PlanHours;
                        }
                        else
                        {
                            factdays = 0;
                            facthours = 0;

                            timesheetindicatorid = excludePeriod.ExcludePeriodType switch
                            {
                                ExcludePerionType.LeaveOrder => 6,
                                ExcludePerionType.LeaveOrderWithoutPay => 14,
                                ExcludePerionType.SickLeave => 16,
                                ExcludePerionType.SickLeaveIsMaternityLeave => 11,
                                ExcludePerionType.SendTrain => 8,
                                _ => 20,
                            };
                        }
                    }

                    tableDay.TimesheetIndicatorId = timesheetindicatorid;
                    tableDay.FactDays = factdays;
                    tableDay.FactHours = facthours;
                    date = date.AddDays(1);
                }

                timeSheetManage.FactDays = timeSheetManage.TableDays
                                                .Sum(x => x.FactDays);

                timeSheetManage.FactHours = timeSheetManage.TableDays
                                                .Sum(x => x.FactHours);

                dto.Tables.Add(timeSheetManage);
            }
            return dto;
        }
        catch (Exception ex)
        {
            AddError(ex.Message);
            return null;
        }
    }
    #endregion

    #region Excel Methods
    public Stream SaveAsExecelForTabel(long id)
    {
        var data = GetMethod(id);

        MemoryStream result = new MemoryStream();
        MemoryStream template = _storageService.GetStaticFile(StaticFileConst.ExcelTemplate.GetFileName(_cultureHelper.CurrentCulture.Code, StaticFileConst.Report.TABEL_TEMPLATE));

        if (IsValid && data != null)
        {
            var MonthsList = _manualService.GetMonthSelectList();
            var curMonth = data.DocOn.Month;
            string monthName = MonthsList.FirstOrDefault(m => m.Value == curMonth).Text;
            int endDay = data.TimesheetTypeId == 1 ? 15 : DateTime.DaysInMonth(data.DocOn.Year, data.DocOn.Month);
            ExcelPackage.LicenseContext = LicenseContext.NonCommercial;
            ExcelPackage excelPackage = new ExcelPackage(template);
            Console.WriteLine(excelPackage.Workbook.Worksheets.Count);
            var department = excelPackage.Workbook.Worksheets[PositionID: 0];
            department.Cells["A12"].Value = !string.IsNullOrWhiteSpace(data.Department) ? data.Department : string.Empty;
            department.Cells["A6"].Value = $"{data.Organization} \r\n {data.Department} xodimlarining 2023-yil 1-{endDay} {monthName} kunlarida bo‘lgan ish soatlari\r\nTABEL JADVALI";
            var importRow = excelPackage.Workbook.Names["InsertRow"];
            int currentRow = importRow.Start.Row;
            int index = 1;
            var ws = importRow.Worksheet;
            ws.Cells.Style.Font.Color.SetColor(System.Drawing.Color.Black);
            ws.Cells.Style.Fill.PatternType = ExcelFillStyle.Solid;
            ws.Cells.Style.Fill.BackgroundColor.SetColor(System.Drawing.Color.White);

            foreach (var item in data.Tables)
            {
                var column = 1;
                ws.InsertRow(currentRow, 1, importRow.Start.Row);
                ws.Cells[currentRow, column++].Value = index++;

                ws.Cells[currentRow, column++].Value = item.Employee;
                ws.Cells[currentRow, column++].Value = item.Position;
                int i = column;
                int startDaysColumn = 4;


                foreach (var item2 in item.TableDays)
                {
                    if (item2.DateOn.DayOfWeek == DayOfWeek.Saturday || item2.DateOn.DayOfWeek == DayOfWeek.Sunday)
                    {
                        ws.Cells[currentRow, i].AutoFitColumns();
                        ws.Cells[Row: currentRow, Col: i].Value = "do";
                        ws.Cells[currentRow, i].Style.Fill.BackgroundColor.SetColor(System.Drawing.Color.LightGray);
                        i++;
                        continue;
                    }
                    else
                    {
                        ws.Cells[currentRow, i].AutoFitColumns();
                        ws.Cells[Row: currentRow, Col: i].Value = item2.FactHours;
                        i++;
                        column = i;
                    }
                }

                GetData(currentRow, ws, item, startDaysColumn);

                var startCell = ws.Cells[currentRow, 4];
                var endCell = ws.Cells[currentRow, 34];
                double sum = 0;
                int numericCellCount = 0;
                for (int row = startCell.Start.Row; row <= endCell.End.Row; row++)
                {
                    for (int col = startCell.Start.Column; col <= endCell.End.Column; col++)
                    {
                        var cell = ws.Cells[row, col];
                        if (cell.Value != null && double.TryParse(cell.Value.ToString(), out double cellValue))
                        {

                            if (cellValue != 0)
                            {
                                numericCellCount++;
                                sum += cellValue;
                            }
                        }
                    }
                }
                ws.Cells[currentRow, 35].Value = sum;
                ws.Cells[currentRow, 36].Value = numericCellCount;
                // ws.Cells[currentRow, 35].Value = item.TableDays.Sum(s => s.FactHours);
                currentRow++;
            }

            ws.DeleteRow(importRow.Start.Row);
            result = new MemoryStream(excelPackage.GetAsByteArray());
            excelPackage.Dispose();
        }
        result.Position = 0;
        return result;
    }
    #endregion

    #region Private Methods
    private TimesheetTableDto GetTableMethod(long id)
    {
        return _crudServices.ProjectFromEntityToDto<TimesheetTable, TimesheetTableDto>(q =>
                      q.Where(a => !a.IsDeleted)).FirstOrDefault(a => a.Id == id);
    }

    private HaveId<long> Save(TimesheetFillDto result)
    {
        using (var transaction = _unitOfWork.BeginTransaction())
        {
            try
            {
                var entity = _repository.Fill(result);
                CombineStatuses(_repository);

                if (HasErrors)
                {
                    transaction.Rollback();
                    return null;
                }

                _unitOfWork.Save();

                if (IsValid)
                {
                    transaction.Commit();
                    return HaveId.Create(entity.Id);
                }
                else
                {
                    transaction.Rollback();
                    return null;
                }
            }
            catch (Exception ex)
            {
                AddError(ex.Message);
                transaction.Rollback();
                return null;
            }
        }
    }

    private HaveId<long> UpdateStatus(UpdateStatusTimesheetDlDto dto, Action<Timesheet> validation)
    {
        using (var transaction = _unitOfWork.BeginTransaction())
        {
            try
            {
                var entity = _repository.UpdateStatus(dto, validation);

                CombineStatuses(_repository);
                if (HasErrors)
                {
                    transaction.Rollback();
                    return null;
                }

                _unitOfWork.Save();
                var res = CreateDocumentChangeLog(entity.Id, entity.StatusId);

                if (IsValid)
                {
                    transaction.Commit();
                    return res;
                }

                transaction.Rollback();
                return null;
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
        var moveDto = _repository.ById<TimesheetDto>(id, applyFilter: false);

        _documentChangeLogService.Create(
            dto: moveDto,
            tableId: TableIdConst.DOC_TIMESHEET,
            organizationId: null,
            statusId: statusId,
            message: message);

        CombineStatuses(_documentChangeLogService);

        if (HasErrors)
            return null;

        return HaveId.Create(id);
    }

    private IEnumerable<EmployeeManageDto> GetEmployeeManageInfo(DateOnly startDate, DateOnly endDate, int? departmentId, long? empManageId,
        int timeSheetTypeId, int year, int month)
    {
        var staffings = _unitOfWork.Context.Set<Staffing>()
                                           .Where(a => a.StatusId == StatusIdConst.RECEIVED &&
                                                a.OrganizationId == _authService.User.OrganizationId &&
                                                a.StartOn <= endDate);

        DateOnly? maxDate = staffings.Any() ? staffings.Max(a => a.StartOn) : null;

        var staffingPositions = _unitOfWork.Context.Set<StaffingPosition>()
            .Include(a => a.Owner)
                .Where(a => a.Owner.StatusId == StatusIdConst.RECEIVED &&
                            a.Owner.OrganizationId == _authService.User.OrganizationId &&
                            a.Owner.StartOn == maxDate)
            .Select(a => a.PositionId);

        var usedManegeId = _unitOfWork.Context.Set<TimesheetTable>()
                          .Where(x => x.Owner.StatusId == StatusIdConst.ACCEPTED
                          && x.Owner.TimesheetTypeId == timeSheetTypeId
                          && x.Owner.MonthOn.Year == year && x.Owner.MonthOn.Month == month
                          && x.Owner.OrganizationId == _authService.User.OrganizationId)
                         .Select(x => x.EmployeeManageId);

        var result = _employeeManageRepository.ReadAsNoTracked<EmployeeManageDto>()
                .Where(a => a.OrganizationId == _authService.User.OrganizationId &&
                            a.EmploymentRate > 0 &&
                            a.StartOn <= endDate &&
                            (a.EndOn == null || a.EndOn >= startDate) &&
                            staffingPositions.Contains(a.PositionId) &&
                            (empManageId.HasValue ? a.Id == empManageId : true) &&
                            (departmentId.HasValue ? a.DepartmentId == departmentId : true)
                            && !usedManegeId.Contains(a.Id));

        result = result.Join(
                _unitOfWork.Context.Set<AppointEmployeeTable>()
                    .Include(a => a.Owner)
                    .Where(a => a.Owner.StatusId == StatusIdConst.ACCEPTED &&
                                a.Owner.OrganizationId == _authService.User.OrganizationId)
                    , emg => emg.Id
                    , aps => aps.EmployeeManageId
                    , (emg, aps) => new EmployeeManageDto()
                    {
                        Id = emg.Id,
                        DepartmentId = emg.DepartmentId,
                        Department = emg.Department,
                        EmpAppointOrderTypeId = emg.EmpAppointOrderTypeId,
                        EmployeeBirthOn = emg.EmployeeBirthOn,
                        Employee = emg.Employee,
                        EmployeeId = emg.EmployeeId,
                        EmployeePhoneNumber = emg.EmployeePhoneNumber,
                        EmployeePinfl = emg.EmployeePinfl,
                        EmploymentRate = emg.EmploymentRate,
                        EmploymentTypeId = emg.EmploymentTypeId,
                        EmploymentTypeName = emg.EmploymentTypeName,
                        EndByDocumentOn = emg.EndByDocumentOn,
                        OrganizationId = emg.OrganizationId,
                        Organization = emg.Organization,
                        Position = emg.Position,
                        PositionId = emg.PositionId,
                        WorkScheduleId = emg.WorkScheduleId,
                        StartOn = aps.Owner.DocOn, //emg.StartOn,
                        DocTableId = TableIdConst.DOC_APPOINT_EMPLOYEE,
                        DocId = aps.Id,
                        EndDocId = emg.EndDocId,
                        EndOn = emg.EndOn, //aps.EndOn,
                        DocumentInfo = $"№ {aps.Owner.DocOn}(Id{aps.Id}) от {aps.Owner.DocOn} (Назначение зарплаты сотрудника)"
                    });

        return result;
    }

    private Dictionary<int, List<ExcludePeriodDto>> GetExcludePeriods(DateOnly startDate, DateOnly endDate)
    {
        var orgId = _authService.User.OrganizationId;
        var result = new Dictionary<int, List<ExcludePeriodDto>>();

        var leaveOrderDic = _unitOfWork.Context.Set<EmployeeLeaveOrderTable>()
            .Where(a => a.Owner.OrganizationId == orgId &&
                        (a.StartOn >= startDate || a.EndOn <= endDate) &&
                        a.Owner.StatusId == StatusIdConst.ACCEPTED &&
                        a.Owner.DocOn <= endDate)
            .Select(a => new ExcludePeriodDto()
            {
                StartDate = a.StartOn,
                EndDate = a.EndOn,
                EmployeeId = a.EmployeeId,
                ExcludePeriodType = a.IsWithOutPay ? ExcludePerionType.LeaveOrderWithoutPay : ExcludePerionType.LeaveOrder
            })
            .AsEnumerable()
            .GroupBy(a => a.EmployeeId)
            .ToDictionary(k => k.Key, v => v.ToList());

        var reCalllLeaveList = _unitOfWork.Context.Set<RecallLeaveTable>()
            .Where(a => a.Owner.OrganizationId == orgId && (a.EmployeeLeaveOrderTable.StartOn >= startDate && a.EmployeeLeaveOrderTable.EndOn <= endDate))
            .Select(a => new ExcludePeriodDto()
            {
                StartDate = a.StartOn,
                EmployeeId = a.EmployeeId
            })
            .ToArray();

        foreach (var reCalllLeave in reCalllLeaveList)
        {
            if (leaveOrderDic.ContainsKey(reCalllLeave.EmployeeId))
                foreach (var leaveOrder in leaveOrderDic[reCalllLeave.EmployeeId])
                    leaveOrder.EndDate = reCalllLeave.StartDate.AddDays(-1);
        }

        var sickLeaveList = _unitOfWork.Context.Set<EmployeeSickLeaveTable>()
            .Where(a => a.Owner.OrganizationId == orgId &&
                       (a.StartOn >= startDate || a.EndOn <= endDate) &&
                        a.Owner.StatusId == StatusIdConst.ACCEPTED &&
                        a.Owner.DocOn <= endDate &&
                        !a.IsMaternityLeave)
            .Select(a => new ExcludePeriodDto()
            {
                StartDate = a.StartOn,
                EmployeeId = a.EmployeeId,
                EndDate = a.EndOn,
                ExcludePeriodType = ExcludePerionType.SickLeave
            })
            .AsEnumerable()
            .GroupBy(a => a.EmployeeId);

        var sickLeaveIsMaternityLeave = _unitOfWork.Context.Set<EmployeeSickLeaveTable>()
            .Where(a => a.Owner.OrganizationId == orgId &&
                       (a.StartOn >= startDate || a.EndOn <= endDate) &&
                        a.Owner.StatusId == StatusIdConst.ACCEPTED &&
                        a.Owner.DocOn <= endDate &&
                        a.IsMaternityLeave)
            .Select(a => new ExcludePeriodDto()
            {
                StartDate = a.StartOn,
                EmployeeId = a.EmployeeId,
                EndDate = a.EndOn,
                ExcludePeriodType = ExcludePerionType.SickLeaveIsMaternityLeave
            })
            .AsEnumerable()
            .GroupBy(a => a.EmployeeId);

        var sendTrainList = _unitOfWork.Context.Set<EmployeeSendTrainTable>()
            .Where(a => a.Owner.OrganizationId == orgId &&
                       (a.StartOn >= startDate || a.EndOn <= endDate) &&
                        a.Owner.StatusId == StatusIdConst.ACCEPTED &&
                        a.Owner.DocOn <= endDate)
            .Select(a => new { a.StartOn, a.EmployeeId, a.EndOn })
            .AsEnumerable()
            .Select(a => new ExcludePeriodDto()
            {
                StartDate = a.StartOn,
                EmployeeId = a.EmployeeId,
                EndDate = a.EndOn,
                ExcludePeriodType = ExcludePerionType.SendTrain
            })
            .GroupBy(a => a.EmployeeId);

        result = leaveOrderDic;
        foreach (var grSource in new[] { sickLeaveList, sickLeaveIsMaternityLeave, sendTrainList })
            foreach (var grItem in grSource)
            {
                if (!result.ContainsKey(grItem.Key))
                    result[grItem.Key] = new List<ExcludePeriodDto>();
                result[grItem.Key].AddRange(grItem);
            }

        return result;
    }

    private void GetData(int currentRow, ExcelWorksheet ws, TimesheetTableDto item, int startDaysColumn)
    {
        var getData_K = _employeeSickLeaveService.GetByEmployeeId(item.EmployeeId);

        if (getData_K != null)
        {
            var start = getData_K.StartOn;
            var end = getData_K.EndOn;

            if (item.StartOn >= getData_K.StartOn && item.EndOn >= getData_K.EndOn)
            {
                start = item.StartOn.Value;
                //end = end
            }
            else if (item.StartOn <= getData_K.StartOn && item.EndOn <= getData_K.EndOn)
            {
                //start = start
                end = item.EndOn.Value;
            }
            else if (item.StartOn >= getData_K.StartOn && item.EndOn <= getData_K.EndOn)
            {
                start = item.StartOn.Value;
                end = item.EndOn.Value;
            }

            if (start >= item.StartOn && end <= item.EndOn)
            {
                var endDay = end.Day;
                var startDay = start.Day;

                int startColumn = (startDaysColumn - 1) + startDay;
                int endColumn = (startDaysColumn) + endDay;
                for (int j = startColumn; j < endColumn; j++)
                {
                    ws.Cells[currentRow, j].AutoFitColumns();
                    if (ws.Cells[Row: currentRow, Col: j].Value != "do")
                    {
                        ws.Cells[Row: currentRow, Col: j].Value = "k";
                        ws.Cells[currentRow, j].Style.Fill.BackgroundColor.SetColor(System.Drawing.Color.White);
                    }

                }
                return;
            }

        }

        var getData_T_OT = _employeeLeaveOrderService.GetByEmployeeId(item.EmployeeId, item.StartOn, item.EndOn);

        if (getData_T_OT != null)
        {
            var start = getData_T_OT.StartOn;
            var end = getData_T_OT.EndOn;

            if (item.StartOn >= getData_T_OT.StartOn && item.EndOn >= getData_T_OT.EndOn)
            {
                start = item.StartOn.Value;
                //end = end
            }
            else if (item.StartOn <= getData_T_OT.StartOn && item.EndOn <= getData_T_OT.EndOn)
            {
                //start = start
                end = item.EndOn.Value;
            }
            else if (item.StartOn >= getData_T_OT.StartOn && item.EndOn <= getData_T_OT.EndOn)
            {
                start = item.StartOn.Value;
                end = item.EndOn.Value;
            }

            if (start >= item.StartOn && end <= item.EndOn)
            {
                var startDay = start.Day;
                var endDay = end.Day;
                int startColumn = (startDaysColumn - 1) + startDay;
                int endColumn = (startDaysColumn) + endDay;
                for (int j = startColumn; j < endColumn; j++)
                {
                    ws.Cells[currentRow, j].AutoFitColumns();

                    if (getData_T_OT.IsWithOutPay == true)
                        ws.Cells[Row: currentRow, Col: j].Value = "o‘t";
                    else
                        ws.Cells[Row: currentRow, Col: j].Value = "t";

                    ws.Cells[currentRow, j].Style.Fill.BackgroundColor.SetColor(System.Drawing.Color.White);
                }

                return;
            }

        }

        var getData_XS = _orderToSendBusinessTripService.GetByEmployeeId(item.EmployeeManageId, item.StartOn, item.EndOn);

        if (getData_XS != null)
        {
            var start = getData_XS.BeginOn;
            var end = getData_XS.EndOn;

            if (item.StartOn >= getData_XS.BeginOn && item.EndOn >= getData_XS.EndOn)
            {
                start = item.StartOn.Value;
                //end = end
            }
            else if (item.StartOn <= getData_XS.BeginOn && item.EndOn <= getData_XS.EndOn)
            {
                //start = start
                end = item.EndOn.Value;
            }
            else if (item.StartOn >= getData_XS.BeginOn && item.EndOn <= getData_XS.EndOn)
            {
                start = item.StartOn.Value;
                end = item.EndOn.Value;
            }

            if (start >= item.StartOn && end <= item.EndOn)
            {
                var startDay = start.Day;
                var endDay = end.Day;
                int startColumn = (startDaysColumn - 1) + startDay;
                int endColumn = (startDaysColumn) + endDay;
                for (int j = startColumn; j < endColumn; j++)
                {
                    ws.Cells[currentRow, j].AutoFitColumns();
                    ws.Cells[Row: currentRow, Col: j].Value = "xs";
                    ws.Cells[currentRow, j].Style.Fill.BackgroundColor.SetColor(System.Drawing.Color.White);
                }

                return;
            }
        }

        var getData_O = _employeeSendStudyService.GetByEmployeeId(item.EmployeeId, item.StartOn, item.EndOn);

        if (getData_O != null)
        {
            var start = getData_O.StartOn;
            var end = getData_O.EndOn;

            if (item.StartOn >= getData_O.StartOn && item.EndOn >= getData_O.EndOn)
            {
                start = item.StartOn.Value;
                //end = end
            }
            else if (item.StartOn <= getData_O.StartOn && item.EndOn <= getData_O.EndOn)
            {
                //start = start
                end = item.EndOn.Value;
            }
            else if (item.StartOn >= getData_O.StartOn && item.EndOn <= getData_O.EndOn)
            {
                start = item.StartOn.Value;
                end = item.EndOn.Value;
            }
            if (start >= item.StartOn && end <= item.EndOn)
            {
                var startDay = start.Day;
                var endDay = end.Day;
                int startColumn = (startDaysColumn - 1) + startDay;
                int endColumn = (startDaysColumn) + endDay;
                for (int j = startColumn; j < endColumn; j++)
                {
                    ws.Cells[currentRow, j].AutoFitColumns();
                    ws.Cells[Row: currentRow, Col: j].Value = "o‘";
                    ws.Cells[currentRow, j].Style.Fill.BackgroundColor.SetColor(System.Drawing.Color.White);
                }

                return;
            }

        }

        var getData_P = _employeeMissedDayService.GetByEmployeeId(item.EmployeeManageId, item.StartOn, item.EndOn);

        if (getData_P != null)
        {
            var start = getData_P.StartAt;
            var end = getData_P.EndAt;

            if (item.StartOn >= DateOnly.FromDateTime(getData_P.StartAt) && item.EndOn >= DateOnly.FromDateTime(getData_P.EndAt))
            {
                start = item.StartOn.Value.ToDateTime(TimeOnly.MinValue);
                //end = end
            }
            else if (item.StartOn <= DateOnly.FromDateTime(getData_P.StartAt) && item.EndOn <= DateOnly.FromDateTime(getData_P.EndAt))
            {
                //start = start
                end = item.EndOn.Value.ToDateTime(TimeOnly.MinValue);
            }
            else if (item.StartOn >= DateOnly.FromDateTime(getData_P.StartAt) && item.EndOn <= DateOnly.FromDateTime(getData_P.EndAt))
            {
                start = item.StartOn.Value.ToDateTime(TimeOnly.MinValue);
                end = item.EndOn.Value.ToDateTime(TimeOnly.MinValue);
            }
            if (start >= item.StartOn.Value.ToDateTime(TimeOnly.MinValue) && end <= item.EndOn.Value.ToDateTime(TimeOnly.MinValue))
            {
                var startDay = start.Day;
                var endDay = end.Day;
                int startColumn = (startDaysColumn - 1) + startDay;
                int endColumn = (startDaysColumn) + endDay;
                for (int j = startColumn; j < endColumn; j++)
                {
                    ws.Cells[currentRow, j].AutoFitColumns();
                    ws.Cells[Row: currentRow, Col: j].Value = "p";
                    ws.Cells[currentRow, j].Style.Fill.BackgroundColor.SetColor(System.Drawing.Color.White);
                }
            }

        }
    }

    private void Validation<TDto>(TimesheetDlDto<TDto> dto, Timesheet entity)
         where TDto : TimesheetDlDto<TDto>
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
    #endregion
}