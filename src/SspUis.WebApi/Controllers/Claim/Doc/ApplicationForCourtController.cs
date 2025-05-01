using Microsoft.AspNetCore.Mvc;
using SspUis.BizLogicLayer;
using SspUis.BizLogicLayer.Claim;
using SspUis.BizLogicLayer.Claim.Doc.ApplicationForCourtServices;
using SspUis.Core.Security;
using SspUis.DataLayer.Repositories;
using SspUis.Integration.AgroBank.Services;
using SspUis.Integration.Sud;
using SspUis.Integration.Sud.Models;
using SspUis.Integration.Sud.Models.AuthModels;
using SspUis.Integration.Sud.Models.MalumotnomaModel;
using SspUis.Integration.Sud.Services;
using SspUis.Integration.XalqBank.Services;
using WEBASE.AspNet;
using WEBASE.Models;
using WEBASE.Storage;

namespace SspUis.WebApi.Controllers
{
    [Authorize]
    [ApiController]
    [Route("ApplicationForCourt/[action]")]
    public class ApplicationForCourtController : WebaseController
    {
        private IApplicationForCourtService _service;
        private readonly IAgroBankService _agroBankService;
        private readonly IXalqBankService _xalqBankService;
        private readonly ISudLoginService _loginService;
        private readonly ISudService _sudService;

        public ApplicationForCourtController(IApplicationForCourtService service, IAgroBankService agroBankService , IXalqBankService xalqBankService, ISudLoginService loginService, ISudService sudService)
            : base(AppSettings.Instance.ControllerConfig)
        {
            _service = service;
            _agroBankService = agroBankService;
            _loginService = loginService;
            _sudService = sudService;
            _xalqBankService = xalqBankService;
        }

        [HttpPost]
        [Authorize(ModuleCode.ApplicationForCourtViewAll)]
        public PagedResult<ApplicationForCourtListDto> GetList([FromBody] ApplicationForCourtSortFilterOptions dto)
        {
            return _service.GetList(dto);
        }

        [HttpGet("{id}")]
        [Authorize(ModuleCode.ApplicationForCourtView)]
        [ProducesResponseType(typeof(ApplicationForCourtDto), 200)]
        public IActionResult Get(long id)
        {
            ApplicationForCourtDto dto = _service.Get(id);

            if (_service.IsValid)
                return Ok(dto);

            return ValidationProblem(ModelState);
        }

        [HttpGet]
        [Authorize(ModuleCode.ApplicationForCourtView)]
        [ProducesResponseType(typeof(ApplicationForCourtDto), 200)]
        public IActionResult Get()
        {
            return Ok(_service.Get());
        }

        [HttpGet]
        public async Task<IActionResult> GetLoanActualDataFromAgroBank(string loanId)
        {
            if (ModelState.IsValid)
            {
                var result = await _agroBankService.GetLoanActualData(loanId);

                if (_service.IsValid)
                    return Ok(result);

                _service.CopyErrorsToModelState(ModelState);
            }

            return ValidationProblem(ModelState);
        }


        [HttpGet]
        [AllowAnonymous]
        public async Task<IActionResult> GetLoanActualDataFromXalqBank(string loanId)
        {
            if (ModelState.IsValid)
            {
                var result = await _xalqBankService.ChekAmountXalqBank(loanId);

                if (_service.IsValid)
                    return Ok(result);

                _service.CopyErrorsToModelState(ModelState);
            }

            return ValidationProblem(ModelState);
        }

        [HttpGet]
        [Authorize(ModuleCode.ApplicationForCourtView)]
        [ProducesResponseType(typeof(ApplicationForCourtDto), 200)]
        public IActionResult GetByMediationId(long mediationId)
        {
            return Ok(_service.GetByMediationId(mediationId));
        }
        [HttpGet]
        [Authorize(ModuleCode.ApplicationForCourtView)]
        [ProducesResponseType(typeof(CheckAmountFromBankDto), 200)]
        public async Task<IActionResult> ChekFromAgroBank(long mediationId)
        {
            //return Ok(_service.ChekFromAgroBank(mediationId));

            if (ModelState.IsValid)
            {
                var dto = await _service.ChekFromAgroBank(mediationId);

                if (_service.IsValid)
                    return Ok(dto);

                _service.CopyErrorsToModelState(ModelState);
            }
            return ValidationProblem(ModelState);
        }

