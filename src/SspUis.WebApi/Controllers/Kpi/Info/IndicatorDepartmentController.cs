using Microsoft.AspNetCore.Mvc;
using SspUis.BizLogicLayer;
using SspUis.Core.Security;
using SspUis.DataLayer.Repositories;
using WEBASE.AspNet;
using WEBASE.Models;

namespace SspUis.WebApi.Controllers;


[Authorize]
[ApiController]
[Route("kpi/[controller]/[action]")]
public class IndicatorDepartmentController : WebaseController
{
	private IIndicatorDepartmentService _service;

	public IndicatorDepartmentController(IIndicatorDepartmentService service)
		: base(AppSettings.Instance.ControllerConfig)
	{
		_service = service;
	}
	[HttpPost]
	[Authorize(ModuleCode.IndicatorDepartmentView)]
	public PagedResult<IndicatorDepartmentListDto> GetList([FromBody] SortFilterPageOptions dto)
	{
		return _service.GetList(dto);
	}

	[HttpGet]
	[Authorize(ModuleCode.IndicatorDepartmentView)]
	[ProducesResponseType(typeof(IndicatorDepartmentDto), 200)]
	public IActionResult Get()
	{
		return Ok(_service.Get());
	}

	[HttpGet("{id}")]
	[Authorize(ModuleCode.IndicatorDepartmentView)]
	[ProducesResponseType(typeof(IndicatorDepartmentDto), 200)]
	public IActionResult Get(int id)
	{
		IndicatorDepartmentDto dto = _service.GetById(id);

		if (_service.IsValid)
			return Ok(dto);

		_service.CopyErrorsToModelState(ModelState);

		return ValidationProblem(ModelState);
	}

	[HttpGet]
	[ProducesResponseType(typeof(SelectList<int>), 200)]
	public IActionResult GetAsSelectList()
	{
		return Ok(_service.AsSelectList());
	}

	[HttpPost]
	[Authorize(ModuleCode.IndicatorDepartmentCreate)]
	[ProducesResponseType(typeof(HaveId<int>), 200)]
	public IActionResult Create(CreateIndicatorDepartmentDlDto dto)
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
	[Authorize(ModuleCode.IndicatorDepartmentEdit)]
	[ProducesResponseType(200)]
	public IActionResult Update(UpdateIndicatorDepartmentDlDto dto)
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
	[Authorize(ModuleCode.IndicatorDepartmentDelete)]
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
}


