using Microsoft.AspNetCore.Mvc;
using SspUis.BizLogicLayer.Administration.ContractorServices;
using SspUis.BizLogicLayer.ContractorServices;
using SspUis.BizLogicLayer.Models;
using SspUis.Core.Security;
using SspUis.DataLayer.Repositories;
using SspUis.Integration.Bandlik.Models;
using SspUis.Integration.BankCredit.Models;
using SspUis.Integration.Bojxona.Models;
using SspUis.Integration.DavAktiv;
using SspUis.Integration.Investitsiya.Models;
using SspUis.Integration.MarkaziyBank;
using SspUis.Integration.Soliq.Models;
using SspUis.Integration.TadbirkorFund;
using WEBASE.AspNet;
using WEBASE.Models;
using WEBASE;

namespace SspUis.WebApi.Controllers
{
    [Authorize]
    [ApiController]
    [Route("[controller]/[action]")]
    public class ContractorController : WebaseController
    {
        private IContractorService _service;

        public ContractorController(IContractorService service)
            : base(AppSettings.Instance.ControllerConfig)
        {
            _service = service;
        }

        [HttpGet("{inn}")]
        [ProducesResponseType(typeof(ContractorDto), 200)]
        public async Task<IActionResult> GetByInn(string inn)
        {
            var dto = await _service.GetByInn(inn);

            if (_service.IsValid)
                return Ok(dto);

            _service.CopyErrorsToModelState(ModelState);

            return ValidationProblem(ModelState);
        }


        [HttpPost]
        [Authorize(ModuleCode.ContractorView)]
        public PagedResult<ContractorListDto> GetList([FromBody] TableSortFilterPageOptions dto)
        {
            return _service.GetList(dto);
        }
        [HttpPost]
        public PagedResult<ContractorListDto> GetListForEmloyee([FromBody] TableSortFilterPageOptions dto)
        {
            return _service.GetListForEmloyee(dto);
        }
        [HttpPost]
        [ProducesResponseType(typeof(SelectList<long>), 200)]
        public IActionResult GetAsSelectList()
        {
            return Ok(_service.AsSelectList());
        }

        [HttpPost]
        [ProducesResponseType(typeof(PagedSelectList<long>), 200)]
        public IActionResult GetAsPagedSelectList(ContractorPagedSelectListOptions options)
        {
            return Ok(_service.AsPagedSelectList(options));
        }
        [HttpGet("{id}")]
        [Authorize(ModuleCode.ContractorView)]
        [ProducesResponseType(typeof(ContractorDto), 200)]
        public IActionResult Get(int id)
        {
            if (ModelState.IsValid)
            {
                var dto = _service.Get(id);

                if (_service.IsValid)
                    return Ok(dto);

                _service.CopyErrorsToModelState(ModelState);
            }

            return ValidationProblem(ModelState);
        }

        [HttpPost]
        [ProducesResponseType(200)]
        public IActionResult ImportNotBudgetContractorInn(List<ImportNotBudgetNotBudgetContractorInnDlDto> listDto)
        {
            if (ModelState.IsValid)
            {
                _service.ImportNotBudgetContractorInn(listDto);

                if (_service.IsValid)
                    return Ok();

                _service.CopyErrorsToModelState(ModelState);
            }

            return ValidationProblem(ModelState);
        }


        [HttpPost]
        [Authorize(ModuleCode.ContractorEdit)]
        [ProducesResponseType(200)]
        public async Task<IActionResult> UpdateAllBySoliq()
        {
            if (ModelState.IsValid)
            {
                await _service.UpdateAllFromSoliq();

                if (_service.IsValid)
                    return Ok();

                _service.CopyErrorsToModelState(ModelState);
            }

            return ValidationProblem(ModelState);
        }

        [HttpPost]
        [Authorize(ModuleCode.ContractorEdit)]
        [ProducesResponseType(200)]
        public async Task<IActionResult> UpdateAllFromDavAktiv()
        {
            if (ModelState.IsValid)
            {
                await _service.UpdateAllFromDavAktiv();

                if (_service.IsValid)
                    return Ok();

                _service.CopyErrorsToModelState(ModelState);
            }

            return ValidationProblem(ModelState);
        }


