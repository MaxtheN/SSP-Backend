using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using SspUis.BizLogicLayer.Administration.ContractorContactService;
using SspUis.BizLogicLayer.Administration.ContractorSettlementAccountService;
using SspUis.BizLogicLayer.ContractorServices;
using SspUis.DataLayer.Repositories;
using SspUis.DataLayer;
using WEBASE.AspNet;
using WEBASE.Models;

namespace SspUis.WebApi.Controllers.Administration
{
    [Authorize]
    [ApiController]
    [Route("[controller]/[action]")]
    public class ContractorSettlementAccountController : WebaseController
    {
        private readonly IContractorSettlementAccountService _service;

        public ContractorSettlementAccountController(IContractorSettlementAccountService service)
            :base(AppSettings.Instance.ControllerConfig)
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
        [ProducesResponseType(typeof(ContractorSettlementAccountDto), 200)]
        public IActionResult Get(int id)
        {
            var dto = _service.Get(id);

            if (_service.IsValid)
                return Ok(dto);

            _service.CopyErrorsToModelState(ModelState);

            return ValidationProblem(ModelState);
        }

        [HttpPost]
        [ProducesResponseType(typeof(HaveId<int>), 200)]
        public IActionResult Create(CreateContractorSettlementAccountDlDto dto)
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
        public IActionResult Update(UpdateContractorSettlementAccountDlDtoo dto)
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
        [ProducesResponseType(typeof(ContractorSettlementAccountListDto), 200)]
        public List<ContractorSettlementAccountListDto> GetList()
        {
            var data = _service.GetList();
            return data;
        }


        [HttpPost("{id}")]
        public IActionResult SetMain(long id)
        {
            var data = _service.SetMain(id);
            return Ok(data);
        }
    }
}
