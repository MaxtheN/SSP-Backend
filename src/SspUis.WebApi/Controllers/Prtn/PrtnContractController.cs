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
using SspUis.BizLogicLayer.PrtnContractServices;
using SspUis.DataLayer.Repositories;
using SspUis.BizLogicLayer.Doc.PrtnContractServices;
using SspUis.BizLogicLayer.ReportServices;
using SspUis.BizLogicLayer;
using DocumentFormat.OpenXml.ExtendedProperties;
using WEBASE.Storage;

namespace SspUis.WebApi.Controllers
{
    [ApiController]
    [Route("[controller]/[action]")]
    public class PrtnContractController : WebaseController
    {
        private IPrtnContractService _service;
        private readonly IHtmlReportService _htmlReportService;

        public PrtnContractController(IPrtnContractService service,
            IHtmlReportService htmlReportService)
            : base(AppSettings.Instance.ControllerConfig)
        {
            _service = service;
            _htmlReportService = htmlReportService;
        }
        [HttpPost]
        [Authorize(ModuleCode.PrtnContractView, ModuleCode.PrtnContractViewAll)]
        public PagedResult<PrtnContractListDto> GetList([FromBody] PrtnDocumentSortFilterOptions dto)
        {
            return _service.GetList(dto);
        }

        [HttpGet]
        [Authorize(ModuleCode.PrtnContractView, ModuleCode.PrtnContractViewAll)]
        [ProducesResponseType(typeof(PrtnContractDto), 200)]
        public IActionResult Get()
        {
            return Ok(_service.Get());
        }

        [HttpGet("{id}")]
        [Authorize(ModuleCode.PrtnContractView, ModuleCode.PrtnContractViewAll)]
        [ProducesResponseType(typeof(PrtnContractDto), 200)]
        public IActionResult Get(long id)
        {
            PrtnContractDto dto = _service.Get(id);

            if (_service.IsValid)
                return Ok(dto);

            _service.CopyErrorsToModelState(ModelState);

            return ValidationProblem(ModelState);
        }

        [HttpGet("{applicationId}")]
        [Authorize(ModuleCode.PrtnContractCreate)]
        [ProducesResponseType(typeof(PrtnContractDto), 200)]
        public IActionResult GetByApplicationId(long applicationId)
        {
            PrtnContractDto dto = _service.GetByApplicationId(applicationId);

            if (_service.IsValid)
                return Ok(dto);

            _service.CopyErrorsToModelState(ModelState);

            return ValidationProblem(ModelState);
        }

        [HttpGet]
        [Authorize]
        [ProducesResponseType(typeof(SelectList<long>), 200)]
        public IActionResult GetAsSelectList()
        {
            return Ok(_service.AsSelectList());
        }

        [HttpPost]
        [Authorize(ModuleCode.PrtnContractCreate)]
        [ProducesResponseType(typeof(HaveId<long>), 200)]
        public IActionResult Create(CreatePrtnContractDlDto dto)
        {
            if (ModelState.IsValid)
            {
                HaveId<long> result = _service.Create(dto);

                if (_service.IsValid)
                    return Ok(result);

                _service.CopyErrorsToModelState(ModelState);
            }

            return ValidationProblem(ModelState);
        }

        [HttpPost]
        [Authorize(ModuleCode.PrtnContractEdit)]
        [ProducesResponseType(200)]
        public IActionResult Update(UpdatePrtnContractDlDto dto)
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
        [Authorize(ModuleCode.PrtnContractDelete)]
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

        [HttpPost]
        [Authorize(ModuleCode.PrtnContractSign)]
        public async Task<IActionResult> Sign(SignStatusPrtnContractDto dto)
        {
            if (ModelState.IsValid)
            {
                await _service.Sign(dto);

                if (_service.IsValid)
                    return Ok();

                _service.CopyErrorsToModelState(ModelState);
            }

            return ValidationProblem(ModelState);
        }

        [HttpPost]
        [Authorize(ModuleCode.PrtnContractRevoke)]
        public IActionResult Revoke(RevokeStatusPrtnContractDto dto)
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

        [HttpPost]
        [Authorize(ModuleCode.PrtnContractReject)]
        public IActionResult Reject(RejectStatusPrtnContractDto dto)
        {
            if (ModelState.IsValid)
            {
                _service.Reject(dto);

                if (_service.IsValid)
                    return Ok();

                _service.CopyErrorsToModelState(ModelState);
            }

            return ValidationProblem(ModelState);
        }

        [HttpPost]
        [Authorize(ModuleCode.PrtnContractCancel)]
        public IActionResult Cancel(CancelStatusPrtnContractDto dto)
        {
            if (ModelState.IsValid)
            {
                _service.Cancel(dto);

                if (_service.IsValid)
                    return Ok();

                _service.CopyErrorsToModelState(ModelState);
            }

            return ValidationProblem(ModelState);
        }

        [HttpPost]
        [Authorize(ModuleCode.PrtnContractPassExpertise)]
        public async Task<IActionResult> PassExpertise(PassExpertiseStatusPrtnContractDto dto)
        {
            if (ModelState.IsValid)
            {
                await _service.PassExpertise(dto);

                if (_service.IsValid)
                    return Ok();

                _service.CopyErrorsToModelState(ModelState);
            }

            return ValidationProblem(ModelState);
        }

