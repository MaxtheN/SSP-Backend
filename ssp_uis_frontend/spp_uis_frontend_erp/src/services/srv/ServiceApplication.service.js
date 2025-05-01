import ApiService from '../api.service';
const ServiceApplicationService = {
   GetList(data) {
      return ApiService.post('/srv/ServiceApplication/GetList', data);
   },
   Get(id) {
      if (id == 0 || id === null || id === undefined) {
         return ApiService.get('/srv/ServiceApplication/Get');
      } else {
         return ApiService.get(`/srv/ServiceApplication/Get/${id}`);
      }
   },
   Accept(data) {
      return ApiService.post(`/srv/ServiceApplication/Accept`, data);
   },
   AcceptForFree(data) {
      return ApiService.post(`/srv/ServiceApplication/AcceptForFree`, data);
   },
   Reject(data) {
      return ApiService.post(`/srv/ServiceApplication/Reject`, data);
   },
   Received(data) {
      return ApiService.post(`/srv/ServiceApplication/Received`, data);
   },
   Cancel(data) {
      return ApiService.post(`/srv/ServiceApplication/Cancel`, data);
   },
   DownloadPdf(id) {
      return ApiService.get(`/srv/ServiceApplication/DownloadPdf?id=${id}&lang=ru`);
   },
   UploadFiles(files) {
      return ApiService.formData(`/srv/ServiceApplication/UploadFile`, files);
   },
   DeleteFile(id) {
      return ApiService.post(`/srv/ServiceApplication/DeleteFile/${id}`);
   },
   DownloadFile(id) {
      return ApiService.get(`/srv/ServiceApplication/DownloadFile/${id}`);
   },
   PrintGraphExcel(data) {
      return ApiService.printtemp('/srv/ServiceApplication/PrinSrvApplicationExcel', data);
   },
   Delete(id) {
      return ApiService.post(`/srv/ServiceApplication/Delete/${id}`);
   }
};
export default ServiceApplicationService;
