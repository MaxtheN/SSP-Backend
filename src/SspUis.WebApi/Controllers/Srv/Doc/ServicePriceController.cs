using Microsoft.AspNetCore.Mvc;
using SspUis.BizLogicLayer;
using SspUis.Core.Security;
using SspUis.DataLayer.Repositories;
using WEBASE.AspNet;
using WEBASE.Models;

namespace SspUis.WebApi.Controllers.Srv
{
    [ApiController]
    [Authorize]
    [Route("srv/[controller]/[action]")]
    public class ServicePriceController : WebaseController
    {
        private readonly IServicePriceService _service;
        public ServicePriceController(IServicePriceService service)
        : base(AppSettings.Instance.ControllerConfig)
        {
            this._service = service;
        }

        [HttpPost]
        [Authorize(ModuleCode.SrvServicePriceViewAll)]
        public PagedResult<ServicePriceListDto> GetList([FromBody] ServicePriceSortFilterOption options)
        {
            return _service.GetList(options);
        }

        [HttpGet]
        [Authorize(ModuleCode.SrvServicePriceView)]
        [ProducesResponseType(typeof(ServicePriceDto), 200)]
        public IActionResult Get()
        {
            return Ok(_service.Get());
        }

        [HttpGet("{id}")]
        [Authorize(ModuleCode.SrvServicePriceView)]
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

        [HttpGet("{id}")]
        [Authorize(ModuleCode.SrvServicePriceView)]
        [ProducesResponseType(typeof(ServicePriceDto), 200)]
        public IActionResult Clone(long id)
        {
            return Ok(_service.CloneServicePrice(id));
        }

        [HttpPost]
        [Authorize(ModuleCode.SrvServicePriceCreate)]
        [ProducesResponseType(typeof(HaveId<long>), 200)]
        public IActionResult Create(CreateServicePriceDlDto dto)
        {
            if (ModelState.IsValid)
            {
                HaveId<long> result = _service.Create(dto);

                if (_service.IsValid)
                    return Ok(result);

                _service.CopyErrorsToModelState(ModelState);
            }

            return ValidationProblem(ModelState);
        }

        [HttpPost]
        [Authorize(ModuleCode.SrvServicePriceEdit)]
        [ProducesResponseType(200)]
        public IActionResult Update(UpdateServicePriceDlDto dto)
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
        [Authorize(ModuleCode.SrvServicePriceDelete)]
        [ProducesResponseType(200)]
        public IActionResult Delete(long id)
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

        [HttpPost]
        [Authorize(ModuleCode.SrvServicePriceAccept)]
        [ProducesResponseType(200)]
        public IActionResult Accept(AcceptStatusSrvPriceDto dto)
        {
            if (ModelState.IsValid)
            {
                var res = _service.Accept(dto);

                if (_service.IsValid)
                    return Ok(res);

                _service.CopyErrorsToModelState(ModelState);
            }

            return ValidationProblem(ModelState);
        }

        [HttpPost]
        [Authorize(ModuleCode.SrvServicePriceCancel)]
        [ProducesResponseType(200)]
        public IActionResult Cancel(CancelStatusSrvPriceDto dto)
        {
            if (ModelState.IsValid)
            {
                var res = _service.Cancel(dto);

                if (_service.IsValid)
                    return Ok(res);

                _service.CopyErrorsToModelState(ModelState);
            }

            return ValidationProblem(ModelState);
        }
    }
}