using Microsoft.AspNetCore.Mvc;
using SspUis.BizLogicLayer;
using SspUis.BizLogicLayer.Hrm.NeedChamberServiceServices;
using SspUis.Core.Security;
using SspUis.DataLayer.Repositories;
using WEBASE.AspNet;
using WEBASE.Models;
using WEBASE.Storage;

namespace SspUis.WebApi.Controllers
{
	[Authorize]
	[ApiController]
	[Route("hrm/[controller]/[action]")]
	public class NeedChamberServiceController : WebaseController
	{
		private INeedChamberServiceService _service;

		public NeedChamberServiceController(INeedChamberServiceService service)
			: base(AppSettings.Instance.ControllerConfig)
		{
			_service = service;
		}
		[HttpPost]
		[AllowAnonymous]
		//[Authorize(ModuleCode.NeedChamberServiceView)]
		public PagedResult<NeedChamberServiceListDto> GetList([FromBody] NeedChamberServiceSortFilterDto dto)
		{
			return _service.GetList(dto);
		}
		[HttpPost]
		[AllowAnonymous]
		//[Authorize(ModuleCode.NeedChamberServiceView)]
		public List<NeedChamberServiceGroupAndChildListDto> GetListGroupAndChild( bool isPaid)
		{
			return _service.GetListGroupAndChild(isPaid);
		}
		[HttpGet]
		[Authorize(ModuleCode.NeedChamberServiceView)]
		[ProducesResponseType(typeof(NeedChamberServiceDto), 200)]
		public IActionResult Get()
		{
			return Ok(_service.Get());
		}

		[HttpGet("{id}")]
		[AllowAnonymous]
		//[Authorize(ModuleCode.NeedChamberServiceView)]
		[ProducesResponseType(typeof(NeedChamberServiceDto), 200)]
		public IActionResult Get(int id)
		{
			NeedChamberServiceDto dto = _service.Get(id);

			if (_service.IsValid)
				return Ok(dto);

			_service.CopyErrorsToModelState(ModelState);

			return ValidationProblem(ModelState);
		}

		[HttpGet]
		[AllowAnonymous]
		[ProducesResponseType(typeof(SelectList<int>), 200)]
		public IActionResult GetAsSelectList(int? groupId)
		{
			return Ok(_service.AsSelectList(groupId));
		}

		[HttpGet]
		[ProducesResponseType(typeof(Dictionary<int, bool>), 200)]
		public IActionResult WihtOfferta()
		{
			return Ok(_service.WithIsOfferta());
		}

		[HttpPost]
		[Authorize(ModuleCode.NeedChamberServiceCreate)]
		[ProducesResponseType(typeof(HaveId<int>), 200)]
		public IActionResult Create(CreateNeedChamberServiceDlDto dto)
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
		[Authorize(ModuleCode.NeedChamberServiceEdit)]
		[ProducesResponseType(200)]
		public IActionResult Update(UpdateNeedChamberServiceDlDto dto)
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
		[Authorize(ModuleCode.NeedChamberServiceDelete)]
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
	}
}
