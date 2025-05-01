using Microsoft.AspNetCore.Mvc;
using SspUis.BizLogicLayer;
using SspUis.Core.Security;
using SspUis.DataLayer.Repositories;
using WEBASE.AspNet;
using WEBASE.Models;
using static SspUis.BizLogicLayer.UpdateStatusCompleteService;

namespace SspUis.WebApi.Controllers.Srv.Doc
{
    [Authorize]
    [ApiController]
    [Route("srv/[controller]/[action]")]
    public class CompleteServiceController : WebaseController
    {
        private readonly ISrvCompleteService _service;
        public CompleteServiceController(ISrvCompleteService service)
            : base(AppSettings.Instance.ControllerConfig)
        {
            _service = service;
        }

        [HttpPost]
        [Authorize(ModuleCode.CompleteServiceViewAll)]
        public PagedResult<CompletedServiceListDto> GetList([FromBody] SrvCompleteSortFilterOption options)
        {
            return _service.GetList(options);
        }

        [HttpGet]
        [Authorize(ModuleCode.CompleteServiceView)]
        [ProducesResponseType(typeof(CompletedServiceDto), 200)]
        public IActionResult Get()
        {
            return Ok(_service.Get());
        }

        [HttpGet("{id}")]
        [Authorize(ModuleCode.CompleteServiceView)]
        [ProducesResponseType(typeof(CompletedServiceDto), 200)]
        public IActionResult Get(long id)
        {
            var dto = _service.Get(id);

            if (_service.IsValid)
                return Ok(dto);

            _service.CopyErrorsToModelState(ModelState);

            return ValidationProblem(ModelState);
        }

        [HttpPost]
        [Authorize(ModuleCode.CompleteServiceCreate)]
        [ProducesResponseType(typeof(HaveId<long>), 200)]
        public IActionResult Create(CreateCompletedServiceDlDto dto)
        {
            if (ModelState.IsValid)
            {
                HaveId<long> result = _service.Create(dto);

                if (_service.IsValid)
                    return Ok(result);

                _service.CopyErrorsToModelState(ModelState);
            }

            return ValidationProblem(ModelState);
        }

        [HttpPost]
        [Authorize(ModuleCode.CompleteServiceEdit)]
        [ProducesResponseType(200)]
        public IActionResult Update(UpdateCompletedServiceDlDto dto)
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

        [HttpPost("{id}")]
        [Authorize(ModuleCode.CompleteServiceDelete)]
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

        [HttpPost]
        [Authorize(ModuleCode.CompleteServiceCancel)]
        [ProducesResponseType(200)]
        public IActionResult Cancel(CancelStatusCompletedSrvDto dto)
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
        [Authorize(ModuleCode.CompleteServiceAccept)]
        [ProducesResponseType(200)]
        public IActionResult Accept(AcceptStatusCompletedSrvDto dto)
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
    }
}
