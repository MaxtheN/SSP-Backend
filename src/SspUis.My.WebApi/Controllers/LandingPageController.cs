using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using SspUis.Core.Security;
using SspUis.BizLogicLayer.DashboardServices;

namespace SspUis.My.WebApi.Controllers
{
    [ApiController]
    [Route("[controller]/[action]")]
    public class LandingPageController : WebaseController
    {
        private IDashboardService _service;
        private readonly IAuthService _authService;

        public LandingPageController(IDashboardService service, IAuthService authService)
            : base(AppSettings.Instance.ControllerConfig)
        {
            _service = service;
            _authService = authService;
        }

        [HttpGet]
        public IActionResult GetPageData()
        {
            return Ok(_service.GetLandingPageData());
        }
    }
}
