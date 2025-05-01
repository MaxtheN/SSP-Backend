import ApiService from '../api.service';

// http://sspuis.apptest.uz/api/hrm/Partisanship/Create

const PartisanshipService = {
   GetList(data) {
      return ApiService.post('hrm/Partisanship/GetList', data);
   },
   Get(id) {
      if (id == 0 || id === null || id === undefined) {
         return ApiService.get('hrm/Partisanship/Get');
      } else {
         return ApiService.get(`hrm/Partisanship/Get/${id}`);
      }
   },
   Update(data) {
      if (data.id == 0) {
         return ApiService.post(`hrm/Partisanship/Create`, data);
      } else {
         return ApiService.post(`hrm/Partisanship/Update`, data);
      }
   },
   Delete(id) {
      return ApiService.post(`hrm/Partisanship/Delete/${id}`);
   },
   GetAsSelectList() {
      return ApiService.get(`hrm/Partisanship/GetAsSelectList`);
   }
};

export default PartisanshipService;
