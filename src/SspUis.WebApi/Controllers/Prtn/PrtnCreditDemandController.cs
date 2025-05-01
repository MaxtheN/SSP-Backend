using Microsoft.AspNetCore.Mvc;
using SspUis.BizLogicLayer;
using SspUis.BizLogicLayer.PrtnCreditDemandServices;
using SspUis.Core.Security;
using WEBASE.AspNet;
using WEBASE.Models;

namespace SspUis.WebApi.Controllers
{
    [Authorize]
    [ApiController]
    [Route("[controller]/[action]")]
    public class PrtnCreditDemandController : WebaseController
    {
        private IPrtnCreditDemandService _service;

        public PrtnCreditDemandController(
            IPrtnCreditDemandService service)
            : base(AppSettings.Instance.ControllerConfig)
        {
            _service = service;
        }
        [HttpPost]
        [Authorize(ModuleCode.PrtnCreditDemandView)]
        public PagedResult<PrtnCreditDemandListDto> GetList([FromBody] PrtnCreditDemandSortFilterOptions dto)
        {
            return _service.GetList(dto);
        }

        [HttpGet("{id}")]
        [Authorize(ModuleCode.PrtnCreditDemandView)]
        [ProducesResponseType(typeof(PrtnCreditDemandDto), 200)]
        public IActionResult Get(long id)
        {
            var dto = _service.Get(id);

            if (_service.IsValid)
                return Ok(dto);

            _service.CopyErrorsToModelState(ModelState);

            return ValidationProblem(ModelState);
        }

        [HttpGet]
        [ProducesResponseType(typeof(SelectList<long>), 200)]
        public IActionResult GetAsSelectList()
        {
            return Ok(_service.AsSelectList());
        }

        //[HttpGet]
        //[ProducesResponseType(typeof(SelectList<long>), 200)]
        //public IActionResult GetList(SortFilterPageOptions options)
        //{
        //    return Ok(_service.GetList(options));
        //}
        [HttpPost]
        public IActionResult SaveAsExecel(PrtnCreditDemandSortFilterOptions dto)
        {
            if (ModelState.IsValid)
            {
                var file = _service.SaveAsExecel(dto);
                if (_service.IsValid)
                    return File(file, "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", "PrtnCreditDemandTemplate.xlsx");

                _service.CopyErrorsToModelState(ModelState);
            }
            return ValidationProblem(ModelState);
        }
        //[HttpPost]
        //[Authorize(ModuleCode.PrtnCreditDemandCreate)]
        //[ProducesResponseType(typeof(HaveId<long>), 200)]
        //public IActionResult Create(CreatePrtnCreditDemandDlDto dto)
        //{
        //    if (ModelState.IsValid)
        //    {
        //        HaveId<long> result = _service.Create(dto);

        //        if (_service.IsValid)
        //        {
        //            return Ok(result);
        //        }

        //        _service.CopyErrorsToModelState(ModelState);
        //    }

        //    return ValidationProblem(ModelState);
        //}

        //[HttpPost]
        //[Authorize(ModuleCode.PrtnCreditDemandEdit)]
        //[ProducesResponseType(200)]
        //public IActionResult Update(UpdatePrtnCreditDemandDlDto dto)
        //{
        //    if (ModelState.IsValid)
        //    {
        //        _service.Update(dto);

        //        if (_service.IsValid)
        //            return Ok();

        //        _service.CopyErrorsToModelState(ModelState);
        //    }

        //    return ValidationProblem(ModelState);
        //}

        //[HttpPost("{id}")]
        //[Authorize(ModuleCode.PrtnCreditDemandDelete)]
        //[ProducesResponseType(200)]
        //public IActionResult Delete(long id)
        //{
        //    if (ModelState.IsValid)
        //    {
        //        _service.Delete(id);

        //        if (_service.IsValid)
        //            return Ok();

        //        _service.CopyErrorsToModelState(ModelState);
        //    }

        //    return ValidationProblem(ModelState);
        //}
    }
}
