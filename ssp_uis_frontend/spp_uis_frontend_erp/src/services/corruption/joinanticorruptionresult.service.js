import ApiService from '../api.service';
const JoinAntiCorruptionResultService = {
   GetList(data) {
      return ApiService.post('/Corruption/JoinAntiCorruptionResult/GetList', data);
   },
   Get(id) {
      if (id == 0 || id === null || id === undefined) {
         return ApiService.get('/Corruption/JoinAntiCorruptionResult/Get');
      } else {
         return ApiService.get(`/Corruption/JoinAntiCorruptionResult/Get/${id}`);
      }
   },
   Update(data) {
      if (data.id == 0) {
         return ApiService.post(`/Corruption/JoinAntiCorruptionResult/Create`, data);
      } else {
         return ApiService.post(`/Corruption/JoinAntiCorruptionResult/Update`, data);
      }
   },
   GetByApplication(applicationId) {
      return ApiService.get(`/Corruption/JoinAntiCorruptionResult/GetByApplicationId/${applicationId}`);
   },
   GetAsSelectList() {
      return ApiService.get('/Corruption/JoinAntiCorruptionResult/GetAsSelectList');
   },
   Accept(data) {
      return ApiService.post(`/Corruption/JoinAntiCorruptionResult/Accept`, data);
   },
   Cancel(data) {
      return ApiService.post(`/Corruption/JoinAntiCorruptionResult/Cancel`, data);
   },
   Delete(id) {
      return ApiService.post(`/Corruption/JoinAntiCorruptionResult/Delete/${id}`);
   },
   DownloadPdf(id) {
      return ApiService.get(`/Corruption/JoinAntiCorruptionResult/DownloadPdf?id=${id}&lang=ru`);
   },
   UploadFiles(files) {
      return ApiService.formData(`/Corruption/JoinAntiCorruptionResult/UploadFile`, files);
   },
   DeleteFile(id) {
      return ApiService.post(`/Corruption/JoinAntiCorruptionResult/DeleteFile/${id}`);
   },
   DownloadFile(id) {
      return ApiService.get(`/Corruption/JoinAntiCorruptionResult/DownloadFile/${id}`);
   },
};
export default JoinAntiCorruptionResultService;
