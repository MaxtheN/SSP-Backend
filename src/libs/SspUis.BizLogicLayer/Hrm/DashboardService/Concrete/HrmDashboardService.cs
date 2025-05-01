using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Dynamic.Core;
using System.Runtime.Serialization;
using System.Security.Policy;
using System.Threading.Tasks;
using DocumentFormat.OpenXml.InkML;
using Humanizer;
using Microsoft.EntityFrameworkCore;
using OpenXmlPowerTools;
using SspUis.BizLogicLayer.Hrm.Info.EmployeeTurnstileReportServices;
using SspUis.Core;
using SspUis.DataLayer;
using SspUis.DataLayer.Constants;
using SspUis.DataLayer.EfClasses;
using SspUis.DataLayer.EfClasses.Hrm;
using StatusGeneric;
using WbAccessControl.Sdk;
using WEBASE.Utility;
namespace SspUis.BizLogicLayer.Hrm;
public class HrmDashboardService : StatusGenericHandler, IHrmDashboardService
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IEmployeeTurnstileReportService _employeeTurnstileReportService;
    private readonly TimeSpan OFFICE_IN_TIME = new TimeSpan(9, 0, 0);
    private readonly TimeSpan OFFICE_OUT_TIME = new TimeSpan(18, 0, 0);
    public HrmDashboardService(IUnitOfWork unitOfWork, IEmployeeTurnstileReportService employeeTurnstileReportService)
    {
        _unitOfWork = unitOfWork;
        _employeeTurnstileReportService = employeeTurnstileReportService;
    }
    public List<HrmEmployeeDto> GetEmployeeList(HrmDashFilterOption options)
    {
        var result = new List<HrmEmployeeDto>();
        var employeeManage = _unitOfWork.Context.Set<EmployeeManage>().Where(a => a.IsDeleted == false && a.EndOn == null && (!options.RegionId.HasValue || a.Organization.RegionId == options.RegionId) && (!options.OrganizationId.HasValue || a.OrganizationId == options.OrganizationId) ).Count();
        var employeeManageMen = _unitOfWork.Context.Set<EmployeeManage>().Include(a => a.Employee).ThenInclude(a => a.Person).Where(a => a.IsDeleted == false && a.EndOn == null && a.Employee.Person.GenderId == 1 && (!options.RegionId.HasValue || a.Organization.RegionId == options.RegionId) && (!options.OrganizationId.HasValue || a.OrganizationId == options.OrganizationId)).Count();
        var employeeManageWomen = _unitOfWork.Context.Set<EmployeeManage>().Include(a => a.Employee).ThenInclude(a => a.Person).Where(a => a.IsDeleted == false && a.EndOn == null && a.Employee.Person.GenderId == 2 && (!options.RegionId.HasValue || a.Organization.RegionId == options.RegionId) && (!options.OrganizationId.HasValue || a.OrganizationId == options.OrganizationId)).Count();
        var dto = new HrmEmployeeDto
        {
            TotalEmployeeCount = employeeManage,
            TotalEmployeeMenCount = employeeManageMen,
            TotalEmployeeWomenCount = employeeManageWomen,
        };
        result.Add(dto);
        return result;
    }
    public List<HrmEmployeeByRegionDto> GetEmployeeByRegionList(HrmDashFilterOption options)
    {
        var result = new List<HrmEmployeeByRegionDto>();
        var employeeManage = _unitOfWork.Context.Set<EmployeeManage>().Include(a => a.Organization).Where(a => a.IsDeleted == false && a.EndOn == null && (!options.RegionId.HasValue || a.Organization.RegionId == options.RegionId)).Count();
        var region = _unitOfWork.Context.Set<Region>().Include(a => a.Translates).FirstOrDefault(a => a.Id == options.RegionId);
        var dto = new HrmEmployeeByRegionDto
        {
            TotalEmployeeCount = employeeManage,
            Region = region.Translates.AsQueryable().FirstOrDefault(RegionTranslate.GetExpr(
                    TranslateColumn.full_name, ServiceProvider.CultureHelper.CurrentCulture.Id))?.TranslateText ?? region.FullName,
        };
        result.Add(dto);
        return result;
    }
    public List<HrmEmployeeGenderDto> GetEmployeeGenderList(HrmDashFilterOption options)
    {
        var result = new List<HrmEmployeeGenderDto>();
        var employeeManage = _unitOfWork.Context.Set<EmployeeManage>().Include(a => a.Employee).ThenInclude(a => a.Person);

        var dto = new HrmEmployeeGenderDto
        {
            TotalEmplpyeeMen = employeeManage.Where(a => a.IsDeleted == false && a.EndOn == null && a.Employee.Person.GenderId == 1 && (!options.RegionId.HasValue || a.Organization.RegionId == options.RegionId)).Distinct().Count(),
            TotalEmplpyeeWomen = employeeManage.Where(a => a.IsDeleted == false && a.EndOn == null && a.Employee.Person.GenderId == 2&& (!options.RegionId.HasValue || a.Organization.RegionId == options.RegionId)).Distinct().Count(),
        };
        result.Add(dto);
        return result;
    }
    public List<HrmEmployeeAgeDto> GetEmployeeAgeList(HrmDashFilterOption options)
    {
        var result = new List<HrmEmployeeAgeDto>();
        var currentYear = DateTime.Now.Year;
        var employeeManage = _unitOfWork.Context.Set<EmployeeManage>().Include(a => a.Employee).ThenInclude(a => a.Person).Where(a => !options.RegionId.HasValue || a.Organization.RegionId == options.RegionId);
        var dto = new HrmEmployeeAgeDto
        {
            TotalUpTo20 = employeeManage
                                       .Where(a => !a.IsDeleted && a.EndOn == null && (currentYear - a.Employee.Person.BirthDate.Year) < 20)
                                       .Select(a => a.Employee.Person.Id)
                                       .Distinct()
                                       .Count(),
            TotalFrom20UpTo30 = employeeManage
                                       .Where(a => !a.IsDeleted && a.EndOn == null &&
                                                   (currentYear - a.Employee.Person.BirthDate.Year) >= 20 &&
                                                   (currentYear - a.Employee.Person.BirthDate.Year) < 30)
                                       .Select(a => a.Employee.Person.Id)
                                       .Distinct()
                                       .Count(),
            TotalFrom30UpTo40 = employeeManage
                                       .Where(a => !a.IsDeleted && a.EndOn == null &&
                                                   (currentYear - a.Employee.Person.BirthDate.Year) >= 30 &&
                                                   (currentYear - a.Employee.Person.BirthDate.Year) < 40)
                                       .Select(a => a.Employee.Person.Id)
                                       .Distinct()
                                       .Count(),
            TotalFrom40UpTo = employeeManage
                                       .Where(a => !a.IsDeleted && a.EndOn == null &&
                                                   (currentYear - a.Employee.Person.BirthDate.Year) >= 40)
                                       .Select(a => a.Employee.Person.Id)
                                       .Distinct()
                                       .Count(),
        };
        result.Add(dto);
        return result;
    }
    public List<HrmEmployeeBirthDay> GetEmployeeBithDate(HrmDashFilterOption options)
    {
        var upcomingBirthdays = new List<HrmEmployeeBirthDay>();
        var currentYear = DateTime.Now.Year;

        var employeesWithBirthdays = _unitOfWork.Context.Set<EmployeeManage>()
            .Include(a => a.Employee)
            .ThenInclude(a => a.Person)
            .Where(a => !options.RegionId.HasValue || a.Organization.RegionId == options.RegionId && a.Employee.Person.BirthDate != null)
            .Distinct();

        foreach (var employeeManage in employeesWithBirthdays)
        {
            var employeeBirthDate = employeeManage.Employee.Person.BirthDate;
            var upcomingBirthday = new DateTime(currentYear, employeeBirthDate.Month, employeeBirthDate.Day);
            var today = new DateTime(currentYear, DateTime.Now.Month, DateTime.Now.Day);

            var daysUntilBirthday = (int)(upcomingBirthday - today).TotalDays;

            if (daysUntilBirthday >= 0 && daysUntilBirthday < 4)
            {
                var employeeName = employeeManage.Employee.Person.FullName;

                upcomingBirthdays.Add(new HrmEmployeeBirthDay
                {
                    Employee = employeeName,
                    EmployeeBithDate = employeeBirthDate,
                    DaysUntilBirthday = daysUntilBirthday,
                    PersonPictureId = employeeManage.Employee.Person.PictureId
                });
            }
        }

        return upcomingBirthdays;
    }
    public List<HrmEmployeeExperience> GetEmployeeExperienceList(HrmDashFilterOption options)
    {
        var result = new List<HrmEmployeeExperience>();
        var currentYear = DateTime.Now.Year;
        var employeeManage = _unitOfWork.Context.Set<EmployeeManage>().Include(a => a.Employee).ThenInclude(a => a.Person).Where(a => !options.RegionId.HasValue || a.Organization.RegionId == options.RegionId);
        var dto = new HrmEmployeeExperience
        {
            TotalUpTo1 = employeeManage
                                       .Where(a => !a.IsDeleted && a.EndOn == null && (currentYear - a.StartOn.Year) <= 1)
                                       .Select(a => a.Employee.Person.Id)
                                       .Distinct()
                                       .Count(),
            TotalUpTo2 = employeeManage
                                       .Where(a => !a.IsDeleted && a.EndOn == null &&
                                                   (currentYear - a.StartOn.Year) > 1 &&
                                                   (currentYear - a.StartOn.Year) <= 2)
                                       .Select(a => a.Employee.Person.Id)
                                       .Distinct()
                                       .Count(),
            TotalUpTo3 = employeeManage
                                       .Where(a => !a.IsDeleted && a.EndOn == null &&
                                                   (currentYear - a.StartOn.Year) > 2 &&
                                                   (currentYear - a.StartOn.Year) <= 3)
                                       .Select(a => a.Employee.Person.Id)
                                       .Distinct()
                                       .Count(),
            TotalFrom3 = employeeManage
                                       .Where(a => !a.IsDeleted && a.EndOn == null &&
                                                   (currentYear - a.StartOn.Year) > 3)
                                       .Select(a => a.Employee.Person.Id)
                                       .Distinct()
                                       .Count(),
        };
        result.Add(dto);
        return result;
    }
    public List<HrmEmployeeHigherEduCount> GetEmployeeHigherEduTypeList(HrmDashFilterOption options)
    {
        var result = new List<HrmEmployeeHigherEduCount>();
        var employeeManage = _unitOfWork.Context.Set<EmployeeManage>().Include(a => a.Employee).ThenInclude(a => a.Person).Include(a => a.Employee).ThenInclude(a => a.HigherEdu).Distinct()
            .Where(a => a.IsDeleted == false && a.EndOn == null && (!options.RegionId.HasValue || a.Organization.RegionId == options.RegionId));

        var dto = new HrmEmployeeHigherEduCount
        {
            TotalEmployeeHigerEduCount = employeeManage.Where(a => a.Employee.HigherEdu.Any(b => b.EmployeeHigherEduDegreeId == 8)).Count(),
            TotalEmployeeSecondaryEduCount = employeeManage.Where(a => a.Employee.HigherEdu.Any(b => b.EmployeeHigherEduDegreeId == 2)).Count(),
            TotalEmployeeHigerEduMenCount = employeeManage.Where(a => a.Employee.HigherEdu.Any(b => b.EmployeeHigherEduDegreeId == 8) && a.Employee.Person.GenderId == 1).Count(),
            TotalEmployeeHigerEduWomenCount = employeeManage.Where(a => a.Employee.HigherEdu.Any(b => b.EmployeeHigherEduDegreeId == 8) && a.Employee.Person.GenderId == 2).Count(),
            TotalEmployeeSecondaryEduMenCount = employeeManage.Where(a => a.Employee.HigherEdu.Any(b => b.EmployeeHigherEduDegreeId == 2) && a.Employee.Person.GenderId == 1).Count(),
            TotalEmployeeSecondaryEduWomenCount = employeeManage.Where(a => a.Employee.HigherEdu.Any(b => b.EmployeeHigherEduDegreeId == 2) && a.Employee.Person.GenderId == 2).Count(),
        };
        result.Add(dto);
        return result;
    }
    public List<HrmEmployeeAcademicDegree> GetEmployeeAcademicDegreeList(HrmDashFilterOption options)
    {
        var result = new List<HrmEmployeeAcademicDegree>();
        var employeeManage = _unitOfWork.Context.Set<EmployeeManage>().Include(a => a.Employee).ThenInclude(a => a.Person).Include(a => a.Employee).ThenInclude(a => a.HigherEdu).Distinct()
            .Where(a => a.IsDeleted == false && a.EndOn == null && (!options.RegionId.HasValue || a.Organization.RegionId == options.RegionId));

        var dto = new HrmEmployeeAcademicDegree
        {
            //TotalEmployeePhdCount = employeeManage.Where(a => a.Employee.HigherEdu.Any(b => b.EmployeeHigherEduDegreeId == 8)).Count(),
            TotalEmployeeMasterCount = employeeManage.Where(a => a.Employee.HigherEdu.Any(b => b.EmployeeHigherEduDegreeId == 10)).Count(),
            TotalEmployeeBachelorCount = employeeManage.Where(a => a.Employee.HigherEdu.Any(b => b.EmployeeHigherEduDegreeId == 9)).Count(),
            //TotalEmployeePhdMenCount = employeeManage.Where(a => a.Employee.HigherEdu.Any(b => b.EmployeeHigherEduDegreeId == 8) && a.Employee.Person.GenderId == 1).Count(),
            //TotalEmployeePhdWomenCount = employeeManage.Where(a => a.Employee.HigherEdu.Any(b => b.EmployeeHigherEduDegreeId == 8) && a.Employee.Person.GenderId == 2).Count(),
            TotalEmployeeMasterMenCount = employeeManage.Where(a => a.Employee.HigherEdu.Any(b => b.EmployeeHigherEduDegreeId == 10) && a.Employee.Person.GenderId == 1).Count(),
            TotalEmployeeMasterWomenCount = employeeManage.Where(a => a.Employee.HigherEdu.Any(b => b.EmployeeHigherEduDegreeId == 10) && a.Employee.Person.GenderId == 2).Count(),
            TotalEmployeeBachelorMenCount = employeeManage.Where(a => a.Employee.HigherEdu.Any(b => b.EmployeeHigherEduDegreeId == 9) && a.Employee.Person.GenderId == 1).Count(),
            TotalEmployeeBachelorWomenCount = employeeManage.Where(a => a.Employee.HigherEdu.Any(b => b.EmployeeHigherEduDegreeId == 9) && a.Employee.Person.GenderId == 2).Count(),
        };
        result.Add(dto);
        return result;
    }
    public List<HrmEmployeeLegalEducation> GetEmployeeLegalEducationList(HrmDashFilterOption options)
    {
        var result = new List<HrmEmployeeLegalEducation>();
        var employeeManage = _unitOfWork.Context.Set<EmployeeManage>().Include(a => a.Employee).ThenInclude(a => a.Person)
            .Where(a => a.IsDeleted == false && a.EndOn == null && (!options.RegionId.HasValue || a.Organization.RegionId == options.RegionId)).Distinct();

        var dto = new HrmEmployeeLegalEducation
        {
            TotalEmployeeLegalEducationMenCount = employeeManage.Where(a => a.Employee.HasLegalEducation == true && a.Employee.Person.GenderId == 1).Count(),
            TotalEmployeeLegalEducationWomenCount = employeeManage.Where(a => a.Employee.HasLegalEducation == true && a.Employee.Person.GenderId == 2).Count(),
        };
        result.Add(dto);
        return result;
    }
    public List<StaffingSinglePageReportDto> GetStaffingSingleReport(StaffingSinglePageReportDtoFilter dto)
    {
        var staffingPositions = _unitOfWork.Context.Set<StaffingPosition>()
           .Include(a => a.Department)
           .Include(a => a.Position)
           .Where(a => a.Owner.StatusId == StatusIdConst.RECEIVED && (dto.OrganizationId == null || a.Owner.OrganizationId == dto.OrganizationId) &&
                (dto.DepartmentId == null || a.DepartmentId == dto.DepartmentId))
           .Select(a => new StaffingSinglePageReportDto
           {
               DepartmentId = a.DepartmentId,
               Department = a.Department.FullName,
               PositionId = a.PositionId,
               Position = a.Position.FullName,
               Quantity = a.Quantity,
               QuantityForNow = a.Quantity - _unitOfWork.Context.Set<EmployeeManage>()
                                .Where(b => (dto.OrganizationId == null || b.OrganizationId == dto.OrganizationId) && b.EndOn == null && !b.IsDeleted && b.PositionId == a.PositionId && b.DepartmentId == a.DepartmentId)
                                .Sum(a => a.EmploymentRate.Value),
               EmployeeManageTables = _unitOfWork.Context.Set<EmployeeManage>()
                                .Include(c => c.Employee)
                                .ThenInclude(p => p.Person)
                                .Where(b => (dto.OrganizationId == null || b.OrganizationId == dto.OrganizationId) &&  b.EndOn == null && !b.IsDeleted && b.PositionId == a.PositionId && b.DepartmentId == a.DepartmentId)
                                .Where(b => dto.HasSearch() ? b.Employee.Person.FullName.ToLower().Contains(dto.Search.ToLower()) : true)
                                .Select(b => new EmployeeManageTable
                                {
                                    EmployeeId = b.EmployeeId,
                                    EmployeeManageId = b.Id,
                                    Pinfl = b.Employee.Person.Pinfl,
                                    Employees = b.Employee.Person.FullName,
                                    EmployeeRate = b.EmploymentRate,
                                    EmployeeManageDocId = b.DocId,
                                    AppointEmployees = _unitOfWork.Context.Set<AppointEmployee>()
                                        .Where(p => p.Id == b.DocId && p.StatusId == StatusIdConst.ACCEPTED && dto.FromDate <= p.DocOn && dto.ToDate >= p.DocOn)
                                        .Select(p => new AppointEmployeeTables
                                        {
                                            DocNumber = p.DocNumber,
                                            DocOn = p.DocOn,
                                            CreatedAt = p.CreatedAt,
                                            StatusId = p.StatusId,
                                        })
                                        .ToList()
                                })
                                .Where(b => b.AppointEmployees.Any())
                                .ToList(),
           }).AsQueryable();
        if (dto.Search != "")
        {
            staffingPositions = staffingPositions
            .Where(a => a.EmployeeManageTables.Any(e => EF.Functions.Like(e.Employees, $"%{dto.Employees}%")));
        }
        return staffingPositions.ToList();
    }
    public List<HrmEmployeeLeaveOrderDto> GetEmployeeDateBeforeRecall(HrmDashFilterOption options)
    {
        var upcomingBirthdays = new List<HrmEmployeeLeaveOrderDto>();

        var employeesLeaveOrder = _unitOfWork.Context.Set<EmployeeLeaveOrder>()
            .Include(a => a.Organization)
            .Include(a => a.Tables)
            .ThenInclude(a => a.EmployeeManage)
            .ThenInclude(a => a.Employee)
            .ThenInclude(a => a.Person)
            .Where(a => (!options.RegionId.HasValue || a.Organization.RegionId == options.RegionId) && a.StatusId == StatusIdConst.ACCEPTED);

        foreach (var employeeLeaveOrders in employeesLeaveOrder)
        {
            foreach (var holiday in employeeLeaveOrders.Tables)
            {
                var employee = holiday.Employee.Person.FullName;

                var endsWithinDays = holiday.EndOn.AsDateTime() >= DateTime.Today;

                if (endsWithinDays)
                {
                    var daysUntilEnd = (int)(holiday.EndOn.AsDateTime() - DateTime.Now).TotalDays;

                    if (daysUntilEnd == 7 ||daysUntilEnd == 3 || daysUntilEnd == 1)
                    {
                        upcomingBirthdays.Add(new HrmEmployeeLeaveOrderDto
                        {
                            Employee = employee,
                            EmployeeDateBeforeRecall = holiday.EndOn,
                            DaysUntilBeforeRecall = daysUntilEnd,
                        });
                    }
                }
            }
        }

        return upcomingBirthdays;
    }

    //shu yirdan boshlangan
    public List<HrmEmployeeDto> GetWithoutReasonEmployeeList(HrmDashFilterOption options)
    {
        options.FromDateTur ??= DateOnly.FromDateTime(DateTime.Now);
        options.ToDateTur ??= DateOnly.FromDateTime(DateTime.Now);
        var result = new List<HrmEmployeeDto>();

        var withoutReasonEmployee = _unitOfWork.Context.Set<EmployeeMissedDayTable>()
            .Include(a => a.Owner)
            .Where(a => a.Owner.StatusId == StatusIdConst.APPROVED
            && a.WithoutReason == false && (options.RegionId.HasValue
            || a.Owner.Organization.RegionId == options.RegionId)
            && a.Owner.OrganizationId == options.OrganizationId
            //&& options.FromDateTur <= DateOnly.FromDateTime(a.StartAt)
            //&& options.ToDateTur >= DateOnly.FromDateTime(a.EndAt)
            );

        var withoutReason = new HrmEmployeeDto
        {
            TotalEmployeeCount = withoutReasonEmployee.Count(),
            TotalEmployeeMenCount = withoutReasonEmployee.Where(x => x.EmployeeManage.Employee.Person.GenderId == GenderIdConst.MALE).Count(),
            TotalEmployeeWomenCount = withoutReasonEmployee.Where(x => x.EmployeeManage.Employee.Person.GenderId == GenderIdConst.FEMALE).Count(),
        };
        result.Add(withoutReason);

        return result;
    }

 

    public async Task<List<HrmEmployeeDto>> GetCameToWorkEmployeeListAsync(HrmDashFilterOption options)
    {
        options.FromDateTur ??= DateOnly.FromDateTime(DateTime.Now);
        options.ToDateTur ??= DateOnly.FromDateTime(DateTime.Now);
        var result = new List<HrmEmployeeDto>();

        var employeeTurnstiles = _unitOfWork.Context.Set<EmployeeTurnstileLog>()
                                       .Where(x => (!options.RegionId.HasValue || x.Organization.RegionId == options.RegionId)
                                       && (!options.OrganizationId.HasValue || x.OrganizationId == options.OrganizationId)
                                       && x.EmployeeTurnstileLogType.Id == (int)PersonEventType.ENTER
                                       && ( x.EventAt.Hour < 9 || (x.EventAt.Hour == 9 
                                       && x.EventAt.Minute == 0 && x.EventAt.Second == 0))
                                       && options.FromDateTur <= x.EventOn
                                       && options.ToDateTur >= x.EventOn);


        var hrmEmployee = new HrmEmployeeDto
        {
            TotalEmployeeCount = employeeTurnstiles.Count(),
            TotalEmployeeWomenCount = employeeTurnstiles
            .Where(x => x.Employee.Person.GenderId == GenderIdConst.FEMALE).Count(),
            TotalEmployeeMenCount = employeeTurnstiles
            .Where(x => x.Employee.Person.GenderId == GenderIdConst.MALE).Count(),
        };
        result.Add(hrmEmployee);

        return result;
    }

    public async Task<List<HrmEmployeeDto>> GetLeaveFromWorkEmployeeListAsync(HrmDashFilterOption options)
    {
        options.FromDateTur ??= DateOnly.FromDateTime(DateTime.Now);
        options.ToDateTur ??= DateOnly.FromDateTime(DateTime.Now);

        var result = new List<HrmEmployeeDto>();

        var employeeTurnstiles = _unitOfWork.Context.Set<EmployeeTurnstileLog>()
                                      .Where(x => (!options.RegionId.HasValue || x.Organization.RegionId == options.RegionId)
                                      && (!options.OrganizationId.HasValue || x.OrganizationId == options.OrganizationId)
                                      && x.EmployeeTurnstileLogType.Id == (int)PersonEventType.EXIT
                                      && (x.EventAt.Hour < 18 || (x.EventAt.Hour == 18
                                      && x.EventAt.Minute == 0 && x.EventAt.Second == 0))
                                      && options.FromDateTur <= x.EventOn
                                      && options.ToDateTur >= x.EventOn);

        var hrmEmployee = new HrmEmployeeDto
        {
            TotalEmployeeCount =  employeeTurnstiles.Count(),
            TotalEmployeeWomenCount =  employeeTurnstiles
            .Where(x => x.Employee.Person.GenderId == GenderIdConst.FEMALE).Count(),
            TotalEmployeeMenCount = employeeTurnstiles
            .Where(x => x.Employee.Person.GenderId == GenderIdConst.MALE).Count(),
        };
        result.Add(hrmEmployee);

        return result;
    }


    public async Task<List<HrmEmployeeDto>> GetLateToWorkEmployeeListAsync(HrmDashFilterOption options)
    {
        options.FromDateTur ??= DateOnly.FromDateTime(DateTime.Now);
        options.ToDateTur ??= DateOnly.FromDateTime(DateTime.Now);

        var result = new List<HrmEmployeeDto>();

        var employeeTurnstiles = _unitOfWork.Context.Set<EmployeeTurnstileLog>()
                                .Where(x => (!options.RegionId.HasValue || x.Organization.RegionId == options.RegionId)
                                && (!options.OrganizationId.HasValue || x.OrganizationId == options.OrganizationId)
                                && x.EmployeeTurnstileLogType.Id == (int)PersonEventType.ENTER
                                && x.EventAt.Hour > 9
                                && options.FromDateTur <= x.EventOn
                                && options.ToDateTur >= x.EventOn);

        var hrmEmployee = new HrmEmployeeDto
        {
            TotalEmployeeCount = employeeTurnstiles.Count(),
            TotalEmployeeWomenCount = employeeTurnstiles
            .Where(x => x.Employee.Person.GenderId == GenderIdConst.FEMALE).Count(),
            TotalEmployeeMenCount = employeeTurnstiles
            .Where(x => x.Employee.Person.GenderId == GenderIdConst.MALE).Count(),
        };
        result.Add(hrmEmployee);

        return result;
    }


    public async Task<List<HrmEmployeeDto>> GetDidNotComeToWorkEmployeeListAsync(HrmDashFilterOption options)
    {
        options.FromDateTur ??= DateOnly.FromDateTime(DateTime.Now);
        options.ToDateTur ??= DateOnly.FromDateTime(DateTime.Now);

        var result = new List<HrmEmployeeDto>();

        var employeeManger = _unitOfWork.Context.Set<EmployeeManage>()
            .Include(x => x.Employee)
            .Where(x => (!options.RegionId.HasValue || x.Organization.RegionId == options.RegionId)
             && (!options.OrganizationId.HasValue || x.OrganizationId == options.OrganizationId));

        var employeeTurnstiles = _unitOfWork.Context.Set<EmployeeTurnstileLog>()
                                      .Where(x => (!options.RegionId.HasValue || x.Organization.RegionId == options.RegionId)
                                      && (!options.OrganizationId.HasValue || x.OrganizationId == options.OrganizationId)
                                      && x.EmployeeTurnstileLogType.Id == (int)PersonEventType.ENTER
                                      && (x.EventAt.Hour < 18 || (x.EventAt.Hour == 18
                                      && x.EventAt.Minute == 0 && x.EventAt.Second == 0))
                                      && options.FromDateTur <= x.EventOn
                                      && options.ToDateTur >= x.EventOn);

        var hrmEmployee = new HrmEmployeeDto
        {
            TotalEmployeeCount = Math.Abs(employeeManger.Count() - employeeTurnstiles.Count()),
            TotalEmployeeWomenCount = Math.Abs(employeeManger.Where(x => x.Employee.Person.GenderId == GenderIdConst.FEMALE).Count() - employeeTurnstiles.Where(x=>x.Employee.Person.GenderId == GenderIdConst.FEMALE).Count()),
            TotalEmployeeMenCount = Math.Abs(employeeManger.Where(x => x.Employee.Person.GenderId == GenderIdConst.MALE).Count() - employeeTurnstiles.Where(x => x.Employee.Person.GenderId == GenderIdConst.MALE).Count()),
        };
        result.Add(hrmEmployee);

        return result;
    }


    

}