import ApiService from '../api.service';

const AppealApplicationService = {
   GetList(data) {
      return ApiService.post('AppealApplication/GetList', data);
   },
   Get(id) {
      if (id == 0 || id === null || id === undefined) {
         return ApiService.get('AppealApplication/Get');
      } else {
         return ApiService.get(`AppealApplication/Get/${id}`);
      }
   },
   Update(data) {
      if (data.id == 0) {
         return ApiService.post(`AppealApplication/Create`, data);
      } else {
         return ApiService.post(`AppealApplication/Update`, data);
      }
   },
   Delete(id) {
      return ApiService.post(`AppealApplication/Delete/${id}`);
   },
   Reject(data) {
      return ApiService.post(`AppealApplication/Reject`, data);
   },
   Accept(data) {
      return ApiService.post(`AppealApplication/Accept`, data);
   },
   Sign(data) {
      return ApiService.post(`AppealApplication/Sign`, data);
   },
   GetAsSelectList() {
      return ApiService.get(`AppealApplication/GetAsSelectList`);
   },
   UploadFile(files) {
      return ApiService.post('AppealApplication/UploadFile', files);
   },
   DownloadFile(fileId) {
      return ApiService.print(`AppealApplication/DownloadFile/${fileId}`);
   },
   DownloadAttachment(fileId) {
      return ApiService.get(`AppealApplication/DownloadAttachment?fileId=${fileId}&isView=true`);
   },
   DeleteFile(fileId) {
      return ApiService.post(`AppealApplication/DeleteFile/${fileId}`);
   },
   PrinAppealApplicationExcel(data) {
      return ApiService.printtemp(`/AppealApplication/PrinAppealApplicationExcel`, data);
   },
   SendToEdoc(id, orgId) {
      return ApiService.printtemp(`/AppealApplication/SendToEdoc?id=${id}&organizationId=${orgId}`);
   }
};
export default AppealApplicationService;