        [HttpGet("{inn}")]
        [ProducesResponseType(typeof(SoliqContractorByTinDto), 200)]
        public async Task<IActionResult> GetFromSoliq(string inn)
        {
            var dto = await _service.GetFromSoliq(inn);

            if (_service.IsValid)
                return Ok(dto);

            _service.CopyErrorsToModelState(ModelState);

            return ValidationProblem(ModelState);
        }
        [HttpGet("{inn}")]
        [ProducesResponseType(typeof(ContractorDto), 200)]
        public async Task<IActionResult> GetByInnFromSoliq(string inn)
        {
            var dto = await _service.GetByInnFromSoliq(inn);

            if (_service.IsValid)
                return Ok(dto);

            _service.CopyErrorsToModelState(ModelState);

            return ValidationProblem(ModelState);
        }
        [HttpGet]
        [ProducesResponseType(typeof(SoliqContractorDebtByTinDataDto), 200)]

        public async Task<IActionResult> GetDebtFromSoliq([FromQuery] string tin, [FromQuery] int year)
        {
            var dto = await _service.GetDebtFromSoliq(tin, year);

            if (_service.IsValid)
                return Ok(dto);

            _service.CopyErrorsToModelState(ModelState);

            return ValidationProblem(ModelState);
        }

        [HttpGet]
        [ProducesResponseType(typeof(FarmerRefundByInnDataDto), 200)]
        public async Task<IActionResult> GetFarmerRefundByInnFromSoliq([FromQuery] string inn, [FromQuery] int year)
        {
            var dto = await _service.GetFarmerRefundByInnFromSoliq(inn, year);

            if (_service.IsValid)
                return Ok(dto);

            _service.CopyErrorsToModelState(ModelState);

            return ValidationProblem(ModelState);
        }

        [HttpGet]
        [ProducesResponseType(typeof(ImtiyozDataByInnDataDto), 200)]
        public async Task<IActionResult> GetImtiyozDataByInnFromSoliq([FromQuery] string inn, [FromQuery] int year)
        {
            var dto = await _service.GetImtiyozDataByInnFromSoliq(inn, year);

            if (_service.IsValid)
                return Ok(dto);

            _service.CopyErrorsToModelState(ModelState);

            return ValidationProblem(ModelState);
        }

        [HttpGet]
        [ProducesResponseType(typeof(SoliqContractorEmployeeCountByTinDataDto), 200)]

        public async Task<ActionResult> GetEmployeeCountFromSoliq([FromQuery] string tin, [FromQuery] int year, [FromQuery] int month)
        {
            var dto = await _service.GetEmployeeCountFromSoliq(tin, year, month);

            if (_service.IsValid)
                return Ok(dto);

            _service.CopyErrorsToModelState(ModelState);

            return ValidationProblem(ModelState);
        }

        [HttpGet]
        [ProducesResponseType(typeof(SoliqContractorFinanceBenefitByTinDataDto), 200)]

        public async Task<IActionResult> GetFinanceBenefitFromSoliq([FromQuery] string tin, [FromQuery] int year, [FromQuery] int period)
        {
            var dto = await _service.GetFinanceBenefitFromSoliq(tin, year, period);

            if (_service.IsValid)
                return Ok(dto);

            _service.CopyErrorsToModelState(ModelState);

            return ValidationProblem(ModelState);
        }

        [HttpGet]
        [ProducesResponseType(typeof(GetStatisticByInnDataDto), 200)]

        public async Task<IActionResult> GetStatisticFromBandlik([FromQuery] string inn)
        {
            var result = await _service.GetStatisticFromBandlik(inn);

            if (_service.IsValid)
                return Ok(result);

            _service.CopyErrorsToModelState(ModelState);

            return ValidationProblem(ModelState);
        }


        [HttpPost]
        [ProducesResponseType(typeof(GetStatisticByInnDataDto), 200)]

        public async Task<IActionResult> GetDaftarBySoatoFromBandlik([FromBody] GetDaftarBySoatoQuery dto)
        {
            var result = await _service.GetDaftarBySoatoFromBandlik(dto);

            if (_service.IsValid)
                return Ok(result);

            _service.CopyErrorsToModelState(ModelState);

            return ValidationProblem(ModelState);
        }

        [HttpPost]
        [ProducesResponseType(typeof(BankCreditApplicationsResponse), 200)]

