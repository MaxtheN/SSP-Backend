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
using SspUis.BizLogicLayer.NotificationServices;
using SspUis.DataLayer.Repositories;

namespace SspUis.WebApi.Controllers
{
    [Authorize]
    [ApiController]
    [Route("notification/[action]")]
    public class NotificationController : WebaseController
    {
        private INotificationService _service;
        private readonly IAuthService _authService;

        public NotificationController(INotificationService service, IAuthService authService)
            : base(AppSettings.Instance.ControllerConfig)
        {
            _service = service;
            _authService = authService;
        }

        [HttpPost]
        public PagedResult<NotificationListDto> GetList([FromBody] NotificationSortFilterPageOptions dto)
        {
            return _service.GetList(dto);
        }

        [HttpGet("{id}")]
        public IActionResult MarkAsRead(long id)
        {
            var dto = _service.MarkAsRead(id);

            if (_service.IsValid)
            {
                return Ok(dto);
            }

            _service.CopyErrorsToModelState(ModelState);

            return ValidationProblem(ModelState);
        }

        [HttpGet]
        public IActionResult MarkAllAsRead()
        {
            _service.MarkAllAsRead();

            if (_service.IsValid)
            {
                return Ok();
            }

            _service.CopyErrorsToModelState(ModelState);

            return ValidationProblem(ModelState);
        }
    }
}
