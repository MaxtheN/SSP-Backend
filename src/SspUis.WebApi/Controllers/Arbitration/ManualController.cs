using Microsoft.AspNetCore.Mvc;
using SspUis.BizLogicLayer.Arbitration;
using SspUis.BizLogicLayer.Claim;
using WEBASE.Models;

namespace SspUis.WebApi.Arbitration.Controllers
{
    [Authorize]
    [ApiController]
    [Route("Arbitration/[controller]/[action]")]
    public class ManualController : ControllerBase
    {
        private readonly IArbitrationManualService _service;

        public ManualController(IArbitrationManualService service)
        {
            _service = service;
        }

        [HttpGet]
        public SelectList<int> ArbitrationCourtResultSelectList()
        {
            return _service.ArbitrationCourtResultSelectList();
        }

        [HttpGet]
        public SelectList<int> ArbitrationApplicationTypeSelectList()
        {
            return _service.ArbitrationApplicationTypeSelectList();
        }

        [HttpGet]
        public SelectList<int> ArbitrationCourtSelectList()
        {
            return _service.ArbitrationCourtSelectList();
        }
    }
}
