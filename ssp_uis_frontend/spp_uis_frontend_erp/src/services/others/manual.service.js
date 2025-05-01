import ApiService from '../api.service';

const ManualService = {
   ProposalSubjectSelectList() {
      return ApiService.get('/Manual/ProposalSubjectSelectList');
   },
   ProposalDisclosureSelectList() {
      return ApiService.get('/Manual/ProposalDisclosureSelectList');
   },
   CompanyTypeSelectList() {
      return ApiService.get('/Manual/CompanyTypeSelectList');
   },
   ContractorSelectList() {
      return ApiService.get('/Manual/ContractorSelectList');
   },
   GetModuleSelectList() {
      return ApiService.get('manual/GetModuleSelectList');
   },
   StateSelectList() {
      return ApiService.get('manual/StateSelectList');
   },
   LanguageSelectList() {
      return ApiService.get('manual/LanguageSelectList');
   },
   GenderSelectList() {
      return ApiService.get('manual/GenderSelectList');
   },
   ApplicantTypeSelectList() {
      return ApiService.get('Manual/ApplicantTypeSelectList');
   },
   BusinessSectorSelectList() {
      return ApiService.get('Manual/BusinessSectorSelectList');
   },
   NotificationTypeSelectList() {
      return ApiService.get('manual/NotificationTypeSelectList');
   },
   StatusSelectList() {
      return ApiService.get('manual/StatusSelectList');
   },
   SignOrganizationTypeSelectList(data = {}) {
      return ApiService.post(`manual/SignOrganizationTypeSelectList?incluceBusinessman`, data);
   },
   GetListByDocumentId(tableId, docId) {
      return ApiService.get(`DocumentChangeLog/GetListByDocumentId/${tableId}/${docId}`);
   },
   EmploymentTypeSelectList() {
      return ApiService.get('hrm/manual/EmploymentTypeSelectList');
   },
   WorkScheduleKindSelectList() {
      return ApiService.get('hrm/Manual/WorkScheduleKindSelectList');
   },
   EmpAppointOrderTypeSelectList() {
      return ApiService.get('hrm/manual/EmpAppointOrderTypeSelectList');
   },
   TimesheetIndicatorSelectList() {
      return ApiService.get('hrm/manual/TimesheetIndicatorSelectList');
   },
   TimesheetTypeSelectList() {
      return ApiService.get('hrm/manual/TimesheetTypeSelectList');
   },
   RoundingTypeSelectList() {
      return ApiService.get('hrm/manual/RoundingTypeSelectList');
   },
   MinimumValueTypeSelectList() {
      return ApiService.get('hrm/manual/MinimumValueTypeSelectList');
   },
   CalculateByTimeTypeSelectList() {
      return ApiService.get('hrm/manual/CalculateByTimeTypeSelectList');
   },
   CalculationMethodSelectList() {
      return ApiService.get('hrm/manual/CalculationMethodSelectList');
   },
   CalculationTypeSelectList() {
      return ApiService.get('hrm/manual/CalculationTypeSelectList');
   },
   TariffScaleTypeSelectList() {
      return ApiService.get('hrm/manual/TariffScaleTypeSelectList');
   },
   LimitOperTypeSelectList() {
      return ApiService.get('hrm/manual/LimitOperTypeSelectList');
   },
   ContractorCategorySelectList() {
      return ApiService.get('memship/Manual/ContractorCategorySelectList');
   },
   MemshipContractTypeSelectList() {
      return ApiService.get('memship/Manual/MemshipContractTypeSelectList');
   },
   PositionPeriodSelectList() {
      return ApiService.get('hrm/Manual/PositionPeriodSelectList');
   },
   MediationTypeSelectList() {
      return ApiService.get('claim/Manual/MediationTypeSelectList');
   },
   MediationResultSelectList() {
      return ApiService.get('claim/Manual/MediationResultSelectList');
   },
   ClaimNeedCourtSelectList() {
      return ApiService.get('claim/Manual/ClaimNeedCourtSelectList');
   },
   StaffingTypeSelectList() {
      return ApiService.get('hrm/Manual/StaffingTypeSelectList');
   },
   ClaimApplicationTypeSelectList() {
      return ApiService.get('claim/Manual/ClaimApplicationTypeSelectList');
   },
   CorruptionReviewTypeSelectList() {
      return ApiService.get('corruption/Manual/CorruptionReviewTypeSelectList');
   },
   JoinAntiCorruptionResultTypeSelectList() {
      return ApiService.get('corruption/Manual/JoinAntiCorruptionResultTypeSelectList');
   },
   CustomJobTypeSelectList() {
      return ApiService.get('Manual/CustomJobTypeSelectList');
   },
   BankCodeSelectList() {
      return ApiService.get('Manual/BankCodeSelectList');
   },
   TableSelectList() {
      return ApiService.get('Manual/TableSelectList');
   },
   MeetingTypeSelectList() {
      return ApiService.get('Manual/MeetingTypeSelectList');
   },
   AnswerTypeSelectList() {
      return ApiService.get('quiz/Manual/AnswerTypeSelectList');
   },
   InspectionTypeSelectList() {
      return ApiService.get('quiz/Manual/InspectionTypeSelectList');
   },
   QuestionnaireTypeSelectList() {
      return ApiService.get('quiz/Manual/QuestionnaireTypeSelectList');
   },
   OrganizationGroupSelectList() {
      return ApiService.get('Manual/OrganizationGroupSelectList');
   },
   OrganizationAsSelectListByGroup(id) {
      if (Array.isArray(id)) {
         let url = '/Manual/OrganizationAsSelectListByGroup?';
         id.forEach((e) => {
            url = url + 'groupId=' + e + '&';
         });
         return ApiService.get(url.slice(0, -1));
      } else {
         return ApiService.get(`/Manual/OrganizationAsSelectListByGroup?groupId=${id}`);
      }
   },

   ApplicationModelCodeSelectList() {
      return ApiService.get('Manual/ApplicationModelCodeSelectList');
   },
   BirthRegionSelectList() {
      return ApiService.get('Manual/BirthRegionSelectList');
   },
   GetAgeGroupSelectList() {
      return ApiService.get('Manual/GetAgeGroupSelectList');
   },
   LanguageDegreeSelectList() {
      return ApiService.get('Manual/LanguageDegreeSelectList');
   },

   EmployeeHigherEduDegreeSelectList() {
      return ApiService.get('/hrm/Manual/EmployeeHigherEduDegreeSelectList');
   },
   GetMonthSelectList() {
      return ApiService.get('Manual/GetMonthSelectList');
   },
   ServicePriceTypeSelectList() {
      return ApiService.get('survey/Manual/ServicePriceTypeSelectList');
   },
   ClaimResponsibleTypeSelectList() {
      return ApiService.get('/claim/Manual/ClaimResponsibleTypeSelectList');
   },
   ArbitrationApplicationTypeSelectList() {
      return ApiService.get('Arbitration/Manual/ArbitrationApplicationTypeSelectList');
   },
   ArbitrationCourtSelectList() {
      return ApiService.get('/Arbitration/Manual/ArbitrationCourtSelectList');
   },
   OrderToSendBusinessTripTypeSelectList() {
      return ApiService.get('/hrm/Manual/OrderToSendBusinessTripTypeSelectList');
   },
   TempCalcKindTypeSelectList() {
      return ApiService.get('/hrm/Manual/TempCalcKindTypeSelectList');
   },
   EmployeeSickLeaveTypeSelectList() {
      return ApiService.get('/hrm/Manual/EmployeeSickLeaveTypeSelectList');
   },
   OpfSelectList() {
      return ApiService.get('/memship/Manual/OpfSelectList');
   },
   AppealTypeSelectList() {
      return ApiService.get('/Manual/AppealTypeSelectList');
   },
   AppealFormatTypeSelectList() {
      return ApiService.get('/Manual/AppealFormatTypeSelectList');
   },
   ContractorTypeSelectList() {
      return ApiService.get('/Manual/ContractorTypeSelectList');
   },
   OkedTypeSelectList() {
      return ApiService.get('/Manual/OkedTypeSelectList');
   },
   RatingSelectList() {
      return ApiService.get('/Manual/RatingSelectList');
   },
   EmpAppointOrderTypeSelectList() {
      return ApiService.get('hrm/Manual/EmpAppointOrderTypeSelectList');
   },
   MissedDaysTypeSelectList() {
      return ApiService.get('hrm/Manual/MissedDaysTypeSelectList');
   },
   AccountNumberSelectList() {
      return ApiService.get('Manual/AccountNumberSelectList');
   },
   OrganizationCorruptionGroupSelectList() {
      return ApiService.get('/Manual/OrganizationCorruptionGroupSelectList');
   }
};

export default ManualService;
