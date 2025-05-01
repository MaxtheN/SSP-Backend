using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using WEBASE;
using WEBASE.AspNet;
using WEBASE.Models;
using SspUis.Core.Security;
using SspUis.BizLogicLayer.ApplicationServices;
using SspUis.DataLayer.Repositories;
using SspUis.BizLogicLayer.ReportServices;
using SspUis.BizLogicLayer.Doc.PrtnContractServices;
using SspUis.BizLogicLayer;
using SspUis.Core.Configurations;
using SspUis.BizLogicLayer.MonoApplicationServices;
using SspUis.BizLogicLayer.Doc;

namespace SspUis.WebApi.Controllers
{
    [Authorize]
    [ApiController]
    [Route("[controller]/[action]")]
    public class MonoApplicationController : WebaseController
    {
        private readonly IMonoApplicationService _service;
        private readonly IMonoApplicationResultService _resultServcie;
        private readonly IHtmlReportService _htmlReportService;
        private readonly SystemConf _systemConf;

        public MonoApplicationController(IMonoApplicationService service,
                                         IMonoApplicationResultService resultService,
                                         IHtmlReportService htmlReportService,
                                         SystemConf systemConf)
                                         : base(AppSettings.Instance.ControllerConfig)
        {
            _service = service;
            _resultServcie = resultService;
            _htmlReportService = htmlReportService;
            _systemConf = systemConf;

        }
        [Authorize]
        [HttpGet]
        public IActionResult GetAllMonoAppRes()
        => Ok(_resultServcie.GetList());

        [Authorize]
        [HttpGet]
        public IActionResult GetByMonoAppId(long appId)
        => Ok(_resultServcie.GetAppId(appId));

        [HttpPost]
        //[Authorize(ModuleCode.MonoApplicationView)]
        public PagedResult<BizLogicLayer.MonoApplicationServices.MonoApplicationListDto> GetList([FromBody] MonoApplicationSortFilterOptions dto)
        {
            return _service.GetList(dto);
        }

        [Authorize]
        [HttpGet("{id}")]
        [ProducesResponseType(typeof(MonoApplicationDto), 200)]
        public IActionResult Get(long id)
        {
            var dto = _service.Get(id);

            if (_service.IsValid)
                return Ok(dto);

            _service.CopyErrorsToModelState(ModelState);

            return ValidationProblem(ModelState);
        }

        [Authorize]
        [HttpPost]
        public async Task<IActionResult> SendForReviewWithoutRabbit(long id)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    await _service.SentForReview(id);
                    if (_service.IsValid)
                        return Ok();
                    _service.CopyErrorsToModelState(ModelState);

                }
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
            return ValidationProblem(ModelState);

        }
    }
}
