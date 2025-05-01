import ApiService from '../api.service';
const ArbitrationResultService = {
   GetList(data) {
      return ApiService.post('/Arbitration/ArbitrationResult/GetList', data);
   },
   Get(id) {
      if (id == 0) {
         return ApiService.get('/Arbitration/ArbitrationResult/Get');
      } else {
         return ApiService.get(`/Arbitration/ArbitrationResult/Get/${id}`);
      }
   },

   Update(data) {
      if (data.id == 0) {
         return ApiService.post(`/Arbitration/ArbitrationResult/Create`, data);
      } else {
         return ApiService.post(`/Arbitration/ArbitrationResult/Update`, data);
      }
   },

   Delete(id) {
      return ApiService.post(`/Arbitration/ArbitrationResult/Delete?id=${id}`);
   },
   Sign(data) {
      return ApiService.post(`/Arbitration/ArbitrationResult/Sign`, data);
   },

   UploadFile(data) {
      return ApiService.post('/Arbitration/ArbitrationResult/UploadFile', data);
   },
   Cancel(data) {
      return ApiService.post('/Arbitration/ArbitrationResult/Cancel', data);
   },
   DownloadFile(fileId) {
      return ApiService.print(`/Arbitration/ArbitrationResult/DownloadFile/${fileId}`);
   },
   DownloadTemplate() {
      return ApiService.print(`/Arbitration/ArbitrationResult/DownloadTemplate`);
   },
   DeleteFile(fileId) {
      return ApiService.post(`/Arbitration/ArbitrationResult/DeleteFile/${fileId}`);
   },
   GetByArbitrationCourtApplicationId(arbitrationCourtApplicationId) {
      return ApiService.get(
         `/Arbitration/ArbitrationResult/GetByArbitrationCourtApplicationId?arbitrationCourtApplicationId=${arbitrationCourtApplicationId}`
      );
   }
};

export default ArbitrationResultService;
