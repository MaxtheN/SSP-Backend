using Microsoft.AspNetCore.Mvc;
using SspUis.BizLogicLayer;
using SspUis.BizLogicLayer.Claim;
using WEBASE.AspNet;
using WEBASE.Models;

namespace SspUis.My.WebApi.Controllers
{
    [Authorize]
    [ApiController]
    [Route("[controller]/[action]")]
    public class MediationController : WebaseController
    {
        private readonly IMediationService _service;
        public MediationController(IMediationService service) 
            : base(AppSettings.Instance.ControllerConfig)
            => _service = service;

        [HttpPost]
        public PagedResult<MediationListDto> GetList([FromBody] MediationSortFilterOptions dto)
        {
            return _service.GetList(dto);
        }

        [HttpGet("{id}")]
        [ProducesResponseType(typeof(MediationDto), 200)]
        public IActionResult Get(long id)
        {
            MediationDto dto = _service.Get(id);

            if (_service.IsValid)
                return Ok(dto);

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

        [HttpGet]
        [ProducesResponseType(typeof(MediationDto), 200)]
        public IActionResult GetByPlanId(int planId)
        {
            return Ok(_service.GetByPlanId(planId));
        }
    }
}
