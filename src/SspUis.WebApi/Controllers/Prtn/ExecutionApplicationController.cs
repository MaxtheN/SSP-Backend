using Microsoft.AspNetCore.Mvc;
using SspUis.BizLogicLayer;
using SspUis.BizLogicLayer.ExecutionApplicationServices;
using SspUis.Core.Security;
using SspUis.DataLayer.Repositories;
using WEBASE.AspNet;
using WEBASE.Models;
using WEBASE.Storage;

namespace SspUis.WebApi.Controllers
{
    [Authorize]
    [ApiController]
    [Route("[controller]/[action]")]
    public class ExecutionApplicationController : WebaseController
    {
        private IExecutionApplicationService _service;

        public ExecutionApplicationController(IExecutionApplicationService service)
            : base(AppSettings.Instance.ControllerConfig)
        {
            _service = service;
        }

        [HttpPost]
        [Authorize(ModuleCode.ExecutionApplicationView)]
        public PagedResult<ExecutionApplicationListDto> GetList([FromBody] ExecutionApplicationSortFilterOptions dto)
        {
            return _service.GetList(dto);
        }

        [HttpGet]
        [Authorize(ModuleCode.ExecutionApplicationView)]
        [ProducesResponseType(typeof(ExecutionApplicationDto), 200)]
        public IActionResult Get()
        {
            var dto = _service.Get();

            if (_service.IsValid)
                return Ok(dto);

            _service.CopyErrorsToModelState(ModelState);

            return ValidationProblem(ModelState);
        }
        [HttpGet]
        [ProducesResponseType(typeof(List<ExecutionApplicationCellDto>), 200)]
        public IActionResult GetFromGraph(int year, int month)
        {
            var dto = _service.GetFromGraph(year, month);

            if (_service.IsValid)
                return Ok(dto);

            _service.CopyErrorsToModelState(ModelState);

            return ValidationProblem(ModelState);
        }

        [HttpGet("{id}")]
        [Authorize(ModuleCode.ExecutionApplicationView)]
        [ProducesResponseType(typeof(ExecutionApplicationDto), 200)]
        public IActionResult Get(long id)
        {
            var dto = _service.Get(id);

            if (_service.IsValid)
                return Ok(dto);

            _service.CopyErrorsToModelState(ModelState);

            return ValidationProblem(ModelState);
        }

        [HttpPost]
        [Authorize(ModuleCode.ExecutionApplicationView)]
        [ProducesResponseType(typeof(HaveId<long>), 200)]
        public async Task<IActionResult> Create(CreateExecutionApplicationDlDto dto)
        {
            var result = await _service.Create(dto);

            if (_service.IsValid)
                return Ok(result);

            _service.CopyErrorsToModelState(ModelState);

            return ValidationProblem(ModelState);
        }

        [HttpPost]
        [Authorize(ModuleCode.ExecutionApplicationView)]
        [ProducesResponseType(200)]
        public IActionResult Update(UpdateExecutionApplicationDlDto dto)
        {
            if (ModelState.IsValid)
            {
                _service.Update(dto);

                if (_service.IsValid)
                    return Ok();

                _service.CopyErrorsToModelState(ModelState);
            }

            return ValidationProblem(ModelState);
        }
        [HttpPost]
        [Authorize(ModuleCode.ExecutionApplicationAccept)]
        [ProducesResponseType(200)]
        public IActionResult Accept(AcceptStatusExecutionApplicationDto dto)
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
        [Authorize(ModuleCode.ExecutionApplicationSign)]
        public async Task<IActionResult> Sign(SignStatusExecutionApplicationDto dto)
        {
            if (ModelState.IsValid)
            {
                await _service.Sign(dto);

                if (_service.IsValid)
                    return Ok();

                _service.CopyErrorsToModelState(ModelState);
            }

            return ValidationProblem(ModelState);
        }
        [HttpPost]
        [Authorize(ModuleCode.ExecutionApplicationCancel)]
        [ProducesResponseType(200)]
        public IActionResult Cancel(CancelStatusExecutionApplicationDto dto)
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

        [HttpGet]
        [AllowAnonymous]
        [ProducesResponseType(200)]
        public async ValueTask<IActionResult> DownloadPdf(Guid id2, string? lang = null)
        {
            if (ModelState.IsValid)
            {
                var bytes =await _service.DownloadPdf(id2, lang);

                if (_service.IsValid)
                    return File(bytes, "application/pdf");

                _service.CopyErrorsToModelState(ModelState);
            }

            return ValidationProblem(ModelState);
        }
    }
}
