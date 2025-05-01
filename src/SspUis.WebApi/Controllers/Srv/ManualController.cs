using Microsoft.AspNetCore.Mvc;
using SspUis.BizLogicLayer;
using WEBASE.Models;

namespace SspUis.WebApi.Controllers.Srv
{
    [Authorize]
    [ApiController]
    [Route("survey/[controller]/[action]")]
    public class ManualController : ControllerBase
    {
        private readonly ISrvManualService _service;
        public ManualController(ISrvManualService service) => _service = service;

        [HttpGet]
        public SelectList<int> ServicePriceTypeSelectList()
            => _service.ServicePriceTypeSelectList();
    }
}
