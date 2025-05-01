using Microsoft.AspNetCore.Mvc;
using SspUis.BizLogicLayer;
using SspUis.BizLogicLayer.Memship;
using SspUis.BizLogicLayer.Srv;
using SspUis.Core.Security;
using SspUis.DataLayer.Repositories;
using System.Security.Cryptography.Pkcs;
using WEBASE.AspNet;
using WEBASE.Models;

namespace SspUis.WebApi.Controllers.Srv.Doc
{
    [ApiController]
    [Route("srv/[controller]/[action]")]
    public class ServiceContractController : WebaseController
    {
        private readonly ISrvContractService _service;
        public ServiceContractController(ISrvContractService service)
             : base(AppSettings.Instance.ControllerConfig)
            => _service = service;

        [HttpPost]
        [Authorize(ModuleCode.ServiceContractViewAll)]
        public PagedResult<SrvContractListDto> GetList([FromBody] SrvContractSortFilterOption dto)
        {
            return _service.GetList(dto);
        }

        [Authorize]
        [HttpPost]
        [ProducesResponseType(200)]
        public async ValueTask<IActionResult> SignWebImzo(WebImzoSignedFilter filter)
        {
            if (ModelState.IsValid)
            {
                (string? Url, bool Result) result = await _service.WebImzoSign(filter);

                if (_service.IsValid && result.Result)
                    return Ok(new { status = "success", message = result.Url, result = result.Result });

                _service.CopyErrorsToModelState(ModelState);

                // return Ok(new { status = "error", message = result.Url, result = result.Result });
            }

            return ValidationProblem(ModelState);
        }

        [HttpGet]
        [Authorize(ModuleCode.ServiceContractView)]
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
        [Authorize(ModuleCode.ServiceContractView)]
        [ProducesResponseType(typeof(SrvContractDto), 200)]
        public IActionResult Get(long id)
        {
            var dto = _service.Get(id);

            if (_service.IsValid)
                return Ok(dto);

            _service.CopyErrorsToModelState(ModelState);

            return ValidationProblem(ModelState);
        }

        [Authorize]
        [HttpGet]
        [ProducesResponseType(200)]
        public IActionResult GetByApplicationId(long applicationId)
        {
            var res = _service.GetByApplicationId(applicationId);

            if (_service.IsValid)
                return Ok(res);

            _service.CopyErrorsToModelState(ModelState);

            return ValidationProblem(ModelState);
        }
        [Authorize]
        [HttpPost]
        [ProducesResponseType(typeof(HaveId<long>), 200)]
        public IActionResult CreateAsync(CreateServiceContractDlDto dto)
        {
            var res = _service.Create(dto);

            if (_service.IsValid)
                return Ok(res);

            _service.CopyErrorsToModelState(ModelState);

            return ValidationProblem(ModelState);
        }

        //[HttpPost]
        //[Authorize(ModuleCode.SrvServiceContractSigned)]
        //[ProducesResponseType(200)]
        //public async Task<IActionResult> Signed(SignStatusSrvContractDto dto)
        //{
        //    if (ModelState.IsValid)
        //    {
        //        await _service.Signed(dto);

        //        if (_service.IsValid)
        //            return Ok();

        //        _service.CopyErrorsToModelState(ModelState);
        //    }

        //    return ValidationProblem(ModelState);
        //}
        [HttpPost]
        [Authorize(ModuleCode.SrvServiceContractSigned)]
        [ProducesResponseType(200)]
        public async Task<IActionResult> Signing(SigningStatusSrvContractDto dto)
        {
            if (ModelState.IsValid)
            {
                await _service.Signing(dto);

                if (_service.IsValid)
                    return Ok();

                _service.CopyErrorsToModelState(ModelState);
            }

            return ValidationProblem(ModelState);
        }
        [HttpPost]
        [Authorize(ModuleCode.SrvServiceContractReject)]
        [ProducesResponseType(200)]
        public async ValueTask<IActionResult> Cancel(CancelStatusSrvContractDto dto)
        {
            if (ModelState.IsValid)
            {
                await _service.Cancel(dto);

                if (_service.IsValid)
                    return Ok();

                _service.CopyErrorsToModelState(ModelState);
            }

            return ValidationProblem(ModelState);
        }
        [Authorize]
        [HttpPost]
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
