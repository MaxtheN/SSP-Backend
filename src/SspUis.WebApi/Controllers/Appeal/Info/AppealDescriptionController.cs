using Microsoft.AspNetCore.Mvc;
using SspUis.BizLogicLayer.AppealDescriptionServices;
using SspUis.Core.Security;
using SspUis.DataLayer.Repositories;
using WEBASE.AspNet;
using WEBASE.Models;

namespace SspUis.WebApi.Controllers
{
    [Authorize]
    [ApiController]
    [Route("[controller]/[action]")]
    public class AppealDescriptionController : WebaseController
    {
        private IAppealDescriptionService _service;

        public AppealDescriptionController(IAppealDescriptionService service)
            : base(AppSettings.Instance.ControllerConfig)
        {
            _service = service;
        }
        [HttpPost]
        [Authorize(ModuleCode.AppealDescriptionView)]
        public PagedResult<AppealDescriptionListDto> GetList([FromBody] SortFilterPageOptions dto)
        {
            return _service.GetList(dto);
        }

        [HttpGet]
        [Authorize(ModuleCode.AppealDescriptionView)]
        [ProducesResponseType(typeof(AppealDescriptionDto), 200)]
        public IActionResult Get()
        {
            return Ok(_service.Get());
        }

        [HttpGet("{id}")]
        [Authorize(ModuleCode.AppealDescriptionView)]
        [ProducesResponseType(typeof(AppealDescriptionDto), 200)]
        public IActionResult Get(int id)
        {
            AppealDescriptionDto dto = _service.Get(id);

            if (_service.IsValid)
                return Ok(dto);

            _service.CopyErrorsToModelState(ModelState);

            return ValidationProblem(ModelState);
        }

        [HttpGet]
        [ProducesResponseType(typeof(SelectList<int>), 200)]
        public IActionResult GetAsSelectList(bool hasParent)
        {
            return Ok(_service.AsSelectList(hasParent));
        }

        [HttpPost]
        [Authorize(ModuleCode.AppealDescriptionCreate)]
        [ProducesResponseType(typeof(HaveId<int>), 200)]
        public IActionResult Create(CreateAppealDescriptionDlDto dto)
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
        [Authorize(ModuleCode.AppealDescriptionEdit)]
        [ProducesResponseType(200)]
        public IActionResult Update(UpdateAppealDescriptionDlDto dto)
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
        [Authorize(ModuleCode.AppealDescriptionDelete)]
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
