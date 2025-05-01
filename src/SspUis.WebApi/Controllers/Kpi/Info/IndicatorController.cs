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
public class IndicatorController : WebaseController
{
	private IIndicatorService _service;

	public IndicatorController(IIndicatorService service)
		: base(AppSettings.Instance.ControllerConfig)
	{
		_service = service;
	}
	[HttpPost]
	[Authorize(ModuleCode.IndicatorView)]
	public PagedResult<IndicatorListDto> GetList([FromBody] SortFilterPageOptions dto)
	{
		return _service.GetList(dto);
	}

	[HttpGet]
	[Authorize(ModuleCode.IndicatorView)]
	[ProducesResponseType(typeof(IndicatorDto), 200)]
	public IActionResult Get()
	{
		return Ok(_service.Get());
	}

	[HttpGet("{id}")]
	[Authorize(ModuleCode.IndicatorView)]
	[ProducesResponseType(typeof(IndicatorDto), 200)]
	public IActionResult Get(int id)
	{
		IndicatorDto dto = _service.GetById(id);

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
	[Authorize(ModuleCode.IndicatorCreate)]
	[ProducesResponseType(typeof(HaveId<int>), 200)]
	public IActionResult Create(CreateIndicatorDlDto dto)
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
	[Authorize(ModuleCode.IndicatorEdit)]
	[ProducesResponseType(200)]
	public IActionResult Update(UpdateIndicatorDlDto dto)
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
	[Authorize(ModuleCode.IndicatorDelete)]
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


