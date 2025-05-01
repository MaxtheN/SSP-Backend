using Microsoft.AspNetCore.Mvc;
using SspUis.BizLogicLayer.Corruption.ManualServices;
using WEBASE.Models;

namespace SspUis.WebApi.Corruption.Controllers
{
    [Authorize]
    [ApiController]
    [Route("corruption/[controller]/[action]")]
    public class ManualController : ControllerBase
    {
        private readonly ICorruptionManualService _service;

        public ManualController(ICorruptionManualService service)
        {
            _service = service;
        }

        [HttpGet]
        public SelectList<int> CorruptionReviewTypeSelectList()
        {
            return _service.CorruptionReviewTypeSelectList();
        }

        [HttpGet]
        public SelectList<int> JoinAntiCorruptionResultTypeSelectList()
        {
            return _service.JoinAntiCorruptionResultTypeSelectList();
        }
    }
}