        [HttpGet]
        [Authorize(ModuleCode.ApplicationForCourtView)]
        [ProducesResponseType(typeof(CheckAmountFromBankDto), 200)]
        public async Task<IActionResult> ChekAllBanks(long mediationId)
        {
            //return Ok(_service.ChekFromAgroBank(mediationId));

            if (ModelState.IsValid)
            {
                var dto = await _service.ChekAllBanks(mediationId);

                if (_service.IsValid)
                    return Ok(dto);

                _service.CopyErrorsToModelState(ModelState);
            }
            return ValidationProblem(ModelState);
        }

        [HttpGet]
        [Authorize(ModuleCode.ApplicationForCourtView)]
        [ProducesResponseType(typeof(CheckAmountFromBankDto), 200)]
        public async Task<IActionResult> ChekFromXalqBank(long mediationId)
        {
            //return Ok(_service.ChekFromAgroBank(mediationId));

            if (ModelState.IsValid)
            {
                CheckAmountFromBankDto? dto = await _service.ChekFromXalqBank(mediationId);

                if (_service.IsValid)
                    return Ok(dto);

                _service.CopyErrorsToModelState(ModelState);
            }
            return ValidationProblem(ModelState);
        }


        [HttpPost]
        [Authorize(ModuleCode.ApplicationForCourtCreate)]
        [ProducesResponseType(typeof(HaveId<long>), 200)]
        public IActionResult Create(CreateApplicationForCourtDlDto dto)
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

        [HttpGet]
        [AllowAnonymous]
        [ProducesResponseType(200)]
        public async ValueTask<IActionResult> DownloadPdf(Guid id2)
        {
            if (ModelState.IsValid)
            {
                var bytes = await _service.DownloadPdf(id2);

                if (_service.IsValid)
                    return File(bytes, "application/pdf");

                _service.CopyErrorsToModelState(ModelState);
            }

            return ValidationProblem(ModelState);
        }

        [HttpGet]
        [AllowAnonymous]
        [ProducesResponseType(200)]
        public async ValueTask<IActionResult> DownloadBankDocument(long mediationId)
        {
            if (ModelState.IsValid)
            {
                var bytes = await _service.DownloadBankDocument(mediationId);

                if (_service.IsValid)
                    return File(bytes, "application/pdf");

                _service.CopyErrorsToModelState(ModelState);
            }

            return ValidationProblem(ModelState);
        }


        [HttpGet]
        [AllowAnonymous]
        [ProducesResponseType(200)]
        public async ValueTask<IActionResult> DownloadXalqBankDocument(long mediationId, string? lang)
        {
            if (ModelState.IsValid)
            {
                var bytes = await _service.DownloadXalqBankDocument(mediationId, lang);

                if (_service.IsValid)
                    return File(bytes, "application/pdf");

                _service.CopyErrorsToModelState(ModelState);
            }

            return ValidationProblem(ModelState);
        }



        [HttpPost]
        [AllowAnonymous]
        [ProducesResponseType(200)]
        [Authorize(ModuleCode.ApplicationForCourtEdit)]
        public IActionResult Update(UpdateApplicationForCourtDlDto dto)
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
        [Authorize(ModuleCode.ApplicationForCourtDelete)]
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

        [HttpPost("{id}")]
        [Authorize(ModuleCode.ApplicationForCourtEdit)]
        [ProducesResponseType(200)]
        public IActionResult Send(long id)
        {
            if (ModelState.IsValid)
            {
                _service.Send(id);

                if (_service.IsValid)
                    return Ok();

                _service.CopyErrorsToModelState(ModelState);
            }

            return ValidationProblem(ModelState);
        }

