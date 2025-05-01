using Microsoft.EntityFrameworkCore;
using OfficeOpenXml;
using SspUis.Core.Security;
using SspUis.DataLayer;
using SspUis.DataLayer.EfClasses.Hrm;
using StatusGeneric;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using WEBASE.i18n;
using WEBASE.Models;
using WEBASE.Storage;
using WEBASE.Utility;

namespace SspUis.BizLogicLayer.Hrm.Info.EmployeeTurnstileReportServices;

public class EmployeeTurnstileReportService : StatusGenericHandler, IEmployeeTurnstileReportService
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly ICultureHelper _cultureHelper;
    private readonly IAuthService _authService;
    private readonly IStorageService _storageService;

    private readonly TimeSpan OFFICE_IN_TIME = new TimeSpan(9, 0, 0);
    private readonly TimeSpan OFFICE_OUT_TIME = new TimeSpan(18, 0, 0);
    private readonly TimeSpan CALC_NIGHT_HOUR = new TimeSpan(7, 0, 0);

    public EmployeeTurnstileReportService(
        IUnitOfWork unitOfWork,
        ICultureHelper cultureHelper,
        IAuthService authService,
        IStorageService storageService)
    {
        _unitOfWork = unitOfWork;
        _cultureHelper = cultureHelper;
        _authService = authService;
        _storageService = storageService;
    }

    public async Task<List<EmployeeTurnstileReportDto>> GetEmployeeTurnstileReportAsync(EmployeeTurnstileReportDtoFilter filter)
    {
        filter.OnDate ??= DateTime.Today;
        filter.EndDate ??= DateTime.Today;

        DateTime startDate = filter.OnDate.Value.StartOfDay(), endDate = filter.EndDate.Value.EndOfDay();
        var languageId = _cultureHelper.CurrentCulture.Id;

        int organizationId = _authService.User.IsAdmin ? (int)filter.OrganizationId : _authService.User.OrganizationId;

        TimeSpan startLunchTime = new TimeSpan(13, 0, 0);
        TimeSpan endLunchTime = new TimeSpan(14, 0, 0);

        try
        {
            IQueryable<EmployeeTurnstileReportDto> query = null;

            if (filter.IsLate)
            {
                query = _unitOfWork.Context.GetEmployeeTurnstileReport(startDate, endDate, organizationId, languageId,
                    filter.EnterTime, filter.ExitTime, startLunchTime, endLunchTime, filter.Employee, OFFICE_IN_TIME, OFFICE_OUT_TIME, CALC_NIGHT_HOUR, true, filter.IsLeftEarly, filter.IsBreakLunchTime);
            }
            else
            {
                query = _unitOfWork.Context.GetEmployeeTurnstileReport(startDate, endDate, organizationId, languageId,
                    filter.EnterTime, filter.ExitTime, startLunchTime, endLunchTime, filter.Employee, OFFICE_IN_TIME, OFFICE_OUT_TIME, CALC_NIGHT_HOUR, false, filter.IsLeftEarly, filter.IsBreakLunchTime);

                if (filter.IsArrived)
                {
                    query = query.Where(a => a.EventOn != null);
                }
                else if (filter.IsMissed)
                {
                    query = query.Where(a => a.EventOn == null);
                }
            }
            return await query.ToListAsync();
        }
        catch (Exception ex)
        {
            AddError(ex.Message);
        }

        return null;
    }

    public List<EmployeeTurnstileTimeReportByIdDto> GetEmployeeTurnstileReportById(EmployeeTurnstileReportByIdDtoFilter filter)
    {
        filter.OnDate ??= DateTime.Today;
        filter.EndDate ??= DateTime.Today;

        DateTime startDate = filter.OnDate.Value.StartOfDay(), endDate = filter.EndDate.Value.EndOfDay();
        var languageId = _cultureHelper.CurrentCulture.Id;

        int organizationId = _authService.User.IsAdmin ? (int)filter.OrganizationId : _authService.User.OrganizationId;

        try
        {
            var result = _unitOfWork.Context.GetEmployeeTurnstileReportById(startDate, endDate, organizationId, languageId,
                filter.EmployeeId, filter.EnterTime, filter.ExitTime, OFFICE_IN_TIME, OFFICE_OUT_TIME, CALC_NIGHT_HOUR)
                .AsEnumerable()
                .GroupBy(r => new
                {
                    r.EventOn,
                    r.WeekDay
                })
                .Select(group => new EmployeeTurnstileTimeReportByIdDto
                {
                    EmployeeId = group.First().EmployeeId,
                    EventOn = group.Key.EventOn.ToString("dd.MM.yyyy"),
                    WeekDay = group.Key.WeekDay,
                    TotalPeriodTime = FormatTimeSpan(TimeSpan.FromMinutes(group.Sum(r => r.PeriodMinute))),
                    TotalPeriodTimeSchedule = FormatTimeSpan(TimeSpan.FromMinutes(group.Sum(r => r.PeriodMinuteSchedule))),
                    TimeReportList = group.Select(item => new TimeReport
                    {
                        EnterAt = item.EnterAt,
                        ExitAt = item.ExitAt,
                        PeriodMinute = item.PeriodMinute,
                        EnterAtSchedule = item.EnterAtSchedule,
                        ExitAtSchedule = item.ExitAtSchedule,
                        PeriodMinuteSchedule = item.PeriodMinuteSchedule
                    }).ToList()
                })
                .ToList();

            return result;
        }
        catch (Exception ex)
        {
            AddError(ex.Message);
        }
        return null;
    }

    public PagedResult<EmployeeTurnstileTimeReportDto> GetEmployeeTurnstileTimeReport(EmployeeTurnstileTimeReportDtoFilter filter)
    {
        var languageId = _cultureHelper.CurrentCulture.Id;
        int organizationId = _authService.User.IsAdmin ? (int)filter.OrganizationId : _authService.User.OrganizationId;

        DateTime startDate = filter.Date.StartOfDay(), endDate = filter.Date.EndOfDay();
        int pageIndex = filter.PageIndex == 0 ? 1 : filter.PageIndex;
        int pageSize = filter.PageSize == 0 ? 20 : filter.PageSize;

        if (filter.ByMonth)
        {
            startDate = new DateTime(filter.Date.Year, filter.Date.Month, 1).StartOfDay();
            endDate = new DateTime(filter.Date.Year, filter.Date.Month, DateTime.DaysInMonth(filter.Date.Year, filter.Date.Month)).EndOfDay();
        }
        else if (filter.ByWeek)
        {
            startDate = filter.Date.AddDays(-(int)filter.Date.DayOfWeek + 1).StartOfDay();
            endDate = startDate.AddDays(6).EndOfDay();
        }
        TimeSpan startLunchTime = new TimeSpan(13, 0, 0);
        TimeSpan endLunchTime = new TimeSpan(14, 0, 0);

        try
        {
            var query = _unitOfWork.Context.GetEmployeeTurnstileReport(startDate, endDate, organizationId, languageId,
                filter.EnterTime, filter.ExitTime, startLunchTime, endLunchTime, filter.Employee, OFFICE_IN_TIME, OFFICE_OUT_TIME, CALC_NIGHT_HOUR, false, false, false)
                .AsEnumerable()
                .GroupBy(emp => new
                {
                    EmployeeId = emp.EmployeeId,
                    EmployeeName = emp.EmployeeName,
                    PositionId = emp.PositionId,
                    PositionName = emp.PositionName
                })
                .Select(g => new EmployeeTurnstileTimeReportDto
                {
                    EmployeeId = g.Key.EmployeeId,
                    EmployeeName = g.Key.EmployeeName,
                    PositionId = g.Key.PositionId,
                    PositionName = g.Key.PositionName,
                    TotalWorkedTime = FormatTimeSpan(TimeSpan.FromMinutes((double)g.Sum(r => r.PeriodMinute))),
                    TotalScheduledWorkTime = FormatTimeSpan(TimeSpan.FromMinutes((double)g.Sum(r => r.PeriodMinuteSchedule))),
                    TurnstileTimeInfos = GetAllDatesInRange(startDate, endDate)
                    .GroupJoin(g,
                        date => date,
                        item => item != null && item.EventOn != null ? DateTime.ParseExact(item.EventOn, "dd.MM.yyyy", CultureInfo.InvariantCulture).Date : default(DateTime),
                        (date, items) => new { Date = date, Items = items })
                       .SelectMany(x => x.Items.DefaultIfEmpty(), (x, item) => new TurnstileTimeInfo
                       {
                           EventDate = x.Date.ToString("dd.MM.yyyy"),
                           WeekDay = item != null ? item.WeekDay : x.Date.ToString("dddd"),
                           WorkTime = (item == null || item.PeriodMinute == null) ? null : FormatTimeSpan(TimeSpan.FromMinutes((double)item.PeriodMinute)),
                           ScheduledWorkTime = (item == null || item.PeriodMinuteSchedule == null) ? null : FormatTimeSpan(TimeSpan.FromMinutes((double)item.PeriodMinuteSchedule)),
                           IsLate = (item == null || string.IsNullOrEmpty(item.EnterAt)) ? false : TimeSpan.Parse(item.EnterAt) > filter.EnterTime,
                           IsLeaveEarly = (item == null || string.IsNullOrEmpty(item.ExitAt)) ? false : TimeSpan.Parse(item.ExitAt) < filter.ExitTime
                       })
                       .ToList()
                });

            var totalCount = query.Count();

            var totalPages = (int)Math.Ceiling((double)totalCount / pageSize);
            pageIndex = Math.Min(pageIndex, totalPages);

            var employeeTurnstileInfo = query
                .Skip((pageIndex - 1) * pageSize)
                .Take(pageSize)
                .ToList();

            return new PagedResult<EmployeeTurnstileTimeReportDto>
            {
                Page = pageIndex,
                PageSize = pageSize,
                Total = totalCount,
                Rows = employeeTurnstileInfo
            };
        }
        catch (Exception ex)
        {
            AddError(ex.Message);
        }
        return null;
    }

    public async Task<Stream> SaveEmployeeTurnstileReportAsExcel(EmployeeTurnstileReportDtoFilter filter)
    {
        var data = await GetEmployeeTurnstileReportAsync(filter);

        MemoryStream result = new MemoryStream();
        MemoryStream template = _storageService.GetStaticFile(StaticFileConst.ExcelTemplate.GetFileName(_cultureHelper.CurrentCulture.Code,
            StaticFileConst.Report.EMPLOYEE_TURNSTILE_REPORT));

        if (IsValid && data != null)
        {
            ExcelPackage.LicenseContext = LicenseContext.NonCommercial;
            ExcelPackage excelPackage = new ExcelPackage(template);

            var ws = excelPackage.Workbook.Worksheets[0];

            var importRow = excelPackage.Workbook.Names["ImportRow"];
            int currentRow = importRow.Start.Row + 1;
            int index = 1;
            foreach (var item in data)
            {
                var column = 1;
                ws.InsertRow(currentRow, 1, importRow.Start.Row);
                ws.Cells[currentRow, column++].Value = index++;
                ws.Cells[currentRow, column++].Value = item.EmployeeName;
                ws.Cells[currentRow, column++].Value = item.PositionName;
                ws.Cells[currentRow, column++].Value = item.EventOn;
                ws.Cells[currentRow, column++].Value = item.EnterAt + "(" + item.EnterAtSchedule + ")";
                ws.Cells[currentRow, column++].Value = item.ExitAt + "(" + item.ExitAtSchedule + ")";
                ws.Cells[currentRow, column++].Value = (item.PeriodMinute.HasValue ? FormatTimeSpan(TimeSpan.FromMinutes((double)item.PeriodMinute)) : "0.0") + "("
                    + (item.PeriodMinuteSchedule.HasValue ? FormatTimeSpan(TimeSpan.FromMinutes((double)item.PeriodMinuteSchedule)) : "0.0") + ")";
                ws.Cells[currentRow, column++].Value = item.PeriodMinuteTotal.HasValue ? FormatTimeSpan(TimeSpan.FromMinutes((double)item.PeriodMinuteTotal)) : "0.0";
                ws.Cells[currentRow, column++].Value = item.EnterCount;
                currentRow++;
            }
            ws.DeleteRow(importRow.Start.Row);
            result = new MemoryStream(excelPackage.GetAsByteArray());
            excelPackage.Dispose();
        }
        result.Position = 0;
        return result;
    }

    public async Task<Stream> SaveEmployeeTurnstileReportById(EmployeeTurnstileReportByIdDtoFilter filter)
    {
        var data = GetEmployeeTurnstileReportById(filter);

        EmployeeManage employeeManage = null;
        if (!filter.EmployeeId.HasValue)
        {
            AddError("Employee id null");
        }
        else
        {
            employeeManage = _unitOfWork.Context.Set<EmployeeManage>()
                    .Include(x => x.Employee).ThenInclude(f => f.Person)
                   .Include(p => p.Position)
                   .ThenInclude(p => p.Translates)
                   .FirstOrDefault(a => a.EmployeeId == filter.EmployeeId.Value && a.EndOn == null && !a.IsDeleted);
        }

        MemoryStream result = new MemoryStream();
        MemoryStream template = _storageService.GetStaticFile(StaticFileConst.ExcelTemplate.GetFileName(_cultureHelper.CurrentCulture.Code,
            StaticFileConst.Report.TURNSTILE_REPORT_ON_EMPLOYEE));

        if (IsValid && data != null)
        {
            ExcelPackage.LicenseContext = LicenseContext.NonCommercial;
            ExcelPackage excelPackage = new ExcelPackage(template);

            var ws = excelPackage.Workbook.Worksheets[0];

            var importRow = excelPackage.Workbook.Names["ImportRow"];
            int currentRow = importRow.Start.Row + 1;
            if(employeeManage == null) { AddError("EmployeeManage null"); }
            ws.Cells[importRow.Start.Row - 4, importRow.Start.Column + 1].Value = employeeManage?.Employee?.Person.FullName;
            ws.Cells[importRow.Start.Row - 3, importRow.Start.Column + 1].Value = employeeManage?.Position?.FullName;
            foreach (var item in data)
            {
                var column = 1;

                var eventOn = item?.EventOn;
                foreach (var temp in item.TimeReportList)
                {
                    ws.InsertRow(currentRow, 1, importRow.Start.Row);
                    column = 1;
                    ws.Cells[currentRow, column++].Value = eventOn;
                    ws.Cells[currentRow, column++].Value = temp?.EnterAt;
                    ws.Cells[currentRow, column++].Value = temp?.ExitAt;
                    ws.Cells[currentRow, column++].Value = $"{temp?.PeriodMinute / 60} : {temp?.PeriodMinute % 60}";
                    currentRow++;
                }
            }
            ws.DeleteRow(importRow.Start.Row);
            result = new MemoryStream(excelPackage.GetAsByteArray());
            excelPackage.Dispose();
        }
        result.Position = 0;
        return result;
    }

    private IEnumerable<DateTime> GetAllDatesInRange(DateTime startDate, DateTime endDate)
    {
        for (var date = startDate.Date; date <= endDate.Date; date = date.AddDays(1))
        {
            yield return date;
        }
    }

    private string FormatTimeSpan(TimeSpan timeSpan)
    {
        int totalHours = (int)timeSpan.TotalHours;
        int minutes = timeSpan.Minutes;

        return $"{totalHours:00}:{minutes:00}";
    }
}
