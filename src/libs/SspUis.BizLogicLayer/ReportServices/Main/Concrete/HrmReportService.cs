using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using iText.Kernel.Utils;
using Microsoft.EntityFrameworkCore;
using OfficeOpenXml;
using SspUis.BizLogicLayer.ReportServices.Main;
using SspUis.Core;
using SspUis.Core.Security;
using SspUis.DataLayer;
using SspUis.DataLayer.EfClasses;
using SspUis.DataLayer.EfClasses.Hrm;
using StatusGeneric;
using WEBASE;
using WEBASE.i18n;
using WEBASE.Storage;

namespace SspUis.BizLogicLayer.ReportServices;

public class HrmReportService : StatusGenericHandler, IHrmReportService
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IStorageService _storageService;
    private readonly ICultureHelper _cultureHelper;
    private readonly IAuthService _authService;
    public HrmReportService(IUnitOfWork unitOfWork,
        IStorageService storageService,
        ICultureHelper cultureHelper,
        IAuthService authServie)
    {
        _unitOfWork = unitOfWork;
        _storageService = storageService;
        _cultureHelper = cultureHelper;
        _authService = authServie;
    }

    public async Task<List<HrmEmployeeActivityInfoDto>> GetHrmEmployeeActivityInfoAsync(HrmEmployeeActivityInfoDtoFilter filter)
    {
        var result = new List<HrmEmployeeActivityInfoDto>();
        var currentYear = DateTime.Now.Year;
        var today = DateOnly.FromDateTime(DateTime.Now);

        var staffingQuery = await _unitOfWork.Context.Set<StaffingPosition>()
            .Include(s => s.Owner)
            .ThenInclude(o => o.Organization)
            .Where(s => (s.Owner.Organization.OrganizationGroupId == OrganizationGroupIdConst.SSP
            || s.Owner.Organization.OrganizationGroupId == OrganizationGroupIdConst.REGIONAL_BRANCH)
                     && s.Owner.StatusId == StatusIdConst.RECEIVED)
            .Select(s => new
            {
                OrgId = s.Owner.OrganizationId,
                DepartmentId = s.DepartmentId,
                PositionId = s.PositionId,
                Quantity = s.Quantity,
                QuantityForNow = s.Quantity - _unitOfWork.Context.Set<EmployeeManage>()
                    .Where(b => s.Owner.OrganizationId == b.OrganizationId && b.EndOn == null && b.IsDeleted == false && b.PositionId == s.PositionId && b.DepartmentId == s.DepartmentId)
                    .Sum(a => a.EmploymentRate)
            })
            .GroupBy(s => s.OrgId)
            .Select(g => new
            {
                OrgId = g.Key,
                EmployeesCount = g.Sum(e => e.Quantity),
                StaffPositionsCount = g.Sum(e => e.QuantityForNow)
            })
            .ToDictionaryAsync(g => g.OrgId, g => new { g.EmployeesCount, g.StaffPositionsCount });

        var employeesInMaternityLeaveQuery = await _unitOfWork.Context.Set<EmployeeSickLeaveTable>()
            .Include(e => e.Employee)
            .ThenInclude(p => p.Person)
            .ThenInclude(g => g.Gender)
            .Include(e => e.Owner)
            .ThenInclude(o => o.Organization)
            .Where(s => (s.Owner.Organization.OrganizationGroupId == OrganizationGroupIdConst.SSP
            || s.Owner.Organization.OrganizationGroupId == OrganizationGroupIdConst.REGIONAL_BRANCH)
                        && s.Owner.StatusId == StatusIdConst.ACCEPTED && s.IsMaternityLeave
                        && (!filter.HasLegalEducation.HasValue || filter.HasLegalEducation == s.Employee.HasLegalEducation)
                        && (!filter.BirthRegionId.HasValue || s.Employee.Person.BirthRegionId == filter.BirthRegionId)
                        && (!filter.GenderId.HasValue || s.Employee.Person.GenderId == filter.GenderId)
                        && (!filter.StartDate.HasValue || s.StartOn >= filter.StartDate.Value)
                        && (!filter.EndDate.HasValue || s.EndOn <= filter.EndDate.Value)
                        && (!filter.MinAge.HasValue || currentYear - s.Employee.Person.BirthDate.Year >= filter.MinAge.Value)
                        && (!filter.MaxAge.HasValue || currentYear - s.Employee.Person.BirthDate.Year < filter.MaxAge.Value))
            .GroupBy(s => s.Owner.OrganizationId)
            .Select(g => new
            {
                OrgId = g.Key,
                EmployeesCountInMaternityLeave = g.Count()
            })
            .ToDictionaryAsync(g => g.OrgId, g => g.EmployeesCountInMaternityLeave);

        var employeesSendTrainQuery = await _unitOfWork.Context.Set<EmployeeSendTrainTable>()
            .Include(e => e.Employee)
            .ThenInclude(o => o.Person)
            .ThenInclude(p => p.Gender)
            .Include(e => e.Owner)
            .ThenInclude(e => e.Organization)
            .Where(e => (e.Owner.Organization.OrganizationGroupId == OrganizationGroupIdConst.SSP
            || e.Owner.Organization.OrganizationGroupId == OrganizationGroupIdConst.REGIONAL_BRANCH)
                        && e.Owner.StatusId == StatusIdConst.RECEIVED && (!filter.HasLegalEducation.HasValue || filter.HasLegalEducation == e.Employee.HasLegalEducation)
                        && (!filter.BirthRegionId.HasValue || e.Employee.Person.BirthRegionId == filter.BirthRegionId)
                        && (!filter.GenderId.HasValue || e.Employee.Person.GenderId == filter.GenderId)
                        && (!filter.StartDate.HasValue || e.StartOn >= filter.StartDate.Value)
                        && (!filter.EndDate.HasValue || e.EndOn <= filter.EndDate.Value)
                        && (!filter.MinAge.HasValue || currentYear - e.Employee.Person.BirthDate.Year >= filter.MinAge.Value)
                        && (!filter.MaxAge.HasValue || currentYear - e.Employee.Person.BirthDate.Year < filter.MaxAge.Value))
            .GroupBy(e => e.Employee.OrganizationId)
            .Select(g => new
            {
                OrgId = g.Key,
                EmployeesCountSentOnTraining = g.Count()
            })
            .ToDictionaryAsync(g => g.OrgId, g => g.EmployeesCountSentOnTraining);

        var employeesRecallLeaveQuery = await _unitOfWork.Context.Set<RecallLeaveTable>()
            .Include(e => e.Owner)
            .ThenInclude(o => o.Organization)
            .Include(g => g.Employee)
            .ThenInclude(e => e.Person)
            .ThenInclude(l => l.Gender)
            .Where(e => (e.Owner.Organization.OrganizationGroupId == OrganizationGroupIdConst.SSP
            || e.Owner.Organization.OrganizationGroupId == OrganizationGroupIdConst.REGIONAL_BRANCH)
                        && e.Owner.StatusId == StatusIdConst.ACCEPTED && (!filter.HasLegalEducation.HasValue || filter.HasLegalEducation == e.Employee.HasLegalEducation)
                        && (!filter.BirthRegionId.HasValue || e.Employee.Person.BirthRegionId == filter.BirthRegionId)
                        && (!filter.GenderId.HasValue || e.Employee.Person.GenderId == filter.GenderId)
                        && (!filter.StartDate.HasValue || e.StartOn >= filter.StartDate.Value)
                        && (!filter.MinAge.HasValue || currentYear - e.Employee.Person.BirthDate.Year >= filter.MinAge.Value)
                        && (!filter.MaxAge.HasValue || currentYear - e.Employee.Person.BirthDate.Year < filter.MaxAge.Value))
            .GroupBy(e => e.Employee.OrganizationId)
            .Select(g => new
            {
                OrgId = g.Key,
                EmployeesCountRecallLeave = g.Count()
            })
            .ToDictionaryAsync(g => g.OrgId, g => g.EmployeesCountRecallLeave);

        var employeesLeaveOrderQuery = await _unitOfWork.Context.Set<EmployeeLeaveOrderTable>()
            .Include(elo => elo.Employee)
            .ThenInclude(e => e.Person)
            .ThenInclude(p => p.Gender)
            .Include(elo => elo.Owner)
            .ThenInclude(o => o.Organization)
            .Where(elo => (elo.Owner.Organization.OrganizationGroupId == OrganizationGroupIdConst.SSP
            || elo.Owner.Organization.OrganizationGroupId == OrganizationGroupIdConst.REGIONAL_BRANCH)
                          && elo.Owner.StatusId == StatusIdConst.ACCEPTED && (!filter.HasLegalEducation.HasValue || filter.HasLegalEducation == elo.Employee.HasLegalEducation)
                          && (!filter.BirthRegionId.HasValue || elo.Employee.Person.BirthRegionId == filter.BirthRegionId)
                          && (!filter.GenderId.HasValue || elo.Employee.Person.GenderId == filter.GenderId)
                          && (!filter.StartDate.HasValue || elo.StartOn >= filter.StartDate.Value)
                          && (!filter.EndDate.HasValue || elo.EndOn <= filter.EndDate.Value)
                          && (!filter.MinAge.HasValue || currentYear - elo.Employee.Person.BirthDate.Year >= filter.MinAge.Value)
                          && (!filter.MaxAge.HasValue || currentYear - elo.Employee.Person.BirthDate.Year < filter.MaxAge.Value))
            .GroupBy(e => e.Employee.OrganizationId)
            .Select(g => new
            {
                OrgId = g.Key,
                EmployeesCountLeaveOrder = g.Count()
            })
            .ToDictionaryAsync(g => g.OrgId, g => g.EmployeesCountLeaveOrder);

        var hiredEmployeesQuery = await _unitOfWork.Context.Set<EmployeeManage>()
            .Include(o => o.Employee)
            .ThenInclude(e => e.Organization)
            .Include(he => he.Employee)
            .ThenInclude(e => e.Person)
            .ThenInclude(p => p.Gender)
            .Where(he => (he.Organization.OrganizationGroupId == OrganizationGroupIdConst.SSP || he.Organization.OrganizationGroupId == OrganizationGroupIdConst.REGIONAL_BRANCH)
                         && (he.EndOn == null && !he.IsDeleted)
                         && (!filter.HasLegalEducation.HasValue || filter.HasLegalEducation == he.Employee.HasLegalEducation)
                         && (!filter.BirthRegionId.HasValue || he.Employee.Person.BirthRegionId == filter.BirthRegionId)
                         && (!filter.GenderId.HasValue || he.Employee.Person.GenderId == filter.GenderId)
                         && (filter.StartDate.HasValue ? he.StartOn >= filter.StartDate.Value : he.StartOn.Year == DateTime.UtcNow.Year)
                         && (!filter.EndDate.HasValue || (he.EndOn.HasValue && he.EndOn.Value <= filter.EndDate.Value))
                         && (!filter.MinAge.HasValue || currentYear - he.Employee.Person.BirthDate.Year >= filter.MinAge.Value)
                         && (!filter.MaxAge.HasValue || currentYear - he.Employee.Person.BirthDate.Year < filter.MaxAge.Value))
            .GroupBy(e => e.Employee.OrganizationId)
            .Select(g => new
            {
                OrgId = g.Key,
                HiredEmployeesCount = g.Count()
            })
            .ToDictionaryAsync(g => g.OrgId, g => g.HiredEmployeesCount);

        var transferredEmployeesQuery = await _unitOfWork.Context.Set<EmployeeManage>()
            .Include(o => o.Employee)
            .ThenInclude(e => e.Organization)
            .Include(he => he.Employee)
            .ThenInclude(e => e.Person)
            .ThenInclude(p => p.Gender)
            .Where(he => (he.Organization.OrganizationGroupId == OrganizationGroupIdConst.SSP
            || he.Organization.OrganizationGroupId == OrganizationGroupIdConst.REGIONAL_BRANCH)
                         && (he.EndOn.Value <= today && !he.IsDeleted)
                         && (!filter.HasLegalEducation.HasValue || filter.HasLegalEducation == he.Employee.HasLegalEducation)
                         && (!filter.BirthRegionId.HasValue || he.Employee.Person.BirthRegionId == filter.BirthRegionId)
                         && (!filter.GenderId.HasValue || he.Employee.Person.GenderId == filter.GenderId)
                         && (!filter.StartDate.HasValue || he.StartOn >= filter.StartDate.Value)
                         && (!filter.EndDate.HasValue || (he.EndOn.HasValue && he.EndOn.Value <= filter.EndDate.Value))
                         && (!filter.MinAge.HasValue || currentYear - he.Employee.Person.BirthDate.Year >= filter.MinAge.Value)
                         && (!filter.MaxAge.HasValue || currentYear - he.Employee.Person.BirthDate.Year < filter.MaxAge.Value))
            .GroupBy(e => e.OrganizationId)
            .Select(g => new
            {
                OrgId = g.Key,
                TransferredEmployeesCount = g.Count()
            })
            .ToDictionaryAsync(g => g.OrgId, g => g.TransferredEmployeesCount);

        var dismissedEmployeesQuery = await _unitOfWork.Context.Set<EmployeeManage>()
            .Include(o => o.Employee)
            .ThenInclude(e => e.Organization)
            .Include(he => he.Employee)
            .ThenInclude(e => e.Person)
            .ThenInclude(p => p.Gender)
            .Where(he => (he.Organization.OrganizationGroupId == OrganizationGroupIdConst.SSP
            || he.Organization.OrganizationGroupId == OrganizationGroupIdConst.REGIONAL_BRANCH) 
                         && (he.EndOn.Value <= today || he.IsDeleted)
                         && (!filter.HasLegalEducation.HasValue || filter.HasLegalEducation == he.Employee.HasLegalEducation)
                         && (!filter.BirthRegionId.HasValue || he.Employee.Person.BirthRegionId == filter.BirthRegionId)
                         && (!filter.GenderId.HasValue || he.Employee.Person.GenderId == filter.GenderId)
                         && (!filter.MinAge.HasValue || currentYear - he.Employee.Person.BirthDate.Year >= filter.MinAge.Value)
                         && (!filter.MaxAge.HasValue || currentYear - he.Employee.Person.BirthDate.Year < filter.MaxAge.Value))
            .GroupBy(e => e.OrganizationId)
            .Select(g => new
            {
                OrgId = g.Key,
                DismissedEmployeesCount = g.Count()
            })
            .ToDictionaryAsync(g => g.OrgId, g => g.DismissedEmployeesCount);

        var sspOrgs = await _unitOfWork.OrganizationRepository.AllAsQueryable.Include(a => a.Translates)
            .Where(o => o.Id == OrganizationIdConst.SSP || o.OrganizationGroupId == OrganizationGroupIdConst.SSP || o.OrganizationGroupId == OrganizationGroupIdConst.REGIONAL_BRANCH)
            .IsActive().ToListAsync();

        var orgsDict = sspOrgs.ToDictionary(
            a => a.Id,
            a => new
            {
                OrderCode = a.OrderCode,
                FullName = a.Translates.AsQueryable().FirstOrDefault(OrganizationTranslate.GetExpr(TranslateColumn.short_name,
                ServiceProvider.CultureHelper.CurrentCulture.Id))?.TranslateText ?? a.ShortName
            });

        var userOrganization = _unitOfWork.Context.Set<Organization>().FirstOrDefault(x => x.Id == _authService.User.OrganizationId);

        // yolg'iz region tanlanganda
        if (filter.RegionId.HasValue)
        {
            var orgs = sspOrgs.Where(o => o.RegionId == filter.RegionId).ToDictionary(
                a => a.Id,
                a => new
                {
                    OrderCode = a.OrderCode,
                    FullName = a.Translates.AsQueryable().FirstOrDefault(OrganizationTranslate.GetExpr(TranslateColumn.short_name,
                    ServiceProvider.CultureHelper.CurrentCulture.Id))?.TranslateText ?? a.ShortName
                });

            foreach (var org in orgs)
            {
                var key = org.Key;
                if (orgs.TryGetValue(key, out var organizationInfo))
                {
                    result.Add(new HrmEmployeeActivityInfoDto
                    {
                        OrganizationId = key,
                        OrganizationName = organizationInfo.FullName,
                        OrganizationOrderCode = organizationInfo.OrderCode,
                        TotalEmployeesCount = staffingQuery.TryGetValue(key, out var staffs) ? (long)staffs.EmployeesCount : 0,
                        TotalStaffingPositionsCount = staffs?.StaffPositionsCount ?? 0.0m,
                        TotalEmployeesInLeaveOrderCount = employeesLeaveOrderQuery.TryGetValue(key, out var employeesLeaveOrder) ? employeesLeaveOrder : 0,
                        TotalEmployeesInRecallLeaveCount = employeesRecallLeaveQuery.TryGetValue(key, out var employeesRecallLeave) ? employeesRecallLeave : 0,
                        TotalEmployeesInMaternityLeaveCount = employeesInMaternityLeaveQuery.TryGetValue(key, out var employeesInMaternityLeave) ? employeesInMaternityLeave : 0,
                        TotalEmployeesSendToTrainingCount = employeesSendTrainQuery.TryGetValue(key, out var employeesSentOnTraining) ? employeesSentOnTraining : 0,
                        TotalHiredEmployeesCount = hiredEmployeesQuery.TryGetValue(key, out var hiredEmployeesCount) ? hiredEmployeesCount : 0,
                        TotalDismissedEmployeesCount = dismissedEmployeesQuery.TryGetValue(key, out var dismissedEmployeesCount) ? dismissedEmployeesCount : 0,
                        TotalTransferredEmployeesCount = transferredEmployeesQuery.TryGetValue(key, out var transferredEmployeesCount) ? transferredEmployeesCount : 0
                    });
                }
            }
        }
        else
        {
            // respublika savdo sanoat palatasi uchun
            if (userOrganization.Id == OrganizationIdConst.SSP)
            {
                foreach (var org in orgsDict)
                {
                    result.Add(new HrmEmployeeActivityInfoDto
                    {
                        OrganizationId = org.Key,
                        OrganizationOrderCode = org.Value.OrderCode,
                        OrganizationName = org.Value.FullName,
                        TotalEmployeesCount = staffingQuery.TryGetValue(org.Key, out var staffs) ? (long)staffs.EmployeesCount : 0,
                        TotalStaffingPositionsCount = staffs?.StaffPositionsCount ?? 0.0m,
                        TotalEmployeesInLeaveOrderCount = employeesLeaveOrderQuery.TryGetValue(org.Key, out var employeesLeaveOrder) ? employeesLeaveOrder : 0,
                        TotalEmployeesInRecallLeaveCount = employeesRecallLeaveQuery.TryGetValue(org.Key, out var employeeRecallLeave) ? employeeRecallLeave : 0,
                        TotalEmployeesInMaternityLeaveCount = employeesInMaternityLeaveQuery.TryGetValue(org.Key, out var employeesnMaternityLeave) ? employeesnMaternityLeave : 0,
                        TotalEmployeesSendToTrainingCount = employeesSendTrainQuery.TryGetValue(org.Key, out var employeeSentOnTraining) ? employeeSentOnTraining : 0,
                        TotalHiredEmployeesCount = hiredEmployeesQuery.TryGetValue(org.Key, out var hiredEmployeesCount) ? hiredEmployeesCount : 0,
                        TotalDismissedEmployeesCount = dismissedEmployeesQuery.TryGetValue(org.Key, out var dismissedCount) ? dismissedCount : 0,
                        TotalTransferredEmployeesCount = transferredEmployeesQuery.TryGetValue(org.Key, out var transferredEmployeesCount) ? transferredEmployeesCount : 0
                    });
                }
                result = result.OrderBy(a => a.OrganizationOrderCode).ToList();
            }
            // qolgan hududiy boshqarmalar
            else if (userOrganization.Id != OrganizationIdConst.SSP
                && (userOrganization.OrganizationGroupId == OrganizationGroupIdConst.SSP
                    || userOrganization.OrganizationGroupId == OrganizationGroupIdConst.REGIONAL_BRANCH))
            {
                var key = userOrganization.Id;
                orgsDict.TryGetValue(key, out var organizationInfo);

                result.Add(new HrmEmployeeActivityInfoDto
                {
                    OrganizationId = key,
                    OrganizationName = organizationInfo.FullName,
                    OrganizationOrderCode = organizationInfo.OrderCode,
                    TotalEmployeesCount = staffingQuery.TryGetValue(key, out var staffs) ? (long)staffs.EmployeesCount : 0,
                    TotalStaffingPositionsCount = staffs?.StaffPositionsCount ?? 0.0m,
                    TotalEmployeesInLeaveOrderCount = employeesLeaveOrderQuery.TryGetValue(key, out var employeesLeaveOrder) ? employeesLeaveOrder : 0,
                    TotalEmployeesInRecallLeaveCount = employeesRecallLeaveQuery.TryGetValue(key, out var employeesRecallLeave) ? employeesRecallLeave : 0,
                    TotalEmployeesInMaternityLeaveCount = employeesInMaternityLeaveQuery.TryGetValue(key, out var employeesInMaternityLeave) ? employeesInMaternityLeave : 0,
                    TotalEmployeesSendToTrainingCount = employeesSendTrainQuery.TryGetValue(key, out var employeesSentOnTraining) ? employeesSentOnTraining : 0,
                    TotalHiredEmployeesCount = hiredEmployeesQuery.TryGetValue(key, out var hiredEmployeesCount) ? hiredEmployeesCount : 0,
                    TotalDismissedEmployeesCount = dismissedEmployeesQuery.TryGetValue(key, out var dismissedEmployeesCount) ? dismissedEmployeesCount : 0,
                    TotalTransferredEmployeesCount = transferredEmployeesQuery.TryGetValue(key, out var transferredEmployeesCount) ? transferredEmployeesCount : 0
                });
            }
            else
            {
                result.Add(new HrmEmployeeActivityInfoDto
                {
                    TotalEmployeesCount = (long)staffingQuery.Values.Sum(a => a.EmployeesCount),
                    TotalStaffingPositionsCount = staffingQuery.Values.Sum(a => a.StaffPositionsCount.Value),
                    TotalEmployeesInLeaveOrderCount = employeesLeaveOrderQuery.Values.Sum(),
                    TotalEmployeesInRecallLeaveCount = employeesRecallLeaveQuery.Values.Sum(),
                    TotalEmployeesInMaternityLeaveCount = employeesInMaternityLeaveQuery.Values.Sum(),
                    TotalEmployeesSendToTrainingCount = employeesSendTrainQuery.Values.Sum(),
                    TotalHiredEmployeesCount = hiredEmployeesQuery.Values.Sum(),
                    TotalDismissedEmployeesCount = dismissedEmployeesQuery.Values.Sum(),
                    TotalTransferredEmployeesCount = transferredEmployeesQuery.Values.Sum()
                });
            }
        }
        return result;
    }
    public async Task<Stream> SaveHrmReportAsExcelAsync(HrmEmployeeActivityInfoDtoFilter filter)
    {
        var data = await GetHrmEmployeeActivityInfoAsync(filter).ConfigureAwait(false);
        var result = new MemoryStream();
        var template = _storageService.GetStaticFile(StaticFileConst.ExcelTemplate.GetFileName(_cultureHelper.CurrentCulture.Code, StaticFileConst.Report.REPORT_HRM_EMPLOYEE_ACTIVITY));

        if (IsValid && data != null)
        {
            ExcelPackage.LicenseContext = LicenseContext.NonCommercial;
            var excelPackage = new ExcelPackage(template);
            //var namerange = excelPackage.Workbook.Names["Organization"];
            //namerange.Value = "";
            
            var importRow = excelPackage.Workbook.Names["ImportRow"];
            var ws = importRow.Worksheet;

            int currentRow = importRow.Start.Row + 1;
            int index = 1;

            foreach (var item in data)
            {
                var column = 1;
                ws.InsertRow(currentRow, 1, importRow.Start.Row);
                ws.Cells[currentRow, column++].Value = index++;
                ws.Cells[currentRow, column++].Value = item.OrganizationName;
                ws.Cells[currentRow, column++].Value = item.TotalEmployeesCount;
                ws.Cells[currentRow, column++].Value = item.TotalStaffingPositionsCount;
                ws.Cells[currentRow, column++].Value = item.TotalEmployeesInLeaveOrderCount;
                ws.Cells[currentRow, column++].Value = item.TotalEmployeesInRecallLeaveCount;
                ws.Cells[currentRow, column++].Value = item.TotalEmployeesInMaternityLeaveCount;
                ws.Cells[currentRow, column++].Value = item.TotalEmployeesSendToTrainingCount;
                ws.Cells[currentRow, column++].Value = item.TotalHiredEmployeesCount;
                ws.Cells[currentRow, column++].Value = item.TotalDismissedEmployeesCount;
                ws.Cells[currentRow, column++].Value = item.TotalTransferredEmployeesCount;
                currentRow++;
            }

            var columnTotal = 2;
            ws.Cells[currentRow, columnTotal++].Value = "Jami:";
            ws.Cells[currentRow, columnTotal++].Value = data.Sum(w => w.TotalEmployeesCount);
            ws.Cells[currentRow, columnTotal++].Value = data.Sum(w => w.TotalStaffingPositionsCount);
            ws.Cells[currentRow, columnTotal++].Value = data.Sum(w => w.TotalEmployeesInLeaveOrderCount);
            ws.Cells[currentRow, columnTotal++].Value = data.Sum(w => w.TotalEmployeesInRecallLeaveCount);
            ws.Cells[currentRow, columnTotal++].Value = data.Sum(w => w.TotalEmployeesInMaternityLeaveCount);
            ws.Cells[currentRow, columnTotal++].Value = data.Sum(w => w.TotalEmployeesSendToTrainingCount);
            ws.Cells[currentRow, columnTotal++].Value = data.Sum(w => w.TotalHiredEmployeesCount);
            ws.Cells[currentRow, columnTotal++].Value = data.Sum(w => w.TotalDismissedEmployeesCount);
            ws.Cells[currentRow, columnTotal++].Value = data.Sum(w => w.TotalTransferredEmployeesCount);
            ws.DeleteRow(importRow.Start.Row);
            result = new MemoryStream(excelPackage.GetAsByteArray());
            excelPackage.Dispose();
        }
        result.Position = 0;
        return result;
    }
}