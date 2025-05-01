import axios from 'axios';
import ApiService from '../api.service';
const MemshipApplicationService = {
   GetList(data) {
      return ApiService.post('/Memship/MemshipApplication/GetList', data);
   },
   Get(id) {
      if (id == 0 || id === null || id === undefined) {
         return ApiService.get('/Memship/MemshipApplication/Get');
      } else {
         return ApiService.get(`/Memship/MemshipApplication/Get/${id}`);
      }
   },
   GetForErp(inn) {
      return ApiService.get(`/Memship/MemshipApplication/GetForErp?inn=${inn}`);
   },
   UploadFiles(data) {
      return ApiService.formData(`/Memship/MemshipApplication/UploadFile`, data);
   },
   CanCreate() {
      return ApiService.get(`/Memship/MemshipApplication/CanCreate`);
   },
   CreateForErp(data) {
      return ApiService.get(`/Memship/MemshipApplication/CreateForErp`, data);
   },
   Delete(id) {
      return ApiService.post(`/Memship/MemshipApplication/Delete`, null, { params: { id } });
   },
   DownloadPdf(id2) {
      return ApiService.get(`/Memship/MemshipApplication/DownloadPdf?id2=${id2}`);
   },
   DownloadPdfCopy(data) {
      return axios.post(`/Memship/MemshipApplication/DownloadPdfCopy`, data, {
         responseType: 'blob'
      });
   },
   Reject(data) {
      return ApiService.post(`/Memship/MemshipApplication/Reject`, data);
   },
   Cancel(data) {
      return ApiService.post(`/Memship/MemshipApplication/Cancel`, data);
   },
   CreateForErp(data) {
      return ApiService.post(`/Memship/MemshipApplication/CreateForErp`, data);
   },
   SaveAsExcel(data) {
      return ApiService.printtemp(`/Memship/MemshipApplication/SaveAsExcel`, data);
   }
};
export default MemshipApplicationService;
