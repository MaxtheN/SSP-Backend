using Microsoft.AspNetCore.Mvc;
using SspUis.BizLogicLayer;
using SspUis.BizLogicLayer.Hrm.NeedChamberServiceServices;
using WEBASE.AspNet;
using WEBASE.Models;
using WEBASE.Storage;

namespace SspUis.My.WebApi.Controllers
{
    [ApiController]
    [Route("[controller]/[action]")]
    public class NeedChamberServiceController : WebaseController
    {
        private readonly INeedChamberServiceService _service;

        public NeedChamberServiceController(INeedChamberServiceService service)
            : base(AppSettings.Instance.ControllerConfig)
            => this._service = service;

        [HttpPost]
        public PagedResult<NeedChamberServiceListDto> GetList([FromBody] NeedChamberServiceSortFilterDto options)
        {
            return _service.GetList(options);
        }
		

		[HttpGet("{id}")]
        [ProducesResponseType(typeof(NeedChamberServiceDto), 200)]
        public IActionResult Get(int id)
        {
            var dto = _service.Get(id);

            if (_service.IsValid)
                return Ok(dto);

            _service.CopyErrorsToModelState(ModelState);

            return ValidationProblem(ModelState);
        }

        [HttpGet]
        [ProducesResponseType(typeof(Dictionary<int, bool>), 200)]
        public IActionResult WihtOfferta()
        {
            return Ok(_service.WithIsOfferta());
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

        [HttpPost]
        public IActionResult GroupingByFreeServices(int? groupId)
            => Ok(_service.GroupingByFreeServices(groupId));
    }
}