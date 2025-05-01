import ApiService from '../api.service';
const ArbitrationCourtService = {
   GetList(data) {
      return ApiService.post('/Arbitration/ArbitrationCourtApplication/GetList', data);
   },
   Get(id) {
      if (id == 0) {
         return ApiService.get('/Arbitration/ArbitrationCourtApplication/Get');
      } else {
         return ApiService.get(`/Arbitration/ArbitrationCourtApplication/Get/${id}`);
      }
   },
   GetByContractorId(id) {
      if (!id) {
         return ApiService.get('/Arbitration/ArbitrationCourtApplication/GetByContractorId');
      } else {
         return ApiService.get(`/Arbitration/ArbitrationCourtApplication/GetByContractorId?contractorId=${id}`);
      }
   },
   Update(data) {
      if (data.id == 0) {
         return ApiService.post(`/Arbitration/ArbitrationCourtApplication/Create`, data);
      } else {
         return ApiService.post(`/Arbitration/ArbitrationCourtApplication/Update`, data);
      }
   },
   Reject(data) {
      return ApiService.post('/Arbitration/ArbitrationCourtApplication/Reject', data);
   },
   Delete(id) {
      return ApiService.post(`/Arbitration/ArbitrationCourtApplication/Delete?id=${id}`);
   },
   Sign(data) {
      return ApiService.post(`/Arbitration/ArbitrationCourtApplication/Sign`, data);
   },
   NextStep(id) {
      return ApiService.post(`/Arbitration/ArbitrationCourtApplication/NextStep?id=${id}`);
   },
   UploadFile(data) {
      return ApiService.post('/Arbitration/ArbitrationCourtApplication/UploadFile', data);
   },
   DownloadFile(fileId) {
      return ApiService.print(`/Arbitration/ArbitrationCourtApplication/DownloadFile/${fileId}`);
   },
   DownloadTemplate(lang) {
      return ApiService.print(`/Arbitration/ArbitrationCourtApplication/DownloadTemplate?lang=${lang}`);
   },
   DeleteFile(fileId) {
      return ApiService.post(`/Arbitration/ArbitrationCourtApplication/DeleteFile/${fileId}`);
   }
};

export default ArbitrationCourtService;
