import ApiService from '../api.service';
const MediationService = {
   GetList(data) {
      return ApiService.post('/Mediation/GetList', data);
   },
   Get(id) {
      if (id == 0 || id === null || id === undefined) {
         return ApiService.get('/Mediation/Get');
      } else {
         return ApiService.get(`/Mediation/Get/${id}`);
      }
   },
   GetByPlanId(id) {
      return ApiService.get(`/Mediation/GetByPlanId?planId=${id}`);
   },
   Update(data) {
      if (data.id == 0) {
         return ApiService.post(`/Mediation/Create`, data);
      } else {
         return ApiService.post(`/Mediation/Update`, data);
      }
   },
   UploadFiles(data) {
      return ApiService.formData(`/Mediation/UploadFile`, data);
   },
   DownloadFile(fileId) {
      return ApiService.print(`/Mediation/DownloadFile/${fileId}`);
   },
   DeleteFile(fileId) {
      return ApiService.post(`/Mediation/DeleteFile/${fileId}`);
   },
   Delete(id) {
      return ApiService.post(`/Mediation/Delete/${id}`);
   },
   Accept(data) {
      return ApiService.post(`/Mediation/Accept`, data);
   },
   Cancel(id) {
      return ApiService.post(`/Mediation/Cancel/${id}`);
   },
   DownloadPdf(id2, lang) {
      return ApiService.print(`/Mediation/DownloadPdf?id2=${id2}&lang=${lang}`);
   }
};
export default MediationService;
