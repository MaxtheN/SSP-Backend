using Microsoft.AspNetCore.Mvc;
using SspUis.BizLogicLayer;
using SspUis.BizLogicLayer.PrtnCreditDemandServices;
using SspUis.Core.Configurations;
using SspUis.Core.Security;
using SspUis.DataLayer.Repositories;
using WEBASE.AspNet;
using WEBASE.Models;
using WEBASE.Storage;

namespace SspUis.My.WebApi.Controllers
{
    [Authorize]
    [ApiController]
    [Route("[controller]/[action]")]
    public class PrtnCreditDemandController : WebaseController
    {
        private IPrtnCreditDemandService _service;
        private readonly SystemConf _systemConf;

        public PrtnCreditDemandController(IPrtnCreditDemandService service, SystemConf systemConf)
            : base(AppSettings.Instance.ControllerConfig)
        {
            _service = service;
            _systemConf = systemConf;
        }
        [HttpPost]
        public PagedResult<PrtnCreditDemandListDto> GetList([FromBody] PrtnCreditDemandSortFilterOptions dto)
        {
            return _service.GetList(dto);
        }

        [HttpPost]
        public IActionResult GetCount()
        {
            var data = _service.GetCount();
            return Ok(data);
        }

        [HttpGet]
        [Authorize]
        [ProducesResponseType(typeof(PrtnCreditDemandDto), 200)]
        public async Task<IActionResult> Get()
        {
            var dto = _service.Get();

            if (_service.IsValid)
                return Ok(dto);

            _service.CopyErrorsToModelState(ModelState);

            return ValidationProblem(ModelState);
        }

        [HttpGet("{id}")]
        [Authorize]
        [ProducesResponseType(typeof(PrtnCreditDemandDto), 200)]
        public IActionResult Get(long id)
        {
            var dto = _service.Get(id);

            if (_service.IsValid)
                return Ok(dto);

            _service.CopyErrorsToModelState(ModelState);

            return ValidationProblem(ModelState);
        }

        [HttpPost]
        [Authorize]
        [ProducesResponseType(typeof(HaveId<long>), 200)]
        public IActionResult Create(CreatePrtnCreditDemandDlDto dto)
        {
            var result = _service.Create(dto);

            if (_service.IsValid)
                return Ok(result);

            _service.CopyErrorsToModelState(ModelState);

            return ValidationProblem(ModelState);
        }

        [HttpPost]
        [Authorize]
        [ProducesResponseType(200)]
        public IActionResult Update(UpdatePrtnCreditDemandDlDto dto)
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

        [Authorize]
        [HttpPost("{id}")]
        [ProducesResponseType(200)]
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

        //[HttpGet]
        //[Authorize]
        //[ProducesResponseType(typeof(Boolean), 200)]
        //public IActionResult CanCreate()
        //{
        //    if (ModelState.IsValid)
        //    {
        //        var result = _service.CanCreate();

        //        if (_service.IsValid)
        //        {
        //            return Ok(result);
        //        }

        //        _service.CopyErrorsToModelState(ModelState);
        //    }

        //    return ValidationProblem(ModelState);
        //}
    }
}
