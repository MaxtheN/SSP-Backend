using Microsoft.AspNetCore.Mvc;
using SspUis.BizLogicLayer;
using WEBASE.AspNet;
using WEBASE.Models;

namespace SspUis.My.WebApi.Controllers.Srv
{
    [ApiController]
    [Route("[controller]/[action]")]
    public class NeedChamberServiceGroupController : WebaseController
    {
        private readonly INeedChamberServiceGroupService _service;
        public NeedChamberServiceGroupController(INeedChamberServiceGroupService service)
        {
            _service = service;
        }

        [HttpPost]
        public PagedResult<NeedChamberServiceGroupListDto> GetList([FromBody] SortFilterPageOptions options)
        {
            return _service.GetList(options);
        }

        [HttpGet("{id}")]
        [ProducesResponseType(typeof(NeedChamberServiceGroupDto), 200)]
        public IActionResult Get(int id)
        {
            var dto = _service.Get(id);

            if (_service.IsValid)
                return Ok(dto);

            _service.CopyErrorsToModelState(ModelState);

            return ValidationProblem(ModelState);
        }
    }
}
