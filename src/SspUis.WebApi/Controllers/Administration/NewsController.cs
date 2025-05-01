using Microsoft.AspNetCore.Mvc;
using SspUis.BizLogicLayer.ApplicationServices;
using SspUis.BizLogicLayer.NewsServices;
using SspUis.Core.Security;
using SspUis.DataLayer.Repositories;
using WEBASE.AspNet;
using WEBASE.Models;
using WEBASE.Storage;
using WEBASE;

namespace SspUis.WebApi.Controllers
{
    [Authorize]
    [ApiController]
    [Route("news/[action]")]
    public class NewsController : WebaseController
    {
        private INewsService _service;
		private IApplicationService _appService;

		public NewsController(INewsService service, IApplicationService appService)
			: base(AppSettings.Instance.ControllerConfig)
		{
			_service = service;
			_appService = appService;
		}

		[HttpPost]
        [Authorize(ModuleCode.NewsView)]
        public PagedResult<NewsListDto> GetList([FromBody] TableSortFilterPageOptions dto)
        {
            return _service.GetList(dto);
        }

        [HttpPost("{tag}")]
        [Authorize(ModuleCode.NewsView)]
        public PagedResult<NewsListDto> GetListByTag(string tag, [FromBody] SortFilterPageOptions dto)
        {
            return _service.GetListByTag(tag, dto);
        }

        [HttpGet]
        [Authorize(ModuleCode.NewsView)]
        [ProducesResponseType(typeof(NewsDto), 200)]
        public IActionResult Get()
        {
            return Ok(_service.Get());
        }

        [HttpGet("{id}")]
        [Authorize(ModuleCode.NewsView)]
        [ProducesResponseType(typeof(NewsDto), 200)]
        public IActionResult Get(int id)
        {
            NewsDto dto = _service.Get(id);

            if (_service.IsValid)
                return Ok(dto);

            _service.CopyErrorsToModelState(ModelState);
            return ValidationProblem(ModelState);
        }

		

		[HttpGet()]
        [ProducesResponseType(typeof(SelectList<int>), 200)]
        public IActionResult GetAsSelectList()
        {
            return Ok(_service.AsSelectList());
        }

        [HttpPost]
        [Authorize(ModuleCode.NewsCreate)]
        [ProducesResponseType(typeof(HaveId<int>), 200)]
        public IActionResult Create(CreateNewsDlDto dto)
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
        [Authorize(ModuleCode.NewsEdit)]
        [ProducesResponseType(200)]
        public IActionResult Update(UpdateNewsDlDto dto)
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
        [Authorize(ModuleCode.NewsDelete)]
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

        [AllowAnonymous]
        [HttpGet("{newsImageId}")]
        public IActionResult GetNewsImage(Guid newsImageId, [FromServices] IMimeMappingService mimeMappingService)
        {
            if (ModelState.IsValid)
            {
                var data = _service.GetNewsImage(newsImageId);

                if (_service.IsValid)
                    return File(data.Value.Item1, mimeMappingService.Map(data.Value.Item2));

                _service.CopyErrorsToModelState(ModelState);
            }

            return ValidationProblem(ModelState);
        }

        [HttpPost]
        [ProducesResponseType(typeof(IStorageFileInfo), 200)]
        public IActionResult UploadNewsImage(IFormFile file)
        {
            if (ModelState.IsValid)
            {
                StorageFile dto = new StorageFile(file.FileName, file.OpenReadStream());
                IStorageFileInfo result = _service.UploadNewsImage(dto);

                if (_service.IsValid)
                    return Ok(result);

                _service.CopyErrorsToModelState(ModelState);
            }

            return ValidationProblem(ModelState);
        }
    }
}
