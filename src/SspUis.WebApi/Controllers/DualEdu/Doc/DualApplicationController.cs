using Microsoft.AspNetCore.Mvc;
using SspUis.BizLogicLayer.DualApplicationServices;
using SspUis.Core.Security;
using WEBASE.AspNet;
using WEBASE.Models;

namespace SspUis.WebApi.Controllers
{
    [Authorize]
    [ApiController]
    [Route("Dual/[controller]/[action]")]
    public class DualApplicationController : WebaseController
    {
        private IDualApplicationService _service;

        public DualApplicationController(IDualApplicationService service)
            : base(AppSettings.Instance.ControllerConfig)
        {
            _service = service;
        }

        [HttpPost]
        [Authorize(ModuleCode.DualApplicationView)]
        public PagedResult<DualApplicationListDto> GetList([FromBody] DualApplicationSortFilterOptions dto)
        {
            return _service.GetList(dto);
        }

		[HttpPost]
		public IActionResult SaveExcelDualApplication(DualApplicationSortFilterOptions dto)
		{
            if (ModelState.IsValid)
            {
                  var file = _service.SaveExcelDualApplication(dto);
                if (_service.IsValid)
                    return File(file, "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", "DualApplication.xlsx");
                _service.CopyErrorsToModelState(ModelState);

            }
            return ValidationProblem(ModelState);
		}

		[HttpGet]
        [Authorize(ModuleCode.DualApplicationView)]
        [ProducesResponseType(typeof(DualApplicationDto), 200)]
        public IActionResult Get()
        {
            var dto = _service.Get();

            if (_service.IsValid)
                return Ok(dto);

            _service.CopyErrorsToModelState(ModelState);

            return ValidationProblem(ModelState);
        }

        [HttpGet("{id}")]
        [Authorize(ModuleCode.DualApplicationView)]
        [ProducesResponseType(typeof(DualApplicationDto), 200)]
        public IActionResult Get(long id)
        {
            var dto = _service.Get(id);

            if (_service.IsValid)
                return Ok(dto);

            _service.CopyErrorsToModelState(ModelState);

            return ValidationProblem(ModelState);
        }

        [HttpPost]
        [Authorize(ModuleCode.DualApplicationReject)]
        [ProducesResponseType(200)]
        public IActionResult Reject(RejectStatusDualApplicationDto dto)
        {
            if (ModelState.IsValid)
            {
                _service.Reject(dto);

                if (_service.IsValid)
                    return Ok();

                _service.CopyErrorsToModelState(ModelState);
            }

            return ValidationProblem(ModelState);
        }

        [HttpPost]
        [Authorize(ModuleCode.DualApplicationAccept)]
        [ProducesResponseType(200)]
        public IActionResult Accept(AcceptStatusDualApplicationDto dto)
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
        [Authorize(ModuleCode.DualApplicationCancel)]
        [ProducesResponseType(200)]
        public IActionResult Cancel(CancelStatusDualApplicationDto dto)
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
        [HttpPost]
        [ProducesResponseType(200)]
        public async Task<IActionResult> SendToHierEdu(long id)
        {
            if (ModelState.IsValid)
            {
                await _service.SendToHierEdu(id);

                if (_service.IsValid)
                    return Ok();

                _service.CopyErrorsToModelState(ModelState);
            }

            return ValidationProblem(ModelState);
        }
    }
}
