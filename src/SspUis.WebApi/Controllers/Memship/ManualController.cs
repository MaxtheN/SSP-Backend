using Microsoft.AspNetCore.Mvc;
using SspUis.BizLogicLayer.Memship;
using WEBASE.Models;

namespace SspUis.WebApi.Memship.Controllers
{
    [Authorize]
    [ApiController]
    [Route("memship/[controller]/[action]")]
    public class ManualController : ControllerBase
    {
        private readonly IMemshipManualService _service;

        public ManualController(IMemshipManualService service)
        {
            _service = service;
        }

        [HttpGet]
        public SelectList<int> ContractorCategorySelectList()
        {
            return _service.ContractorCategorySelectList();
        }

        [HttpGet]
        public SelectList<int> MemshipContractTypeSelectList()
        {
            return _service.MemshipContractTypeSelectList();
        }

        [HttpGet]
        public SelectList<int> OpfSelectList()
        {
            return _service.OpfSelectList();
        }
    }
}
