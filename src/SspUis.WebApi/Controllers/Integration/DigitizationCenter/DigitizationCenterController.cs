using Microsoft.AspNetCore.Mvc;
using SspUis.Integration.DigitizationCenter.Models.GSP;
using SspUis.Integration.DigitizationCenter.Soliq;
using SspUis.BizLogicLayer.DigitizationCenterServices;
using WEBASE.AspNet;
using SspUis.Integration.DigitizationCenter;
using SspUis.Integration.DigitizationCenter.Models.FHDYO;

namespace SspUis.WebApi.Controllers
{
    [Route("[controller]/[action]")]
    [ApiController]
    public class DigitizationCenterController : WebaseController
    {
        private readonly IDigitizationCenterService _service;

        public DigitizationCenterController(IDigitizationCenterService service)
            : base(AppSettings.Instance.ControllerConfig)
        {
            _service = service;
        }

        #region Mehnat
        [HttpPost]
        [ProducesResponseType(200)]
        public async Task<IActionResult> GetMehnatHistory(WorkPositionHistoryRequestDto dto)
        {
            if (ModelState.IsValid)
            {
                var result = await _service.GetMehnatHistory(dto);

                if (_service.IsValid)
                    return Ok(result);

                _service.CopyErrorsToModelState(ModelState);
            }

            return ValidationProblem(ModelState);
        }

        [HttpPost]
        [ProducesResponseType(200)]
        public async Task<IActionResult> GetNumberOfWorkers(NumberOfWorkersRequestDto dto)
        {
            if (ModelState.IsValid)
            {
                var result = await _service.GetWorkersCount(dto);

                if (_service.IsValid)
                    return Ok(result);

                _service.CopyErrorsToModelState(ModelState);
            }

            return ValidationProblem(ModelState);
        }
        #endregion

        #region Soliq
        [HttpPost]
        [ProducesResponseType(200)]
        public async Task<IActionResult> GetFromSoliq(LegalentityDebtRequestDto dto)
        {
            if (ModelState.IsValid)
            {
                var result = await _service.GetLegalentityDebt(dto);

                if (_service.IsValid)
                    return Ok(result);

                _service.CopyErrorsToModelState(ModelState);
            }

            return ValidationProblem(ModelState);
        }
        #endregion

        #region Gsp
        [HttpPost]
        [ProducesResponseType(200)]     
        public async Task<IActionResult> GetFromGSP(GSPNewApiRequestDto dto)
        {
            if (ModelState.IsValid)
            {
                var result = await _service.GetFromGSP(dto);

                if (_service.IsValid)
                    return Ok(result);

                _service.CopyErrorsToModelState(ModelState);
            }

            return ValidationProblem(ModelState);
        }
        #endregion

        #region FHDYO
        [HttpGet]
        [ProducesResponseType(200)]
        public async Task<IActionResult> GetDeathInfoByPinflFromFHDYO(string pinfl)
        {
            if (ModelState.IsValid)
            {
                var result = await _service.GetDeathInfoByPinfl(pinfl);

                if (_service.IsValid)
                    return Ok(result);

                _service.CopyErrorsToModelState(ModelState);
            }

            return ValidationProblem(ModelState);
        }

        [HttpPost]
        [ProducesResponseType(200)]
        public async Task<IActionResult> GetBirthInfoFromFHDYO(BirthInfoRequestDto dto)
        {
            if (ModelState.IsValid)
            {
                var result = await _service.GetBirthInfo(dto);

                if (_service.IsValid)
                    return Ok(result);

                _service.CopyErrorsToModelState(ModelState);
            }

            return ValidationProblem(ModelState);
        }
        #endregion
    }
}
