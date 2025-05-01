using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging.Abstractions;
using SspUis.BizLogicLayer;
using SspUis.BizLogicLayer.Doc.MonoApplicationServices;
using SspUis.BizLogicLayer.ReportServices;
using SspUis.BizLogicLayer.ReportServices.Main;
using SspUis.BizLogicLayer.ReportServices.Main.QueryObjects;
using SspUis.BizLogicLayer.ReportServices.Main.ReportDtos.Memship;
using SspUis.BizLogicLayer.ReportServices.Main.ReportDtos.Partner;
using SspUis.Core.Security;
using SspUis.DataLayer;
using SspUis.DataLayer.EfClasses.Exapidata;
using SspUis.DataLayer.EfClasses.Memship;
using SspUis.DataLayer.EfClasses.Report;
using SspUis.DataLayer.EfClasses.Report.Func;
using SspUis.Integration.Bandlik.Models;
using SspUis.Integration.BankCredit.Models;
using WEBASE.AspNet;
using WEBASE.Models;

namespace SspUis.WebApi.Controllers
{
    [Authorize]
    [ApiController]
    [Route("[controller]/[action]")]
    public class ReportController : WebaseController
    {
        private IReportService _service;
        private IAuthService _authService;
        public ReportController(IReportService service, IAuthService authService)
            : base(AppSettings.Instance.ControllerConfig)
        {
            _service = service;
            _authService = authService;
        }

        #region CLAIM
        [HttpPost]
        [ProducesResponseType(typeof(List<AppealsSentToClaimAppDto>), 200)]
        public IActionResult AppealsSentToClaimApplicationReport(ClaimApplicationReportsDtoFilter filter)
        {
            return Ok(_service.GetAppealsSentToClaimApplication(filter));
        }
        [HttpPost]
        public IActionResult SaveAsExcelAppealsSentToClaimApplication(ClaimApplicationReportsDtoFilter filter)
        {
            if (ModelState.IsValid)
            {
                var file = _service.SaveAsExcelAppealsSentToClaimApplication(filter);
                if (_service.IsValid)
                    return File(file, "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", "AppealsSentToClaimApplication.xlsx");

                _service.CopyErrorsToModelState(ModelState);
            }
            return ValidationProblem(ModelState);
        }

        [HttpPost]
        [ProducesResponseType(typeof(ReceivedClaimAppDto), 200)]
        public IActionResult ReceivedClaimApplicationReport(ClaimApplicationReportsDtoFilter filter)
        {
            return Ok(_service.GetReceivedClaimApplication(filter));
        }
        [HttpPost]
        public IActionResult SaveAsExcelReceivedClaimApplication(ClaimApplicationReportsDtoFilter filter)
        {
            if (ModelState.IsValid)
            {
                var file = _service.SaveAsExcelReceivedClaimApplication(filter);
                if (_service.IsValid)
                    return File(file, "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", "ReceivedClaimApplication.xlsx");

                _service.CopyErrorsToModelState(ModelState);
            }
            return ValidationProblem(ModelState);
        }
        [HttpPost]
        [ProducesResponseType(typeof(ClaimApplicationDto), 200)]
        public IActionResult GetClaimApplicationReport(ClaimApplicationDtoFilter filter)
        {
            return Ok(_service.GetClaimApplicationReport(filter));
        }

        [HttpPost]
        [Authorize(ModuleCode.ClaimApplicationReportView)]
        [ProducesResponseType(typeof(ClaimApplicationDto), 200)]
        public IActionResult ClaimApplicationReport(ClaimApplicationDtoFilter filter)
        {
            return Ok(_service.ClaimApplicationReport(filter));
        }

        [HttpPost]
        public IActionResult SaveAsExcelClaimApplicationReport(ClaimApplicationDtoFilter filter)
        {
            if (ModelState.IsValid)
            {
                Stream? file = _service.SaveAsExcelClaimApplicationReport(filter);
                if (_service.IsValid)
                    return File(file, "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", "Davo Arizalari hisoboti.xlsx");
                _service.CopyErrorsToModelState(ModelState);
            }

            return ValidationProblem(ModelState);
        }

        [HttpPost]
        [ProducesResponseType(typeof(List<ClaimApplicationAmountDto>), 200)]
        public IActionResult GetClaimApplicationAmount(ClaimApplicationAmountDtoFilter filter)
        {
            return Ok(_service.GetClaimApplicationAmount(filter));
        }
        [HttpPost]
        [ProducesResponseType(typeof(List<SummaOfClaimAppDto>), 200)]
        public IActionResult SummaOfClaimApplicationReport(ClaimApplicationReportsDtoFilter filter)
        {
            return Ok(_service.GetSummaOfClaimApplication(filter));
        }
        [HttpPost]
        public IActionResult SaveAsExcelSummaOfClaimApplication(ClaimApplicationReportsDtoFilter filter)
        {
            if (ModelState.IsValid)
            {
                var file = _service.SaveAsExcelSummaOfClaimApplication(filter);
                if (_service.IsValid)
                    return File(file, "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", "SummaOfClaimApplication.xlsx");

                _service.CopyErrorsToModelState(ModelState);
            }
            return ValidationProblem(ModelState);
        }
        #endregion

        #region MEMSHIP

        [HttpPost]
        [Authorize(ModuleCode.ReportMemshipApplicationAndContractInfoView)]
        public IActionResult GetMemshipDocsInfoReestr(MemshipDocsInfoReestrDtoFilter filter)
        {
            return Ok(_service.GetMemshipDocsInfoReestr(filter));
        }

        [HttpPost]
        [Authorize(ModuleCode.ReportMemshipFreeOfChargeView)]
        public IActionResult GetMemshipReports(MemshipReportDtoFilter filter)
        {
            return Ok(_service.GetMemshipReports(filter));
        }
        //[HttpPost]
        ////[Authorize(ModuleCode.ReportMemshipFreeOfChargeView)]
        //public IActionResult GetPrtnApplicationByRegion(PrtnApplicationByRegionDtoFilter filter)
        //{
        //    return Ok(_service.GetPrtnApplicationByRegion(filter));
        //}
        [HttpPost]
        [Authorize(ModuleCode.ReportMemshipFreeOfChargeView)]
        public IActionResult SaveAsExcelGetMemshipReport(MemshipReportDtoFilter filter)
        {
            if (ModelState.IsValid)
            {
                var file = _service.SaveAsExcelGetMemshipReport(filter);
                if (_service.IsValid)
                    return File(file, "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", "MemshipReportByMonth.xlsx");

                _service.CopyErrorsToModelState(ModelState);
            }
            return ValidationProblem(ModelState);
        }

