using Microsoft.AspNetCore.Mvc;
using SspUis.BizLogicLayer;
using SspUis.BizLogicLayer.AppealDescriptionServices;
using SspUis.BizLogicLayer.AppealTypeArriveServices;
using SspUis.BizLogicLayer.Arbitration;
using SspUis.BizLogicLayer.BankServices;
using SspUis.BizLogicLayer.Claim;
using SspUis.BizLogicLayer.Claim.ClaimThemeServices;
using SspUis.BizLogicLayer.Corruption.ContractorUnionActivityTypeServices;
using SspUis.BizLogicLayer.Corruption.ManualServices;
using SspUis.BizLogicLayer.CurrencyServices;
using SspUis.BizLogicLayer.DistrictServices;
using SspUis.BizLogicLayer.EnumServices;
using SspUis.BizLogicLayer.Hrm.ContractorActivityGroupServices;
using SspUis.BizLogicLayer.Hrm.ContractorActivityTypeServices;
using SspUis.BizLogicLayer.Hrm.DualEducationTypeServices;
using SspUis.BizLogicLayer.Hrm.FixedMinimumValueServices;
using SspUis.BizLogicLayer.Hrm.InstituteServices;
using SspUis.BizLogicLayer.Hrm.NeedChamberServiceServices;
using SspUis.BizLogicLayer.Hrm.PositionClassificationServices;
using SspUis.BizLogicLayer.Hrm.SpecialtyServices;
using SspUis.BizLogicLayer.ManualServices;
using SspUis.BizLogicLayer.Memship;
using SspUis.BizLogicLayer.MfyServices;
using SspUis.BizLogicLayer.My.ManualServices;
using SspUis.BizLogicLayer.OkedServices;
using SspUis.BizLogicLayer.OrganizationServices;
using SspUis.BizLogicLayer.PositionServices;
using SspUis.BizLogicLayer.PrtnContractTypeServices;
using SspUis.BizLogicLayer.PrtnRejectReasonServices;
using SspUis.BizLogicLayer.RegionServices;
using SspUis.BizLogicLayer.ReportServices;
using SspUis.DataLayer.Repositories;
using SspUis.Integration.Billing.Services;
using WEBASE.AspNet;
using WEBASE.Models;

namespace SspUis.My.WebApi.Controllers
{
    [ApiController]
    [Route("[controller]/[action]")]
    public class ManualController : WebaseController
    {
        private readonly IOrganizationService _organizationService;
        private readonly IOkedService _okedService;
        private readonly IPrtnContractTypeService _prtnContractTypeService;
        private readonly IPrtnRejectReasonService _prtnRejectReasonService;
        private readonly IRegionService _regionService;
        private readonly IBankService _bankService;
        private readonly IDistrictService _districtService;
        private readonly IMfyService _mfyService;
        private readonly IMemshipManualService _memshipService;
        private readonly IClaimThemeService _claimThemeService;
        private readonly IManualService _manualService;
        private readonly IClaimManualService _claimManualService;
        private readonly IArbitrationManualService _arbitrationManualService;
        private readonly IMyManualService _myService;
        private readonly INeedChamberServiceService _needChamberServiceService;
        private readonly IContractorActivityGroupService _contractorActivityGroupService;
        private readonly IContractorActivityTypeService _contractorActivityTypeService;
        private readonly ICurrencyService _currencyService;
        private readonly ICorruptionManualService _corruptionService;
        private readonly IContractorUnionActivityTypeService _contractorUnionActivityType;
        private readonly IDualEducationTypeService _dualEduTypeService;
        private readonly IPositionService _positionService;
        private readonly IInstituteService _instituteService;
        private readonly ISpecialtyService _specialtyService;
        private readonly IEducationItemService _educationItem;
        private readonly IFixedMinimumValueService _fixedMinimum;
        private readonly IPositionClassificationService _positionClassification;
        private readonly IBillingService _billingService;
        private readonly IInstituteBillingService _instituteBillingService;
        private readonly ISpecialtyBillingService _specialtyBillingService;

