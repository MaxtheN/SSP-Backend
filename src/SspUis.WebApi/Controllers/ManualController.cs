using Microsoft.AspNetCore.Mvc;
using SspUis.BizLogicLayer.EnumServices;
using SspUis.BizLogicLayer.ManualServices;
using WEBASE.AspNet;
using WEBASE.Models;

namespace SspUis.WebApi.Controllers
{
    [Authorize]
    [ApiController]
    [Route("[controller]/[action]")]
    public class ManualController : ControllerBase
    {
        private readonly IManualService _service;

        public ManualController(IManualService service)
        {
            _service = service;
        }

        [HttpGet]
        public IEnumerable<BizLogicLayer.ModuleServices.ModuleGroupSelectListDto> GetModuleSelectList()
        {
            return _service.GetModuleSelectList();
        }


        [HttpGet]
        public SelectList<int> StateSelectList()
        {
            return _service.StateSelectList();
        }

        [HttpGet]
        public SelectList<long> ContractorSelectList()
        {
            return _service.BankInExcelSelectList();
        }
        [HttpGet]
        public SelectList<int> LanguageDegreeSelectList()
        {
            return _service.LanguageDegreeSelectList();
        }

        [HttpGet]
        public SelectList<int> ProposalSubjectSelectList()
        {
            return _service.ProposalSubjectSelectList();
        }

        [HttpGet]
        public SelectList<int> ProposalDisclosureSelectList()
        {
            return _service.ProposalDisclosureSelectList();
        }

        [HttpGet]
        public SelectList<int> ApplicantTypeSelectList()
        {
            return _service.ApplicantTypeSelectList();
        }
        [HttpGet]
        public SelectList<int> OrganizationGroupSelectList()
        {
            return _service.OrganizationGroupSelectList();
        }

		[HttpGet]
		public SelectList<int> OrganizationCorruptionGroupSelectList()
		{
			return _service.OrganizationCorruptionGroupSelectList();
		}

		[HttpGet]
        public SelectList<int> BusinessSectorSelectList()
        {
            return _service.BusinessSectorSelectList();
        }

        [HttpGet]
        public SelectList<int> CompanyTypeSelectList()
        {
            return _service.CompanyTypeSelectList();
        }

        [HttpGet]
        public SelectList<int> EmploymentTypeSelectList()
        {
            return _service.EmploymentTypeSelectList();
        }

        [HttpGet]
        public SelectList<int> RegionSelectList()
        {
            return _service.RegionSelectList();
        }

        [HttpGet]
        public SelectList<long> MfySelectList(int? districtId)
        {
            return _service.MfySelectList(districtId);
        }

        [HttpGet]
        public SelectList<int> DistrictSelectList(int? regionId)
        {
            return _service.DistrictSelectList(regionId);
        }

        [HttpGet]
        [AllowAnonymous]
        public SelectList<int, LanguageSelectListDto> LanguageSelectList()
        {
            return _service.LanguageSelectList();
        }

        [HttpGet]
        public SelectList<int> GenderSelectList()
        {
            return _service.GenderSelectList();
        }

        [HttpGet]
        public SelectList<int> NotificationTypeSelectList()
        {
            return _service.NotificationTypeSelectList();
        }

        [HttpGet]
        public SelectList<int> StatusSelectList()
        {
            return _service.StatusSelectList();
        }

        [HttpPost]
        public SelectList<int> SignOrganizationTypeSelectList([FromBody] SignOrganizationTypeSelectListDtoFilter filter)
        {
            return _service.SignOrganizationTypeSelectList(filter);
        }

        [HttpGet]
        public SelectList<int> GetMonthSelectList()
        {
            return _service.GetMonthSelectList();
        }
        [HttpGet]
        public SelectList<int> TableSelectList()
        {
            return _service.TableSelectList();
        }
        [HttpGet]
        public IActionResult ClearContractorInfo(int contractorId, bool isOnlyDocument)
        {
            if (ModelState.IsValid)
            {
                _service.ClearContractorInfo(contractorId, isOnlyDocument);

                if (_service.IsValid)
                    return Ok();

                _service.CopyErrorsToModelState(ModelState);
            }

            return ValidationProblem(ModelState);
        }
        [HttpGet]
        public SelectList<int> CustomJobTypeSelectList()
        {
            return _service.CustomJobTypeSelectList();
        }
        [HttpGet]
        public SelectList<int> BankCodeSelectList()
        {
            return _service.BankCodeSelectList();
        }
        [HttpGet]
        public SelectList<int> MeetingTypeSelectList()
        {
            return _service.MeetingTypeSelectList();
        }
        [HttpGet]
        public SelectList<int> ApplicationModelCodeSelectList()
        {
            return _service.ApplicationModelCodeSelectList();
        }

        [HttpGet]
        public SelectList<int> BirthRegionSelectList()
        {
            return _service.BirthRegionSelectList();
        }

        [HttpGet]
        public SelectList<int> ApplicationTypeStepSelectList(int applicationTypeId)
        {
            return _service.ApplicationTypeStepSelectList(applicationTypeId);
        }

        [HttpGet]
        public SelectList<int> ApplicationTypeSelectList()
        {
            return _service.ApplicationTypeSelectList();
        }
        [HttpGet]
        public SelectList<int> AppealFormatTypeSelectList()
        {
            return _service.AppealFormatTypeSelectList();
        }
        [HttpGet]
        public SelectList<int> AppealTypeSelectList()
        {
            return _service.AppealTypeSelectList();
        }

        [HttpGet()]
        public SelectList<int> OrganizationAsSelectListByGroup([FromQuery] int[]? groupId)
        {
            return _service.OrganizationAsSelectListByGroup(groupId);
        }
        [HttpGet]
        public SelectList<int> RatingSelectList()
        {
            return _service.RatingSelectList();
        }
        [HttpGet]
        public SelectList<int> ContractorTypeSelectList()
        {
            return _service.ContractorTypeSelectList();
        }
        [HttpGet]
        public SelectList<int> OkedTypeSelectList()
        {
            return _service.OkedTypeSelectList();
        }
        [HttpGet]
        public SelectList<int> AccountNumberSelectList()
        {
            return _service.AccountNumberSelectList();
        }
        [HttpGet]
        public SelectList<int> AsSelectList()
        {
            return _service.AsSelectList();
        }
    }
}