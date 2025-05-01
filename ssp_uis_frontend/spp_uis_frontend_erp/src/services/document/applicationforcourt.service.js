// eslint-disable-next-line import/no-named-as-default
import ApiService from '../api.service';
const ApplicationForCourtService = {
   GetList(data) {
      return ApiService.post('/ApplicationForCourt/GetList', data);
   },
   Get(id) {
      if (id == 0 || id === null || id === undefined) {
         return ApiService.get('/ApplicationForCourt/Get');
      } else {
         return ApiService.get(`/ApplicationForCourt/Get/${id}`);
      }
   },
   Update(data) {
      if (data.id == 0) {
         return ApiService.post(`/ApplicationForCourt/Create`, data);
      } else {
         return ApiService.post(`/ApplicationForCourt/Update`, data);
      }
   },
   GetByMediationId(mediationId) {
      return ApiService.get(`/ApplicationForCourt/GetByMediationId?mediationId=${mediationId}`);
   },
   UploadFiles(data) {
      return ApiService.formData(`/ApplicationForCourt/UploadFiles`, data);
   },
   Delete(id) {
      return ApiService.post(`/ApplicationForCourt/Delete/${id}`);
   },
   GetForFiles(mediationId) {
      return ApiService.get(`/ApplicationForCourt/GetForFiles?mediationId=${mediationId}`);
   },
   ChekFromXalqBank(mediationId) {
      return ApiService.get(`/ApplicationForCourt/ChekFromXalqBank?mediationId=${mediationId}`);
   },
   ChekAllBanks(mediationId) {
      return ApiService.get(`/ApplicationForCourt/ChekAllBanks?mediationId=${mediationId}`);
   },
   Send(id) {
      return ApiService.post(`/ApplicationForCourt/Send/${id}`);
   },
   Accept(data) {
      return ApiService.post(`/ApplicationForCourt/Accept`, data);
   },
   AcceptForEmployee(data) {
      return ApiService.post(`/ApplicationForCourt/AcceptForEmployee`, data);
   },
   NotAccept(id) {
      return ApiService.post(`/ApplicationForCourt/NotAccept/${id}`);
   },
   DownloadFileWithQrCode(id) {
      return ApiService.print(`ApplicationForCourt/DownloadFileWithQrCode?id=${id}`);
   },
   DownloadBankDocument(id, lang) {
      return ApiService.print(`ApplicationForCourt/DownloadXalqBankDocument?mediationId=${id}&lang=${lang}`);
   },
   GetSudRegionList() {
      return ApiService.get('/ApplicationForCourt/GetSudRegionList');
   },
   GetSudDistrictList(id) {
      return ApiService.get(`/ApplicationForCourt/GetSudDistrictList?regionId=${id}`);
   },
   GetSudPostReasonList() {
      return ApiService.get('/ApplicationForCourt/GetSudPostReasonList');
   },
   GetSudParticipantTypeList() {
      return ApiService.get('/ApplicationForCourt/GetSudParticipantTypeList');
   },
   GetSudCategoryList() {
      return ApiService.get('/ApplicationForCourt/GetSudCategoryList');
   },
   GetSudCurrencyList() {
      return ApiService.get('/ApplicationForCourt/GetSudCurrencyList');
   },
   GetSudEntityTypeList() {
      return ApiService.get('/ApplicationForCourt/GetSudEntityTypeList');
   },
   GetSudClaimKindList() {
      return ApiService.get('/ApplicationForCourt/GetSudClaimKindList');
   },
   GetSudDocumentTypesList() {
      return ApiService.get('/ApplicationForCourt/GetSudDocumentTypesList');
   },
   GetSudAmountCategoryList() {
      return ApiService.get('/ApplicationForCourt/GetSudAmountCategoryList');
   },
   GetSudCountryList() {
      return ApiService.get('/ApplicationForCourt/GetSudCountryList');
   },
   GetSudRegionList() {
      return ApiService.get('/ApplicationForCourt/GetSudRegionList');
   },
   SudUploadFileIntegration(data) {
      return ApiService.post('/ApplicationForCourt/SudUploadFileIntegration', data);
   },
   SendSudIntegration(data) {
      return ApiService.post('/ApplicationForCourt/SendSudIntegration', data);
   },
   GetSudCourtList() {
      return ApiService.get('/ApplicationForCourt/GetSudCourtList');
   }
};
export default ApplicationForCourtService;
