import ApiService from '../api.service';

const NeedChamberServiceService = {
   GetList(data) {
      return ApiService.post('hrm/NeedChamberService/GetList', data);
   },
   Get(id) {
      if (id == 0 || id === null || id === undefined) {
         return ApiService.get('hrm/NeedChamberService/Get');
      } else {
         return ApiService.get(`hrm/NeedChamberService/Get/${id}`);
      }
   },
   Update(data) {
      if (data.id == 0) {
         return ApiService.post(`hrm/NeedChamberService/Create`, data);
      } else {
         return ApiService.post(`hrm/NeedChamberService/Update`, data);
      }
   },
   Delete(id) {
      return ApiService.post(`hrm/NeedChamberService/Delete/${id}`);
   },
   DownloadFile(id) {
      return ApiService.print(`hrm/NeedChamberService/DownloadFile/${id}`);
   },
   DeleteFile(id) {
      return ApiService.post(`hrm/NeedChamberService/DeleteFile/${id}`);
   },
   UploadFiles(data) {
      return ApiService.post(`hrm/NeedChamberService/UploadFile`, data);
   },
   GetAsSelectList(groupId) {
      return ApiService.get(
         groupId
            ? `hrm/NeedChamberService/GetAsSelectList?groupId=${groupId}`
            : 'hrm/NeedChamberService/GetAsSelectList'
      );
   },
   Create(data) {
      if (data.id == 0) {
         return ApiService.post(`hrm/NeedChamberService/CreateWithUser`, data);
      } else {
         return ApiService.post(`hrm/NeedChamberService/UpdateWithUser`, data);
      }
   }
};
export default NeedChamberServiceService;
