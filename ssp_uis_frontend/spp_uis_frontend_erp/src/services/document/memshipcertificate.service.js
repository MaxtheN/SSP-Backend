import axios from 'axios';
import ApiService from '../api.service';
const MemshipCertificateService = {
   GetList(data) {
      return ApiService.post('/Memship/MemshipCertificate/GetList', data);
   },
   Get(id) {
      if (id == 0 || id === null || id === undefined) {
         return ApiService.get('/Memship/MemshipCertificate/Get');
      } else {
         return ApiService.get(`/Memship/MemshipCertificate/Get/${id}`);
      }
   },
   Update(data) {
      if (data.id == 0) {
         return ApiService.post(`/Memship/MemshipCertificate/Create`, data);
      } else {
         return ApiService.post(`/Memship/MemshipCertificate/Update`, data);
      }
   },
   GetByMemshipContractId(memshipContractId) {
      return ApiService.get(`/Memship/MemshipCertificate/GetByMemshipContractId/${memshipContractId}`);
   },
   GetAsSelectList() {
      return ApiService.get('/Memship/MemshipCertificate/GetAsSelectList');
   },
   Accept(data) {
      return ApiService.post(`/Memship/MemshipCertificate/Accept`, data);
   },
   Cancel(data) {
      return ApiService.post(`/Memship/MemshipCertificate/Cancel`, data);
   },
   Delete(id) {
      return ApiService.post(`/Memship/MemshipCertificate/Delete/${id}`);
   },
   DownloadPdf(id, lang) {
      return axios.get(`/Memship/MemshipCertificate/DownloadPdf?id2=${id}&lang=${lang}`, {
         responseType: 'blob'
      });
   },
   DownloadFile(id2, fileId) {
      return ApiService.print(`/Memship/MemshipCertificate/DownloadFile/${fileId}`);
   },
   DownloadPdfCopy(data) {
      return axios.post(`/Memship/MemshipCertificate/DownloadPdfCopy`, data, {
         responseType: 'blob'
      });
   },
   SaveAsExcel(data) {
      return ApiService.printtemp(`/Memship/MemshipCertificate/SaveAsExcel`, data);
   },
   ProlongExpireOn(data) {
      return ApiService.post(`/Memship/MemshipCertificate/ProlongExpireOn`, data);
   },
   GetFromSoliq(inn) {
      return ApiService.get(`Memship/MemshipCertificate/GetFromSoliq?inn=${inn}`);
   },
   GetQqsAylanma(data) {
      return ApiService.get(`/Soliq/GetQqsAylanma?month=${data.month}&inn=${data.inn}&year=${data.year}`);
   },
   GetAosAylanma(data) {
      return ApiService.get(`/Soliq/GetAosAylanma?month=null&inn=${data.inn}&year=${data.year}`);
   },
   UploadFile(data) {
      return ApiService.post('/Memship/MemshipCertificate/UploadFile', data);
   },
   ExpiredPaymentCertificate() {
      return ApiService.get(`Memship/MemshipCertificate/ExpiredPaymentCertificate`);
   },
   MemshipCertificateToPaidNotification() {
      return ApiService.get('Memship/MemshipCertificate/MemshipCertificateToPaidNotification');
   }
};
export default MemshipCertificateService;
