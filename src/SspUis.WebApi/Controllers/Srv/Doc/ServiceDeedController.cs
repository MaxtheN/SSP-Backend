using Microsoft.AspNetCore.Mvc;
using SspUis.BizLogicLayer;
using SspUis.BizLogicLayer.Srv;
using SspUis.Core.Security;
using SspUis.DataLayer.Repositories;
using WEBASE.AspNet;
using WEBASE.Models;

namespace SspUis.WebApi.Controllers.Srv.Doc
{
    [ApiController]
    [Route("srv/[controller]/[action]")]
    public class ServiceDeedController : WebaseController
    {
        private readonly ISrvDeedService _service;
        public ServiceDeedController(ISrvDeedService service)
             : base(AppSettings.Instance.ControllerConfig)
            => _service = service;

        [HttpPost]
        [Authorize(ModuleCode.ServiceDeedViewAll)]
        public PagedResult<SrvDeedListDto> GetList([FromBody] SrvDeedSortFilterOption dto)
        {
            return _service.GetList(dto);
        }

        [HttpGet]
        [Authorize(ModuleCode.ServiceDeedView)]
        [ProducesResponseType(typeof(SrvDeedDto), 200)]
        public IActionResult Get()
        {
            var dto = _service.Get();

            if (_service.IsValid)
                return Ok(dto);

            _service.CopyErrorsToModelState(ModelState);

            return ValidationProblem(ModelState);
        }

        [HttpGet("{id}")]
        [Authorize(ModuleCode.ServiceDeedView)]
        [ProducesResponseType(typeof(SrvDeedDto), 200)]
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
        //[Authorize(ModuleCode.ServiceDeedView)]
        [ProducesResponseType(200)]
        public IActionResult GetBySrvContractId(long srvContractId)
        {
            var res = _service.GetBySrvContractId(srvContractId);

            if (_service.IsValid)
                return Ok(res);

            _service.CopyErrorsToModelState(ModelState);

            return ValidationProblem(ModelState);
        }

        [Authorize]
        [HttpPost]
        [ProducesResponseType(typeof(HaveId<long>), 200)]
        public IActionResult CreateAsync(CreateServiceDeedDlDto dto)
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


        [HttpPost]
        [Authorize(ModuleCode.SrvServiceDeedSigned)]
        [ProducesResponseType(200)]
        public async Task<IActionResult> Signing(SigningStatusSrvDeedDto dto)
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
        [Authorize(ModuleCode.SrvServiceDeedReject)]
        [ProducesResponseType(200)]
        public async ValueTask<IActionResult> Cancel(CancelStatusSrvDeedDto dto)
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
        public IActionResult Update(UpdateServiceDeedDlDto dto)
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
        public ActionResult DownloadPdf(Guid id2, string? lang)
        {
            if (ModelState.IsValid)
            {
                var bytes = _service.DownloadPdf(id2, lang);
                //var bytes = _service.DownloadPdfOld(id2, lang);

                if (_service.IsValid)
                    return File(bytes, "application/pdf");

                _service.CopyErrorsToModelState(ModelState);
            }

            return ValidationProblem(ModelState);
        }
        [Authorize]
        [HttpPost]
        public IActionResult SaveAsExcel(SrvDeedSortFilterOption dto)
        {
            if (ModelState.IsValid)
            {
                var file = _service.SaveAsExcel(dto);
                if (_service.IsValid)
                    return File(file, "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", "ServiceDeed.xlsx");

                _service.CopyErrorsToModelState(ModelState);
            }
            return ValidationProblem(ModelState);
        }
    }
}