        [HttpPost]
        [Authorize(ModuleCode.ReportMemshipApplicationAndContractInfoView)]
        [ProducesResponseType(typeof(List<MemshipDocsInfoDto>), 200)]
        public IActionResult GetMemshipApplicationAndContractInfo(MemshipDocsInfoDtoFilter filter)
        {
            return Ok(_service.GetMemshipDocsInfo(filter));
        }
        [HttpPost]
        [Authorize(ModuleCode.ReportMemshipContractView)]
        [ProducesResponseType(typeof(List<MemshipContractPaidDto>), 200)]
        public IActionResult GetMemshipContract(MemshipContractPaidDtoFilter filter)
        {
            return Ok(_service.GetMemshipContract(filter));
        }
        [HttpPost]
        //[Authorize(ModuleCode.ReportMemshipContractView)]
        [ProducesResponseType(typeof(List<ContractorCategoryDto>), 200)]
        public IActionResult GetContractorCategoryType(ContractorCategoryDtoFilter filter)
        {
            return Ok(_service.GetContractorCategoryType(filter));
        }


        [HttpPost]
        [Authorize(ModuleCode.ReportMemshipContractView)]
        public IActionResult SaveAsExcelContractorCategoryType(ContractorCategoryDtoFilter filter)
        {
            if (ModelState.IsValid)
            {
                var file = _service.SaveAsExcelContractorCategoryType(filter);
                if (_service.IsValid)
                    return File(file, "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", "ContractorCategoryType.xlsx");

                _service.CopyErrorsToModelState(ModelState);
            }
            return ValidationProblem(ModelState);
        }

        //[HttpPost]
        //[ProducesResponseType(typeof(List<MemshipContractPaidDto>), 200)]
        //public async Task<IActionResult> GetMemshipContract(MemshipContractPaidDtoFilter filter)
        //{
        //    var result = await _service.GetMemshipContract(filter);

        //    if (_service.IsValid)
        //        return Ok(result);

        //    _service.CopyErrorsToModelState(ModelState);

        //    return ValidationProblem(ModelState);
        //}
        [HttpPost]
        [Authorize(ModuleCode.ReportMemshipApplicationAndContractInfoView)]
        public IActionResult SaveAsExcelGetMemshipDocsInfo(MemshipDocsInfoDtoFilter filter)
        {
            if (ModelState.IsValid)
            {
                var file = _service.SaveAsExcelGetMemshipDocsInfo(filter);
                if (_service.IsValid)
                    return File(file, "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", "MemshipReport.xlsx");

                _service.CopyErrorsToModelState(ModelState);
            }
            return ValidationProblem(ModelState);
        }

        [HttpPost]
        [Authorize(ModuleCode.ReportMemshipApplicationByOrganizationView)]
        public IActionResult GetMemshipApplicationReportByOrganizations(MemshipReportByOrganizationDtoFilter filter)
        {
            return Ok(_service.GetMemshipReportByOrganization(filter));
        }
        [HttpPost]
        public IActionResult SaveAsExcelMemshipReportByOrganization(MemshipReportByOrganizationDtoFilter filter)
        {
            if (ModelState.IsValid)
            {
                var file = _service.SaveAsExcelGetMemshipReportByOrganization(filter);
                if (_service.IsValid)
                    return File(file, "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", "MemshipReportByOrganization.xlsx");

                _service.CopyErrorsToModelState(ModelState);
            }
            return ValidationProblem(ModelState);
        }

        [HttpPost]
        [Authorize(ModuleCode.ReportMemshipApplicationByPersonTypeView)]
        public IActionResult GetMemshipApplicationReportByPersonType(MemshipReportByPersonTypeFilter filter)
        {
            return Ok(_service.GetMemshipReportByPersonTypes(filter));
        }
        [HttpPost]
        public IActionResult SaveAsExcelMemshipReportByPersonType(MemshipReportByPersonTypeFilter filter)
        {
            if (ModelState.IsValid)
            {
                var file = _service.SaveAsExcelGetMemshipReportByPersonType(filter);
                if (_service.IsValid)
                    return File(file, "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", "MemshipReportByPersonType.xlsx");

                _service.CopyErrorsToModelState(ModelState);
            }
            return ValidationProblem(ModelState);
        }

        [HttpPost]
        public IActionResult SaveAsExcelSrvServiceGetInfo(GetSrvServiceInfoRequestDto filter)
        {
            if (ModelState.IsValid)
            {
                var file = _service.SaveAsExcelGetSrvServiceInfo(filter);
                if (_service.IsValid)
                    return filter.IsFree ? File(file, "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", "BepulXizmatlar.xlsx") : File(file, "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", "PullikXizmatlar.xlsx");

                _service.CopyErrorsToModelState(ModelState);
            }
            return ValidationProblem(ModelState);
        }

        [HttpPost]
        [Authorize(ModuleCode.ReportMemshipPaidOfChargeView)]
        public IActionResult GetPaidMemshipReport(MemshipPaidReportDtoFilter filter)
        {
            return Ok(_service.GetPaidMemshipReport(filter));
        }
        [HttpPost]
        [Authorize(ModuleCode.ReportMemshipPaidOfChargeView)]
        public IActionResult SaveAsExcelGetPaidMemshipReport(MemshipPaidReportDtoFilter filter)
        {
            if (ModelState.IsValid)
            {
                var file = _service.SaveAsExcelGetPaidMemshipReport(filter);
                if (_service.IsValid)
                    return File(file, "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", "PaidMemshipReport.xlsx");

                _service.CopyErrorsToModelState(ModelState);
            }
            return ValidationProblem(ModelState);
        }
        #endregion

        #region PARTNER
        [HttpPost]
        [Authorize(ModuleCode.ReportPrntExpiredDocumentsView)]
        [ProducesResponseType(typeof(PrtnEmploymentGraphReportDto), 200)]
        public IActionResult PrtnEmploymentGraphReport(PrtnEmploymentGraphPageOption filter, bool isPrint = false)
        {
            return Ok(_service.GetPrtnEmploymentGraphReport(filter, false));
        }
        [HttpPost]
        [Authorize(ModuleCode.ApplicationStatusView)]
        [ProducesResponseType(typeof(List<PrtnEmploymentGraphReportNewDto>), 200)]
        public IActionResult PrtnEmploymentGraphNewReport(PrtnDocumentSortFilterOptions filter) 
        => Ok(_service.GetPrtnEmploymentGraphNewReportWithPagination(filter));

        [HttpPost]
        [Authorize(ModuleCode.ApplicationStatusView)]
        [ProducesResponseType(typeof(List<PrtnApplicationByPetitionInfoDto>), 200)]
        public IActionResult PrtnApplicationByPetitionInfo(PrtnDocumentSortFilterOptions filter)
        => Ok(_service.GetPrtnApplicationByPetitionInfoWithPagination(filter));

        [HttpPost]
        [Authorize(ModuleCode.ApplicationStatusView)]
        [ProducesResponseType(typeof(List<PrtnApplicationByContractInfoDto>), 200)]
        public IActionResult GetPrtnApplicationByContractNewInfo(PrtnDocumentSortFilterOptions filter)
        => Ok(_service.GetPrtnApplicationByContractNewInfoWithPagination(filter));

