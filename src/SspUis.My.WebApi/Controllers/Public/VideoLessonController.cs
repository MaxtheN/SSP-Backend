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
using SspUis.BizLogicLayer.VideoLessonServices;
using SspUis.DataLayer.Repositories;
using SspUis.BizLogicLayer.Models;

namespace SspUis.My.WebApi.Controllers
{
    [Authorize]
    [ApiController]
    [Route("videolesson/[action]")]
    public class VideoLessonController : WebaseController
    {
        private IVideoLessonService _service;

        public VideoLessonController(IVideoLessonService service)
            :base(AppSettings.Instance.ControllerConfig)
        {
            _service = service;
        }

        [HttpPost]
        [AllowAnonymous]
        public PagedResult<VideoLessonListDto> GetList([FromBody] TableSortFilterPageOptions dto)
        {
            return _service.GetList(dto);
        }

        [HttpGet("{id}")]
        [ProducesResponseType(typeof(VideoLessonDto), 200)]
        public IActionResult Get(long id)
        {
            VideoLessonDto dto = _service.Get(id);
            
            if(_service.IsValid)
                return Ok(dto);

            _service.CopyErrorsToModelState(ModelState);
            return ValidationProblem(ModelState);
        }

        [HttpGet()]
        [AllowAnonymous]
        [ProducesResponseType(typeof(SelectList<long>), 200)]
        public IActionResult GetAsSelectList(int? categoryId = null)
        {
            return Ok(_service.AsSelectList(categoryId));
        }
    }
}