        public ManualController(IOrganizationService organizationService,
            IOkedService okedService,
            IManualService manualService,
            IPrtnContractTypeService prtnContractTypeService,
            IPrtnRejectReasonService prtnRejectReasonService,
            IRegionService regionService,
            IBankService bankService,
            IDistrictService districtService,
            IMfyService mfyService,
            IClaimManualService claimManualService,
            IClaimThemeService claimThemeService,
            IMyManualService myService,
            INeedChamberServiceService needChamberServiceService,
            IContractorActivityGroupService contractorActivityGroupService,
            ICurrencyService currencyService,
            IContractorActivityTypeService contractorActivityTypeService,
            ICorruptionManualService corruptionService,
            IContractorUnionActivityTypeService contractorUnionActivityType,
            IMemshipManualService memshipService,
            IDualEducationTypeService dualEduTypeService,
            IPositionService positionService,
            IInstituteService instituteService,
            ISpecialtyService specialtyService,
            IEducationItemService educationItem,
            IArbitrationManualService arbitrationManualService,
            IPositionClassificationService positionClassification,
            IBillingService billingService,
            IInstituteBillingService instituteBillingService,
            ISpecialtyBillingService specialtyBillingService)
            : base(AppSettings.Instance.ControllerConfig)
        {
            _organizationService = organizationService;
            _okedService = okedService;
            _manualService = manualService;
            _claimManualService = claimManualService;
            _prtnContractTypeService = prtnContractTypeService;
            _prtnRejectReasonService = prtnRejectReasonService;
            _regionService = regionService;
            _bankService = bankService;
            _districtService = districtService;
            _mfyService = mfyService;
            _currencyService = currencyService;
            _educationItem = educationItem;
            _claimThemeService = claimThemeService;
            _corruptionService = corruptionService;
            _contractorUnionActivityType = contractorUnionActivityType;
            _positionClassification = positionClassification;
            this._myService = myService;
            _needChamberServiceService = needChamberServiceService;
            this._contractorActivityGroupService = contractorActivityGroupService;
            this._contractorActivityTypeService = contractorActivityTypeService;
            this._memshipService = memshipService;
            this._dualEduTypeService = dualEduTypeService;
            this._positionService = positionService;
            this._instituteService = instituteService;
            this._specialtyService = specialtyService;
            this._arbitrationManualService = arbitrationManualService;
            _billingService = billingService;
            _instituteBillingService = instituteBillingService;
            _specialtyBillingService = specialtyBillingService;
        }

        [HttpGet]
        public SelectList<int> ContractorCategorySelectList()
        {
            return _memshipService.ContractorCategorySelectList();
        }

        [HttpGet()]
        [ProducesResponseType(typeof(SelectList<int>), 200)]
        public IActionResult GetOrganizationAsSelectList(int? parentId = null, bool authorizedOnly = false, bool inspectionOnly = false)
        {
            return Ok(_organizationService.AsSelectList(parentId, authorizedOnly, inspectionOnly));
        }

        [HttpGet()]
        [ProducesResponseType(typeof(string), 200)]
        public IActionResult GetOrganizationNameByLocation(int regionId, int districtId, int prtnContractTypeId)
        {
            if (ModelState.IsValid)
            {
                var result = _organizationService.GetOrganizationNameByLocation(regionId, districtId, prtnContractTypeId);

                if (_organizationService.IsValid)
                    return Ok(result);

                _organizationService.CopyErrorsToModelState(ModelState);
            }

            return ValidationProblem(ModelState);
        }
        [HttpGet()]
        [ProducesResponseType(typeof(SelectList<int>), 200)]
        public IActionResult GetAsSelectList()
        {
            return Ok(_positionClassification.AsSelectList());
        }
        [HttpGet]
        [ProducesResponseType(typeof(SelectList<int>), 200)]
        public IActionResult GetOkedAsSelectList(int level = 5)
        {
            return Ok(_okedService.AsSelectList(level));
        }

        [HttpGet]
        [ProducesResponseType(typeof(SelectList<int>), 200)]
        public IActionResult GetFixedMinimumValueAsSelectList()
        {
            return Ok(_fixedMinimum.AsSelectList());
        }

        [HttpGet]
        public SelectList<int> TableSelectList()
        {
            return _manualService.TableSelectList();
        }


        [HttpGet]
        public SelectList<int> StateSelectList()
        {
            return _manualService.StateSelectList();
        }

        [HttpGet]
        [AllowAnonymous]
        public SelectList<int, LanguageSelectListDto> LanguageSelectList()
        {
            return _manualService.LanguageSelectList();
        }

        [HttpGet]
        public SelectList<int> GenderSelectList()
        {
            return _manualService.GenderSelectList();
        }

        [HttpGet]
        public SelectList<int> NotificationTypeSelectList()
        {
            return _manualService.NotificationTypeSelectList();
        }

        [HttpGet]
        public SelectList<int> StatusSelectList()
        {
            return _manualService.StatusSelectList();
        }

        [HttpPost]
        public SelectList<int> SignOrganizationTypeSelectList([FromBody] SignOrganizationTypeSelectListDtoFilter filter)
        {
            return _manualService.SignOrganizationTypeSelectList(filter);
        }

        [HttpGet]
        public SelectList<int> PrtnContractTypeSelectList()
        {
            return _prtnContractTypeService.AsSelectList();
        }


        [HttpGet]
        public SelectList<int> GetMonthSelectList()
        {
            return _manualService.GetMonthSelectList();
        }

