import ApiService from '../api.service';

// http://sspuis.apptest.uz/api/hrm/StateAward/GetList

const StateAwardService = {
   GetList(data) {
      return ApiService.post('hrm/StateAward/GetList', data);
   },
   Get(id) {
      if (id == 0 || id === null || id === undefined) {
         return ApiService.get('hrm/StateAward/Get');
      } else {
         return ApiService.get(`hrm/StateAward/Get/${id}`);
      }
   },
   Update(data) {
      if (data.id == 0) {
         return ApiService.post(`hrm/StateAward/Create`, data);
      } else {
         return ApiService.post(`hrm/StateAward/Update`, data);
      }
   },
   Delete(id) {
      return ApiService.post(`hrm/StateAward/Delete/${id}`);
   },
   GetAsSelectList() {
      return ApiService.get(`hrm/StateAward/GetAsSelectList`);
   }
};

export default StateAwardService;
