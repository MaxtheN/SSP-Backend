using Microsoft.AspNetCore.Mvc;
using SspUis.BizLogicLayer;
using SspUis.Core.Security;
using SspUis.DataLayer.Repositories;
using WEBASE.AspNet;
using WEBASE.Models;

namespace SspUis.WebApi.Controllers
{
    [Authorize]
    [ApiController]
    [Route("public/[controller]/[action]")]
    public class RestrictionSendingAppController : WebaseController
    {
        private readonly IRestrictionSendingAppService _service;
        public RestrictionSendingAppController(IRestrictionSendingAppService service)
            => _service = service;

        [HttpPost]
        [Authorize(ModuleCode.RestrictionSendingAppViewAll)]
        public PagedResult<RestrictionSendingAppListDto> GetList([FromBody] RestrictionSendingAppDtoSortFilter dto)
        {
            return _service.GetList(dto);
        }

        [HttpGet]
        [Authorize(ModuleCode.RestrictionSendingAppView)]
        [ProducesResponseType(typeof(RestrictionSendingAppDto), 200)]
        public IActionResult Get()
        {
            return Ok(_service.Get());
        }

        [HttpGet("{id}")]
        [Authorize(ModuleCode.RestrictionSendingAppView)]
        [ProducesResponseType(typeof(RestrictionSendingAppDto), 200)]
        public IActionResult Get(long id)
        {
            RestrictionSendingAppDto dto = _service.Get(id);

            if (_service.IsValid)
                return Ok(dto);

            _service.CopyErrorsToModelState(ModelState);

            return ValidationProblem(ModelState);
        }

        [HttpPost]
        [Authorize(ModuleCode.RestrictionSendingAppCreate)]
        [ProducesResponseType(typeof(HaveId<long>), 200)]
        public IActionResult Create(CreateRestrictionSendingAppDlDto dto)
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
        [Authorize(ModuleCode.RestrictionSendingAppEdit)]
        [ProducesResponseType(200)]
        public IActionResult Update(UpdateRestrictionSendingAppDlDto dto)
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
        [Authorize(ModuleCode.RestrictionSendingAppDelete)]
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
    }
}