        [HttpPost]
        [Authorize(ModuleCode.ApplicationForCourtAccept)]
        [ProducesResponseType(200)]
        public async Task<IActionResult> Accept(AcceptUpdateStatusApplicationForCourtDlDto dto)
        {
            if (ModelState.IsValid)
            {
                await _service.Accept(dto);

                if (_service.IsValid)
                    return Ok();

                _service.CopyErrorsToModelState(ModelState);
            }

            return ValidationProblem(ModelState);
        }

        [HttpPost]
        [Authorize(ModuleCode.ApplicationForCourtAcceptEmployee)]
        [ProducesResponseType(200)]
        public async Task<IActionResult> AcceptForEmployee(AcceptUpdateStatusApplicationForCourtDlDto dto)
        {
            if (ModelState.IsValid)
            {
                await _service.AcceptForEmployee(dto);

                if (_service.IsValid)
                    return Ok();

                _service.CopyErrorsToModelState(ModelState);
            }

            return ValidationProblem(ModelState);
        }

        [HttpPost("{id}")]
        [Authorize(ModuleCode.ApplicationForCourtCancel)]
        [ProducesResponseType(200)]
        public IActionResult Reject(long id, string message)
        {
            if (ModelState.IsValid)
            {
                _service.Reject(id, message);

                if (_service.IsValid)
                    return Ok();

                _service.CopyErrorsToModelState(ModelState);
            }

            return ValidationProblem(ModelState);
        }

        [HttpPost]
        [ProducesResponseType(typeof(IEnumerable<IStorageFileInfo>), 200)]
        [Authorize(ModuleCode.ApplicationForCourtCreate)]
        public IActionResult UploadFiles(IEnumerable<IFormFile> files)
        {
            if (ModelState.IsValid)
            {
                StorageFile[] dto = files.Select(a => new StorageFile(a.FileName, a.OpenReadStream())).ToArray();
                IEnumerable<IStorageFileInfo> result = _service.UploadFiles(dto);

                if (_service.IsValid)
                    return Ok(result);

                _service.CopyErrorsToModelState(ModelState);
            }

            return ValidationProblem(ModelState);
        }

        [HttpGet]
        [AllowAnonymous]
        [ProducesResponseType(200)]
        public async ValueTask<IActionResult> DownloadFileWithQrCode(Guid id)
        {
            if (ModelState.IsValid)
            {
                var bytes = await _service.DownloadFileWithQrCode(id);

                if (_service.IsValid)
                    return File(bytes, "application/pdf");

                _service.CopyErrorsToModelState(ModelState);
            }

            return ValidationProblem(ModelState);
        }

        [HttpGet("{fileId}")]
        [AllowAnonymous]
        [ProducesResponseType(typeof(IEnumerable<IStorageFileInfo>), 200)]
        public IActionResult DownloadFile(Guid fileId, [FromServices] IMimeMappingService mimeMappingService)
        {
            if (ModelState.IsValid)
            {
                StorageFile file = _service.DownloadFile(fileId, false).Item1;

                if (_service.IsValid)
                    return File(file.GetStream(), mimeMappingService.Map(file.FileName));

                _service.CopyErrorsToModelState(ModelState);
            }

            return ValidationProblem(ModelState);
        }
        [HttpGet]
        [Authorize(ModuleCode.ApplicationForCourtView)]
        [ProducesResponseType(typeof(ApplicationForCourtForGetDto), 200)]
        public IActionResult GetForFiles(long mediationId)
        {
            var dto = _service.GetForFiles(mediationId);

            if (_service.IsValid)
                return Ok(dto);

            _service.CopyErrorsToModelState(ModelState);

            return ValidationProblem(ModelState);
        }


        //sud 

