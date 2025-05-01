using Microsoft.AspNetCore.Mvc;
using SspUis.BizLogicLayer.Claim;
using WEBASE.Models;

namespace SspUis.WebApi.ClaimApi.Controllers
{
    [Authorize]
    [ApiController]
    [Route("claim/[controller]/[action]")]
    public class ManualController : ControllerBase
    {
        private readonly IClaimManualService _service;

        public ManualController(IClaimManualService service)
        {
            _service = service;
        }

        [HttpGet]
        public SelectList<int> MediationTypeSelectList()
        {
            return _service.MediationTypeSelectList();
        }

        [HttpGet]
        public SelectList<int> ClaimApplicationTypeSelectList(int? langId)
        {
            return _service.ClaimApplicationTypeSelectList(langId);
        }

        [HttpGet]
        public SelectList<int> MediationResultSelectList()
        {
            return _service.MediationResultSelectList();
        }

        [HttpGet]
        public SelectList<int> ClaimNeedCourtSelectList()
        {
            return _service.ClaimNeedCourtSelectList();
        }

        [HttpGet]
        public SelectList<int> ClaimResponsibleTypeSelectList()
        {
            return _service.ClaimResponsibleTypeSelectList(null);
        }
    }
}