        [HttpPost]
        [Authorize(ModuleCode.ApplicationStatusView)]
        [ProducesResponseType(typeof(List<PrtnApplicationByFullInfoDto>), 200)]
        public IActionResult GetPrtnApplicationByFullInfo(PrtnDocumentSortFilterOptions filter)
        => Ok(_service.GetPrtnApplicationByFullInfoWithPagination(filter));

        [HttpPost]
        public IActionResult PrtnEmploymentGraphExcel(PrtnEmploymentGraphPageOption dto)
        {
            if (ModelState.IsValid)
            {
                var file = _service.SaveAsExcelPrtnEmploymentGraph(dto);
                if (_service.IsValid)
                    return File(file, "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", "EmploymentGraph.xlsx");

                _service.CopyErrorsToModelState(ModelState);
            }
            return ValidationProblem(ModelState);
        }

        [HttpPost]
        [Authorize(ModuleCode.ReportPrtnApplicationAndContractInfoView)]
        [ProducesResponseType(typeof(List<PrtnApplicationAndContractInfoDto>), 200)]
        public IActionResult GetPrtnApplicationAndContractInfo(PrtnApplicationAndContractInfoDtoFilter filter)
        {
            return Ok(_service.GetPrtnApplicationAndContractInfo(filter));
        }

        [HttpPost]
        [Authorize(ModuleCode.ReportPrtnApplicationAndContractInfoView)]
        [ProducesResponseType(typeof(List<PrtnApplicationAndContractInfoDto>), 200)]
        public IActionResult GetPrtnApplicationAndContractInfoPaged(PrtnDocumentSortFilterOptions filter)
        {
            return Ok(_service.GetPrtnApplicationAndContractInfoPaged(filter));
        }

        //[HttpPost]
        //[AllowAnonymous]
        ////[Authorize(ModuleCode.ReportPrtnApplicationAndContractInfoView)]
        //[ProducesResponseType(typeof(IQueryable<ExecutationApplicationDtoFilter>), 200)]
        //public IActionResult GetExecutionApplication(ExecutationApplicationDtoFilter filter)
        //{
        //    return Ok(_service.GetExecutionApplication(filter));
        //}

        [HttpPost]
        [Authorize(ModuleCode.ReportPrtnCreditDemandInfoView)]
        [ProducesResponseType(typeof(List<PrtnCreditDemandInfoDto>), 200)]
        public IActionResult GetPrtnCreditDemandInfo(PrtnCreditDemandInfoDtoFilter filter)
        {
            return Ok(_service.GetPrtnCreditDemandInfo(filter));
        }

        [HttpPost]
        [Authorize(ModuleCode.ReportPrtnCreditDemandInfoView)]
        [ProducesResponseType(typeof(PrtnCreditDemandInfoDto), 200)]
        public IActionResult GetPrtnCreditDemandInfoPaged(PrtnCreditDemandInfoDtoFilterPaged filter)
        {
            return Ok(_service.GetPrtnCreditDemandInfoPaged(filter));
        }


        [HttpPost]
        public IActionResult SaveAsExecelForPrtnCreditDemandPaged(PrtnCreditDemandInfoDtoFilterPaged dto)
        {
            if (ModelState.IsValid)
            {
                var file = _service.SaveAsExecelForPrtnCreditDemandPaged(dto);
                if (_service.IsValid)
                    return File(file, "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", "PrtnCreditDemandPaged.xlsx");

                _service.CopyErrorsToModelState(ModelState);
            }
            return ValidationProblem(ModelState);
        }

        [HttpPost]
        public IActionResult SaveAsExecelForPrtnCreditDemand(PrtnCreditDemandInfoByBankDtoFilter dto)
        {
            if (ModelState.IsValid)
            {
                var file = _service.SaveAsExecelForPrtnCreditDemand(dto);
                if (_service.IsValid)
                    return File(file, "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", "PrtnCreditDemand.xlsx");

                _service.CopyErrorsToModelState(ModelState);
            }
            return ValidationProblem(ModelState);
        }

        [HttpPost]
        [Authorize(ModuleCode.ReportPrtnCreditDemandInfoByBankView)]
        [ProducesResponseType(typeof(List<PrtnCreditDemandInfoByBankDto>), 200)]
        public IActionResult GetPrtnCreditDemandInfoByBank(PrtnCreditDemandInfoByBankDtoFilter filter)
        {
            return Ok(_service.GetPrtnCreditDemandInfoByBank(filter));
        }
        [HttpPost]
        public IActionResult SaveAsExecelForPrtnCreditBank(PrtnCreditDemandInfoByBankDtoFilter dto)
        {
            if (ModelState.IsValid)
            {
                var file = _service.SaveAsExecelForPrtnCreditBank(dto);
                if (_service.IsValid)
                    return File(file, "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", "PrtnCreditDemand.xlsx");

                _service.CopyErrorsToModelState(ModelState);
            }
            return ValidationProblem(ModelState);
        }

        [HttpPost]
        [Authorize(ModuleCode.ReportPrtnApplicationAndContractInfoByRegionView)]
        [ProducesResponseType(typeof(List<PrtnApplicationAndContractInfoDto>), 200)]
        public IActionResult GetPrtnApplicationAndContractInfoByRegion(PrtnApplicationAndContractInfoDtoFilter filter)
        {
            // O'z viloyati bo'yicha hisobotni ko'rish
            filter.ByRegion = false;
            filter.RegionId = _authService.Organization.RegionId;
            return Ok(_service.GetPrtnApplicationAndContractInfo(filter));
        }
        [HttpPost]
        public IActionResult SaveAsExecel(PrtnApplicationAndContractInfoDtoFilter dto)
        {
            if (ModelState.IsValid)
            {
                var file = _service.SaveAsExecel(dto);
                if (_service.IsValid)
                    return File(file, "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", "PrtnApplicationAndContractTemplate.xlsx");

                _service.CopyErrorsToModelState(ModelState);
            }
            return ValidationProblem(ModelState);
        }
        [HttpPost]
        public IActionResult SaveAsExecelForSum(PrtnApplicationAndContractInfoDtoFilter dto)
        {
            if (ModelState.IsValid)
            {
                var file = _service.SaveAsExecelForSum(dto);
                if (_service.IsValid)
                    return File(file, "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", "PrtnApplicationAndContractTemplate.xlsx");

                _service.CopyErrorsToModelState(ModelState);
            }
            return ValidationProblem(ModelState);
        }

        [HttpPost]
        [Authorize(ModuleCode.ReportPrtnApplicationByContractTypeInfoView)]
        [ProducesResponseType(typeof(PrtnApplicationByContractTypeDto), 200)]
        public IActionResult GetPrtnApplicationByContractType(PrtnApplicationByContractTypeDtoFilter filter)
        {
            return Ok(_service.GetPrtnApplicationByContractType(filter));
        }

