using Microsoft.AspNetCore.Mvc;
using SspUis.BizLogicLayer;
using SspUis.BizLogicLayer.Kpi;
using SspUis.BizLogicLayer.Models;
using SspUis.Core.Security;
using SspUis.DataLayer.Repositories;
using WEBASE.AspNet;
using WEBASE.Models;
using WEBASE.Storage;

namespace SspUis.WebApi.Controllers
{


    [Authorize]
    [ApiController]
    [Route("kpi/[controller]/[action]")]
    public class KpiRatingEmployeeController : WebaseController
    {
        private IKpiRatingEmployeeService _service;

        public KpiRatingEmployeeController(IKpiRatingEmployeeService service)
            : base(AppSettings.Instance.ControllerConfig)
        {
            this._service = service;
        }

        [HttpPost]
        [Authorize(ModuleCode.KpiRatingEmployeeView)]
        public PagedResult<KpiRatingEmployeeListDto> GetList([FromBody] KpiRatingEmployeeSortFilterOptions dto)
        {
            return _service.GetList(dto);
        }

        [HttpGet]
        [Authorize(ModuleCode.KpiRatingEmployeeView)]
        [ProducesResponseType(typeof(KpiRatingEmployeeDto), 200)]
        public IActionResult Get()
        {
            return Ok(_service.Get());
        }

        [HttpGet("{id}")]
        [Authorize(ModuleCode.KpiRatingEmployeeView)]
        [ProducesResponseType(typeof(KpiRatingEmployeeDto), 200)]
        public IActionResult Get(long id)
        {
            var dto = _service.Get(id);

            if(_service.IsValid)
                return Ok(dto);

            _service.CopyErrorsToModelState(ModelState);

            return ValidationProblem(ModelState);
        }

        [HttpPost]
        [ProducesResponseType(typeof(KpiRatingEmployeeDto), 200)]
        public IActionResult FillTable(int planId)
        {
            return Ok(_service.FillTable(planId));
        }
        [HttpPost]
        [Authorize(ModuleCode.KpiRatingEmployeeView)]
        [ProducesResponseType(typeof(SelectList<long>), 200)]
        public IActionResult GetAsSelectList()
        {
            return Ok(_service.AsSelectList());
        }

        [HttpPost]
        [Authorize(ModuleCode.KpiRatingEmployeeCreate)]
        [ProducesResponseType(typeof(HaveId<long>), 200)]
        public IActionResult Create(CreateKpiRatingEmployeeDlDto dto)
        {
            if(ModelState.IsValid)
            {
                HaveId<long> result = _service.Create(dto);

                if(_service.IsValid)
                    return Ok(result);

                _service.CopyErrorsToModelState(ModelState);
            }

            return ValidationProblem(ModelState);
        }

        [HttpPost]
        [Authorize(ModuleCode.KpiRatingEmployeeEdit)]
        [ProducesResponseType(200)]
        public IActionResult Update(UpdateKpiRatingEmployeeDlDto dto)
        {
            if(ModelState.IsValid)
            {
                _service.Update(dto);

                if(_service.IsValid)
                    return Ok();

                _service.CopyErrorsToModelState(ModelState);
            }

            return ValidationProblem(ModelState);
        }
        [HttpPost]
        [Authorize(ModuleCode.KpiRatingEmployeeAccept)]
        [ProducesResponseType(200)]
        public IActionResult Accept(UpdateStatusKpiRatingEmployeeDto dto)
        {
            if (ModelState.IsValid)
            {
                _service.Accept(dto);

                if (_service.IsValid)
                    return Ok();

                _service.CopyErrorsToModelState(ModelState);
            }

            return ValidationProblem(ModelState);
        }

        [HttpPost]
        [Authorize(ModuleCode.KpiRatingEmployeeCancel)]
        [ProducesResponseType(200)]
        public IActionResult Cancel(UpdateStatusKpiRatingEmployeeDto dto)
        {
            if (ModelState.IsValid)
            {
                _service.Cancel(dto);

                if (_service.IsValid)
                    return Ok();

                _service.CopyErrorsToModelState(ModelState);
            }

            return ValidationProblem(ModelState);
        }
        [HttpPost("{id}")]
        [Authorize(ModuleCode.KpiRatingEmployeeDelete)]
        [ProducesResponseType(200)]
        public IActionResult Delete(long id)
        {
            if(ModelState.IsValid)
            {
                _service.Delete(id);

                if(_service.IsValid)
                    return Ok();

                _service.CopyErrorsToModelState(ModelState);
            }

            return ValidationProblem(ModelState);
        }
    }

}