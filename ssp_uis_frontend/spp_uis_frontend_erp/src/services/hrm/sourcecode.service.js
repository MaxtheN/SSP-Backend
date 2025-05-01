import ApiService from '../api.service';

const SourceCodeService = {
   GetList(data) {
      return ApiService.post('hrm/SourceCode/GetList', data);
   },
   Get(id) {
      if (id == 0 || id === null || id === undefined) {
         return ApiService.get('hrm/SourceCode/Get');
      } else {
         return ApiService.get(`hrm/SourceCode/Get/${id}`);
      }
   },
   Update(data) {
      if (data.id == 0) {
         return ApiService.post(`hrm/SourceCode/Create`, data);
      } else {
         return ApiService.post(`hrm/SourceCode/Update`, data);
      }
   },
   Delete(id) {
      return ApiService.post(`hrm/SourceCode/Delete/${id}`);
   },
   GetAsSelectList() {
      return ApiService.get(`hrm/SourceCode/GetAsSelectList`);
   },
   Create(data) {
      if (data.id == 0) {
         return ApiService.post(`hrm/SourceCode/CreateWithUser`, data);
      } else {
         return ApiService.post(`hrm/SourceCode/UpdateWithUser`, data);
      }
   }
};
export default SourceCodeService;
