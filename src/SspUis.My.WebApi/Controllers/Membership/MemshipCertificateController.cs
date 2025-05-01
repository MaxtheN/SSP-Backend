using Microsoft.AspNetCore.Mvc;
using SspUis.BizLogicLayer;
using WEBASE.AspNet;
using WEBASE.Models;

namespace SspUis.My.WebApi.Controllers
{
    [ApiController]
    [Route("[controller]/[action]")]
    public class MemshipCertificateController : WebaseController
    {
        private readonly IMemshipCertificateService _service;
        public MemshipCertificateController(IMemshipCertificateService _service)
        {
            this._service = _service;
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

        //[HttpGet("status/{id2}")]
        //[AllowAnonymous]
        //public IActionResult GetCertificateStatus(Guid id2)
        //{
        //    var certificate = _unitOfWork.Context.Set<MemshipCertificate>()
        //        .FirstOrDefault(c => c.Id2 == id2);

        //    if (certificate is null)
        //    {
        //        return NotFound(new { message = "Bunday sertifikat mavjud emas!" });
        //    }

        //    return Ok(new
        //    {
        //        Id = certificate.Id2,
        //        Status = certificate.StatusId == StatusIdConst.CANCELED ? "Bekor qilingan" : "Faol",
        //        ExpireOn = certificate.ExpireOn.ToString("dd.MM.yyyy"),
        //        Contractor = certificate.Contractor.FullName
        //    });
        //}

        [HttpGet]
        [AllowAnonymous]
        [ProducesResponseType(200)]
        public IActionResult DownloadPdfByInnPinfl(string innPinfl, string? lang)
        {
            if (ModelState.IsValid)
            {
                var bytes = _service.DownloadPdf(innPinfl, lang);

                if (_service.IsValid)
                    return File(bytes, "application/pdf");

                _service.CopyErrorsToModelState(ModelState);
            }

            return ValidationProblem(ModelState);
        }


        [HttpGet]
        [AllowAnonymous]
        [ProducesResponseType(200)]
        public IActionResult GetByInnPinflForChamber(string innPinfl)
        {
            if (ModelState.IsValid)
            {
                var res = _service.GetByInnPinflForChamber(innPinfl);

                if (_service.IsValid)
                    return Ok(res);

                _service.CopyErrorsToModelState(ModelState);
            }

            return ValidationProblem(ModelState);
        }

        [HttpPost]
        [AllowAnonymous]
        [ProducesResponseType(200)]
        public IActionResult DownloadPdfCopy(MemshipCertificateForPdf dto)
        {
            if (ModelState.IsValid)
            {
                var bytes = _service.DownloadPdf(dto);

                if (_service.IsValid)
                    return File(bytes, "application/pdf");

                _service.CopyErrorsToModelState(ModelState);
            }

            return ValidationProblem(ModelState);
        }
        [HttpPost]
        public PagedResult<MemshipCertificateListDto> GetList([FromBody] MemshipCertificateSortFilterOptions options)
        {
            return _service.GetList(options);
        }

		[HttpPost]
		[Authorize]
		public IActionResult GetCount()
		{
			int result = _service.GetCountMy();
			return Ok(result);
		}

		[HttpGet]
        [ProducesResponseType(typeof(MemshipCertificateDto), 200)]
        public IActionResult Get()
        {
            return Ok(_service.Get());
        }

        [HttpGet("{id}")]
        [ProducesResponseType(typeof(MemshipCertificateDto), 200)]
        public IActionResult Get(long id)
        {
            var dto = _service.Get(id);

            if (_service.IsValid)
                return Ok(dto);

            _service.CopyErrorsToModelState(ModelState);

            return ValidationProblem(ModelState);
        }
    }
}
