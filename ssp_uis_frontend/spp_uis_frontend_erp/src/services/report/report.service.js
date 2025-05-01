import ApiService from '../api.service';

const ReportService = {
   GetPrtnApplicationAndContractInfo(data) {
      return ApiService.post(`/Report/GetPrtnApplicationAndContractInfo`, data);
   },
   GetPrtnApplicationAndContractInfoPaged(data) {
      return ApiService.post(`/Report/GetPrtnApplicationAndContractInfoPaged`, data);
   },
   GetPrtnApplicationAndContractInfoByRegion(data) {
      return ApiService.post(`/Report/GetPrtnApplicationAndContractInfoByRegion`, data);
   },
   GetPrtnApplicationByContractType(data) {
      return ApiService.post(`/Report/GetPrtnApplicationByContractType`, data);
   },
   GetSrvDeedReport(data) {
      return ApiService.post(`/Report/GetSrvDeedReport`, data);
   },
   GetSrvFreeDeedReport(data) {
      return ApiService.post(`/Report/GetSrvFreeDeedReport`, data);
   },
   SaveAsExcelCustomsRelief(data) {
      return ApiService.printtemp(`/Report/SaveAsExcelCustomsRelief`, data);
   },
   SaveAsExcelClaimApplicationReport(data) {
      return ApiService.printtemp(`/Report/SaveAsExcelClaimApplicationReport`, data);
   },
   SaveAsExecelForPrtnCreditDemand(data) {
      return ApiService.printtemp(`/Report/SaveAsExecelForPrtnCreditDemand`, data);
   },
   SaveAsExecelForPrtnCreditDemandPaged(data) {
      return ApiService.printtemp(`/Report/SaveAsExecelForPrtnCreditDemandPaged`, data);
   },
   PrtnApplicationByContractTypeExcel(data) {
      return ApiService.printtemp(`/Report/PrtnApplicationByContractTypeExcel`, data);
   },
   GetPrtnCreditDemandInfo(data) {
      return ApiService.post(`/Report/GetPrtnCreditDemandInfo`, data);
   },
   GetPrtnCreditDemandInfoPaged(data) {
      return ApiService.post(`/Report/GetPrtnCreditDemandInfoPaged`, data);
   },
   GetBankCreditReport(data) {
      return ApiService.post(`/Report/GetBankCreditReport`, data);
   },
   GetPagedBankCreditReport(data) {
      return ApiService.post(`/Report/GetPagedBankCreditReport`, data);
   },
   GetSoliqReportByContractor(data) {
      return ApiService.post(`/Report/GetSoliqReportByContractor`, data);
   },
   SaveAsExecel(data) {
      return ApiService.printtemp(`/Report/SaveAsExecel`, data);
   },
   SaveAsExecelForSum(data) {
      return ApiService.printtemp(`/Report/SaveAsExecelForSum`, data);
   },
   SaveAsExcelSoliqReportByContractor(data) {
      return ApiService.printtemp(`/Report/SaveAsExcelSoliqReportByContractor`, data);
   },
   GetEmployeeCard(data) {
      return ApiService.post(`/Report/GetEmployeeCard`, data);
   },
   GetPrntExpiredDocuments(data) {
      return ApiService.post(`/Report/GetPrntExpiredDocuments`, data);
   },
   GetPrtnCreditDemandInfoByBank(data) {
      return ApiService.post(`/Report/GetPrtnCreditDemandInfoByBank`, data);
   },
   GetTadbirkorFundReport(data) {
      return ApiService.post(`/Report/GetTadbirkorFundReport`, data);
   },
   GetStaffingSingleReport(data) {
      return ApiService.post(`/Report/GetStaffingSingleReport`, data);
   },
   GetStaffingSingleReportForParent(data) {
      return ApiService.post(`/Report/GetStaffingSingleReportForParent`, data);
   },
   GetBojxonaImtiyozReportByContractor(data) {
      return ApiService.post(`/Report/GetBojxonaImtiyozReportByContractor`, data);
   },
   GetOffertaCalculate(data) {
      return ApiService.post(`/Report/GetOffertaCalculate`, data);
   },
   GetFreeAreaFromBandlik(data) {
      return ApiService.post(`/Report/GetFreeAreaFromBandlik`, data);
   },
   PrtnEmploymentGraphReport(data, config) {
      return ApiService.post2(`/Report/PrtnEmploymentGraphReport`, data, config);
   },
   GetMemshipDocsInfoReestr(data) {
      return ApiService.post(`/Report/GetMemshipDocsInfoReestr`, data);
   },
   GetMemshipApplicationAndContractInfo(data) {
      return ApiService.post(`/Report/GetMemshipApplicationAndContractInfo`, data);
   },
   GetTaxCreditReport(data) {
      return ApiService.post(`/Report/GetTaxCreditReport`, data);
   },
   GetPagedTaxCreditReport(data) {
      return ApiService.post(`/Report/GetPagedTaxCreditReport`, data);
   },
   PrtnEmploymentGraphExcel(data) {
      return ApiService.printtemp(`/Report/PrtnEmploymentGraphExcel`, data);
   },
   SaveAsExcelGetHrmCommands(data) {
      return ApiService.printtemp(`/Report/SaveAsExcelGetHrmCommands`, data);
   },
   SaveAsExcelBojxonaImtiyoz(data) {
      return ApiService.printtemp(`/Report/SaveAsExcelBojxonaImtiyoz`, data);
   },
   SaveasExcelSoliqImtiyoz(data) {
      return ApiService.printtemp(`/Report/SaveasExcelSoliqImtiyoz`, data);
   },
   SaveAsExcelDavaktivImtiyoz(data) {
      return ApiService.printtemp(`/Report/SaveAsExcelDavaktivImtiyoz`, data);
   },
   SaveAsExcelStateAssetApplications(data) {
      return ApiService.printtemp(`/Report/SaveAsExcelStateAssetApplications`, data);
   },
   SaveBankCreditReportAsExcel(data) {
      return ApiService.printtemp(`/Report/SaveBankCreditReportAsExcel`, data);
   },
   SaveSecondBankCreditReportAsExcel(data) {
      return ApiService.printtemp(`/Report/SaveSecondBankCreditReportAsExcel`, data);
   },
   SaveAsExcelGetSrvDeedReport(data) {
      return ApiService.printtemp(`/Report/SaveAsExcelGetSrvDeedReport`, data);
   },
   SaveAsExcelGetSrvFreeDeedReport(data) {
      return ApiService.printtemp(`/Report/SaveAsExcelGetSrvFreeDeedReport`, data);
   },
   SaveAsExcelTaxCreditReport(data) {
      return ApiService.printtemp(`/Report/SaveAsExcelTaxCreditReport`, data);
   },
   GetBankCreditApplicationReportByRegionAndDistrict(data) {
      return ApiService.post(`/Report/GetBankCreditApplicationReportByRegionAndDistrict`, data);
   },
   GetBusinessActivityTypeReport(data) {
      return ApiService.post(`/Report/GetBusinessActivityTypeReport`, data);
   },

   SaveAsExcelContractorFundByBank(data) {
      return ApiService.printtemp('/Report/SaveAsExcelContractorFundByBank', data);
   },
   SaveAsExcelFund(data) {
      return ApiService.post(`/Report/SaveAsExcelFund`, data);
   },
   GetBusinessActivityTypeReportByRegion(data) {
      return ApiService.post(`/Report/GetBusinessActivityTypeReportByRegion`, data);
   },
   SaveAsExcelContractorFundByRegion(data) {
      return ApiService.printtemp('/Report/SaveAsExcelContractorFundByRegion', data);
   },
   SaveAsExcelGetMemshipDocsInfo(data) {
      return ApiService.printtemp(`/Report/SaveAsExcelGetMemshipDocsInfo`, data);
   },
   GetStaffCountReport(data) {
      return ApiService.post('/Report/GetStaffCountReport', data);
   },
   GetStaffCountByGenderReport(data) {
      return ApiService.post('/Report/GetStaffCountByGenderReport', data);
   },
   GetStateAssetApplicationReport(data) {
      return ApiService.post('/Report/GetStateAssetApplicationReport', data);
   },
   SaveAsExcelBankCreditePrivilegeReport(data) {
      return ApiService.printtemp('Report/SaveAsExcelBankCreditePrivilegeReport', data);
   },
   SaveAsExecelForPrtnCreditBank(data) {
      return ApiService.printtemp('/Report/SaveAsExecelForPrtnCreditBank', data);
   },
   GetMemshipReports(data) {
      return ApiService.post('/Report/GetMemshipReports', data);
   },
   GetPaidMemshipReport(data) {
      return ApiService.post('/Report/GetPaidMemshipReport', data);
   },
   GetMemshipApplicationReportByOrganizations(data) {
      return ApiService.post('/Report/GetMemshipApplicationReportByOrganizations', data);
   },
   GetMemshipApplicationReportByPersonType(data) {
      return ApiService.post('/Report/GetMemshipApplicationReportByPersonType', data);
   },
   PrtnApplicationByContractTypeExcel(data) {
      return ApiService.printtemp('/Report/PrtnApplicationByContractTypeExcel', data);
   },
   PrintPrtnEmploymentGraphNewReport(data) {
      return ApiService.printtemp('/Report/PrintPrtnEmploymentGraphNewReport ', data);
   },
   PrintPrtnApplicationByPetitionInfo(data) {
      return ApiService.printtemp('/Report/PrintPrtnApplicationByPetitionInfo', data);
   },
   PrintPrtnApplicationByContractNewInfo(data) {
      return ApiService.printtemp('/Report/PrintPrtnApplicationByContractNewInfo ', data);
   },
   PrintPrtnApplicationByFullInfo(data) {
      return ApiService.printtemp('/Report/PrintPrtnApplicationByFullInfo ', data);
   },
   ClaimApplicationAmount(data) {
      return ApiService.post('/Report/GetClaimApplicationAmount', data);
   },
   PrtnEmploymentGraphNewReport(data) {
      return ApiService.post('/Report/PrtnEmploymentGraphNewReport ', data);
   },
   PrtnApplicationByPetitionInfo(data) {
      return ApiService.post('/Report/PrtnApplicationByPetitionInfo ', data);
   },
   GetPrtnApplicationByContractNewInfo(data) {
      return ApiService.post('/Report/GetPrtnApplicationByContractNewInfo ', data);
   },
   GetPrtnApplicationByFullInfo(data) {
      return ApiService.post('/Report/GetPrtnApplicationByFullInfo ', data);
   },
   SummaOfClaimApplicationReport(data) {
      return ApiService.post('/Report/SummaOfClaimApplicationReport', data);
   },
   ReceivedClaimApplicationReport(data) {
      return ApiService.post('/Report/ReceivedClaimApplicationReport', data);
   },
   AppealsSentToClaimApplicationReport(data) {
      return ApiService.post('/Report/AppealsSentToClaimApplicationReport', data);
   },
   SaveAsExcelGetMemshipReport(data) {
      return ApiService.printtemp('/Report/SaveAsExcelGetMemshipReport', data);
   },
   SaveAsExcelGetPaidMemshipReport(data) {
      return ApiService.printtemp('/Report/SaveAsExcelGetPaidMemshipReport', data);
   },
   SaveAsExcelMemshipReportByPersonType(data) {
      return ApiService.printtemp('/Report/SaveAsExcelMemshipReportByPersonType', data);
   },
   SaveAsExcelMemshipReportByOrganization(data) {
      return ApiService.printtemp('/Report/SaveAsExcelMemshipReportByOrganization', data);
   },
   GetSmsLogReport(data) {
      return ApiService.post('/Report/GetSmsLogReport', data);
   },
   SaveAsExcelGetSmsLog(data) {
      return ApiService.printtemp('/Report/SaveAsExcelGetSmsLog', data);
   },
   SaveAsExcelSummaOfClaimApplication(data) {
      return ApiService.printtemp('/Report/SaveAsExcelSummaOfClaimApplication', data);
   },
   SaveAsExcelGetCallCenterReportByWeek(data) {
      return ApiService.printtemp('/Report/SaveAsExcelGetCallCenterReportByWeek', data);
   },
   SaveAsExcelReceivedClaimApplication(data) {
      return ApiService.printtemp('/Report/SaveAsExcelReceivedClaimApplication', data);
   },
   SaveAsExcelAppealsSentToClaimApplication(data) {
      return ApiService.printtemp('/Report/SaveAsExcelAppealsSentToClaimApplication', data);
   },
   SaveAsExcelAllIntegrationReportByContractor() {
      return ApiService.printtemp('Report/SaveAsExcelAllIntegrationReportByContractor');
   },
   SaveAsExcelSummaryReportByokedTypes(data) {
      return ApiService.printtemp('Report/SaveAsExcelSummaryReportByokedTypes', data);
   },

   SaveAsExcelAllIntegrationReportByRegion(filter) {
      return ApiService.printtemp('/Report/SaveAsExcelAllEmployeeCountReport', filter);
   },
   SaveAsExcelTaxQqsAylanmaReport(filter) {
      return ApiService.printtemp('Report/SaveAsExcelTaxQqsAylanmaReport', filter);
   },
   // coruptionreport
   GetCharterMembersRegisterReport(data) {
      return ApiService.post('Report/GetCharterMembersRegisterReport', data);
   },
   GetCorruptionByRegion(data) {
      return ApiService.post('Report/GetAntiCorruptionByRegion', data);
   },
   SaveAsExcelCharterMembersRegisterReport(data) {
      return ApiService.printtemp('Report/SaveAsExcelCharterMembersRegisterReport', data);
   },
   GetPrtnApplicationByContractTypePaged(data) {
      return ApiService.post('Report/GetPrtnApplicationByContractTypePaged', data);
   },
   GetMemshipContract(data) {
      return ApiService.post('/Report/GetMemshipContract', data);
   },
   GetClaimApplicationReport(data) {
      return ApiService.post('/Report/GetClaimApplicationReport', data);
   },
   ClaimApplicationReport(data) {
      return ApiService.post('/Report/ClaimApplicationReport', data);
   },
   GetAppealApplicationReport(data) {
      return ApiService.post('/Report/GetAppealApplicationReport', data);
   },
   GetCallCenterReportByWeek(data) {
      return ApiService.post('/Report/GetCallCenterReportByWeek', data);
   },
   GetArbitrationApplicationReport(data) {
      return ApiService.post('/Report/GetArbitrationApplicationReport', data);
   },
   MonoApplicationReport(data) {
      return ApiService.post('/Report/MonoApplicationReport', data);
   },
   GetTaxQqsAylanmaReport(data) {
      return ApiService.post('Report/GetTaxQqsAylanmaReport', data);
   },
   GetContractorCategoryType(data) {
      return ApiService.post(`/Report/GetContractorCategoryType`, data);
   },
   GetClaimApplicationAmount(data) {
      return ApiService.post('/Report/GetClaimApplicationAmount', data);
   },
   GetCallCenterByRegion(data) {
      return ApiService.post('/Report/GetCallCenterByRegion', data);
   },
   CallCenterAppealReportByOkedType(data) {
      return ApiService.post('/Report/CallCenterAppealReportByOkedType', data);
   },
   SaveAsExcelContractorCategoryType(data) {
      return ApiService.printtemp(`/Report/SaveAsExcelContractorCategoryType`, data);
   },
   SaveAsExcelGetCallCenterByRegion(data) {
      return ApiService.printtemp(`/Report/SaveAsExcelGetCallCenterByRegion`, data);
   },
   SaveAsExcelCallCenterAppealReportByOkedType(data) {
      return ApiService.printtemp(`/Report/SaveAsExcelCallCenterAppealReportByOkedType`, data);
   },
   SaveAsExecelByContractType(data) {
      return ApiService.printtemp(`/Report/SaveAsExecelByContractType`, data);
   },
   ReportOnProjectImplement(data) {
      return ApiService.post('/Report/ReportOnProjectImplementationAndBenefitsGranted', data);
   },
   GetExpiredContractorsReport(data, config) {
      return ApiService.post2('/Report/GetExpiredContractorsReport', data, config);
   },
   GetExpiredReportByContractors(data, config) {
      return ApiService.post2('/Report/GetExpiredReportByContractors', data, config);
   },
   SaveAsExcelExpiredContractorsReport(data) {
      return ApiService.printtemp('/Report/SaveAsExcelExpiredContractorsReport', data);
   },
   SaveAsExcelAllExpiredContractorsReport(data) {
      return ApiService.printtemp('/Report/SaveAsExcelAllExpiredContractorsReport', data);
   },
   MonoApplicationReportSaveExcel(data) {
      return ApiService.printtemp('/Report/MonoApplicationReportSaveExcel', data);
   },
   GetPrtnEmployeeJobReport(data) {
      return ApiService.post('/Report/GetPrtnEmployeeJobReport', data);
   }
};
export default ReportService;
