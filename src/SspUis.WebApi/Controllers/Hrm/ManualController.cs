using GenericServices;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Dynamic;
using System.Linq;
using System.Threading.Tasks;
using WEBASE;
using WEBASE.AspNet;
using WEBASE.Models;
using SspUis.BizLogicLayer;
using SspUis.BizLogicLayer.Hrm.ManualServices;
using SspUis.DataLayer.EfClasses;
using SspUis.Core.Security;
using static iText.StyledXmlParser.Jsoup.Select.Evaluator;

namespace SspUis.WebApi.Hrm.Controllers
{
    [Authorize]
    [ApiController]
    [Route("hrm/[controller]/[action]")]
    public class ManualController : ControllerBase
    {
        private readonly IHrmManualService _service;

        public ManualController(IHrmManualService service)
        {
            _service = service;
        }


        [HttpGet]
        public SelectList<int> EmpAppointOrderTypeSelectList()
        {
            return _service.EmpAppointOrderTypeSelectList();
        }

        [HttpGet]
        public SelectList<int> MinimumValueTypeSelectList()
        {
            return _service.MinimumValueTypeSelectList();
        }

        [HttpGet]
        public SelectList<int> EmploymentTypeSelectList()
        {
            return _service.EmploymentTypeSelectList();
        }

        [HttpGet]
        public SelectList<int> WorkScheduleKindSelectList()
        {
            return _service.WorkScheduleKindSelectList();
        }

        [HttpGet]
        public SelectList<int> TimesheetIndicatorSelectList()
        {
            return _service.TimesheetIndicatorSelectList();
        }

        [HttpGet]
        public SelectList<int> TimesheetTypeSelectList()
        {
            return _service.TimesheetTypeSelectList();
        }

        [HttpGet]
        public SelectList<int> RoundingTypeSelectList()
        {
            return _service.RoundingTypeSelectList();
        }

        [HttpGet]
        public SelectList<int> CalculateByTimeTypeSelectList()
        {
            return _service.CalculateByTimeTypeSelectList();
        }

        [HttpGet]
        public SelectList<int> CalculationMethodSelectList()
        {
            return _service.CalculationMethodSelectList();
        }
        [HttpGet]
        public SelectList<int> TempCalcKindTypeSelectList()
        {
            return _service.TempCalcKindTypeSelectList();
        }
        [HttpGet]
        public SelectList<int> EmployeeSickLeaveTypeSelectList()
        {
            return _service.EmployeeSickLeaveTypeSelectList();
        }
        [HttpGet]
        public SelectList<int> OrderToSendBusinessTripTypeSelectList()
        {
            return _service.OrderToSendBusinessTripTypeSelectList();
        }
        [HttpGet]
        public SelectList<int> CalculationTypeSelectList()
        {
            return _service.CalculationTypeSelectList();
        }

        [HttpGet]
        public SelectList<int> TariffScaleTypeSelectList()
        {
            return _service.TariffScaleTypeSelectList();
        }
        [HttpGet]
        public SelectList<int> EmployeeHigherEduDegreeSelectList()
        {
            return _service.EmployeeHigherEduDegreeSelectList();
        }
        [HttpGet]
        public SelectList<int> PositionPeriodSelectList()
        {
            return _service.PositionPeriodSelectList();
        }
        [HttpGet]
        public SelectList<int> LimitOperTypeSelectList()
        {
            return _service.LimitOperTypeSelectList();
        }
        [HttpGet]
        public SelectList<int> StaffingTypeSelectList()
        {
            return _service.StaffingTypeSelectList();
        }
        [HttpGet]
        public SelectList<int> MissedDaysTypeSelectList()
        {
            return _service.MissedDaysTypeSelectList();
        }
    }
}