        [HttpPost]
        [Authorize(ModuleCode.PrtnContractNotPassExpertise)]
        public IActionResult NotPassExpertise(NotPassExpertiseStatusPrtnContractDto dto)
        {
            if (ModelState.IsValid)
            {
                _service.NotPassExpertise(dto);

                if (_service.IsValid)
                    return Ok();

                _service.CopyErrorsToModelState(ModelState);
            }

            return ValidationProblem(ModelState);
        }
        [HttpPost]
        [Authorize(ModuleCode.PrtnContractReSendForExpertise)]
        public async Task<IActionResult> ResendExpertise(ResendExpertiseStatusPrtnContractDto dto)
        {
            if (ModelState.IsValid)
            {
                await _service.ResendExpertise(dto);

                if (_service.IsValid)
                    return Ok();

                _service.CopyErrorsToModelState(ModelState);
            }

            return ValidationProblem(ModelState);
        }

        [HttpPost]
        [ServiceFilter(typeof(UploadFileAttribute))]
        [ProducesResponseType(typeof(IEnumerable<IStorageFileInfo>), 200)]
        public IActionResult UploadFile([FromForm] List<IFormFile> files)
        {
            if (ModelState.IsValid)
            {
                StorageFile[] dto = files.Select(a => new StorageFile(a.FileName, a.OpenReadStream())).ToArray();
                var result = _service.UploadFiles(dto);

                if (_service.IsValid)
                    return Ok(result);

                _service.CopyErrorsToModelState(ModelState);
            }

            return ValidationProblem(ModelState);
        }

        [HttpGet("{fileId}")]
        [AllowAnonymous]
        [ProducesResponseType(typeof(IEnumerable<IStorageFileInfo>), 200)]
        public IActionResult DownloadFile(Guid fileId, [FromServices] IMimeMappingService mimeMappingService)
        {
            if (ModelState.IsValid)
            {
                StorageFile file = _service.DownloadFile(fileId);

                if (_service.IsValid)
                    return File(file.GetStream(), mimeMappingService.Map(file.FileName));

                _service.CopyErrorsToModelState(ModelState);
            }

            return ValidationProblem(ModelState);
        }

        [HttpPost("{fileId}")]
        public IActionResult DeleteFile(Guid fileId)
        {
            if (ModelState.IsValid)
            {
                _service.DeleteFile(fileId);

                if (_service.IsValid)
                    return Ok();

                _service.CopyErrorsToModelState(ModelState);
            }

            return ValidationProblem(ModelState);
        }


        [HttpPost]
        [Authorize(ModuleCode.PrtnContractView)]
        public IActionResult GetHtmlTemplate(PrtnContractDto dto)
        {
            if (ModelState.IsValid)
            {
                var res = _service.GetHtmlTemplate(dto);

                if (_service.IsValid)
                    return Ok(res);

                _service.CopyErrorsToModelState(ModelState);
            }

            return ValidationProblem(ModelState);
        }

        [HttpGet]
        [AllowAnonymous]
        public IActionResult GetPdfTemplate(long applicationId)
        {
            if (ModelState.IsValid)
            {
                var bytes = _service.GetPdfTemplate(applicationId);

                if (_service.IsValid)
                    return File(bytes, "application/pdf");

                _service.CopyErrorsToModelState(ModelState);
            }

            return ValidationProblem(ModelState);

        }

        [HttpGet]
        [AllowAnonymous]
        public IActionResult PrintPrtnContractPdf(Guid Id2, string lang = null!)
        {
            if (ModelState.IsValid)
            {
                var bytes = _htmlReportService.DownloadPrtnContractPdf(Id2, lang);

                if (_service.IsValid)
                    return File(bytes, "application/pdf");

                _service.CopyErrorsToModelState(ModelState);
            }
            return ValidationProblem(ModelState);
        }


        [HttpGet]
        [AllowAnonymous]
        public IActionResult GetPrtnContractAsHtml(Guid Id2)
        {
            if (ModelState.IsValid)
            {
                var res = _htmlReportService.DownloadPrtnContractAsHtml(Id2);

                if (_service.IsValid)
                    return Content(res, "text/html");
                _service.CopyErrorsToModelState(ModelState);
            }
            return ValidationProblem(ModelState);
        }
        [HttpPost]
        public IActionResult SaveAsExecel(PrtnDocumentSortFilterOptions dto)
        {
            if (ModelState.IsValid)
            {
                var file = _service.SaveAsExecel(dto);
                if (_service.IsValid)
                    return File(file, "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", "PrtnContractTemplate.xlsx");

                _service.CopyErrorsToModelState(ModelState);
            }
            return ValidationProblem(ModelState);
        }


        [HttpPost]
        public async Task<IActionResult> InsertPrtnContractDataBase()
        {
            if (ModelState.IsValid)
            {
                await _service.InsertPrtnContractDataBase();
                if (_service.IsValid)
                    return Ok();

                _service.CopyErrorsToModelState(ModelState);
            }
            return ValidationProblem(ModelState);
        }

        [HttpPost]
        public async Task<IActionResult> InsertPrtnCertifcatCulumn()
        {
            if (ModelState.IsValid)
            {
                await _service.InsertPrtnCertifcatCulumn();
                if (_service.IsValid)
                    return Ok();

                _service.CopyErrorsToModelState(ModelState);
            }
            return ValidationProblem(ModelState);
        }
    }
}
