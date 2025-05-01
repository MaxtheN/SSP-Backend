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
using SspUis.DataLayer.Repositories;
using SspUis.BizLogicLayer.Claim.ClaimOrganizationServices;

namespace SspUis.WebApi.Controllers
{
    [Authorize]
    [ApiController]
    [Route("[controller]/[action]")]
    public class ClaimOrganizationController : WebaseController
    {
        private IClaimOrganizationService _service;

        public ClaimOrganizationController(IClaimOrganizationService service)
            : base(AppSettings.Instance.ControllerConfig)
        {
            _service = service;
        }
        [HttpPost]
        [Authorize(ModuleCode.ClaimOrganizationView)]
        public PagedResult<ClaimOrganizationListDto> GetList([FromBody] SortFilterPageOptions dto)
        {
            return _service.GetList(dto);
        }

        [HttpGet]
        [Authorize(ModuleCode.ClaimOrganizationView)]
        [ProducesResponseType(typeof(ClaimOrganizationDto),200)]
        public IActionResult Get()
        {
            return Ok(_service.Get());
        }

        [HttpGet("{id}")]
        [Authorize(ModuleCode.ClaimOrganizationView)]
        [ProducesResponseType(typeof(ClaimOrganizationDto), 200)]
        public IActionResult Get(int id)
        {
            ClaimOrganizationDto dto = _service.Get(id);

            if(_service.IsValid)
                return Ok(dto);

            _service.CopyErrorsToModelState(ModelState);
            
            return ValidationProblem(ModelState);
        }

        [HttpGet]
        [ProducesResponseType(typeof(SelectList<int>), 200)]
        public IActionResult GetAsSelectList()
        {
            return Ok(_service.AsSelectList());
        }

        [HttpPost]
        [Authorize(ModuleCode.ClaimOrganizationCreate)]
        [ProducesResponseType(typeof(HaveId<int>), 200)]
        public IActionResult Create(CreateClaimOrganizationDlDto dto)
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
        [Authorize(ModuleCode.ClaimOrganizationEdit)]
        [ProducesResponseType(200)]
        public IActionResult Update(UpdateClaimOrganizationDlDto dto)
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
        [Authorize(ModuleCode.ClaimOrganizationDelete)]
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
