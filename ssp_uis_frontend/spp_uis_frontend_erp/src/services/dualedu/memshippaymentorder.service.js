import ApiService from '../api.service';
const MemshipPaymentOrderService = {
   GetList(data) {
      return ApiService.post('/dualedu/MemshipPaymentOrder/GetList', data);
   },
   Get(id) {
      if (id == 0 || id === null || id === undefined) {
         return ApiService.get('/dualedu/MemshipPaymentOrder/Get');
      } else {
         return ApiService.get(`/dualedu/MemshipPaymentOrder/Get/${id}`);
      }
   },
   Update(data) {
      if (data.id == 0) {
         return ApiService.post(`/dualedu/MemshipPaymentOrder/Create`, data);
      } else {
         return ApiService.post(`/dualedu/MemshipPaymentOrder/Update`, data);
      }
   },
   GetAsSelectList() {
      return ApiService.get('/dualedu/MemshipPaymentOrder/GetAsSelectList');
   },
   Delete(id) {
      return ApiService.post(`/dualedu/MemshipPaymentOrder/Delete/${id}`);
   },
   Accept(data) {
      return ApiService.post(`dualedu/MemshipPaymentOrder/Accept?id=${data.id}`, data);
   },
   Cancel(data) {
      return ApiService.post(`dualedu/MemshipPaymentOrder/Cancel/${data.id}?message=${data.message}`);
   },
   UploadFile(files) {
      return ApiService.post('/dualedu/MemshipPaymentOrder/UploadFile', files);
   },
   DownloadFile(fileId) {
      return ApiService.print(`/dualedu/MemshipPaymentOrder/DownloadFile/${fileId}`);
   },
   DeleteFile(fileId) {
      return ApiService.post(`/dualedu/MemshipPaymentOrder/DeleteFile/${fileId}`);
   },
   SaveAsExcelForGetListMemshipPaymentOrder(data) {
      return ApiService.printtemp(`/dualedu/MemshipPaymentOrder/SaveAsExcelForGetListMemshipPaymentOrder`, data);
   }
};
export default MemshipPaymentOrderService;
