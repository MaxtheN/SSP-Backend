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
using SspUis.BizLogicLayer.VideoCategoryServices;
using SspUis.DataLayer.Repositories;

namespace SspUis.My.WebApi.Controllers
{
    [Authorize]
    [ApiController]
    [Route("videocategory/[action]")]
    public class VideoCategoryController : WebaseController
    {
        private IVideoCategoryService _service;

        public VideoCategoryController(IVideoCategoryService service)
            :base(AppSettings.Instance.ControllerConfig)
        {
            _service = service;
        }

        [HttpPost]
        public PagedResult<VideoCategoryListDto> GetList([FromBody] SortFilterPageOptions dto)
        {
            return _service.GetList(dto);
        }

        [HttpGet("{id}")]
        [ProducesResponseType(typeof(VideoCategoryDto), 200)]
        public IActionResult Get(int id)
        {
            VideoCategoryDto dto = _service.Get(id);
            
            if(_service.IsValid)
                return Ok(dto);

            _service.CopyErrorsToModelState(ModelState);
            return ValidationProblem(ModelState);
        }

        [HttpGet]
        [AllowAnonymous]
        [ProducesResponseType(typeof(SelectList<int>), 200)]
        public IActionResult GetAsSelectList()
        {
            return Ok(_service.AsSelectList());
        }
    }
}
