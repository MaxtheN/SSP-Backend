import ApiService from '../api.service';

const DegreeTitleService = {
   GetList(data) {
      return ApiService.post('hrm/DegreeTitle/GetList', data);
   },
   Get(id) {
      if (id == 0 || id === null || id === undefined) {
         return ApiService.get('hrm/DegreeTitle/Get');
      } else {
         return ApiService.get(`hrm/DegreeTitle/Get/${id}`);
      }
   },
   Update(data) {
      if (data.id == 0) {
         return ApiService.post(`hrm/DegreeTitle/Create`, data);
      } else {
         return ApiService.post(`hrm/DegreeTitle/Update`, data);
      }
   },
   Delete(id) {
      return ApiService.post(`hrm/DegreeTitle/Delete/${id}`);
   },
   GetAsSelectList() {
      return ApiService.get(`hrm/DegreeTitle/GetAsSelectList`);
   }
};

export default DegreeTitleService;