        [HttpPost]
        [Authorize(ModuleCode.ReportPrtnApplicationByContractTypeInfoView)]
        public async Task<ActionResult> SaveAsExecelByContractType(PrtnApplicationByContractTypeDtoFilter filter)
        {
            if (ModelState.IsValid)
            {
                var file = await _service.SaveAsExecelForCollectedReport(filter);
                if (_service.IsValid)
                    return File(file, "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", "YigmaHisobot.xlsx");

                _service.CopyErrorsToModelState(ModelState);
            }
            return ValidationProblem(ModelState);
        }
        [HttpPost]
        [Authorize(ModuleCode.ReportPrtnApplicationByContractTypeInfoView)]
        [ProducesResponseType(typeof(PrtnApplicationByContractTypeDto), 200)]
        public IActionResult GetPrtnApplicationByContractTypePaged(PrtnApplicationByContractTypeDtoFiler2 filter)
        {
            return Ok(_service.GetPrtnApplicationByContractTypePaged(filter));
        }
        [HttpPost]
        [Authorize(ModuleCode.ReportOnProjectImplementationAndBenefitsGrantedView)]
        [ProducesResponseType(typeof(PrtnApplicationByContractTypeDto), 200)]
        public IActionResult ReportOnProjectImplementationAndBenefitsGranted(PrtnFilterDto filter)
        {
            return Ok(_service.ReportOnProjectImplementationAndBenefitsGranted(filter));
        }


        [HttpPost]
        public IActionResult PrtnApplicationByContractTypeExcel(PrtnApplicationByContractTypeDtoFilter dto)
        {
            if (ModelState.IsValid)
            {
                var file = _service.PrtnApplicationByContractTypeExcel(dto);
                if (_service.IsValid)
                    return File(file, "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", "PrtnApplicationByContractType.xlsx");

                _service.CopyErrorsToModelState(ModelState);
            }
            return ValidationProblem(ModelState);
        }
        [HttpPost]
        public IActionResult SaveAsExcelBojxonaImtiyoz(PrtnApplicationByContractTypeDtoFilter filter)
        {
            if (ModelState.IsValid)
            {
                var file = _service.SaveAsExcelPrtnBojxonaContracts(filter);
                if (_service.IsValid)
                    return File(file, "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", "BojxonaImtiyoz.xlsx");
                _service.CopyErrorsToModelState(ModelState);
            }
            return ValidationProblem(ModelState);
        }
        [HttpPost]
        public IActionResult SaveasExcelSoliqImtiyoz(PrtnApplicationByContractTypeDtoFilter filter)
        {
            if (ModelState.IsValid)
            {
                var file = _service.SaveAsExcelPrtnSoliqImtiyozContracts(filter);
                if (_service.IsValid)
                    return File(file, "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", "SoliqImtiyoz.xlsx");

                _service.CopyErrorsToModelState(ModelState);
            }
            return ValidationProblem(ModelState);
        }
        [HttpPost]
        public IActionResult SaveAsExcelDavaktivImtiyoz(PrtnApplicationByContractTypeDtoFilter filter)
        {
            if (ModelState.IsValid)
            {
                var file = _service.SaveAsExcelPrtnDavAkticContract(filter);
                if (_service.IsValid)
                    return File(file, "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", "DavaktivImtiyoz.xlsx");

                _service.CopyErrorsToModelState(ModelState);
            }
            return ValidationProblem(ModelState);
        }
        [HttpPost]
        public IActionResult SaveAsExcelAllIntegrationReportByContractor()
        {
            if (ModelState.IsValid)
            {
                var file = _service.SaveAsExcelAllIntegrationReportByContractor();
                if (_service.IsValid)
                    return File(file, "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", "AllIntegrationReportByContractor.xlsx");

                _service.CopyErrorsToModelState(ModelState);
            }
            return ValidationProblem(ModelState);
        }
        [HttpPost]
        public IActionResult SaveAsExcelAllIntegrationReportByRegion()
        {
            if (ModelState.IsValid)
            {
                var file = _service.SaveAsExcelAllIntegrationReportByRegion();
                if (_service.IsValid)
                    return File(file, "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", "SaveAsExcelAllIntegrationReportByRegion.xlsx");

                _service.CopyErrorsToModelState(ModelState);
            }
            return ValidationProblem(ModelState);
        }
        [HttpPost]
        [AllowAnonymous]
        public IActionResult SaveAsExcelAllEmployeeCountReport(AllEmployeeCountReportDto dto)
        {
            if (ModelState.IsValid)
            {
                var file = _service.SaveAsExcelAllEmployeeCountReport(dto);
                if (_service.IsValid)
                    return File(file, "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", "EmployeeCountReport.xlsx");

                _service.CopyErrorsToModelState(ModelState);
            }
            return ValidationProblem(ModelState);
        }
        [HttpPost]
        [Authorize(ModuleCode.ReportPrtnCertificateByContractInfoView)]
        [ProducesResponseType(typeof(PrtnApplicationByContractTypeDto), 200)]
        public IActionResult GetPrtnCertificateByContract(PrtnCertificateByContractDtoFilter filter)
        {
            return Ok(_service.GetPrtnCertificateByContract(filter));
        }

        [HttpPost]
        public IActionResult SaveAsExcelPrtnContracts(PrtnDocumentSortFilterOptions dto)
        {
            if (ModelState.IsValid)
            {
                var file = _service.SaveAsExcelPrtnContracts(dto);
                if (_service.IsValid)
                    return File(file, "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", "Hamkorlik.xlsx");

                _service.CopyErrorsToModelState(ModelState);
            }

            return ValidationProblem(ModelState);
        }
        [HttpPost]
        public IActionResult SaveAsExcelSummaryReportByokedTypes(PrtnApplicationByContractTypeDtoFilter filter)
        {
            if (ModelState.IsValid)
            {
                var file = _service.SaveAsExcelSummaryReportByOkedTypes(filter);
                if (_service.IsValid)
                    return File(file, "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", "Jamg`arma.xlsx");

                _service.CopyErrorsToModelState(ModelState);
            }

            return ValidationProblem(ModelState);
        }
        #endregion

        #region HRM
        [HttpPost]
        [Authorize(ModuleCode.StaffingSingleReportView)]
        [ProducesResponseType(typeof(List<StaffCountDto>), 200)]
        public IActionResult GetStaffCountReport(StaffCountDtoFilter filter)
        {
            return Ok(_service.GetStaffCountReport(filter));
        }

        [HttpPost]
        [Authorize(ModuleCode.StaffingSingleReportView)]
        [ProducesResponseType(typeof(List<StaffCountByGenderDto>), 200)]
        public IActionResult GetStaffCountByGenderReport(StaffCountByGenderDtoFilter filter)
        {
            return Ok(_service.GetStaffCountByGenderReport(filter));
        }