        [HttpPost]
        [AllowAnonymous]
        [ProducesResponseType(typeof(SudAuthCreateResponseDto),200)]
        public async Task<IActionResult> SudAuth()
        {
            var result = await _loginService.SudAuthLoginCreate();

            if (_service.IsValid)
                return Ok(result);

            _service.CopyErrorsToModelState(ModelState);

            return ValidationProblem(ModelState);

        }



        [HttpPost]
        [AllowAnonymous]
        [ProducesResponseType(200)]
        public  async Task<IActionResult> SudUploadFileIntegration(IFormFile file)
        {
             var result = await _service.SudUploadFileIntegration(file);

            if (_service.IsValid)
                return Ok(result);

            _service.CopyErrorsToModelState(ModelState);

            return ValidationProblem(ModelState);

        }

        [HttpPost]
        [ProducesResponseType(typeof(SendingNewClaimResponseDto),200)]
        public async Task<IActionResult> SendSudIntegration(CreateCourtntegrationDlDto dto)
        {
            var result = await _service.SendSudIntegration(dto);

            if (_service.IsValid)
                return Ok(result);

            _service.CopyErrorsToModelState(ModelState);

            return ValidationProblem(ModelState);

        }


        [HttpPost]
        [ProducesResponseType(200)]
        [ProducesResponseType(400)]
        public async Task<IActionResult> SendSudIntegration2(SendingNewClaimDto dto)
        {
            //var result = null; //await _sudService.SudSendingNewClaim(dto);

            //if (_service.IsValid)
            //    return Ok(result);

            //_service.CopyErrorsToModelState(ModelState);

            //return ValidationProblem(ModelState);r
            return null;

        }


        [HttpPost]
        [ProducesResponseType(typeof(InvoiceResponseModel), 200)]
        public async Task<IActionResult> SudInvoice(SudInvoiceDto dto)
        {
            var result = await _service.SudInvoice(dto);

            if (_service.IsValid)
                return Ok(result);

            _service.CopyErrorsToModelState(ModelState);

            return ValidationProblem(ModelState);

        }


        [HttpGet]
        [ProducesResponseType(typeof(CommonEntity),200)]
        public  async Task<IActionResult> GetSudRegionList()
        {
            var result = await _service.GetSudRegionList();

            if (_service.IsValid)
                return Ok(result);

            _service.CopyErrorsToModelState(ModelState);

            return ValidationProblem(ModelState);
        }

        [HttpGet]
        
        [ProducesResponseType(typeof(CommonEntity), 200)]
        public async Task<IActionResult> GetSudDistrictList(Guid regionId)
        {
            var result = await _service.GetSudDistrictList(regionId);

            if (_service.IsValid)
                return Ok(result);

            _service.CopyErrorsToModelState(ModelState);

            return ValidationProblem(ModelState);
        }

        [HttpGet]
        [ProducesResponseType(typeof(CommonEntity), 200)]
        public async Task<IActionResult> GetSudPostReasonList()
        {
            var result = await _service.GetSudPostReasonList();

            if (_service.IsValid)
                return Ok(result);

            _service.CopyErrorsToModelState(ModelState);

            return ValidationProblem(ModelState);
        }

        [HttpGet]
        [ProducesResponseType( 200)]
        public async Task<IActionResult> GetSudParticipantTypeList()
        {
            var result = await _service.GetSudParticipantTypeList();

            if (_service.IsValid)
                return Ok(result);

            _service.CopyErrorsToModelState(ModelState);

            return ValidationProblem(ModelState);
        }

        [HttpGet]
        [ProducesResponseType(typeof(CommonEntity), 200)]
        public async Task<IActionResult> GetSudDutyReasonList()
        {
            var result = await _service.GetSudDutyReasonList();

            if (_service.IsValid)
                return Ok(result);

            _service.CopyErrorsToModelState(ModelState);

            return ValidationProblem(ModelState);
        }

