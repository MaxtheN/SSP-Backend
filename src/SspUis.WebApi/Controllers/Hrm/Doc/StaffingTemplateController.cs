using Microsoft.AspNetCore.Mvc;
using SspUis.BizLayer.Hrm.StaffingTemplateServices;
using SspUis.Core.Security;
using SspUis.DataLayer.Repositories.Hrm;
using WEBASE.AspNet;
using WEBASE.Models;

namespace SspUis.WebApi.Controllers
{
    [Authorize]
    [ApiController]
    [Route("hrm/[controller]/[action]")]
    public class StaffingTemplateController : WebaseController
    {
        private IStaffingTemplateService _service;

        public StaffingTemplateController(IStaffingTemplateService service)
        : base(AppSettings.Instance.ControllerConfig)
        {
            _service = service;
        }

        [HttpPost]
        [Authorize(ModuleCode.StaffingTemplateView)]
        public PagedResult<StaffingTemplateListDto> GetList([FromBody] StaffingTemplateListSortFilterDto dto)
        {
            return _service.GetList(dto);
        }

        [HttpGet]
        [Authorize(ModuleCode.StaffingTemplateView)]
        [ProducesResponseType(typeof(StaffingTemplateDto), 200)]
        public IActionResult Get()
        {
            return Ok(_service.Get());
        }

        [HttpGet("{id}")]
        [Authorize(ModuleCode.StaffingTemplateView)]
        [ProducesResponseType(typeof(StaffingTemplateDto), 200)]
        public IActionResult Get(long id)
        {
            StaffingTemplateDto dto = _service.Get(id);

            if (_service.IsValid)
                return Ok(dto);

            _service.CopyErrorsToModelState(ModelState);

            return ValidationProblem(ModelState);
        }

        [HttpGet()]
        [ProducesResponseType(typeof(SelectList<long>), 200)]
        public IActionResult GetAsSelectList()
        {
            return Ok(_service.AsSelectList());
        }

        [HttpGet()]
        [ProducesResponseType(typeof(IEnumerable<StaffingTemplateTableDto>), 200)]
        public IActionResult GetTableAsSelectList(string? organizationSettlementAccountCode = null, int? staffingTemplateId = null)
        {
            return Ok(_service.GetPositionsAsSelectList(organizationSettlementAccountCode, staffingTemplateId));
        }

        [HttpPost]
        [Authorize(ModuleCode.StaffingTemplateCreate, ModuleCode.AllStaffingTemplateCreate)]
        [ProducesResponseType(typeof(HaveId<long>), 200)]
        //[UserAction("Намунавий штатлар жадвали ҳужжатини яратиш")]
        public IActionResult Create(CreateStaffingTemplateDlDto dto)
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
        [Authorize(ModuleCode.StaffingTemplateEdit)]
        [ProducesResponseType(200)]
        //[UserAction("Намунавий штатлар жадвали ҳужжатини таҳрирлаш")]
        public IActionResult Update(UpdateStaffingTemplateDlDto dto)
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
        [Authorize(ModuleCode.StaffingTemplateAccept)]
        [ProducesResponseType(200)]
        //[UserAction("Намунавий штатлар жадвали ҳужжатини тасдиқлаш")]
        public IActionResult Accept(UpdateStatusStaffingTemplateDlDto statusDto)
        {
            if (ModelState.IsValid)
            {
                _service.Accept(statusDto);

                if (_service.IsValid)
                    return Ok();

                _service.CopyErrorsToModelState(ModelState);
            }

            return ValidationProblem(ModelState);
        }

        [HttpPost]
        [Authorize(ModuleCode.StaffingTemplateCancel)]
        [ProducesResponseType(200)]
        //[UserAction("Намунавий штатлар жадвали ҳужжатини бекор қилиш")]
        public IActionResult Cancel(UpdateStatusStaffingTemplateDlDto statusDto)
        {
            if (ModelState.IsValid)
            {
                _service.Cancel(statusDto);

                if (_service.IsValid)
                    return Ok();

                _service.CopyErrorsToModelState(ModelState);
            }

            return ValidationProblem(ModelState);
        }
        [HttpPost("{id}")]
        [Authorize(ModuleCode.StaffingTemplateDelete)]
        [ProducesResponseType(200)]
        //[UserAction("Намунавий штатлар жадвали ҳужжатини ўчириш")]
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