        [HttpGet]
        [AllowAnonymous]
        public SelectList<int> ProposalSubjectSelectList()
        {
            return _manualService.ProposalSubjectSelectList();
        }

        [HttpGet]
        [AllowAnonymous]
        public SelectList<int> ProposalDisclosureSelectList()
        {
            return _manualService.ProposalDisclosureSelectList();
        }


        [HttpGet]
        [AllowAnonymous]
        public SelectList<int> ApplicantTypeSelectList()
        {
            return _manualService.ApplicantTypeSelectList();
        }

        [HttpGet]
        [AllowAnonymous]
        public SelectList<int> BusinessSectorSelectList()
        {
            return _manualService.BusinessSectorSelectList();
        }

        [HttpGet]
        [AllowAnonymous]
        public SelectList<int> CompanyTypeSelectList()
        {
            return _manualService.CompanyTypeSelectList();
        }

        [HttpGet]
        [AllowAnonymous]
        public SelectList<int> EmploymentTypeSelectList()
        {
            return _manualService.EmploymentTypeSelectList();
        }

        [HttpGet]
        [AllowAnonymous]
        public SelectList<int> RegionSelectList()
        {
            return _manualService.RegionSelectList();
        }

        [HttpGet]
        [AllowAnonymous]
        public SelectList<long> MfySelectList(int? districtId)
        {
            return _manualService.MfySelectList(districtId);
        }

        [HttpGet]
        [AllowAnonymous]
        public SelectList<int> DistrictSelectList(int? regionId)
        {
            return _manualService.DistrictSelectList(regionId);
        }

        [HttpGet]
        public SelectList<int> PrtnRejectReasonSelectList(int? prtnContractTypeId = null)
        {
            return _prtnRejectReasonService.AsSelectList(prtnContractTypeId);
        }

        [HttpGet("{regionId}")]
        [AllowAnonymous]
        public SelectList<int> DistrictSelectList(int regionId)
        {
            return _districtService.AsSelectList(regionId);
        }

        [HttpGet("{regionId}/{districtid}")]
        [AllowAnonymous]
        public SelectList<long> MfySelectList(int? regionId, int? districtid)
        {
            return _mfyService.AsSelectList(regionId, districtid);
        }

        [HttpGet]
        public SelectList<int> BankSelectList()
        {
            return _bankService.AsSelectList();
        }

        [HttpGet]
        public SelectList<int> ClaimThemeSelectList(int? langId)
        {
            return _claimThemeService.AsSelectList(langId);
        }

        [HttpGet]
        public SelectList<int> ClaimApplicationTypeSelectList(int? langId)
        {
            return _claimManualService.ClaimApplicationTypeSelectList(langId);
        }

        [HttpGet]
        public SelectList<int> ClaimResponsibleTypeSelectList(int? langId)
        {
            return _claimManualService.ClaimResponsibleTypeSelectList(langId);
        }

        [HttpGet]
        public SelectList<int> ArbitrationApplicationTypeSelectList()
        {
            return _arbitrationManualService.ArbitrationApplicationTypeSelectList();
        }

        [HttpGet]
        public SelectList<int> ArbitrationCourtSelectList()
        {
            return _arbitrationManualService.ArbitrationCourtSelectList();
        }

        [HttpGet]
        public SelectList<int> ContactTypeSelectList()
        {
            return _myService.ContactTypeSelectList();
        }


        [HttpGet]
        public SelectList<int> NeedChamberServiceSelectList(int? groupId)
        {
            return _needChamberServiceService.AsSelectList(groupId);
        }

        [HttpGet]
        public SelectList<int> ContractorActivityGroupSelectList()
        {
            return _contractorActivityGroupService.AsSelectList();
        }

        [HttpGet]
        public SelectList<int> ContractorActivityTypeSelectList()
        {
            return _contractorActivityTypeService.AsSelectList();
        }

        [HttpGet]
        public SelectList<int> CurrencySelectList(int langId)
        {
            return _currencyService.AsSelectList(langId);
        }

        [HttpGet]
        public SelectList<int> CorruptionReviewTypeSelectList()
        {
            return _corruptionService.CorruptionReviewTypeSelectList();
        }

        [HttpGet]
        public SelectList<int> JoinAntiCorruptionResultTypeSelectList()
        {
            return _corruptionService.JoinAntiCorruptionResultTypeSelectList();
        }

        [HttpGet]
        public SelectList<int> ContractorUnionActivityTypeSelectList()
        {
            return _contractorUnionActivityType.AsSelectList();
        }

        [HttpGet]
        [ProducesResponseType(typeof(SelectList<int>), 200)]
        public IActionResult DualEducationTypeSelectList()
        {
            return Ok(_dualEduTypeService.AsSelectList());
        }

