using Microsoft.AspNetCore.Mvc;
using SspUis.BizLogicLayer.ManualServices;
using SspUis.BizLogicLayer.OrganizationServices;
using WEBASE.AspNet;
using WEBASE.Models;
using WEBASE.Storage;

namespace SspUis.My.WebApi.Controllers;

[ApiController]
[Route("[controller]/[action]")]
public class OrganizationController : WebaseController
{
    private readonly IOrganizationService _service;

    public OrganizationController(IOrganizationService service)
        : base(AppSettings.Instance.ControllerConfig)
    {
        _service = service;
    }
    [HttpPost]
    public List<OrganizationMyPageDto> GetList()
    {
        return _service.GetListForMyPage();
    }


    [HttpGet()]
    public SelectList<int> OrganizationAsSelectListByGroup([FromQuery] int[]? groupId, [FromServices] IManualService service)
    {
        return service.OrganizationAsSelectListByGroup(groupId);
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
}
