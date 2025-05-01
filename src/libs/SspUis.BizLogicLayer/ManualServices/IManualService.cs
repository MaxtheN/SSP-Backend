using SspUis.BizLogicLayer.EnumServices;
using StatusGeneric;
using System.Collections.Generic;
using WEBASE.Models;

namespace SspUis.BizLogicLayer.ManualServices
{
    public interface IManualService : IStatusGeneric
    {
        IEnumerable<ModuleServices.ModuleGroupSelectListDto> GetModuleSelectList();
        SelectList<int> TableSelectList();
        SelectList<int> LanguageDegreeSelectList();
        SelectList<int> StateSelectList();
        SelectList<long> BankInExcelSelectList();
        SelectList<int, LanguageSelectListDto> LanguageSelectList();
        SelectList<int> GenderSelectList();
        SelectList<int> NotificationTypeSelectList();
        SelectList<int> StatusSelectList();
        SelectList<int> RegionSelectList();
        SelectList<long> MfySelectList(int? districtId);
        SelectList<int> DistrictSelectList(int? regionId);
        SelectList<int> ApplicantTypeSelectList();
        SelectList<int> OrganizationGroupSelectList();
		SelectList<int> OrganizationCorruptionGroupSelectList();
		SelectList<int> BusinessSectorSelectList();
        SelectList<int> CompanyTypeSelectList();
        SelectList<int> ProposalSubjectSelectList();
        SelectList<int> ProposalDisclosureSelectList();
        SelectList<int> EmploymentTypeSelectList();
        SelectList<int> SignOrganizationTypeSelectList(SignOrganizationTypeSelectListDtoFilter filter);
        SelectList<int> GetMonthSelectList();
        SelectList<int> CustomJobTypeSelectList();
        SelectList<int> MeetingTypeSelectList();
        SelectList<int> BankCodeSelectList();
        SelectList<int> ApplicationModelCodeSelectList();
        SelectList<int> BirthRegionSelectList();
        void ClearContractorInfo(int contractorId, bool isOnlyDocument = false);
        SelectList<int> ApplicationTypeStepSelectList(int applicationTypeId);
        SelectList<int> ApplicationTypeSelectList();
        SelectList<int> AppealFormatTypeSelectList();
        SelectList<int> AppealTypeSelectList();
        SelectList<int> OrganizationAsSelectListByGroup(int[]? groupId);
        SelectList<int> ContractorTypeSelectList();
        SelectList<int> RatingSelectList();
        SelectList<int> OkedTypeSelectList();

        SelectList<int> AccountNumberSelectList();
        SelectList<int> AsSelectList();

    }
}
