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
using SspUis.BizLogicLayer.TagServices;
using SspUis.DataLayer.Repositories;

namespace SspUis.WebApi.Controllers
{
    [Authorize]
    [ApiController]
    [Route("Tag/[action]")]
    public class TagController : WebaseController
    {
        private ITagService _service;

        public TagController(ITagService service)
            :base(AppSettings.Instance.ControllerConfig)
        {
            _service = service;
        }

        [HttpPost]
        public PagedResult<TagListDto> GetList([FromBody] SortFilterPageOptions dto)
        {
            return _service.GetList(dto);
        }

        [HttpGet]
        [ProducesResponseType(typeof(TagDto),200)]
        public IActionResult Get()
        {
            return Ok(_service.Get());
        }

        [HttpGet("{id}")]
        [ProducesResponseType(typeof(TagDto), 200)]
        public IActionResult Get(int id)
        {
            TagDto dto = _service.Get(id);
            
            if(_service.IsValid)
                return Ok(dto);

            _service.CopyErrorsToModelState(ModelState);
            return ValidationProblem(ModelState);
        }

        [HttpGet()]
        [ProducesResponseType(typeof(SelectList<int>), 200)]
        public IActionResult GetAsSelectList()
        {
            return Ok(_service.AsSelectList());
        }

        [HttpPost]
        [ProducesResponseType(typeof(HaveId<int>), 200)]
        public IActionResult Create(CreateTagDlDto dto)
        {
            if (ModelState.IsValid)
            {
                HaveId<int> result =_service.Create(dto);

                if (_service.IsValid)
                    return Ok(result);

                _service.CopyErrorsToModelState(ModelState);
            }
            return ValidationProblem(ModelState);
        }

        [HttpPost]
        [ProducesResponseType(200)]
        public IActionResult Update(UpdateTagDlDto dto)
        {
            if (ModelState.IsValid)
            {
                _service.Update(dto);

                if (_service.IsValid)
                    return Ok();

                _service.CopyErrorsToModelState(ModelState);
            }
            return ValidationProblem(ModelState);
        }

        [HttpPost("{id}")]
        [ProducesResponseType(200)]
        public IActionResult Delete(int id)
        {
            if (ModelState.IsValid)
            {
                _service.Delete(id);

                if (_service.IsValid)
                    return Ok();

                _service.CopyErrorsToModelState(ModelState);
            }
            return ValidationProblem(ModelState);
        }
    }
}
