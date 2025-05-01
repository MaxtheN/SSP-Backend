import ApiService from '../api.service';
const MediationPlanService = {
   GetList(data) {
      return ApiService.post('/MediationPlan/GetList', data);
   },
   GetByApplication(applicationId) {
      return ApiService.get('/MediationPlan/GetByApplication?applicationId=' + applicationId);
   },
   GetAsSelectList() {
      return ApiService.get(`/MediationPlan/GetAsSelectList`);
   },
   Get(id) {
      if (id == 0 || id === null || id === undefined) {
         return ApiService.get('/MediationPlan/Get');
      } else {
         return ApiService.get(`/MediationPlan/Get/${id}`);
      }
   },
   Update(data) {
      if (data.id == 0) {
         return ApiService.post(`/MediationPlan/Create`, data);
      } else {
         return ApiService.post(`/MediationPlan/Update`, data);
      }
   },
   Accept(data) {
      return ApiService.post(`/MediationPlan/Accept`, data);
   },
   Cancel(data) {
      return ApiService.post(`/MediationPlan/Cancel`, data);
   },
   Delete(id) {
      return ApiService.post(`/MediationPlan/Delete/${id}`);
   },
   DownloadPdf(id2, lang) {
      return ApiService.print(`/MediationPlan/DownloadPdf?id2=${id2}&lang=${lang}`);
   }
};
export default MediationPlanService;
