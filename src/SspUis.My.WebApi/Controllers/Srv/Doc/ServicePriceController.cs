using Microsoft.AspNetCore.Mvc;
using SspUis.BizLogicLayer;
using WEBASE.AspNet;
using WEBASE.Models;

namespace SspUis.My.WebApi.Controllers.Srv.Doc
{
    [Authorize]
    [ApiController]
    [Route("srv/[controller]/[action]")]
    public class ServicePriceController : WebaseController
    {
        private readonly IServicePriceService _service;
        public ServicePriceController(IServicePriceService service)
            : base(AppSettings.Instance.ControllerConfig)
        {
            _service = service;
        }

        [HttpPost]
        [ProducesResponseType(typeof(SelectList<long>), 200)]
        public IActionResult GetAsSelectList(ServicePriceSortFilterOption options)
        {
            return Ok(_service.AsSelectList(options));
        }

        [HttpPost]
        public PagedResult<ServicePriceListDto> GetList([FromBody] ServicePriceSortFilterOption dto)
        {
            return _service.GetList(dto);
        }

        [HttpGet]
        [ProducesResponseType(typeof(ServicePriceDto), 200)]
        public IActionResult Get()
        {
            var dto = _service.Get();

            if (_service.IsValid)
                return Ok(dto);

            _service.CopyErrorsToModelState(ModelState);

            return ValidationProblem(ModelState);
        }

        [HttpGet("{id}")]
        [ProducesResponseType(typeof(ServicePriceDto), 200)]
        public IActionResult Get(long id)
        {
            var dto = _service.Get(id);

            if (_service.IsValid)
                return Ok(dto);

            _service.CopyErrorsToModelState(ModelState);

            return ValidationProblem(ModelState);
        }

        [HttpPost]
        public IActionResult GroupingByServicePrice(GroupingByServicesPriceDtoFilter dto)
        {
            var res = _service.GroupingByServicePrice(dto);

            return Ok(res);
        }
    }
}
