using SspUis.BizLogicLayer.AccessServices;
using SspUis.BizLogicLayer.RoleServices;
using SspUis.Core.Security;
using SspUis.DataLayer.Repositories;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using WEBASE.AspNet;
using WEBASE.Models;

namespace SspUis.WebApi.Controllers
{
    [Authorize]
    [ApiController]
    [Route("[controller]/[action]")]
    public class AccessController : WebaseController
    {
        private IAccessService _service;

        public AccessController(IAccessService service)
            : base(AppSettings.Instance.ControllerConfig)
        {
            _service = service;
        }

        [HttpPost]
        [Authorize(ModuleCode.AccessView)]
        public PagedResult<AccessListDto> GetList([FromBody] SortFilterPageOptions dto)
        {
            return _service.GetList(dto);
        }

        [HttpGet("{id}")]
        [Authorize(ModuleCode.RoleView)]
        [ProducesResponseType(typeof(AccessDto), 200)]
        public IActionResult Get(int id)
        {
            AccessDto dto = _service.Get(id);

            if (_service.IsValid)
                return Ok(dto);

            _service.CopyErrorsToModelState(ModelState);

            return ValidationProblem(ModelState);
        }

        [HttpGet()]
        [ProducesResponseType(typeof(SelectList<int>), 200)]
        public IActionResult GetAsSelectList()
        {
            return Ok(_service.AsSelectList());
        }
        
        [HttpPost]
        [Authorize(ModuleCode.AccessEdit)]
        [ProducesResponseType(200)]
        public IActionResult Update(UpdateAccessibilityDlDto dto)
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
    }
}
