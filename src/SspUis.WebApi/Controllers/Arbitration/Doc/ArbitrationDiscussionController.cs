using Microsoft.AspNetCore.Mvc;
using SspUis.BizLogicLayer;
using SspUis.BizLogicLayer.Arbitration.Doc.ArbitrationDiscussionServices.QueryObjects;
using SspUis.Core.Security;
using SspUis.DataLayer.Repositories;
using WEBASE.AspNet;
using WEBASE.Models;
using WEBASE.Storage;

namespace SspUis.WebApi.Controllers.Arbitration.Doc
{
    [Authorize]
    [ApiController]
    [Route("Arbitration/[controller]/[action]")]
    public class ArbitrationDiscussionController : WebaseController
    {
        private readonly IArbitrationDiscussionService _service;

        public ArbitrationDiscussionController(
            IArbitrationDiscussionService arbitrationDiscussionService
            )
        {
            this._service = arbitrationDiscussionService;
        }

        [HttpPost]
        [Authorize(ModuleCode.ArbitrationDiscussionView)]
        public PagedResult<ArbitrationDiscussionListDto> GetList([FromBody] ArbitrationDiscussionSortFilterOptions dto)
        {
            return _service.GetList(dto);
        }


        [HttpGet]
        [Authorize(ModuleCode.ArbitrationDiscussionView)]
        [ProducesResponseType(typeof(ArbitrationDiscussionDto), 200)]
        public async Task<IActionResult> Get()
        {
            var dto = _service.Get();

            if (_service.IsValid)
                return Ok(dto);

            _service.CopyErrorsToModelState(ModelState);

            return ValidationProblem(ModelState);
        }

        [HttpGet("{id}")]
        [Authorize(ModuleCode.ArbitrationDiscussionView)]
        [ProducesResponseType(typeof(ArbitrationDiscussionDto), 200)]
        public IActionResult Get(long id)
        {
            var dto = _service.Get(id);

            if (_service.IsValid)
                return Ok(dto);

            _service.CopyErrorsToModelState(ModelState);

            return ValidationProblem(ModelState);
        }

        [HttpPost]
        [Authorize(ModuleCode.ArbitrationDiscussionDelete)]
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
        [Authorize(ModuleCode.ArbitrationDiscussionCreate)]
        [ProducesResponseType(typeof(HaveId<long>), 200)]
        public IActionResult Create([FromBody] CreateArbitrationDiscussionDlDto dto)
        {
            if (ModelState.IsValid)
            {
                HaveId<long> res = _service.Create(dto);

                if (_service.IsValid)
                    return Ok(res);

                _service.CopyErrorsToModelState(ModelState);
            }

            return ValidationProblem(ModelState);
        }


        [HttpPost]
        [Authorize(ModuleCode.ArbitrationDiscussionEdit)]
        [ProducesResponseType(200)]
        public async Task<IActionResult> Update([FromBody] UpdateArbitrationDiscussionDlDto dto)
        {
            if (ModelState.IsValid)
            {
                var res = await _service.Update(dto);

                if (_service.IsValid)
                    return Ok(res);

                _service.CopyErrorsToModelState(ModelState);
            }

            return ValidationProblem(ModelState);
        }

        //[HttpPost]
        //[Authorize(ModuleCode.ArbitrationDiscussionCancel)]
        //[ProducesResponseType(200)]
        //public IActionResult Cancel([FromBody] CancelStatusArbitrationDiscussionDto dto)
        //{
        //    if (ModelState.IsValid)
        //    {
        //        _service.Cancel(dto);

        //        if (_service.IsValid)
        //            return Ok();

        //        _service.CopyErrorsToModelState(ModelState);
        //    }

        //    return ValidationProblem(ModelState);
        //}


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
                {
                    if (Path.GetExtension(file.FileName) == ".pdf")
                        return File(file.GetStream(), "application/pdf");
                    return File(file.GetStream(), mimeMappingService.Map(file.FileName));
                }
                _service.CopyErrorsToModelState(ModelState);
            }

            return ValidationProblem(ModelState);
        }

        [HttpPost("{fileId}")]
        [Authorize(ModuleCode.ArbitrationDiscussionDelete)]
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

        [HttpGet]
        [AllowAnonymous]
        [ProducesResponseType(200)]
        public async Task<IActionResult> DownloadTemplate()
        {
            if (ModelState.IsValid)
            {
                var bytes = await _service.DownloadTemplate();

                if (_service.IsValid)
                    return File(bytes, "application/vnd.openxmlformats-officedocument.wordprocessingml.document", "DownloadTemp.docx");

                _service.CopyErrorsToModelState(ModelState);
            }

            return ValidationProblem(ModelState);
        }


        [HttpPost]
        [Authorize(ModuleCode.ArbitrationDiscussionDelete)]
        public async Task<IActionResult> Sign([FromBody] SignStatusArbitrationDiscussionDto dto)
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

        [HttpGet("{arbitrationCourtApplicationId}")]
        [Authorize(ModuleCode.ArbitrationDiscussionView)]
        [ProducesResponseType(typeof(ArbitrationDiscussionDto), 200)]
        public async Task<IActionResult> GetByArbitrationCourtApplicationId(int arbitrationCourtApplicationId)
        {
            var dto = await _service.GetByArbitrationCourtApplicationId(arbitrationCourtApplicationId);

            if (_service.IsValid)
                return Ok(dto);

            _service.CopyErrorsToModelState(ModelState);

            return ValidationProblem(ModelState);
        }
    }
}
