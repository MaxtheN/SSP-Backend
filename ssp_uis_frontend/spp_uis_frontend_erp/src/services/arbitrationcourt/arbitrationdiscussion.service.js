import ApiService from '../api.service';
const ArbitrationDiscussionService = {
   GetList(data) {
      return ApiService.post('/Arbitration/ArbitrationDiscussion/GetList', data);
   },
   Get(id) {
      if (id == 0) {
         return ApiService.get('/Arbitration/ArbitrationDiscussion/Get');
      } else {
         return ApiService.get(`/Arbitration/ArbitrationDiscussion/Get/${id}`);
      }
   },

   Update(data) {
      if (data.id == 0) {
         return ApiService.post(`/Arbitration/ArbitrationDiscussion/Create`, data);
      } else {
         return ApiService.post(`/Arbitration/ArbitrationDiscussion/Update`, data);
      }
   },

   Delete(id) {
      return ApiService.post(`/Arbitration/ArbitrationDiscussion/Delete?id=${id}`);
   },
   Sign(data) {
      return ApiService.post(`/Arbitration/ArbitrationDiscussion/Sign`, data);
   },

   UploadFile(data) {
      return ApiService.post('/Arbitration/ArbitrationDiscussion/UploadFile', data);
   },
   Cancel(data) {
      return ApiService.post('/Arbitration/ArbitrationDiscussion/Cancel', data);
   },
   DownloadFile(fileId) {
      return ApiService.print(`/Arbitration/ArbitrationDiscussion/DownloadFile/${fileId}`);
   },
   DownloadTemplate() {
      return ApiService.print(`/Arbitration/ArbitrationDiscussion/DownloadTemplate`);
   },
   DeleteFile(fileId) {
      return ApiService.post(`/Arbitration/ArbitrationDiscussion/DeleteFile/${fileId}`);
   },
   GetByArbitrationCourtApplicationId(arbitrationCourtApplicationId) {
      return ApiService.get(
         `/Arbitration/ArbitrationDiscussion/GetByArbitrationCourtApplicationId/${arbitrationCourtApplicationId}`
      );
   }
};

export default ArbitrationDiscussionService;