        [HttpPost]
        [Authorize(ModuleCode.StaffingSingleReportView)]
        [ProducesResponseType(typeof(List<StaffingSinglePageReportDto>), 200)]
        public IActionResult GetStaffingSingleReport(StaffingSinglePageReportDtoFilter filter)
        {
            return Ok(_service.GetStaffingSingleReport(filter));
        }

        [HttpPost]
        [Authorize(ModuleCode.GetStaffingSingleReportForParentView)]
        [ProducesResponseType(typeof(PagedResult<StaffingSinglePageReportDto>), 200)]
        public IActionResult GetStaffingSingleReportForParent(StaffingSinglePageReportDtoFilter filter)
        {
            return Ok(_service.GetStaffingSingleReportForParent(filter));
        }
        [HttpPost]
        [AllowAnonymous]
        //[Authorize(ModuleCode.HrmDocumentReport)]
        [ProducesResponseType(typeof(List<HrmEmployeeDocumentDto>), 200)]
        public IActionResult GetReportDocumentsForHrm(HrmEmployeeDocumentDtoFilter dto)
        {
            return Ok(_service.GetReportDocumentsForHrm(dto));
        }
        [HttpPost]
        [ProducesResponseType(typeof(List<EmployeeCardDto>), 200)]
        public IActionResult GetEmployeeCard(EmployeeCardDtoFilter filter)
        {
            return Ok(_service.GetEmployeeCard(filter));
        }

        [HttpPost]
        [Authorize(ModuleCode.ReportTaxQqsAylanmaView)]
        [AllowAnonymous]
        [ProducesResponseType(typeof(PagedResult<GetTaxQqsAylanmaDto>), 200)]
        public PagedResult<GetTaxQqsAylanmaDto> GetTaxQqsAylanmaReport(GetTaxQqsAylanmaDtoFilter options)
        {
            return _service.GetTaxQqsAylanmaReport(options);
        }

        [HttpPost]
        [AllowAnonymous]
        public IActionResult SaveAsExcelGetHrmCommands(HrmEmployeeDocumentDtoFilter dto)
        {
            if (ModelState.IsValid)
            {
                var file = _service.SaveAsExcelGetHrmCommands(dto);
                if (_service.IsValid)
                    return File(file, "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", "Buyruqlar hisoboti.xlsx");

                _service.CopyErrorsToModelState(ModelState);
            }
            return ValidationProblem(ModelState);
        }

        [HttpPost]
        [AllowAnonymous]
        public IActionResult SaveAsExcelGetStaffCountByGenderReport(StaffCountByGenderDtoFilter dto)
        {
            if (ModelState.IsValid)
            {
                var file = _service.SaveAsExcelGetStaffCountByGenderReport(dto);
                if (_service.IsValid)
                    return File(file, "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", "Shtat birliklari bo'yicha hisobot.xlsx");

                _service.CopyErrorsToModelState(ModelState);
            }
            return ValidationProblem(ModelState);
        }

        [HttpPost]
        [AllowAnonymous]
        public IActionResult SaveAsExcelGetStateEmploymentReport(StaffCountDtoFilter dto)
        {
            if (ModelState.IsValid)
            {
                var file = _service.SaveAsExcelGetStateEmploymentReport(dto);
                if (_service.IsValid)
                    return File(file, "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", "Shtat bandligi boʼyicha hisobot.xlsx");

                _service.CopyErrorsToModelState(ModelState);
            }
            return ValidationProblem(ModelState);
        }
        #endregion

        #region OTHERS
        [HttpPost]
        [Authorize(ModuleCode.ReportSmsLogView)]
        [ProducesResponseType(typeof(List<SmsLogReportDto>), 200)]

        public IActionResult GetSmsLogReport(SmsLogReportFilterDto options)
        {
            return Ok(_service.GetSmsLogReportList(options));
        }

        [HttpPost]
        public IActionResult SaveAsExcelGetSmsLog(SmsLogReportFilterDto filter)
        {
            if (ModelState.IsValid)
            {
                var file = _service.SaveAsExcelGetSmsLog(filter);
                if (_service.IsValid)
                    return File(file, "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", "sms-log.xlsx");

                _service.CopyErrorsToModelState(ModelState);
            }
            return ValidationProblem(ModelState);
        }

        [HttpPost]
        [ProducesResponseType(typeof(IEnumerable<OffertaCalculateInfoDto>), 200)]
        public IActionResult GetOffertaCalculate(OffertaCalculateInfoDtoFilter filter)
        {
            return Ok(_service.GetOffertaCalculate(filter));
        }
        [HttpPost]
        [Authorize(ModuleCode.CorruptionApplicationView)]
        [ProducesResponseType(typeof(IEnumerable<CorruptionApplicationDto>), 200)]
        public IActionResult GetAntiCorruptionByRegion(CorruptionApplicationFilterDto options)
        {
            return Ok(_service.GetAntiCorruptionByRegion(options));
        }
        [HttpPost]
        [ProducesResponseType(typeof(IEnumerable<ArbitrationApplicationDto>), 200)]
        public IActionResult GetArbitrationApplicationReport(ArbitrationApplicationDtoFilter options)
        {
            return Ok(_service.GetArbitrationApplicationReport(options));
        }
        [HttpPost]
        [Authorize(ModuleCode.ReportStateAssetApplicationView)]
        [ProducesResponseType(typeof(StateAssetApplicationReportDto), 200)]
        public IActionResult GetStateAssetApplicationReport(StateAssetApplicationFilter filter)
        {
            return Ok(_service.GetStateAssetApplicationReport(filter));
        }
        [HttpPost]
        public IActionResult SaveAsExcelStateAssetApplications(StateAssetApplicationFilter filter)
        {
            if (ModelState.IsValid)
            {
                var file = _service.SaveAsExcelStateAssetApplications(filter);
                if (_service.IsValid)
                    return File(file, "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", "DavlatAktivlari.xlsx");

                _service.CopyErrorsToModelState(ModelState);
            }
            return ValidationProblem(ModelState);
        }

        [HttpPost]
        [ProducesResponseType(typeof(BusinessActivityTypeReportDto), 200)]
        public List<BusinessActivityTypeReportDto> GetBusinessActivityTypeReport(BusinessActivityTypeReprotFilter options)
        {
            return _service.GetBusinessActivityTypeReport(options);
        }

        [HttpPost]
        [ProducesResponseType(typeof(BusinessActivityTypeReportDto), 200)]
        public List<BusinessActivityTypeReportByRegion> GetBusinessActivityTypeReportByRegion(BusinessActivityTypeReportByRegionFilter options)
        {
            return _service.GetBusinessActivityTypeReportByRegion(options);
        }

        [HttpPost]
        public IActionResult SaveAsExcelContractorFundByBank(BusinessActivityTypeReprotFilter dto)
        {
            if (ModelState.IsValid)
            {
                var file = _service.SaveAsExcelContractorFundByBank(dto);
                if (_service.IsValid)
                    return File(file, "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", "TadbirkorJamg'armasi");

                _service.CopyErrorsToModelState(ModelState);
            }
            return ValidationProblem(ModelState);
        }

