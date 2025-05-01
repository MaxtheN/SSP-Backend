using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Dynamic.Core;
using System.Threading.Tasks;
using DocumentFormat.OpenXml.InkML;
using Microsoft.EntityFrameworkCore;
using OpenXmlPowerTools;
using SspUis.Core;
using SspUis.DataLayer;
using SspUis.DataLayer.EfClasses;
using SspUis.DataLayer.EfClasses.Hrm;
using SspUis.Integration.DigitizationCenter;
using StatusGeneric;
using WEBASE;

namespace SspUis.BizLogicLayer.Hrm;
public class EmployeeWorkScheduleService : StatusGenericHandler, IEmployeeWorkScheduleService
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IDigitizationCenterMehnatService _mehnatService;
    public EmployeeWorkScheduleService(IUnitOfWork unitOfWork, IDigitizationCenterMehnatService mehnatService) 
    {
        _unitOfWork = unitOfWork;
        _mehnatService = mehnatService;
    }
    public async Task<Data> GetMehnatHistory(string pinfl)
    {
        var dto = new WorkPositionHistoryRequestDto { pin = pinfl };
        var res = await _mehnatService.GetMehnatHistory(dto);
        CombineStatuses(_mehnatService);
        return res;
    }

    public EmployeeWorkScheduleFromMehnatDto GetWorkYearFromMehnat(EmployeeWorkScheduleFromMehnatFilterOption option)
    {
        var result = new EmployeeWorkScheduleFromMehnatDto();

        var getMehnatHistory = GetMehnatHistory(option.Pinfl).Result.Experiences
            .Where(a => a.EndDate != null)
            .OrderBy(a => DateTime.Parse(a.StartDate.ToString()))
            .ToList();

        int totalDays = 0;

        foreach (var record in getMehnatHistory)
        {
            var startDate = DateTime.Parse(record.StartDate);
            var endDateRecord = DateTime.Parse(record.EndDate.ToString());

            var duration = endDateRecord - startDate;
            totalDays += duration.Days;
        }

        DateTime referenceDate = new DateTime(1, 1, 1);
        DateTime calculatedEndDate = referenceDate.AddDays(totalDays);

        result.MehnatYear = calculatedEndDate.Year - referenceDate.Year;
        result.MehnatMonth = calculatedEndDate.Month - referenceDate.Month;
        result.MehnatDay = calculatedEndDate.Day - referenceDate.Day;

        if (result.MehnatDay < 0)
        {
            result.MehnatMonth -= 1;
            result.MehnatDay += DateTime.DaysInMonth(calculatedEndDate.Year, calculatedEndDate.Month - 1);
        }

        if (result.MehnatMonth < 0)
        {
            result.MehnatYear -= 1;
            result.MehnatMonth += 12;
        }

        return result;
    }
    public EmployeeWorkScheduleDto GetTotalWorkYears(EmployeeWorkScheduleFilterOption option)
    {
        var result = new EmployeeWorkScheduleDto();

        if (!string.IsNullOrEmpty(option.Pinfl) && option.EmployeeId.HasValue)
        {
            var data = GetMehnatHistory(option.Pinfl);
            if (data == null || data.Result == null) return new EmployeeWorkScheduleDto();
            var getMehnatHistory = data.Result.Experiences
                .Where(a => a.EndDate != null)
                .OrderBy(a => DateTime.Parse(a.StartDate))
                .ToList();

            int totalDays = 0;

            foreach (var record in getMehnatHistory)
            {
                var startDate = DateTime.Parse(record.StartDate.ToString());
                var endDateRecord = DateTime.Parse(record.EndDate.ToString());

                var duration = endDateRecord - startDate;
                totalDays += duration.Days;
            }

            DateTime referenceDate = new DateTime(1, 1, 1);
            DateTime calculatedEndDate = referenceDate.AddDays(totalDays);

            result.MehnatYear = calculatedEndDate.Year - referenceDate.Year;
            result.MehnatMonth = calculatedEndDate.Month - referenceDate.Month;
            result.MehnatDay = calculatedEndDate.Day - referenceDate.Day;

            if (result.MehnatDay < 0)
            {
                result.MehnatMonth -= 1;
                result.MehnatDay += DateTime.DaysInMonth(calculatedEndDate.Year, calculatedEndDate.Month - 1);
            }

            if (result.MehnatMonth < 0)
            {
                result.MehnatYear -= 1;
                result.MehnatMonth += 12;
            }
            var employeeManage = _unitOfWork.Context.Set<EmployeeManage>()
                .Include(a => a.Employee)
                .ThenInclude(b => b.Person)
                .Where(a => a.EmployeeId == option.EmployeeId && a.EmpAppointOrderTypeId == EmpAppointOrderTypeIdConst.HIRE)
                .OrderBy(a => a.StartOn)
                .ToList();

            int totalEmployeeDays = 0;

            foreach (var record in employeeManage)
            {
                var startDate = record.StartOn.ToDateTime(new TimeOnly(0, 0));
                var recordEndDate = record.EndOn?.ToDateTime(new TimeOnly(0, 0)) ?? DateTime.Today;

                totalEmployeeDays += (recordEndDate - startDate).Days;
            }

            DateTime referenceEmployeeDate = new DateTime(1, 1, 1);
            DateTime endDate = referenceEmployeeDate.AddDays(totalEmployeeDays);

            result.Year = endDate.Year - referenceEmployeeDate.Year;
            result.Month = endDate.Month - referenceEmployeeDate.Month;
            result.Day = endDate.Day - referenceEmployeeDate.Day;

            if (result.Day < 0)
            {
                result.Month -= 1;
                result.Day += DateTime.DaysInMonth(endDate.Year, endDate.Month - 1);
            }

            if (result.Month < 0)
            {
                result.Year -= 1;
                result.Month += 12;
            }
        }
        else 
        {
            AddError("EmployeeId va Pinfl majburiy!");
            return null;
        }

        return result;
    }
    public EmployeeWorkScheduleFromSSPDto GetWorkYearFromEmpManage(EmployeeWorkScheduleFromSSPFilterOption option)
    {
        var result = new EmployeeWorkScheduleFromSSPDto();

        var employeeManage = _unitOfWork.Context.Set<EmployeeManage>()
            .Include(a => a.Employee)
            .ThenInclude(b => b.Person)
            .Where(a => a.EmployeeId == option.EmployeeId && a.EmpAppointOrderTypeId == EmpAppointOrderTypeIdConst.HIRE)
            .OrderBy(a => a.StartOn)
            .ToList();

        int totalDays = 0;
         
        foreach (var record in employeeManage)
        {
            var startDate = record.StartOn.ToDateTime(new TimeOnly(0, 0));
            var recordEndDate = record.EndOn?.ToDateTime(new TimeOnly(0, 0)) ?? DateTime.Today;

            totalDays += (recordEndDate - startDate).Days;
        }

        DateTime referenceDate = new DateTime(1, 1, 1);
        DateTime endDate = referenceDate.AddDays(totalDays);

        result.Year = endDate.Year - referenceDate.Year;
        result.Month = endDate.Month - referenceDate.Month;
        result.Day = endDate.Day - referenceDate.Day;

        if (result.Day < 0)
        {
            result.Month -= 1;
            result.Day += DateTime.DaysInMonth(endDate.Year, endDate.Month - 1);
        }

        if (result.Month < 0)
        {
            result.Year -= 1;
            result.Month += 12;
        }

        return result;
    }
}