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

namespace SspUis.WebApi.Controllers
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

        [HttpGet]
        [Authorize(ModuleCode.VideoCategoryView)]
        [ProducesResponseType(typeof(VideoCategoryDto),200)]
        public IActionResult Get()
        {
            return Ok(_service.Get());
        }

        [HttpGet("{id}")]
        [Authorize(ModuleCode.VideoCategoryView)]
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
        [ProducesResponseType(typeof(SelectList<int>), 200)]
        public IActionResult GetAsSelectList()
        {
            return Ok(_service.AsSelectList());
        }

        [HttpPost]
        [Authorize(ModuleCode.VideoCategoryCreate)]
        [ProducesResponseType(typeof(HaveId<int>), 200)]
        public IActionResult Create(CreateVideoCategoryDlDto dto)
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
        [Authorize(ModuleCode.VideoCategoryEdit)]
        [ProducesResponseType(200)]
        public IActionResult Update(UpdateVideoCategoryDlDto dto)
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
        [Authorize(ModuleCode.VideoCategoryDelete)]
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