        [HttpGet]
        [ProducesResponseType(typeof(CommonEntity), 200)]
        public async Task<IActionResult> GetSudDocumentTypesList()
        {
            var result = await _service.GetSudDocumentTypesList();

            if (_service.IsValid)
                return Ok(result);

            _service.CopyErrorsToModelState(ModelState);

            return ValidationProblem(ModelState);
        }

        [HttpGet]
        [ProducesResponseType( 200)]
        public async Task<IActionResult> GetSudEntityTypeList()
        {
            var result = await _service.GetSudEntityTypeList();

            if (_service.IsValid)
                return Ok(result);

            _service.CopyErrorsToModelState(ModelState);

            return ValidationProblem(ModelState);
        }

        [HttpGet]
        [ProducesResponseType(typeof(CurrencyModel), 200)]
        public async Task<IActionResult> GetSudCurrencyList()
        {
            var result = await _service.GetSudCurrencyList();

            if (_service.IsValid)
                return Ok(result);

            _service.CopyErrorsToModelState(ModelState);

            return ValidationProblem(ModelState);
        }

        [HttpGet]
        [ProducesResponseType(typeof(CommonEntity), 200)]
        public async Task<IActionResult> GetSudCourtList()
        {
            var result = await _service.GetSudCourtList();

            if (_service.IsValid)
                return Ok(result);

            _service.CopyErrorsToModelState(ModelState);

            return ValidationProblem(ModelState);
        }

        [HttpGet]
        [ProducesResponseType(typeof(CommonEntity), 200)]
        public async Task<IActionResult> GetSudCountryList()
        {
            var result = await _service.GetSudCountryList();

            if (_service.IsValid)
                return Ok(result);

            _service.CopyErrorsToModelState(ModelState);

            return ValidationProblem(ModelState);
        }

        [HttpGet]
        [ProducesResponseType( 200)]
        public async Task<IActionResult> GetSudClaimKindList()
        {
            var result = await _service.GetSudClaimKindList();

            if (_service.IsValid)
                return Ok(result);

            _service.CopyErrorsToModelState(ModelState);

            return ValidationProblem(ModelState);
        }

        [HttpGet]
        [ProducesResponseType(typeof(CommonEntity), 200)]
        public async Task<IActionResult> GetSudCategoriesSubList()
        {
            var result = await _service.GetSudCategoriesSubList();

            if (_service.IsValid)
                return Ok(result);

            _service.CopyErrorsToModelState(ModelState);

            return ValidationProblem(ModelState);
        }

        [HttpGet]
        [ProducesResponseType(typeof(CommonEntity), 200)]
        public async Task<IActionResult> GetSudCategoriesSecondList()
        {
            var result = await _service.GetSudCategoriesSecondList();

            if (_service.IsValid)
                return Ok(result);

            _service.CopyErrorsToModelState(ModelState);

            return ValidationProblem(ModelState);
        }

        [HttpGet]
        [ProducesResponseType(typeof(CommonEntity), 200)]
        public async Task<IActionResult> GetSudAmountCategoryList()
        {
            var result = await _service.GetSudAmountCategoryList();

            if (_service.IsValid)
                return Ok(result);

            _service.CopyErrorsToModelState(ModelState);

            return ValidationProblem(ModelState);
        }

        [HttpGet]
        [ProducesResponseType(typeof(BankModel), 200)]
        public async Task<IActionResult> GetSudBankList()
        {
            var result = await _service.GetSudBankList();

            if (_service.IsValid)
                return Ok(result);

            _service.CopyErrorsToModelState(ModelState);

            return ValidationProblem(ModelState);
        }

        [HttpGet]
        [ProducesResponseType(typeof(CommonEntity), 200)]
        public async Task<IActionResult> GetSudCategoryList()
        {
            var result = await _service.GetSudCategoryList();

            if (_service.IsValid)
                return Ok(result);

            _service.CopyErrorsToModelState(ModelState);

            return ValidationProblem(ModelState);
        }
    }

}