        [HttpPost]
        public IActionResult SaveAsExcelContractorFundByRegion(BusinessActivityTypeReportByRegionFilter dto)
        {
            if (ModelState.IsValid)
            {
                var file = _service.SaveAsExcelContractorFundByRegion(dto);
                if (_service.IsValid)
                    return File(file, "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", "TadbirkorJamg'armasiViloyatlarKesimida");

                _service.CopyErrorsToModelState(ModelState);
            }
            return ValidationProblem(ModelState);
        }

        #endregion

        #region INTEGRATION

        [HttpPost]
        [Authorize(ModuleCode.ReportSoliqByContractorView)]
        [ProducesResponseType(typeof(GetSoliqReportByContractorDto), 200)]
        public List<GetSoliqReportByContractorDto> GetSoliqReportByContractor(GetSoliqReportByContractorDtoFilter options)
        {
            return _service.GetSoliqReportByContractor(options);
        }
        //[HttpPost]
        //[ProducesResponseType(typeof(AppealReportDto), 200)]
        //public List<AppealReportByTypeReportDto> GetReportByAppealType(AppealReportDtoFilter option)
        //{
        //    return _service.GetAppealReportByTypeReport(option);
        //}
        [HttpPost]
        public IActionResult SaveAsExcelSoliqReportByContractor(GetSoliqReportByContractorDtoFilter dto)
        {
            if (ModelState.IsValid)
            {
                var file = _service.SaveAsExcelSoliqReportByContractor(dto);
                if (_service.IsValid)
                    return File(file, "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", "SoliqReportByContractor.xlsx");

                _service.CopyErrorsToModelState(ModelState);
            }
            return ValidationProblem(ModelState);
        }

        [HttpPost]
        [ProducesResponseType(typeof(List<GetFreeAreaByInnDataDto>), 200)]
        public async Task<IActionResult> GetFreeAreaFromBandlik()
        {
            var result = await _service.GetFreeAreaFromBandlik();

            if (_service.IsValid)
                return Ok(result);

            _service.CopyErrorsToModelState(ModelState);

            return ValidationProblem(ModelState);
        }

        [HttpPost]
        [Authorize(ModuleCode.ReportTadbirkorFundReportView)]
        [ProducesResponseType(typeof(List<TadbirkorFundReportDto>), 200)]
        public IActionResult GetTadbirkorFundReport(TadbirkorFundReportDtoFilter filter)
        {
            return Ok(_service.GetTadbirkorFundReport(filter));
        }
        [HttpPost]
        [Authorize(ModuleCode.CallCenterByRegionReportView)]
        [ProducesResponseType(typeof(List<CallCenterDto>), 200)]
        public IActionResult GetCallCenterByRegion(CallCenterDtoFilter dto)
        {
            return Ok(_service.GetCallCenterByRegion(dto));
        }
        [HttpPost]
        [Authorize(ModuleCode.GetTaxReport)]
        [ProducesResponseType(typeof(PagedResult<TaxCreditreportDto>), 200)]
        public IActionResult GetPagedTaxCreditReport(TaxCreditReportDtoFilterPageOptions dto)
        {
            var res = _service.GetTaxCreditReport(dto);
            if (_service.IsValid)
                return Ok(res);

            _service.CopyErrorsToModelState(ModelState);

            return ValidationProblem(ModelState);
        }
        [HttpPost]
        [Authorize(ModuleCode.GetTaxReport)]
        [ProducesResponseType(typeof(List<TaxCreditreportDto>), 200)]
        public IActionResult GetTaxCreditReport(TaxCreditReportDtoFilter dto)
        {
            var res = _service.GetTaxCreditReport(dto);
            if (_service.IsValid)
                return Ok(res);

            _service.CopyErrorsToModelState(ModelState);

            return ValidationProblem(ModelState);
        }
        [HttpPost]
        public IActionResult SaveAsExcelTaxCreditReport(TaxCreditReportDtoFilter filter)
        {
            if (ModelState.IsValid)
            {
                var file = _service.SaveAsExcelTaxReport(filter);
                if (_service.IsValid)
                    return File(file, "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", "SoliqImtiyozlari.xlsx");

                _service.CopyErrorsToModelState(ModelState);
            }
            return ValidationProblem(ModelState);
        }

        [HttpPost]
        [Authorize(ModuleCode.GetTaxReport)]
        [ProducesResponseType(typeof(List<TaxCreditreportDto>), 200)]
        public IActionResult GetBankCreditApplicationReportByRegionAndDistrict(BankCreditApplicationReportByRegionAndDistrictDtoFilter dto)
        {
            var res = _service.GetBankCreditApplicationReportByRegionAndDistrict(dto);
            if (_service.IsValid)
                return Ok(res);

            _service.CopyErrorsToModelState(ModelState);

            return ValidationProblem(ModelState);
        }
        [HttpPost]
        public IActionResult SaveAsExcelBankCreditePrivilegeReport(BankCreditApplicationReportByRegionAndDistrictDtoFilter filter)
        {
            if (ModelState.IsValid)
            {
                var file = _service.SaveAsExcelBankCreditePrivilegeReport(filter);
                if (_service.IsValid)
                    return File(file, "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", "SoliqImtiyozlari.xlsx");

                _service.CopyErrorsToModelState(ModelState);
            }
            return ValidationProblem(ModelState);
        }
        [HttpPost]
        [Authorize(ModuleCode.BojxonaImtiyozReportByContractorView)]
        [ProducesResponseType(typeof(BojxonaImtiyozReportByContractorDto), 200)]
        public List<BojxonaImtiyozReportByContractorDto> GetBojxonaImtiyozReportByContractor(BojxonaImtiyozReportByContractorDtoFilter options)
        {
            return _service.GetBojxonaImtiyozReportByContractor(options);
        }
        [HttpPost]
        public IActionResult SaveAsExecelBojxonaImtiyozReportByContractor(BojxonaImtiyozReportByContractorDtoFilter dto)
        {
            if (ModelState.IsValid)
            {
                var file = _service.SaveAsExecelBojxonaImtiyozReportByContractor(dto);
                if (_service.IsValid)
                    return File(file, "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", "BojxonaImtiyozReportByContractor.xlsx");

                _service.CopyErrorsToModelState(ModelState);
            }
            return ValidationProblem(ModelState);
        }
        [HttpPost]
        public IActionResult SaveAsExcelCustomsRelief(BojxonaImtiyozReportByContractorDtoFilter filter)
        {
            if (ModelState.IsValid)
            {
                var file = _service.SaveAsExcelCustomsRelief(filter);
                if (_service.IsValid)
                    return File(file, "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", "BojxonaImtiyozlari.xlsx");

                _service.CopyErrorsToModelState(ModelState);
            }
            return ValidationProblem(ModelState);
        }

