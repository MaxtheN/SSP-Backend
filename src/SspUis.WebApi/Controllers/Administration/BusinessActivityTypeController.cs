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
using SspUis.BizLogicLayer.BusinessActivityTypeServices;
using SspUis.BizLogicLayer.BusinessmanCardServices;
using SspUis.BizLogicLayer.NationalityServices;

namespace SspUis.WebApi.Controllers
{
    [Authorize]
    [ApiController]
    [Route("[controller]/[action]")]
    public class BusinessActivityTypeController : WebaseController
    {
        private IBusinessActivityTypeService _service;

        public BusinessActivityTypeController(IBusinessActivityTypeService service)
            : base(AppSettings.Instance.ControllerConfig)
        {
            _service = service;
        }

        [HttpPost]
        [Authorize(ModuleCode.BusinessActivityTypeView)]
        public PagedResult<BusinessActivityTypeListDto> GetList([FromBody] SortFilterPageOptions dto)
        {
            return _service.GetList(dto);
        }

        [HttpPost]
        [Authorize(ModuleCode.BusinessActivityTypeView)]
        [ProducesResponseType(typeof(BusinessActivityTypeDto), 200)]
        public async Task<ActionResult> SyncContractorInfo()
        {
            var dto = await _service.SyncContractorInfo();

            if (_service.IsValid)
                return Ok(dto);

            _service.CopyErrorsToModelState(ModelState);

            return ValidationProblem(ModelState);
        }
    }
}
