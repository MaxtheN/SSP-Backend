using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using SspUis.BizLogicLayer.Administration.ContractorContactService;
using SspUis.BizLogicLayer.ContractorServices;
using SspUis.DataLayer.Repositories;
using SspUis.DataLayer;
using WEBASE.Models;
using WEBASE.AspNet;

namespace SspUis.My.WebApi.Controllers.Administration
{
    [Authorize]
    [ApiController]
    [Route("[controller]/[action]")]
    public class ContractorContactController : WebaseController
    {
        private readonly IContractorContactService _service;

        public ContractorContactController(IContractorContactService service)
            : base(AppSettings.Instance.ControllerConfig)
        {
            _service = service;
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

        [HttpGet("{id}")]
        [ProducesResponseType(typeof(ContractorContactDto), 200)]
        public IActionResult Get(int id)
        {
            var dto = _service.Get(id);

            if (_service.IsValid)
                return Ok(dto);

            _service.CopyErrorsToModelState(ModelState);

            return ValidationProblem(ModelState);
        }


        [HttpGet]
        [ProducesResponseType(typeof(ContractorContactDto), 200)]
        public IActionResult Get()
        {
            var dto = _service.Get();

            if (_service.IsValid)
                return Ok(dto);

            _service.CopyErrorsToModelState(ModelState);

            return ValidationProblem(ModelState);
        }

        [HttpPost]
        [ProducesResponseType(typeof(HaveId<int>), 200)]
        public IActionResult Create(CreateContractorContactDlDto dto)
        {
            if (ModelState.IsValid)
            {
                var result = _service.Create(dto);

                if (_service.IsValid)
                    return Ok(result);

                _service.CopyErrorsToModelState(ModelState);
            }

            return ValidationProblem(ModelState);
        }


        [HttpPost]
        [ProducesResponseType(200)]
        public IActionResult Update(UpdateContractorContactDlDto dto)
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

        [HttpPost]
        [ProducesResponseType(typeof(ContractorContactListDto), 200)]
        public List<ContractorContactListDto> GetList()
        {
            List<ContractorContactListDto>? data = _service.GetList();
            return data;
        }
    }
}
