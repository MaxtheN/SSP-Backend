using Microsoft.AspNetCore.Mvc;
using WEBASE.AspNet;
using WEBASE.Models;
using SspUis.Core.Security;
using SspUis.BizLogicLayer.MfyServices;

namespace SspUis.WebApi.Controllers
{
    [ApiController]
    [Route("[controller]/[action]")]
    public class MfyController : WebaseController
    {
        private IMfyService _service;

        public MfyController(IMfyService service)
            : base(AppSettings.Instance.ControllerConfig)
        {
            _service = service;
        }

        [HttpPost]
        [Authorize(ModuleCode.MfyView)]
        public PagedResult<MfyListDto> GetList([FromBody] SortFilterPageOptions dto)
            => _service.GetList(dto);

        [HttpGet("{regionId}")]
        [ProducesResponseType(typeof(SelectList<long>), 200)]
        public IActionResult GetAsSelectList(int? regionId,
                                             int? districtId)
            => Ok(_service.AsSelectList(regionId, districtId));

        [HttpGet("{id}")]
        [ProducesResponseType(typeof(MfyDto), 200)]
        [Authorize(ModuleCode.MfyView)]
        public IActionResult Get(long id)
        {
            if (ModelState.IsValid)
            {
                MfyDto dto = _service.Get(id);

                if (_service.IsValid)
                    return Ok(dto);

                _service.CopyErrorsToModelState(ModelState);
            }

            return ValidationProblem(ModelState);
        }

        [HttpPost]
        [ProducesResponseType(200)]
        [Authorize(ModuleCode.MfyView)]
        public async Task<IActionResult> SyncMfy()
        {

            if (ModelState.IsValid)
            {
                await _service.SyncMfy();

                if (_service.IsValid)
                    return Ok();

                _service.CopyErrorsToModelState(ModelState);
            }

            return ValidationProblem(ModelState);
        }
    }
}