        [HttpPost]
        [ProducesResponseType(typeof(List<ContractorBankCreditReportResponse>), 200)]
        public async Task<ActionResult> GetBankCreditReport(ContractorBankCreditReportDtoFilter dto)
        {
            var res = await _service.GetBankCreditReport(dto);
            if (_service.IsValid)
                return Ok(res);

            _service.CopyErrorsToModelState(ModelState);

            return ValidationProblem(ModelState);
        }

        [HttpPost]
        [ProducesResponseType(typeof(PagedResult<TaxCreditreportDto>), 200)]
        public async Task<IActionResult> GetPagedBankCreditReport(ContractorBankCreditReportDtoFilterPageOptions dto)
        {
            var res = await _service.GetPagedBankCreditReport(dto);
            if (_service.IsValid)
                return Ok(res);

            _service.CopyErrorsToModelState(ModelState);

            return ValidationProblem(ModelState);
        }
        [HttpPost]
        public IActionResult SaveAsExcelBankCreditReport(ContractorBankCreditReportDtoFilter dto)
        {
            if (ModelState.IsValid)
            {
                var file = _service.SaveAsExcelBankCreditReport(dto);
                if (_service.IsValid)
                    return File(file, "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", "BankCreditReport.xlsx");

                _service.CopyErrorsToModelState(ModelState);
            }
            return ValidationProblem(ModelState);
        }

        [HttpPost]
        public async Task<IActionResult> SaveBankCreditReportAsExcel(ContractorBankCreditReportDtoFilter filter)
        {
            if (ModelState.IsValid)
            {
                var file = await _service.SaveAsExcelBankCreditReportAsync(filter);
                if (_service.IsValid)
                    return File(file, "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", "BankTezkorHisoboti.xlsx");

                _service.CopyErrorsToModelState(ModelState);
            }
            return ValidationProblem(ModelState);
        }
        [HttpPost]
        public async Task<IActionResult> SaveSecondBankCreditReportAsExcel(ContractorBankCreditReportDtoFilter filter)
        {
            if (ModelState.IsValid)
            {
                var file = await _service.SaveAsExcelSecondBankCreditReportAsync(filter);
                if (_service.IsValid)
                    return File(file, "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", "TadbirkorDasturBankReport.xlsx");

                _service.CopyErrorsToModelState(ModelState);
            }
            return ValidationProblem(ModelState);
        }

        #endregion

        #region CORRUPTION

        [HttpPost]
        [Authorize(ModuleCode.ReportCharterMembersRegisterView)]
        [ProducesResponseType(typeof(List<CharterMembersRegisterReportDto>), 200)]
        public IActionResult GetCharterMembersRegisterReport(CharterMembersRegisterFilterDto options)
        {
            return Ok(_service.GetCharterMembersRegisterReportList(options));
        }
        [HttpPost]
        [AllowAnonymous]
        [ProducesResponseType(typeof(List<CharterMembersRegisterReportDto>), 200)]
        public IActionResult GetCharterMembersRegisterReportForChamber(CharterMembersRegisterFilterDto options)
        {
            return Ok(_service.GetCharterMembersRegisterReportList(options));
        }
        [HttpPost]
        public IActionResult SaveAsExcelCharterMembersRegisterReport(CharterMembersRegisterFilterDto filter)
        {
            if (ModelState.IsValid)
            {
                var file = _service.SaveAsExcelCharterMembersRegisterReport(filter);
                if (_service.IsValid)
                    return File(file, "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", "��������������p������.xlsx");

                _service.CopyErrorsToModelState(ModelState);
            }

            return ValidationProblem(ModelState);
        }
        #endregion

        #region SRV
        [HttpPost]
        [Authorize(ModuleCode.ReportSrvServiceGetInfo)]
        public IActionResult GetSrvServiceInfo(GetSrvServiceInfoRequestDto filter)
        {
            return Ok(_service.GetSrvServicesInfo(filter));
        }

        [HttpPost]
        [Authorize(ModuleCode.DeedSwotReportView)]
        public IActionResult GetSrvDeedReport(SrvDeedListReportSortFilter filter)
        {
            return Ok(_service.GetSrvDeedReport(filter));
        }


        [HttpPost]
        [Authorize(ModuleCode.DeedSwotReportView)]
        public IActionResult GetSrvFreeDeedReport(SrvDeedListReportSortFilter filter)
        {
            return Ok(_service.GetSrvFreeDeedReport(filter));
        }

        [HttpPost]
        [Authorize(ModuleCode.DeedSwotReportView)]
        public IActionResult GetSrvFreeDeedReportByFunction(SrvDeedListReportSortFilter filter)
        {
            return Ok(_service.GetSrvFreeDeedReportFromFunction(filter));
        }


		[HttpPost]
		[Authorize(ModuleCode.MonoReportView)]
		public List<MonoApplicationReportDto> MonoApplicationReport(MonoApplicationReportSortFilter filter)
		{
			var data = _service.MonoApplicationReport(filter);
            return data;
		}

		[HttpPost]
		public IActionResult MonoApplicationReportSaveExcel(MonoApplicationReportSortFilter filter)
		{
			if (ModelState.IsValid)
			{
				var file =  _service.MonoApplicationReportSaveExcel(filter);
				if (_service.IsValid)
					return File(file, "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", "BankTezkorHisoboti.xlsx");

				_service.CopyErrorsToModelState(ModelState);
			}
			return ValidationProblem(ModelState);
		}
		[HttpPost]
        public IActionResult SaveAsExcelGetSrvDeedReport(SrvDeedListReportSortFilter options)
        {
            if (ModelState.IsValid)
            {
                Stream? file = _service.SaveAsExcelGetSrvDeedReport(options);
                if (_service.IsValid)
                    return File(file, "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", "deed swot.xlsx");

                _service.CopyErrorsToModelState(ModelState);
            }

            return ValidationProblem(ModelState);
        }

        [HttpPost]
        public IActionResult SaveAsExcelGetSrvFreeDeedReport(SrvDeedListReportSortFilter options)
        {
            if (ModelState.IsValid)
            {
                Stream? file = _service.SaveAsExcelGetSrvFreeDeedReport(options);
                if (_service.IsValid)
                    return File(file, "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", "deed swot.xlsx");

                _service.CopyErrorsToModelState(ModelState);
            }

            return ValidationProblem(ModelState);
        }
        #endregion

        #region Appeal
        [HttpPost]
        [Authorize(ModuleCode.AppealApplicationReport)]
        [ProducesResponseType(typeof(List<AppealReportByTypeReportDto>), 200)]
        public IActionResult GetAppealApplicationReport(AppealReportDtoFilter filter)
        {
            return Ok(_service.GetAppealApplicationReport(filter));
        }

