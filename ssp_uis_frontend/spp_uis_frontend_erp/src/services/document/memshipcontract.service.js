import axios from 'axios';
import ApiService from '../api.service';
const MemshipContractService = {
   GetList(data) {
      return ApiService.post('/Memship/MemshipContract/GetList', data);
   },
   Get(id) {
      if (id == 0 || id === null || id === undefined) {
         return ApiService.get('/Memship/MemshipContract/Get');
      } else {
         return ApiService.get(`/Memship/MemshipContract/Get/${id}`);
      }
   },
   Update(data) {
      if (data.id == 0) {
         return ApiService.post(`/Memship/MemshipContract/Create`, data);
      } else {
         return ApiService.post(`/Memship/MemshipContract/Update`, data);
      }
   },
   GetByApplication(applicationId) {
      return ApiService.get(`/Memship/MemshipContract/GetByApplicationId/${applicationId}`);
   },
   GetAsSelectList() {
      return ApiService.get('/Memship/MemshipContract/GetAsSelectList');
   },
   Sign(data) {
      return ApiService.post(`/Memship/MemshipContract/Sign`, data);
   },
   ChangeContractorToPaid(data) {
      return ApiService.post(`/Memship/MemshipContract/ChangeContractorToPaid`, data);
   },
   Reject(data) {
      return ApiService.post(`/Memship/MemshipContract/Reject`, data);
   },
   Cancel(data) {
      return ApiService.post(`/Memship/MemshipContract/Cancel`, data);
   },
   Delete(id) {
      return ApiService.post(`/Memship/MemshipContract/Delete/${id}`);
   },
   Import(data) {
      return ApiService.post(`/Memship/MemshipContract/Import`, data);
   },
   DownloadPdf(id) {
      return ApiService.get(`/Memship/MemshipContract/DownloadPdf?id=${id}&lang=ru`);
   },
   // /Memship/MemshipContract/DownloadTemplate?id2=878b0184-8c96-433e-9db9-e5c3b36f9ed2'
   DownloadTemplate(id2) {
      return ApiService.print(`/Memship/MemshipContract/DownloadTemplate?id2=${id2}`);
   },
   MemshipContractUpload(files) {
      return ApiService.post('/Memship/MemshipContract/UploadFile', files);
   },
   DownloadPdfCopy(data) {
      return axios.post(`/Memship/MemshipContract/DownloadPdfCopy`, data, {
         responseType: 'blob'
      });
   },
   Confirm(data) {
      return ApiService.get(`/Memship/MemshipContract/Comfirm?id=${data.id}`);
   },
   SaveAsExcel(data) {
      return ApiService.printtemp(`/Memship/MemshipContract/SaveAsExcel`, data);
   },
   UpdatingBird(data) {
      return ApiService.printtemp(`/Memship/MemshipContract/UpdatingBird`, data);
   },
   ChangeContractorDocnumber(data) {
      return ApiService.post(`/Memship/MemshipContract/ChangeContractorDocnumber`, data);
   },
   UploadFile(data) {
      return ApiService.post('/Memship/MemshipContract/UploadFile', data);
   },
   DownloadFile(id) {
      return ApiService.print(`/Memship/MemshipContract/DownloadByIdFile/${id}`);
   }
};
export default MemshipContractService;