        public async Task<IActionResult> GetApplicationsFromBankCredit([FromQuery] string tin, [FromQuery] int offerSigned)
        {
            var result = await _service.GetApplicationsFromBankCredit(tin, offerSigned);

            if (_service.IsValid)
                return Ok(result);

            _service.CopyErrorsToModelState(ModelState);

            return ValidationProblem(ModelState);
        }

        [HttpPost]
        [ProducesResponseType(typeof(BankCreditContractStatusResponse), 200)]

        public async Task<IActionResult> GetContractStatusFromBankCredit()
        {
            var result = await _service.GetContractStatusFromBankCredit();

            if (_service.IsValid)
                return Ok(result);

            _service.CopyErrorsToModelState(ModelState);

            return ValidationProblem(ModelState);
        }

        [HttpPost]
        [ProducesResponseType(typeof(GTDFromBojxonaDto), 200)]
        public async Task<IActionResult> GetGTDFromBojxona([FromBody] GetGTDByInnRequestDto dto)
        {
            var result = await _service.GetGTDFromBojxona(dto);

            if (_service.IsValid)
                return Ok(result);

            _service.CopyErrorsToModelState(ModelState);

            return ValidationProblem(ModelState);
        }
        [HttpPost]
        public async Task<IActionResult> SaveAsExecelFromBojxona(GetGTDByInnRequestDto dto)
        {
            if (ModelState.IsValid)
            {
                var file = await _service.SaveAsExecelFromBojxona(dto);

                if (_service.IsValid)
                    return File(file, "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", "PrintFromBojxona.xlsx");

                _service.CopyErrorsToModelState(ModelState);
            }
            return ValidationProblem(ModelState);
        }
        [HttpGet("{inn}")]
        [ProducesResponseType(typeof(DavAktivContractorDto), 200)]
        public async Task<IActionResult> GetFromDavAkiv(string inn)
        {
            var dto = await _service.GetByInnFromDavAktiv(inn);

            if (_service.IsValid)
                return Ok(dto);

            _service.CopyErrorsToModelState(ModelState);

            return ValidationProblem(ModelState);
        }

        [HttpGet("{innPinfl}")]
        public IActionResult GetAylanmaByInnPinfl(string innPinfl)
        {
            var dto = _service.GetAylanmaByInn(innPinfl);

            if (_service.IsValid)
                return Ok(dto);

            _service.CopyErrorsToModelState(ModelState);

            return ValidationProblem(ModelState);
        }

        [HttpGet("{tin}")]
        [ProducesResponseType(typeof(TadbirkorFundContractorDto), 200)]
        [AllowAnonymous]
        public async Task<IActionResult> GetFromTadbirkorFund(string tin)
        {
            var dto = await _service.GetByInnFromTadbirkorFund(tin);

            if (_service.IsValid)
                return Ok(dto);

            _service.CopyErrorsToModelState(ModelState);

            return ValidationProblem(ModelState);
        }
        [HttpPost]
        [ProducesResponseType(typeof(InvestitsiyaResponseDto), 200)]
        public async Task<IActionResult> GetFromInvestment(InvestitsiyaRequestDto dto)
        {
            var data = await _service.GetInvestmentContracts(dto);

            if (_service.IsValid)
                return Ok(data);

            _service.CopyErrorsToModelState(ModelState);

            return ValidationProblem(ModelState);
        }

        [HttpGet]
		public async Task<IActionResult> SearchByInnPnfl(string innpnfl)
        {
            if (ModelState.IsValid)
            {
                var result = await _service.SearchByInnPnfl(innpnfl : innpnfl);

                if (_service.IsValid)
                {
                    return Ok(result);
                }

                _service.CopyErrorsToModelState(ModelState);
            }

            return ValidationProblem(ModelState);
        }

        [HttpGet]
        [ProducesResponseType(typeof(List<MarkaziyBankContractorCreditHistoryDto>), 200)]
        public async Task<IActionResult> GetMarkaziyBankCreditHistoryByInn([FromBody] MarkaziyBankContractorCreditHistoryRequestDto requestDto)
        {
            var dto = await _service.GetMarkaziyBankCreditHistoryByInn(requestDto);

            if (_service.IsValid)
                return Ok(dto);

            _service.CopyErrorsToModelState(ModelState);

            return ValidationProblem(ModelState);
        }

    }
}
