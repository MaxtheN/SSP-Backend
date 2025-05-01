using Microsoft.AspNetCore.Mvc;
using SspUis.BizLogicLayer.JoinAntiCorruptionApplicationServices;
using SspUis.Core.Security;
using WEBASE.AspNet;
using WEBASE.Models;

namespace SspUis.WebApi.Controllers
{
    [Authorize]
    [ApiController]
    [Route("Corruption/[controller]/[action]")]
    public class JoinAntiCorruptionApplicationController : WebaseController
    {
        private IJoinAntiCorruptionApplicationService _service;
        public JoinAntiCorruptionApplicationController(IJoinAntiCorruptionApplicationService service)
            : base(AppSettings.Instance.ControllerConfig)
        {
            _service = service;
        }
        [HttpPost]
        [Authorize(ModuleCode.JoinAntiCorruptionApplicationView)]
        public PagedResult<JoinAntiCorruptionApplicationListDto> GetList([FromBody] JoinAntiCorruptionApplicationSortFilterOptions dto)
        {
            return _service.GetList(dto);
        }

        [HttpGet]
        [Authorize(ModuleCode.JoinAntiCorruptionApplicationView)]
        [ProducesResponseType(typeof(JoinAntiCorruptionApplicationDto), 200)]
        public async Task<IActionResult> Get()
        {
            var dto = _service.Get();

            if (_service.IsValid)
                return Ok(dto);

            _service.CopyErrorsToModelState(ModelState);

            return ValidationProblem(ModelState);
        }

        [HttpGet("{id}")]
        [Authorize(ModuleCode.JoinAntiCorruptionApplicationView)]
        [ProducesResponseType(typeof(JoinAntiCorruptionApplicationDto), 200)]
        public IActionResult Get(long id)
        {
            var dto = _service.Get(id);

            if (_service.IsValid)
                return Ok(dto);

            _service.CopyErrorsToModelState(ModelState);

            return ValidationProblem(ModelState);
        }

        [HttpPost]
        [Authorize(ModuleCode.JoinAntiCorruptionApplicationDelete)]
        [ProducesResponseType(200)]
        public IActionResult Delete(long id)
        {
            if (ModelState.IsValid)
            {
                _service.Delete(id);

                if (_service.IsValid)
                    return Ok();

                _service.CopyErrorsToModelState(ModelState);
            }

            return ValidationProblem(ModelState);
        }
        [HttpGet]
        [ProducesResponseType(typeof(Boolean), 200)]
        public IActionResult CanCreate()
        {
            if (ModelState.IsValid)
            {
                var result = _service.CanCreate();

                if (_service.IsValid)
                {
                    return Ok(result);
                }

                _service.CopyErrorsToModelState(ModelState);
            }

            return ValidationProblem(ModelState);
        }

        [HttpPost]
        [Authorize(ModuleCode.JoinAntiCorruptionApplicationReject)]
        [ProducesResponseType(200)]
        public IActionResult Reject(RejectStatusJoinAntiCorruptionApplicationDto dto)
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
        [Authorize(ModuleCode.JoinAntiCorruptionApplicationAccept)]
        [ProducesResponseType(200)]
        public IActionResult Accept(AcceptStatusJoinAntiCorruptionApplicationDto dto)
        {
            if (ModelState.IsValid)
            {
                _service.Accept(dto);

                if (_service.IsValid) return Ok();
                _service.CopyErrorsToModelState(ModelState);
            }
            return ValidationProblem(ModelState);
        }

        [HttpPost]
        [Authorize(ModuleCode.JoinAntiCorruptionApplicationCancel)]
        [ProducesResponseType(200)]
        public IActionResult Cancel(CancelStatusJoinAntiCorruptionApplicationDto dTo)
        {
            if (ModelState.IsValid)
            {
                _service.Cancel(dTo);

                if (_service.IsValid)
                    return Ok();

                _service.CopyErrorsToModelState(ModelState);
            }

            return ValidationProblem(ModelState);
        }

        [HttpGet]
        [AllowAnonymous]
        [ProducesResponseType(200)]
        public async ValueTask<IActionResult> DownloadPdf(Guid id2, string? lang = null)
        {
            if (ModelState.IsValid)
            {
                var bytes = await _service.DownloadPdf(id2, lang);

                if (_service.IsValid)
                    return File(bytes, "application/pdf");

                _service.CopyErrorsToModelState(ModelState);
            }

            return ValidationProblem(ModelState);
        }
    }
}
