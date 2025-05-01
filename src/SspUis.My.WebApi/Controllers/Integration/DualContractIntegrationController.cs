using Microsoft.AspNetCore.Mvc;
using SspUis.BizLogicLayer;
using SspUis.DataLayer.Repositories;
using SspUis.Integration.Billing.Services;
using WEBASE.AspNet;
using WEBASE.AspNet.Security;
using WEBASE.Models;

namespace SspUis.My.WebApi.Controllers.Integration
{
    [ApiController]
    [Route("Dual/[controller]/[action]")]
    public class DualContractIntegrationController : WebaseController
    {
        private readonly IDualContractService _service;
        private readonly IBillingService _billingService;

        public DualContractIntegrationController(IDualContractService dualContractService, IBillingService billingService)
        {
            _service = dualContractService;
            _billingService = billingService;
        }

        /// <summary>
        /// Billing tizimidagilar 
        /// </summary> Billing tizimidagilar bizga DualConract create qiladi
        /// <param name="dto"></param>
        /// <returns></returns>
        [HttpPost]
        [BasicAuth("dual_contract")]
        [ProducesResponseType(typeof(HaveId<long>), 200)]
        public IActionResult Create(CreateDualContractDlDto dto)
        {
            var res = _service.Create(dto);

            if (_service.IsValid)
                return Ok(res);

            _service.CopyErrorsToModelState(ModelState);

            return ValidationProblem(ModelState);
        }

        /// <summary>
        /// Billing tizimidan shartnomani pdf ko'rinishida o'qib olish
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpGet("{id}")]
        [ProducesResponseType(200)]
        public async Task<IActionResult> DownloadContract(string id)
        {
            if (ModelState.IsValid)
            {
                var bytes = await _billingService.DownloadContract(Guid.Parse(id));

                if (_billingService.IsValid)
                    return File(bytes, "application/pdf");

                _service.CopyErrorsToModelState(ModelState);
            }

            return ValidationProblem(ModelState);
        }

        /// <summary>
        /// Billing tizimidagilar shartnoma holatini kuzatish uchun API
        /// </summary>
        /// <param name="pinfl"></param>
        /// <returns></returns>
        [HttpGet]
        [BasicAuth("dual_contract")]
        [ProducesResponseType(typeof(bool), 200)]
        public async Task<IActionResult> Get(string pinfl)
        {
            var dto = _service.GetStatus(pinfl);

            if (_service.IsValid)
                return Ok(dto);

            _service.CopyErrorsToModelState(ModelState);

            return ValidationProblem(ModelState);
        }
    }
}