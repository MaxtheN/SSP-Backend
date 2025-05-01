using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using SspUis.BizLogicLayer.Administration.ContractorServices;
using SspUis.BizLogicLayer.ContractorServices;
using SspUis.BizLogicLayer.IntegrationServices.Bojxona;
using SspUis.BizLogicLayer.IntegrationServices.Finance.Concrete;
using SspUis.Integration.Finance.Services;
using WEBASE.AspNet;
using WEBASE.Models;
using WEBASE.AspNet.Security;
using SspUis.BizLogicLayer.IntegrationServices.Finance;
using SspUis.Core.Security;
using SspUis.BizLogicLayer;

namespace SspUis.WebApi.Controllers.Integration
{
    [ApiController]
    [Route("financeIntegration/[action]")]
    public class FinanceIntegrationController : WebaseController
    {
        private readonly IFinanceIntegrationService _service;
        private readonly IFinanceService _financeService;
        private readonly IContractorService _contractorService;

        public FinanceIntegrationController(IFinanceIntegrationService service,IContractorService contractorService, IFinanceService financeService)
        {
            _service = service;
            _contractorService = contractorService;
            _financeService = financeService;
        }


        [Authorize(ModuleCode.MemshipPaymentOrderView)]
        [HttpPost]
        [ProducesResponseType(200)]
        public IActionResult GetPayDocsByAccEqualsAndBankDateBetween([FromBody] FinancePayDocsByAccSortFilterPageOptions options)
        {
            if (ModelState.IsValid)
            {
                var result = _service.GetFinancePayDocsByAccList(options);

                if (_service.IsValid)
                    return Ok(result);

                _service.CopyErrorsToModelState(ModelState);
            }

            return ValidationProblem(ModelState);
        }

        //[Authorize(ModuleCode.MemshipPaymentOrderView)]
        [HttpPost]
        [ProducesResponseType(200)]
        public IActionResult GetPayDocsByAccEqualsAndBankDateBetweenTest()
        {
            if (ModelState.IsValid)
            {
                var result = _financeService.GetPayDocsAsync("20212000003781497001","05042024");

                if (_service.IsValid)
                    return Ok(result);

                _service.CopyErrorsToModelState(ModelState);
            }

            return ValidationProblem(ModelState);
        }



        [Authorize(ModuleCode.MemshipPaymentOrderView)]
        [HttpPost]
        [ProducesResponseType(200)]
        public IActionResult GetFinancePaymentByInn([FromBody] FinancePaymentByInnDtoSortFilterPageOptions options)
        {
            if (ModelState.IsValid)
            {
                var result = _service.GetFinancePaymentByInn(options);

                if (_service.IsValid)
                    return Ok(result);

                _service.CopyErrorsToModelState(ModelState);
            }

            return ValidationProblem(ModelState);
        }


    }
}
