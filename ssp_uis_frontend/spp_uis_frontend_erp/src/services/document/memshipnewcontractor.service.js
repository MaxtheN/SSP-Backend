import ApiService from '../api.service';
const MemshipNewContractorService = {
   GetList(data) {
      return ApiService.post('/memship/MemshipNewContractor/GetList', data);
   },
   Get(id) {
      if (id == 0 || id === null || id === undefined) {
         return ApiService.get('/memship/MemshipNewContractor/Get');
      } else {
         return ApiService.get(`/memship/MemshipNewContractor/Get/${id}`);
      }
   },
   Update(data) {
      if (data.id == 0) {
         return ApiService.post(`/memship/MemshipNewContractor/Create`, data);
      } else {
         return ApiService.post(`/memship/MemshipNewContractor/Update`, data);
      }
   },

   GetAsSelectList() {
      return ApiService.get('/memship/MemshipNewContractor/GetAsSelectList');
   },
   Accept(data) {
      return ApiService.post(`/memship/MemshipNewContractor/Accept`, data);
   },
   FillTable(regionId) {
      return ApiService.get(`/memship/MemshipNewContractor/FillTable?regionId=${regionId}`);
   },

   Cancel(data) {
      return ApiService.post(`/memship/MemshipNewContractor/Cancel`, data);
   },
   Delete(id) {
      return ApiService.post(`/memship/MemshipNewContractor/Delete/${id}`);
   },
   DeleteFile(id) {
      return ApiService.post(`/memship/MemshipNewContractor/DeleteFile/${id}`);
   },

   DownloadFile(id) {
      return ApiService.get(`/memship/MemshipNewContractor/DownloadFile?id=${id}&lang=ru`);
   },
   // /memship/MemshipNewContractor/DownloadTemplate?id2=878b0184-8c96-433e-9db9-e5c3b36f9ed2'

   UploadFile(files) {
      return ApiService.post('/memship/MemshipNewContractor/UploadFile', files);
   }
};
export default MemshipNewContractorService;
