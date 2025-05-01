using Microsoft.AspNetCore.Mvc;
using SspUis.BizLogicLayer;
using SspUis.BizLogicLayer.Memship;
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
    public class ServiceContractController : WebaseController
    {
        private ISrvContractService _service;
        private readonly SystemConf _systemConf;
        public ServiceContractController(ISrvContractService service, SystemConf systemConf)
            : base(AppSettings.Instance.ControllerConfig)
        {
            _service = service;
            _systemConf = systemConf;
        }
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
		public IActionResult GetCount()
		{
			int result = _service.GetCount();
			return Ok(result);
		}
		[HttpPost]
        [Authorize]
        [ProducesResponseType(typeof(SrvContractListDto), 200)]
        public PagedResult<SrvContractListDto> GetList([FromBody] SrvContractSortFilterOption dto)
        {
            return _service.GetList(dto);
        }

        [HttpGet]
        [Authorize]
        [ProducesResponseType(typeof(SrvContractDto), 200)]
        public IActionResult Get()
        {
            var dto = _service.Get();

            if (_service.IsValid)
                return Ok(dto);

            _service.CopyErrorsToModelState(ModelState);

            return ValidationProblem(ModelState);
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
        public IActionResult GetByApplicationId(long applicationId)
        {
            var res = _service.GetByApplicationId(applicationId);

            if (_service.IsValid)
                return Ok(res);

            _service.CopyErrorsToModelState(ModelState);

            return ValidationProblem(ModelState);
        }

        [HttpPost]
        [AllowAnonymous]
        //[Authorize]
        [ProducesResponseType(typeof(HaveId<long>), 200)]
        public IActionResult CreateAsync(CreateServiceContractDlDto dto)
        {
            var res = _service.Create(dto);

            if (_service.IsValid)
                return Ok(res);

            _service.CopyErrorsToModelState(ModelState);

            return ValidationProblem(ModelState);
        }

        [HttpPost]
        [Authorize]
        [ProducesResponseType(200)]
        public IActionResult Update(UpdateServiceContractDlDto dto)
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
        [Authorize]
        [ProducesResponseType(200)]
        public async ValueTask<IActionResult> Reject(RejectStatusSrvContractDto dto)
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
        [HttpPost]
        [Authorize]
        [ProducesResponseType(200)]
        public async Task<IActionResult> Signed(SignStatusSrvContractDto dto)
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
    }
}