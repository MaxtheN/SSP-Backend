using Microsoft.AspNetCore.Mvc;
using SspUis.BizLogicLayer.DualApplicationServices;
using SspUis.Core.Configurations;
using SspUis.DataLayer.Repositories;
using WEBASE.AspNet;
using WEBASE.Models;

namespace SspUis.My.WebApi.Controllers
{
    [Authorize]
    [ApiController]
    [Route("Dual/[controller]/[action]")]
    public class DualApplicationController : WebaseController
    {
        private IDualApplicationService _service;
        private readonly SystemConf _systemConf;

        public DualApplicationController(IDualApplicationService service, SystemConf systemConf)
            :base(AppSettings.Instance.ControllerConfig) 
        {
            _service = service;
            _systemConf = systemConf;
        }

        [HttpPost]
        public PagedResult<DualApplicationListDto> GetList([FromBody] DualApplicationSortFilterOptions dto)
        {
            return _service.GetList(dto);
        }
        [HttpPost]
        public IActionResult GetCount()
        {
            var result = _service.GetCount();
            return Ok(result);
        }

        [HttpGet]
        [ProducesResponseType(typeof(DualApplicationDto), 200)]
        public async Task<IActionResult> Get()
        {
            var dto = _service.Get();

            if (_service.IsValid)
                return Ok(dto);

            _service.CopyErrorsToModelState(ModelState);

            return ValidationProblem(ModelState);
        }

        [HttpGet("{id}")]
        [ProducesResponseType(typeof(DualApplicationDto), 200)]
        public IActionResult Get(long id)
        {
            var dto = _service.Get(id);

            if (_service.IsValid)
                return Ok(dto);

            _service.CopyErrorsToModelState(ModelState);

            return ValidationProblem(ModelState);
        }
        [HttpPost]
        [ProducesResponseType(typeof(HaveId<long>), 200)]
        public IActionResult Create(CreateDualApplicationDlDto dto)
        {
            var result = _service.Create(dto);

            if (_service.IsValid)
                return Ok(result);

            _service.CopyErrorsToModelState(ModelState);

            return ValidationProblem(ModelState);
        }
        [HttpPost]
        [ProducesResponseType(200)]
        public IActionResult Update(UpdateDualApplicationDlDto dto)
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
        [ProducesResponseType(200)]
        public async Task<IActionResult> Send(SendStatusDualApplicationDto dto)
        {
            if (ModelState.IsValid)
            {
                await _service.Send(dto);

                if (_service.IsValid)
                    return Ok();

                _service.CopyErrorsToModelState(ModelState);
            }

            return ValidationProblem(ModelState);
        }
        [HttpPost]
        //[Authorize]
        [AllowAnonymous]
        [ProducesResponseType(200)]
        public async Task<IActionResult> SendToHierEdu(long id)
        {
            if (ModelState.IsValid)
            {
                await _service.SendToHierEdu(id);

                if (_service.IsValid)
                    return Ok();

                _service.CopyErrorsToModelState(ModelState);
            }

            return ValidationProblem(ModelState);
        }
        [HttpGet]
        [Authorize]
        [ProducesResponseType(typeof(Boolean), 200)]
        public IActionResult CanCreate()
        {
            if (ModelState.IsValid)
            {
                var result = _service.CanCreate();

                if (_service.IsValid)
                {
                    return Ok(result);
                }

                _service.CopyErrorsToModelState(ModelState);
            }

            return ValidationProblem(ModelState);
        }

        [HttpPost]
        [ProducesResponseType(200)]
        public IActionResult Revoke(RevokeStatusDualApplicationDto dto)
        {
            if (ModelState.IsValid)
            {
                _service.Revoke(dto);

                if (_service.IsValid)
                    return Ok();

                _service.CopyErrorsToModelState(ModelState);
            }

            return ValidationProblem(ModelState);
        }
    }
}