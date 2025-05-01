using Microsoft.AspNetCore.Mvc;
using SspUis.BizLogicLayer.Hrm.TariffScaleCoefServices;
using SspUis.Core.Security;
using SspUis.DataLayer.Repositories;
using WEBASE.AspNet;
using WEBASE.Models;

namespace SspUis.WebApi.Controllers
{
    [Authorize]
    [ApiController]
    [Route("hrm/[controller]/[action]")]
    public class TariffScaleCoefController : WebaseController
    {
        private ITariffScaleCoefService _service;

        public TariffScaleCoefController(ITariffScaleCoefService service)
            : base(AppSettings.Instance.ControllerConfig)
        {
            _service = service;
        }
        [HttpPost]
        [Authorize(ModuleCode.TariffScaleCoefView)]
        public PagedResult<TariffScaleCoefListDto> GetList([FromBody] SortFilterPageOptions dto)
        {
            return _service.GetList(dto);
        }

        [HttpGet]
        [Authorize(ModuleCode.TariffScaleCoefView)]
        [ProducesResponseType(typeof(TariffScaleCoefDto), 200)]
        public IActionResult Get()
        {
            return Ok(_service.Get());
        }

        [HttpGet("{id}")]
        [Authorize(ModuleCode.TariffScaleCoefView)]
        [ProducesResponseType(typeof(TariffScaleCoefDto), 200)]
        public IActionResult Get(int id)
        {
            TariffScaleCoefDto dto = _service.Get(id);

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

        [HttpGet()]
        [ProducesResponseType(typeof(SelectList<int>), 200)]
        public IActionResult GetTableAsSelectList(int? tariffScaleId = null, int? tariffScaleTableId = null)
        {
            return Ok(_service.GetTableAsSelectList(tariffScaleId, tariffScaleTableId));
        }

        [HttpPost]
        [Authorize(ModuleCode.TariffScaleCoefCreate)]
        [ProducesResponseType(typeof(HaveId<int>), 200)]
        public IActionResult Create(CreateTariffScaleCoefDlDto dto)
        {
            if(ModelState.IsValid)
            {
                HaveId<int> result = _service.Create(dto);

                if(_service.IsValid)
                    return Ok(result);

                _service.CopyErrorsToModelState(ModelState);
            }

            return ValidationProblem(ModelState);
        }

        [HttpPost]
        [Authorize(ModuleCode.TariffScaleCoefEdit)]
        [ProducesResponseType(200)]
        public IActionResult Update(UpdateTariffScaleCoefDlDto dto)
        {
            if(ModelState.IsValid)
            {
                _service.Update(dto);

                if(_service.IsValid)
                    return Ok();

                _service.CopyErrorsToModelState(ModelState);
            }

            return ValidationProblem(ModelState);
        }

        [HttpPost("{id}")]
        [Authorize(ModuleCode.TariffScaleCoefDelete)]
        [ProducesResponseType(200)]
        public IActionResult Delete(int id)
        {
            if(ModelState.IsValid)
            {
                _service.Delete(id);

                if(_service.IsValid)
                    return Ok();

                _service.CopyErrorsToModelState(ModelState);
            }

            return ValidationProblem(ModelState);
        }
    }
}
