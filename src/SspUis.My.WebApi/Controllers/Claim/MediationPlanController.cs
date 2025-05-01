using Microsoft.AspNetCore.Mvc;
using SspUis.BizLogicLayer.Claim;
using WEBASE.AspNet;
using WEBASE.AspNet.Security;
using WEBASE.Models;

namespace SspUis.My.WebApi.Controllers
{
    [ApiController]
    [Authorize]
    [Route("[controller]/[action]")]
    public class MediationPlanController : WebaseController
    {
        private readonly IMediationPlanService _service;

        public MediationPlanController(IMediationPlanService service)
            : base(AppSettings.Instance.ControllerConfig)
            => this._service = service;

        [HttpPost]
        public PagedResult<MediationPlanListDto> GetList([FromBody] MediationPlanSortFilterOptions options)
        {
            return _service.GetList(options);
        }

        [HttpGet]
        [ProducesResponseType(typeof(MediationPlanDto), 200)]
        public IActionResult GetByApplication(int applicationId)
        {
            return Ok(_service.GetByApplication(applicationId));
        }

        [HttpGet("{id}")]
        [ProducesResponseType(typeof(MediationPlanDto), 200)]
        public IActionResult Get(long id)
        {
            var dto = _service.Get(id);

            if (_service.IsValid)
                return Ok(dto);

            _service.CopyErrorsToModelState(ModelState);

            return ValidationProblem(ModelState);
        }
        [HttpGet]
        [AllowAnonymous]
        [ProducesResponseType(200)]
        public IActionResult DownloadPdf(Guid id2, string? lang)
        {
            if (ModelState.IsValid)
            {
                var bytes = _service.DownloadPdf(id2, lang);

                if (_service.IsValid)
                    return File(bytes, "application/pdf");

                _service.CopyErrorsToModelState(ModelState);
            }

            return ValidationProblem(ModelState);
        }
        [HttpPost]
        [ProducesResponseType(typeof(SelectList<long>), 200)]
        public IActionResult GetAsSelectList(MediationPlanSortFilterOptions options)
        {
            return Ok(_service.AsSelectList(options));
        }
    }
}
