using Microsoft.AspNetCore.Mvc;
using SspUis.BizLogicLayer;
using SspUis.BizLogicLayer.Srv;
using SspUis.Core.Configurations;
using SspUis.DataLayer.Repositories;
using WbImzo.Models;
using WEBASE.AspNet;
using WEBASE.AspNet.Security;
using WEBASE.Models;

namespace SspUis.My.WebApi.Controllers.Srv.Doc
{
    [ApiController]
    [Route("srv/[controller]/[action]")]
    public class ServiceDeedController : WebaseController
    {
        private ISrvDeedService _service;
        private readonly SystemConf _systemConf;
        public ServiceDeedController(ISrvDeedService service, SystemConf systemConf)
            : base(AppSettings.Instance.ControllerConfig)
        {
            _service = service;
            _systemConf = systemConf;
        }

        [HttpPost]
        [Authorize]
        [ProducesResponseType(typeof(SrvDeedListDto), 200)]
        public PagedResult<SrvDeedListDto> GetList([FromBody] SrvDeedSortFilterOption dto)
        {
            return _service.GetList(dto);
        }

        [HttpPost]
        [Authorize]
        public IActionResult GetCount()
        {
		    var count =	_service.GetCount();
            return Ok(count);

        }

        [HttpGet("{id}")]
        [Authorize]
        [ProducesResponseType(typeof(SrvContractDto), 200)]
        public IActionResult Get(long id)
        {
            var dto = _service.Get(id);

            if (_service.IsValid)
                return Ok(dto);

            _service.CopyErrorsToModelState(ModelState);

            return ValidationProblem(ModelState);
        }

        [HttpGet]
        [Authorize]
        [ProducesResponseType(200)]
        public IActionResult GetBySrvContractId(long srvContractId)
        {
            var res = _service.GetBySrvContractId(srvContractId);

            if (_service.IsValid)
                return Ok(res);

            _service.CopyErrorsToModelState(ModelState);

            return ValidationProblem(ModelState);
        }

        [HttpPost]
        [Authorize]
        [ProducesResponseType(200)]
        public async ValueTask<IActionResult> Reject(RejectStatusSrvDeedDto dto)
        {
            if (ModelState.IsValid)
            {
                await _service.Reject(dto);

                if (_service.IsValid)
                    return Ok();

                _service.CopyErrorsToModelState(ModelState);
            }

            return ValidationProblem(ModelState);
        }

        //[HttpPost]
        //[Authorize]
        //[ProducesResponseType(200)]
        //public async Task<IActionResult> Signing(SigningStatusSrvContractDto dto)
        //{
        //    if (ModelState.IsValid)
        //    {
        //        await _service.Signing(dto);

        //        if (_service.IsValid)
        //            return Ok();

        //        _service.CopyErrorsToModelState(ModelState);
        //    }

        //    return ValidationProblem(ModelState);
        //}

        [Authorize]
        [HttpPost]
        [ProducesResponseType(200)]
        public async ValueTask<IActionResult> SignWebImzo(WebImzoSignedFilter filter)
        {
            if (ModelState.IsValid)
            {
                (string? Url, bool Result) result = await _service.WebImzoSign(filter);

                if (_service.IsValid)
                    return Ok(new { status = "success", message = result.Url });

                _service.CopyErrorsToModelState(ModelState);
            }

            return ValidationProblem(ModelState);
        }

        [HttpPost]
        [Authorize]
        [ProducesResponseType(200)]
        public async Task<IActionResult> Signed(SignStatusSrvDeedDto dto)
        {
            if (ModelState.IsValid)
            {
                await _service.Signed(dto);

                if (_service.IsValid)
                    return Ok();

                _service.CopyErrorsToModelState(ModelState);
            }

            return ValidationProblem(ModelState);
        }
        [HttpGet]
        [AllowAnonymous]
        [ProducesResponseType(200)]
        public async Task<IActionResult> DownloadPdf(Guid id2, string? lang)
        {
            if (ModelState.IsValid)
            {
                var bytes =  _service.DownloadPdf(id2, lang);

                if (_service.IsValid)
                    return File(bytes, "application/pdf");

                _service.CopyErrorsToModelState(ModelState);
            }

            return ValidationProblem(ModelState);
        }
    }
}