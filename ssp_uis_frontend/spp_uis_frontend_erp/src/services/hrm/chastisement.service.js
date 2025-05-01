import ApiService from '../api.service';
const ChastisementService = {
   GetList(data) {
      return ApiService.post('hrm/Chastisement/GetList', data);
   },
   GetListAll(data) {
      return ApiService.post('hrm/Chastisement/GetListAll', data);
   },
   GetListForSigner(data) {
      return ApiService.post('hrm/Chastisement/GetListForSigner', data);
   },
   Get(id) {
      if (id == 0 || id === null || id === undefined) {
         return ApiService.get('hrm/Chastisement/Get');
      } else {
         return ApiService.get(`hrm/Chastisement/Get/${id}`);
      }
   },
   Update(data) {
      if (data.id == 0) {
         return ApiService.post(`hrm/Chastisement/Create`, data);
      } else {
         return ApiService.post(`hrm/Chastisement/Update`, data);
      }
   },
   Accept(data) {
      return ApiService.post(`hrm/Chastisement/Accept`, data);
   },
   Sign(data) {
      return ApiService.post(`hrm/Chastisement/Sign`, data);
   },
   Cancel(data) {
      return ApiService.post(`hrm/Chastisement/Cancel`, data);
   },
   Delete(id) {
      return ApiService.post(`hrm/Chastisement/Delete/${id}`);
   },
   GetAsSelectList(data) {
      return ApiService.get(`hrm/Chastisement/GetAsSelectList`, { params: data });
   },
   GetCalculatedDays(data) {
      return ApiService.get(`hrm/Chastisement/GetCalculatedDays`, { params: data });
   },
   GetTableAsSelectList(data) {
      return ApiService.get(`hrm/Chastisement/GetTableAsSelectList`, { params: data });
   },
   DownloadTemplate() {
      return ApiService.print(`/hrm/Chastisement/DownloadTemplate`);
   },
   ChastisementContractUpload(files) {
      return ApiService.post('/hrm/Chastisement/UploadFile', files);
   }
};
export default ChastisementService;