        [HttpGet]
        [ProducesResponseType(typeof(SelectList<int>), 200)]
        public IActionResult PositionSelectList()
        {
            return Ok(_positionService.AsSelectList());
        }

        [HttpGet]
        [ProducesResponseType(typeof(SelectList<int>), 200)]
        public IActionResult InstituteSelectList()
        {
            return Ok(_instituteService.AsSelectList());
        }

        [HttpGet]
        [ProducesResponseType(typeof(SelectList<int>), 200)]
        public IActionResult SpecialtySelectList(int? instituteId = null)
        {
            return Ok(_specialtyService.AsSelectList(instituteId));
        }
        [HttpGet]
        public SelectList<int> EducationItemSelectList()
        {
            return _educationItem.AsSelectList();
        }

        [HttpGet]
        public SelectList<int> ApplicationTypeStepSelectList(int applicationTypeId)
        {
            return _manualService.ApplicationTypeStepSelectList(applicationTypeId);
        }
        [HttpGet]
        public SelectList<int> AppealFormatTypeSelectList()
        {
            return _manualService.AppealFormatTypeSelectList();
        }

        [HttpGet]
        public SelectList<int> AppealTypeSelectList()
        {
            return _manualService.AppealTypeSelectList();
        }
        [HttpGet]
        public SelectList<int> AppealDescriptionSelectList(
            [FromServices] IAppealDescriptionService appealDescriptionService,
            bool hasParent)
        {
            return appealDescriptionService.AsSelectList(hasParent);
        }
        [HttpGet]
        public SelectList<int> AppealTypeArriveSelectList(
            [FromServices] IAppealTypeArriveService appealTypeArriveService,
            bool hasParent)
        {
            return appealTypeArriveService.AsSelectList();
        }
        [HttpGet]
        [ProducesResponseType(200)]
        public async Task<IActionResult> GetBillingUniversityList()
        {
            var result = await _billingService.GetUniversityList();
            if (result == null || result.Count == 0)
                return BadRequest(new { success = false, message = "Ma'lumot topilmadi!" });
            return Ok(result);
        }
        [HttpGet]
        [ProducesResponseType(200)]
        public async Task<IActionResult> GetBillingSpecialityList([FromQuery] int organizationId)
        {
            if (organizationId <= 0)
            {
                return BadRequest("Notog'ri organizationId.");
            }

            var specialityList = await _billingService.GetSpecialityList(organizationId);

            if (specialityList == null)
            {
                return NotFound("Maxsusliklar ro'yxati topilmadi");
            }

            return Ok(specialityList);
        }
        [HttpGet]
        [ProducesResponseType(200)]
        public async Task<IActionResult> GetUniversity([FromQuery] int organizationId)
        {
            if (organizationId <= 0)
            {
                return BadRequest("Notog'ri organizationId.");
            }

            var university = await _billingService.GetUniversity(organizationId);

            if (university == null)
                return NotFound($"{organizationId} bu id bilan Universitet topilmadi!");

            return Ok(university);
        }
        [HttpGet]
        [ProducesResponseType(200)]
        public async Task<IActionResult> GetSpeciality([FromQuery] int specialityId)
        {
            if (specialityId <= 0)
            {
                return BadRequest("Notog'ri organizationId.");
            }

            var speciality = await _billingService.GetSpeciality(specialityId);

            if (speciality == null)
                return NotFound("Maxsusliklar topilmadi");

            return Ok(speciality);
        }
        [HttpPost]
        public IActionResult CreateInstituteBilling([FromBody] CreateInstituteBillingDlDto dto)
        {
            try
            {
                var result = _instituteBillingService.Create(dto);

                if (result != null)
                    return Ok(new { id = result.Id, message = "Institute billing muvaffaqiyatli qo'shildi." });
                else
                    return BadRequest("Institut qo'shilmadi.");
            }
            catch (InvalidOperationException ex)
            {
                return Conflict(new { message = ex.Message });
            }
            catch (Exception ex)
            {
                return BadRequest(new { success = false, message = ex.Message});
            }
        }
        [HttpPost]
        public IActionResult CreateSpecialtyBilling([FromBody] CreateSpecialtyBillingDlDto dto)
        {
            try
            {
                var result = _specialtyBillingService.Create(dto);

                if (result != null)
                    return Ok(new { id = result.Id, message = "Mutaxassislik muvaffaqiyatli qo'shildi." });
                else
                    return BadRequest("Mutaxassislik qo'shilmadi.");
            }
            catch (InvalidOperationException ex)
            {
                return Conflict(new { message = ex.Message });
            }
            catch (Exception ex)
            {
                return BadRequest(new { success = false, message = ex.Message });
            }
        }
    }
}