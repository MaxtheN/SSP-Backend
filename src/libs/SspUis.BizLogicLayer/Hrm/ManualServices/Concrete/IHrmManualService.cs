using SspUis.BizLogicLayer.EnumServices;
using StatusGeneric;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WEBASE.AspNet;
using WEBASE.Models;

namespace SspUis.BizLogicLayer.Hrm.ManualServices
{
    public interface IHrmManualService : IStatusGeneric
    {
        SelectList<int> EmpAppointOrderTypeSelectList();
        SelectList<int> MinimumValueTypeSelectList();
        SelectList<int> EmploymentTypeSelectList();
        SelectList<int> WorkScheduleKindSelectList();
        SelectList<int> TimesheetIndicatorSelectList();
        SelectList<int> TimesheetTypeSelectList();
        SelectList<int> RoundingTypeSelectList();
        SelectList<int> CalculateByTimeTypeSelectList();
        SelectList<int> EmployeeSickLeaveTypeSelectList();
        SelectList<int> TempCalcKindTypeSelectList();
        SelectList<int> CalculationMethodSelectList();
        SelectList<int> OrderToSendBusinessTripTypeSelectList();
        SelectList<int> CalculationTypeSelectList();
        SelectList<int> TariffScaleTypeSelectList();
        SelectList<int> EmployeeHigherEduDegreeSelectList();
        SelectList<int> PositionPeriodSelectList();
        SelectList<int> LimitOperTypeSelectList();
        SelectList<int> StaffingTypeSelectList();
        SelectList<int> MissedDaysTypeSelectList();
    }
}
