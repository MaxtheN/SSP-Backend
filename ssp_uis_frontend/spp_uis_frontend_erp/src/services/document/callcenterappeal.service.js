import ApiService from '../api.service';

const CallCenterAppealService = {
   GetList(data) {
      return ApiService.post('CallCenterAppeal/GetList', data);
   },
   Get(id) {
      if (id == 0 || id === null || id === undefined) {
         return ApiService.get('CallCenterAppeal/Get');
      } else {
         return ApiService.get(`CallCenterAppeal/Get/${id}`);
      }
   },
   Update(data) {
      if (data.id == 0) {
         return ApiService.post(`CallCenterAppeal/Create`, data);
      } else {
         return ApiService.post(`CallCenterAppeal/Update`, data);
      }
   },
   Delete(id) {
      return ApiService.post(`CallCenterAppeal/Delete/${id}`);
   },
   Reject(data) {
      return ApiService.post(`CallCenterAppeal/Reject`, data);
   },
   Accept(data) {
      return ApiService.post(`CallCenterAppeal/Accept`, data);
   },
   Sign(data) {
      return ApiService.post(`CallCenterAppeal/Sign`, data);
   },
   GetAsSelectList() {
      return ApiService.get(`CallCenterAppeal/GetAsSelectList`);
   },
   UploadFile(files) {
      return ApiService.post('CallCenterAppeal/UploadFile', files);
   },
   DownloadFile(fileId) {
      return ApiService.print(`CallCenterAppeal/DownloadFile/${fileId}`);
   },
   DownloadAttachment(fileId) {
      return ApiService.get(`CallCenterAppeal/DownloadAttachment?fileId=${fileId}&isView=true`);
   },
   DeleteFile(fileId) {
      return ApiService.post(`CallCenterAppeal/DeleteFile/${fileId}`);
   },
   PrinCallCenterAppealExcel(data) {
      return ApiService.printtemp(`/CallCenterAppeal/PrinAppealApplicationExcel`, data);
   },
   SendToEdoc(id) {
      return ApiService.printtemp(`/CallCenterAppeal/SendToEdoc?id=${id}`);
   }
};
export default CallCenterAppealService;
