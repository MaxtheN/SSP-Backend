using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using WEBASE;
using WEBASE.AspNet;
using WEBASE.Models;
using SspUis.Core.Security;
using SspUis.BizLogicLayer.AccountServices;
using SspUis.DataLayer.Repositories;
using SspUis.BizLogicLayer.DashboardServices;

namespace SspUis.WebApi.Controllers
{
    [Authorize]
    [ApiController]
    [Route("[controller]/[action]")]
    public class DashboardController : WebaseController
    {
        private IDashboardService _service;

        public DashboardController(IDashboardService service)
            : base(AppSettings.Instance.ControllerConfig)
        {
            _service = service;
        }


        [HttpGet]
        [Authorize/*(ModuleCode.DashboardView)*/]
        public IActionResult GetDashboardData()
        {
            return Ok(_service.GetDashboardData());
        }
    }
}
