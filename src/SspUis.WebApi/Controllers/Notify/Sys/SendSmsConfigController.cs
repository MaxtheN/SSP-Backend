using Microsoft.AspNetCore.Mvc;
using SspUis.BizLogicLayer.Notify;
using SspUis.Core.Security;
using SspUis.DataLayer.Repositories;
using WEBASE.AspNet;
using WEBASE.Models;

namespace SspUis.WebApi.Controllers.Notify
{
    [ApiController]
    [Authorize]
    [Route("Notify/[controller]/[action]")]
    public class SendSmsConfigController : WebaseController
    {
        private readonly ISendSmsConfigService _service;
        public SendSmsConfigController(ISendSmsConfigService service)
            => _service = service;

        [HttpPost]
        [Authorize(ModuleCode.SendSmsConfigViewAll)]
        public PagedResult<SendSmsConfigListDto> GetList(
            [FromBody] SendSmsConfigListDtoSortFilterPageOption options)
        {
            return _service.GetList(options);
        }

        [HttpGet]
        [Authorize(ModuleCode.SendSmsConfigView)]
        [ProducesResponseType(typeof(SendSmsConfigDto), 200)]
        public IActionResult Get()
        {
            return Ok(_service.Get());
        }

        [HttpGet("{id}")]
        [Authorize(ModuleCode.SendSmsConfigView)]
        [ProducesResponseType(typeof(SendSmsConfigDto), 200)]
        public IActionResult Get(int id)
        {
            var dto = _service.Get(id);

            if (_service.IsValid)
                return Ok(dto);

            _service.CopyErrorsToModelState(ModelState);

            return ValidationProblem(ModelState);
        }

        [HttpPost]
        [Authorize(ModuleCode.SendSmsConfigCreate)]
        [ProducesResponseType(typeof(HaveId<long>), 200)]
        public IActionResult Create(CreateSendSmsConfigDlDto dto)
        {
            if (ModelState.IsValid)
            {
                HaveId<int> result = _service.Create(dto);

                if (_service.IsValid)
                    return Ok(result);

                _service.CopyErrorsToModelState(ModelState);
            }

            return ValidationProblem(ModelState);
        }

        [HttpPost]
        [Authorize(ModuleCode.SendSmsConfigEdit)]
        [ProducesResponseType(200)]
        public IActionResult Update(UpdateSendSmsConfigDlDto dto)
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
        [Authorize(ModuleCode.SendSmsConfigDelete)]
        [ProducesResponseType(200)]
        public IActionResult Delete(int id)
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
    }
}
