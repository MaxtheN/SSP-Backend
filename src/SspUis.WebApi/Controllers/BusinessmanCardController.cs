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
using SspUis.BizLogicLayer.ContractorServices;
using SspUis.BizLogicLayer.Models;
using SspUis.DataLayer.Repositories;
using SspUis.BizLogicLayer.BusinessmanCardServices;
using SspUis.Integration.Soliq.Models;

namespace SspUis.WebApi.Controllers
{
    [Authorize]
    [ApiController]
    [Route("[controller]/[action]")]
    public class BusinessmanCardController : WebaseController
    {
        private IBusinessmanCardService _service;

        public BusinessmanCardController(IBusinessmanCardService service)
            :base(AppSettings.Instance.ControllerConfig)
        {
            _service = service;
        }
        [HttpGet("{inn}")]
        [Authorize(ModuleCode.BusinessmanCardView)]
        [ProducesResponseType(typeof(BusinessmanCardDto), 200)]
        public async Task<ActionResult> GetByInn(string inn)
        {
            var dto = await _service.GetByInn(inn);

            if (_service.IsValid)
                return Ok(dto);

            _service.CopyErrorsToModelState(ModelState);

            return ValidationProblem(ModelState);
        }

    }
}
