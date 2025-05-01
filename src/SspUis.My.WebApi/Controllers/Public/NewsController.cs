using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using WEBASE;
using WEBASE.AspNet;
using WEBASE.Models;
using SspUis.BizLogicLayer.NewsServices;
using SspUis.BizLogicLayer.Models;

namespace SspUis.My.WebApi.Controllers
{
    [ApiController]
    [Route("[controller]/[action]")]
    public class NewsController : WebaseController
    {
        private INewsService _service;

        public NewsController(INewsService service)
            : base(AppSettings.Instance.ControllerConfig)
        {
            _service = service;
        }

        [HttpPost]
        public PagedResult<NewsListDto> GetList([FromBody] TableSortFilterPageOptions dto)
        {
            return _service.GetList(dto);
        }

        [HttpPost("{tag}")]
        public PagedResult<NewsListDto> GetListByTag(string tag, [FromBody] SortFilterPageOptions dto)
        {
            return _service.GetListByTag(tag, dto);
        }

        [HttpGet("{id}")]
        [ProducesResponseType(typeof(NewsViewDto), 200)]
        public IActionResult Get(int id)
        {
            var dto = _service.GetForView(id);

            if (_service.IsValid)
                return Ok(dto);

            _service.CopyErrorsToModelState(ModelState);
            return ValidationProblem(ModelState);
        }

        [HttpGet("{newsImageId}")]
        public IActionResult GetNewsImage(Guid newsImageId, [FromServices] IMimeMappingService mimeMappingService)
        {
            if (ModelState.IsValid)
            {
                var data = _service.GetNewsImage(newsImageId);

                if (_service.IsValid)
                    return File(data.Value.Item1, mimeMappingService.Map(data.Value.Item2));

                _service.CopyErrorsToModelState(ModelState);
            }

            return ValidationProblem(ModelState);
        }

        [HttpGet]
        public IActionResult GetTags()
        {
            if (ModelState.IsValid)
            {
                var res = _service.GetTags();

                if (_service.IsValid)
                    return Ok(res);

                _service.CopyErrorsToModelState(ModelState);
            }

            return ValidationProblem(ModelState);
        }


    }
}
