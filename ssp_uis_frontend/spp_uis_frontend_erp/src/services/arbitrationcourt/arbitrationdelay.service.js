import ApiService from '../api.service';
const ArbitrationDelayService = {
   GetList(data) {
      return ApiService.post('/Arbitration/ArbitrationDelay/GetList', data);
   },
   Get(id) {
      if (id == 0) {
         return ApiService.get('/Arbitration/ArbitrationDelay/Get');
      } else {
         return ApiService.get(`/Arbitration/ArbitrationDelay/Get/${id}`);
      }
   },

   Update(data) {
      if (data.id == 0) {
         return ApiService.post(`/Arbitration/ArbitrationDelay/Create`, data);
      } else {
         return ApiService.post(`/Arbitration/ArbitrationDelay/Update`, data);
      }
   },

   Delete(id) {
      return ApiService.post(`/Arbitration/ArbitrationDelay/Delete?id=${id}`);
   },
   Sign(data) {
      return ApiService.post(`/Arbitration/ArbitrationDelay/Sign`, data);
   },

   UploadFile(data) {
      return ApiService.post('/Arbitration/ArbitrationDelay/UploadFile', data);
   },
   Cancel(data) {
      return ApiService.post('/Arbitration/ArbitrationDelay/Cancel', data);
   },
   DownloadFile(fileId) {
      return ApiService.print(`/Arbitration/ArbitrationDelay/DownloadFile/${fileId}`);
   },
   DownloadTemplate() {
      return ApiService.print(`/Arbitration/ArbitrationDelay/DownloadTemplate`);
   },
   DeleteFile(fileId) {
      return ApiService.post(`/Arbitration/ArbitrationDelay/DeleteFile/${fileId}`);
   },
   GetByArbitrationCourtApplicationId(arbitrationCourtApplicationId) {
      return ApiService.get(
         `/Arbitration/ArbitrationDelay/GetByArbitrationCourtApplicationId/${arbitrationCourtApplicationId}`
      );
   }
};

export default ArbitrationDelayService;
