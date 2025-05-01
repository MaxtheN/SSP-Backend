import ApiService from './api.service';
const ManualService = {
    GetOrganizationAsSelectList(parentId, authorizedOnly, inspectionOnly) {
        return ApiService.get(`/Manual/GetOrganizationAsSelectList?parentId=${parentId}&authorizedOnly=${authorizedOnly}&inspectionOnly=${inspectionOnly}`);
    },
    GetOkedAsSelectList(level) {
        return ApiService.get(`/Manual/GetOkedAsSelectList?level=${level}`);
    },
    PrtnContractTypeSelectList() {
        return ApiService.get(`/Manual/PrtnContractTypeSelectList`);
    },
    GetOrganizationNameByLocation(regionId, districtId, prtnContractTypeId) {
        return ApiService.get(`/Manual/GetOrganizationNameByLocation?regionId=${regionId}&districtId=${districtId}&prtnContractTypeId=${prtnContractTypeId}`);
    },
    GetMonthSelectList() {
        return ApiService.get(`/Manual/GetMonthSelectList`);
    },
    RegionSelectList() {
        return ApiService.get(`/Manual/RegionSelectList`);
    },
    DistrictSelectList(regionId) {
        return ApiService.get(`/Manual/DistrictSelectList?regionId=${regionId}`);
    },
    MfySelectList(districtId) {
        return ApiService.get(`/Manual/MfySelectList?districtId=${districtId}`);
    },
    MfySelectListForApplication(regionId, districtId) {
        return ApiService.get(`/Manual/MfySelectList/${regionId}/${districtId}`);
    },
    BusinessSectorSelectList() {
        return ApiService.get(`/Manual/BusinessSectorSelectList`);
    },
    GenderSelectList() {
        return ApiService.get(`/Manual/GenderSelectList`);
    },
    ApplicantTypeSelectList() {
        return ApiService.get(`/Manual/ApplicantTypeSelectList`);
    },
    OrganizationTypeSelectList() {
        return ApiService.get(`/Manual/OrganizationTypeSelectList`);
    },
    EmploymentTypeSelectList() {
        return ApiService.get(`/Manual/EmploymentTypeSelectList`);
    },
    ProposalSubjectSelectList() {
        return ApiService.get(`/Manual/ProposalSubjectSelectList`);
    },
    ApplicationTypeStepSelectList(applicationTypeId) {
        return ApiService.get(`/Manual/ApplicationTypeStepSelectList?applicationTypeId=${applicationTypeId}`);
    },
    ProposalDisclosureSelectList() {
        return ApiService.get(`/Manual/ProposalDisclosureSelectList`);
    },
    ArbitrationApplicationTypeSelectList() {
        return ApiService.get(`/Manual/ArbitrationApplicationTypeSelectList`);
    },
    ArbitrationCourtSelectList() {
        return ApiService.get(`/Manual/ArbitrationCourtSelectList`);
    },
    ArbitrationCourtSelectList() {
        return ApiService.get(`/Manual/ArbitrationCourtSelectList`);
    },
    CompanyTypeSelectList() {
        return ApiService.get(`/Manual/CompanyTypeSelectList`);
    },
    BankSelectList() {
        return ApiService.get(`/Manual/BankSelectList`);
    },
    StateSelectList() {
        return ApiService.get(`/Manual/StateSelectList`);
    },
    PrtnRejectReasonSelectList(prtnContractTypeId) {
        return ApiService.get(`/Manual/PrtnRejectReasonSelectList?prtnContractTypeId=${prtnContractTypeId}`);
    },
    ContactTypeSelectList() {
        return ApiService.get(`/Manual/ContactTypeSelectList`);
    },
    ClaimApplicationTypeSelectList() {
        return ApiService.get(`/Manual/ClaimApplicationTypeSelectList`);
    },
    ClaimResponsibleTypeSelectList() {
        return ApiService.get(`/Manual/ClaimResponsibleTypeSelectList`);
    },
    ClaimThemeSelectList() {
        return ApiService.get(`/Manual/ClaimThemeSelectList`);
    },
    CurrencySelectList() {
        return ApiService.get('/Manual/CurrencySelectList');
    },
    NeedChamberServiceSelectList() {
        return ApiService.get('/Manual/NeedChamberServiceSelectList');
    },
    ContractorActivityTypeSelectList() {
        return ApiService.get('/Manual/ContractorActivityTypeSelectList');
    },
    ContractorCategorySelectList() {
        return ApiService.get('/Manual/ContractorCategorySelectList');
    },
    CorruptionReviewTypeSelectList() {
        return ApiService.get('/Manual/CorruptionReviewTypeSelectList');
    },
    JoinAntiCorruptionResultTypeSelectList() {
        return ApiService.get('/Manual/JoinAntiCorruptionResultTypeSelectList');
    },
    ContractorUnionActivityTypeSelectList() {
        return ApiService.get('/Manual/ContractorUnionActivityTypeSelectList');
    },
    DualEducationTypeSelectList() {
        return ApiService.get('/Manual/DualEducationTypeSelectList');
    },
    PositionSelectList() {
        return ApiService.get('/Manual/PositionSelectList');
    },
    InstituteSelectList() {
        return ApiService.get('/Manual/InstituteSelectList');
    },
    SpecialtySelectList(instituteId) {
        return ApiService.get(`/Manual/SpecialtySelectList?instituteId=${instituteId}`);
    },
    GetOkedAsSelectList() {
        return ApiService.get(`/Manual/GetOkedAsSelectList?level=5`);
    },
    EducationItemSelectList() {
        return ApiService.get(`/Manual/EducationItemSelectList`);
    },
    AppealFormatTypeSelectList() {
        return ApiService.get(`/Manual/AppealFormatTypeSelectList`);
    },
    AppealTypeSelectList() {
        return ApiService.get(`/Manual/AppealTypeSelectList`);
    },
    AppealDescriptionSelectList() {
        return ApiService.get(`/Manual/AppealDescriptionSelectList`);
    },
    AppealTypeArriveSelectList() {
        return ApiService.get(`/Manual/AppealTypeArriveSelectList`);
    },
    GetAsSelectList() {
        return ApiService.get(`/Manual/GetAsSelectList`);
    },
    GetBillingUniversityList() {
        return ApiService.get(`/Manual/GetBillingUniversityList`);
    },
    GetUniversity(id) {
        return ApiService.get(`/Manual/GetUniversity?organizationId=${id}`);
    },

    CreateInstituteBilling(data) {
        return ApiService.post(`/Manual/CreateInstituteBilling`, data);
    },

    GetBillingSpecialityList(id) {
        return ApiService.get(`/Manual/GetBillingSpecialityList?organizationId=${id}`);
    },
    GetSpeciality(id) {
        return ApiService.get(`/Manual/GetSpeciality?specialityId=${id}`);
    },
    CreateSpecialtyBilling(data) {
        return ApiService.post(`/Manual/CreateSpecialtyBilling`, data);
    }
};
export default ManualService;
