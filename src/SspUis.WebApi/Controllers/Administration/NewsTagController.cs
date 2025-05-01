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
using SspUis.BizLogicLayer.NewsTagServices;
using SspUis.DataLayer.Repositories;

namespace SspUis.WebApi.Controllers
{
    [Authorize]
    [ApiController]
    [Route("NewsTag/[action]")]
    public class NewsTagController : WebaseController
    {
        private INewsTagService _service;

        public NewsTagController(INewsTagService service)
            :base(AppSettings.Instance.ControllerConfig)
        {
            _service = service;
        }

        [HttpPost]
        [Authorize(ModuleCode.NewsTagView)]
        public PagedResult<NewsTagListDto> GetList([FromBody] SortFilterPageOptions dto)
        {
            return _service.GetList(dto);
        }

        [HttpGet]
        [Authorize(ModuleCode.NewsTagView)]
        [ProducesResponseType(typeof(NewsTagDto),200)]
        public IActionResult Get()
        {
            return Ok(_service.Get());
        }

        [HttpGet("{id}")]
        [Authorize(ModuleCode.NewsTagView)]
        [ProducesResponseType(typeof(NewsTagDto), 200)]
        public IActionResult Get(int id)
        {
            NewsTagDto dto = _service.Get(id);
            
            if(_service.IsValid)
                return Ok(dto);

            _service.CopyErrorsToModelState(ModelState);
            return ValidationProblem(ModelState);
        }

        [HttpGet()]
        [Authorize(ModuleCode.NewsTagView)]
        [ProducesResponseType(typeof(SelectList<int>), 200)]
        public IActionResult GetAsSelectList()
        {
            return Ok(_service.AsSelectList());
        }

        [HttpPost]
        [Authorize(ModuleCode.NewsTagCreate)]
        [ProducesResponseType(typeof(HaveId<int>), 200)]
        public IActionResult Create(CreateNewsTagDlDto dto)
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
        [Authorize(ModuleCode.NewsTagEdit)]
        public IActionResult Update(UpdateNewsTagDlDto dto)
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
        [Authorize(ModuleCode.NewsTagDelete)]
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
