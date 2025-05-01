using Microsoft.AspNetCore.Mvc;
using SspUis.BizLogicLayer;
using SspUis.BizLogicLayer.DualContractServices;
using SspUis.DataLayer.Repositories;
using WEBASE.AspNet;
using WEBASE.AspNet.Security;
using WEBASE.Models;

namespace SspUis.WebApi.Controllers;

[Route("DualContract/[controller]/[action]")]
[ApiController]
public class DualContractController : WebaseController
{
    private readonly IDualContractService _service;
    public DualContractController(IDualContractService service)
    {
        _service = service;
    }

    [HttpPost]
    public PagedResult<DualContractListDto> GetList([FromBody] DualContractDtoSortFilterOptions dto)
    {
        return _service.GetList(dto);
    }
    [HttpGet("{id}")]
    [ProducesResponseType(typeof(DualContractDto), 200)]
    public IActionResult Get(int id)
    {
        DualContractDto dto = _service.Get(id);

        if (_service.IsValid)
            return Ok(dto);

        _service.CopyErrorsToModelState(ModelState);

        return ValidationProblem(ModelState);
    }
    [HttpPost]
    [BasicAuth("dual_contract")]
    [ProducesResponseType(typeof(HaveId<long>), 200)]
    public IActionResult Create(CreateDualContractDlDto dto)
    {
        var res = _service.Create(dto);

        if (_service.IsValid)
            return Ok(res);

        _service.CopyErrorsToModelState(ModelState);

        return ValidationProblem(ModelState);
    }

    [HttpPost]
    //[AllowAnonymous]
    [ProducesResponseType(200)]
    public async Task<IActionResult> Sign(UpdateDualContract dto)
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
    [HttpGet]
    [AllowAnonymous]
    [ProducesResponseType(200)]
    public async Task<IActionResult> DownloadPdf(Guid id2, string? lang)
    {
        if (ModelState.IsValid)
        {
            var bytes = await _service.DownloadPdf(id2, lang);

                if (_service.IsValid)
                    return File(bytes, "application/pdf", fileDownloadName: "DualContract.pdf");

            _service.CopyErrorsToModelState(ModelState);
        }
        return ValidationProblem(ModelState);
    }
}