        [HttpPost]
        [Authorize(ModuleCode.CallCenterAppealReportByOkedType)]
        [ProducesResponseType(typeof(List<CallCenterAppealReportByOkedTypeDto>), 200)]
        public IActionResult CallCenterAppealReportByOkedType(CallCenterAppealReportByOkedTypeFilter filter)
        {
            return Ok(_service.CallCenterAppealReportByOkedType(filter));
        }


        [HttpPost]
        [Authorize(ModuleCode.CallCenterReportByWeekReport)]
        [ProducesResponseType(typeof(List<CallCenterReportByWeekDto>), 200)]
        public IActionResult GetCallCenterReportByWeek(CallCenterReportByWeekFilterDto filter)
        {
            return Ok(_service.GetCallCenterReportByWeek(filter));
        }

        [HttpPost]
        public IActionResult SaveAsExcelTaxQqsAylanmaReport(GetTaxQqsAylanmaDtoFilter filter)
        {
            if (ModelState.IsValid)
            {
                var file = _service.SaveAsExcelTaxQqsAylanmaReport(filter);
                if (_service.IsValid)
                    return File(file, "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", "TaxQqsAylanmaReport.xlsx");

                _service.CopyErrorsToModelState(ModelState);
            }

            return ValidationProblem(ModelState);
        }

        [HttpPost]
        public IActionResult SaveAsExcelCallCenterAppealReportByOkedType(CallCenterAppealReportByOkedTypeFilter filter)
        {
            if (ModelState.IsValid)
            {
                var file = _service.SaveAsExcelCallCenterAppealReportByOkedType(filter);
                if (_service.IsValid)
                    return File(file, "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", "Мурожаатлар йуналиши va фаолият тури бўйича ҳисобот.xlsx");

                _service.CopyErrorsToModelState(ModelState);
            }

            return ValidationProblem(ModelState);
        }

        [HttpPost]
        public IActionResult SaveAsExcelGetCallCenterReportByWeek(CallCenterReportByWeekFilterDto filter)
        {
            if (ModelState.IsValid)
            {
                var file = _service.SaveAsExcelGetCallCenterReportByWeek(filter);
                if (_service.IsValid)
                    return File(file, "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", "Ish vaqtida va ish vaqtidan tashqari tushgan murojaatlar bo'yicha hisobot.xlsx");

                _service.CopyErrorsToModelState(ModelState);
            }

            return ValidationProblem(ModelState);
        }
        [HttpPost]
        public IActionResult SaveAsExcelGetCallCenterByRegion(CallCenterDtoFilter filter)
        {
            if (ModelState.IsValid)
            {
                var file = _service.SaveAsExcelGetCallCenterByRegion(filter);
                if (_service.IsValid)
                    return File(file, "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", "Худудлар кесимида мурожаатлар.xlsx");

                _service.CopyErrorsToModelState(ModelState);
            }

            return ValidationProblem(ModelState);
        }

        [HttpPost]
        [Authorize(ModuleCode.ExpiredReportView)]
        public IActionResult GetExpiredContractorsReport(PrtnApplicationAndContractInfoDtoFilter options)
        {
            var data =  _service.GetExpiredContractorsReport(options);
            return Ok(data);
        }



        [HttpPost]
        public IActionResult SaveAsExcelExpiredContractorsReport(PrtnApplicationAndContractInfoDtoFilter options)
        {
            if (ModelState.IsValid)
            {
                Stream? file = _service.SaveAsExcelExpiredContractorsReport(options);
                if (_service.IsValid)
                    return File(file, "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", "Ijro muddati kechikkan.xlsx");

                _service.CopyErrorsToModelState(ModelState);
            }

            return ValidationProblem(ModelState);
        }

        [HttpPost]
        public IActionResult PrintPrtnEmploymentGraphNewReport(PrtnDocumentSortFilterOptions filter)
        {
            Stream? file =  _service.PrintPrtnEmploymentGraphNewReport(filter);
            if (_service.IsValid)
                return File(file, "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", "ApplicationStatus.xlsx");

            _service.CopyErrorsToModelState(ModelState);

            return Ok();
        }
        [HttpPost]
        public IActionResult PrintPrtnApplicationByPetitionInfo(PrtnDocumentSortFilterOptions filter)
        {
            Stream? file = _service.PrintPrtnApplicationByPetitionInfo(filter);
            if (_service.IsValid)
                return File(file, "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", "ApplicationProcessInfo.xlsx");

            _service.CopyErrorsToModelState(ModelState);

            return Ok();
        }

        [HttpPost]
        public IActionResult PrintPrtnApplicationByContractNewInfo(PrtnDocumentSortFilterOptions filter)
        {
            Stream? file = _service.PrintPrtnApplicationByContractNewInfo(filter);
            if (_service.IsValid)
                return File(file, "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", "ContractsInfo.xlsx");

            _service.CopyErrorsToModelState(ModelState);

            return Ok();
        }

        [HttpPost]
        public IActionResult PrintPrtnApplicationByFullInfo(PrtnDocumentSortFilterOptions filter)
        {
            Stream? file = _service.PrintPrtnApplicationByFullInfo(filter);
            if (_service.IsValid)
                return File(file, "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", "ContractsFullInfo.xlsx");

            _service.CopyErrorsToModelState(ModelState);

            return Ok();
        }

        [HttpPost]
        public IActionResult SaveAsExcelAllExpiredContractorsReport(PrtnDocumentSortFilterOptions options)
        {
            if (ModelState.IsValid)
            {
                Stream? file = _service.SaveAsExcelAllExpiredContractorsReport(options);
                if (_service.IsValid)
                    return File(file, "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", "Ijro muddati kechikkan.xlsx");

                _service.CopyErrorsToModelState(ModelState);
            }

            return ValidationProblem(ModelState);
        }

        [HttpPost]
        [Authorize(ModuleCode.ExpiredReportView)]
        public IActionResult GetExpiredReportByContractors(PrtnDocumentSortFilterOptions options)
        {
            var data = _service.GetExpiredContractorsReportPageResult(options);
            if (_service.IsValid)
                return Ok(data);

            _service.CopyErrorsToModelState(ModelState);

            return ValidationProblem(ModelState);
        }

        //[HttpPost]
        //[Authorize(ModuleCode.ExpiredReportView)]
        //public IActionResult GetPrtnApplicationAndContractInfoMethod(PrtnApplicationAndContractInfoDtoFilter options)
        //{
        //    var data = _service.GetPrtnApplicationAndContractInfoMethod(options);
        //    if (_service.IsValid)
        //        return Ok(data);

        //    _service.CopyErrorsToModelState(ModelState);

        //    return ValidationProblem(ModelState);
        //}

        [HttpPost]
        //[Authorize(ModuleCode.ReportPrtnEmployeeJobView)]
        [ProducesResponseType(typeof(List<PrtnEmployeeJobReportDto>), 200)]
        public IActionResult GetPrtnEmployeeJobReport(PrtnEmployeeJobDtoFilter filter)
        {
            return Ok(_service.GetPrtnEmployeeJobReport(filter));
        }
        #endregion
    }
}
