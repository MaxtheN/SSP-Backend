import ApiService from '../api.service';
const ServiceDeedService = {
   GetList(data) {
      return ApiService.post('/srv/ServiceDeed/GetList', data);
   },
   Get(id) {
      if (id == 0 || id === null || id === undefined) {
         return ApiService.get('/srv/ServiceDeed/Get');
      } else {
         return ApiService.get(`/srv/ServiceDeed/Get/${id}`);
      }
   },
   Update(data) {
      if (data.id == 0) {
         return ApiService.post(`/srv/ServiceDeed/Create`, data);
      } else {
         return ApiService.post(`/srv/ServiceDeed/Update`, data);
      }
   },
   GetBySrvContractId(srvContractId) {
      return ApiService.get(`/srv/ServiceDeed/GetBySrvContractId?srvContractId=${srvContractId}`);
   },
   Signed(data) {
      return ApiService.post(`/srv/ServiceDeed/Signed`, data);
   },
   Signing(data) {
      return ApiService.post(`/srv/ServiceDeed/Signing`, data);
   },
   Reject(data) {
      return ApiService.post(`/srv/ServiceDeed/Reject`, data);
   },
   DownloadPdf(id) {
      return ApiService.get(`/srv/ServiceDeed/DownloadPdf?id=${id}&lang=ru`);
   },
   SaveAsExcel(data) {
      return ApiService.printtemp(`/srv/ServiceDeed/SaveAsExcel`, data);
   }
};
export default ServiceDeedService;
