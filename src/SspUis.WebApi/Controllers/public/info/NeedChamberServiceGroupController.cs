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
    public class NeedChamberServiceGroupController : WebaseController
    {
        private readonly INeedChamberServiceGroupService _service;
        public NeedChamberServiceGroupController(INeedChamberServiceGroupService service)
            => _service = service;

        [HttpPost]
        [Authorize(ModuleCode.NeedChamberServiceGroupView)]
        public PagedResult<NeedChamberServiceGroupListDto> GetList([FromBody] SortFilterPageOptions dto)
            => _service.GetList(dto);

        [HttpGet]
        [Authorize(ModuleCode.NeedChamberServiceGroupView)]
        [ProducesResponseType(typeof(NeedChamberServiceGroupDto), 200)]
        public IActionResult Get() => Ok(_service.Get());

        [HttpGet("{id}")]
        [Authorize(ModuleCode.NeedChamberServiceGroupView)]
        [ProducesResponseType(typeof(NeedChamberServiceGroupDto), 200)]
        public IActionResult Get(int id)
        {
            NeedChamberServiceGroupDto dto = _service.Get(id);

            if (_service.IsValid)
                return Ok(dto);

            _service.CopyErrorsToModelState(ModelState);

            return ValidationProblem(ModelState);
        }

        [HttpGet()]
        [ProducesResponseType(typeof(SelectList<int>), 200)]
        public IActionResult GetAsSelectList() => Ok(_service.AsSelectList());

        [HttpPost]
        [Authorize(ModuleCode.NeedChamberServiceGroupCreate)]
        [ProducesResponseType(typeof(HaveId<int>), 200)]
        public IActionResult Create(CreateNeedChamberServiceGroupDlDto dto)
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
        [Authorize(ModuleCode.NeedChamberServiceGroupEdit)]
        [ProducesResponseType(200)]
        public IActionResult Update(UpdateNeedChamberServiceGroupDlDto dto)
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
        [Authorize(ModuleCode.NeedChamberServiceGroupDelete)]
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
