import ApiService from '../api.service';

const LevelCodeService = {
   GetList(data) {
      return ApiService.post('hrm/LevelCode/GetList', data);
   },
   Get(id) {
      if (id == 0 || id === null || id === undefined) {
         return ApiService.get('hrm/LevelCode/Get');
      } else {
         return ApiService.get(`hrm/LevelCode/Get/${id}`);
      }
   },
   Update(data) {
      if (data.id == 0) {
         return ApiService.post(`hrm/LevelCode/Create`, data);
      } else {
         return ApiService.post(`hrm/LevelCode/Update`, data);
      }
   },
   Delete(id) {
      return ApiService.post(`hrm/LevelCode/Delete/${id}`);
   },
   GetAsSelectList() {
      return ApiService.get(`hrm/LevelCode/GetAsSelectList`);
   },
   Create(data) {
      if (data.id == 0) {
         return ApiService.post(`hrm/LevelCode/CreateWithUser`, data);
      } else {
         return ApiService.post(`hrm/LevelCode/UpdateWithUser`, data);
      }
   }
};
export default LevelCodeService;
