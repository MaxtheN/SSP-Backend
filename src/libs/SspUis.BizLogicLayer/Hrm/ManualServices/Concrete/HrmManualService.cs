using GenericServices;
using Microsoft.EntityFrameworkCore;
using SspUis.Core.Security;
using SspUis.DataLayer;
using SspUis.DataLayer.EfClasses;
using SspUis.DataLayer.EfClasses.Hrm;
using StatusGeneric;
using WEBASE.Models;

namespace SspUis.BizLogicLayer.Hrm.ManualServices
{
    public class HrmManualService : StatusGenericHandler, IHrmManualService
    {
        private readonly ICrudServices _service;
        private readonly DbContext _context;
        private readonly IAuthService _authService;
        private readonly IUnitOfWork _unitOfWork;

        public HrmManualService(
            ICrudServices crudService,
            DbContext context,
            IAuthService authService,
            IUnitOfWork unitOfWork)
        {
            _service = crudService;
            _context = context;
            _authService = authService;
            _unitOfWork = unitOfWork;
        }

        public SelectList<int> EmpAppointOrderTypeSelectList()
        {
            return _context.Set<EmpAppointOrderType>().Include(a => a.Translates).AsSelectList();
        }
        public SelectList<int> MinimumValueTypeSelectList()
        {
            return _context.Set<MinimumValueType>().Include(a => a.Translates).AsSelectList();
        }
        public SelectList<int> EmploymentTypeSelectList()
        {
            return _context.Set<EmploymentType>().Include(a => a.Translates).AsSelectList();
        }
        public SelectList<int> WorkScheduleKindSelectList()
        {
            return _context.Set<WorkScheduleKind>().Include(a => a.Translates).AsSelectList();
        }
        public SelectList<int> TimesheetIndicatorSelectList()
        {
            return _context.Set<TimesheetIndicator>().Include(a => a.Translates).AsSelectList();
        }

        public SelectList<int> TimesheetTypeSelectList()
        {
            return _context.Set<TimesheetType>().Include(a => a.Translates).AsSelectList();
        }
        public SelectList<int> RoundingTypeSelectList()
        {
            return _context.Set<RoundingType>().Include(a => a.Translates).AsSelectList();
        }

        public SelectList<int> CalculateByTimeTypeSelectList()
        {
            return _context.Set<CalculateByTimeType>().Include(a=>a.Translates).AsSelectList();
        }
        public SelectList<int> EmployeeSickLeaveTypeSelectList()
        {
            return _context.Set<EmployeeSickLeaveType>().Include(a => a.Translates).AsSelectList();
        }
        public SelectList<int> TempCalcKindTypeSelectList()
        {
            return _context.Set<TempCalcKindType>().Include(a => a.Translates).AsSelectList();
        }
        public SelectList<int> OrderToSendBusinessTripTypeSelectList()
        {
            return _context.Set<OrderToSendBusinessTripType>().Include(a => a.Translates).AsSelectList();
        }
        public SelectList<int> CalculationMethodSelectList()
        {
            return _context.Set<CalculationMethod>().Include(a => a.Translates).AsSelectList();
        }

        public SelectList<int> LimitOperTypeSelectList()
        {
            return _context.Set<LimitOperType>().Include(a => a.Translates).AsSelectList();
        }

        public SelectList<int> CalculationTypeSelectList()
        {
            return _context.Set<CalculationType>().Include(a => a.Translates).AsSelectList();
        }

        public SelectList<int> TariffScaleTypeSelectList()
        {
            return _context.Set<TariffScaleType>().Include(a => a.Translates).AsSelectList();
        }
        public SelectList<int> EmployeeHigherEduDegreeSelectList()
        {
            return _context.Set<EmployeeHigherEduDegree>().Include(a => a.Translates).AsSelectList();
        }
        public SelectList<int> PositionPeriodSelectList()
        {
            return _context.Set<PositionPeriod>().Include(a => a.Translates).AsSelectList();
        }
        public SelectList<int> StaffingTypeSelectList()
        {
            return _context.Set<StaffingType>().Include(a => a.Translates).AsSelectList();
        }
        public SelectList<int> MissedDaysTypeSelectList()
        {
            return _context.Set<MissedDaysType>().Include(a => a.Translates).AsSelectList();
        }
    }
}
