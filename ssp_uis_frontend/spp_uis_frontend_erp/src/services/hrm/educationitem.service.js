import ApiService from '../api.service';

const EducationItemService = {
   GetList(data) {
      return ApiService.post('EducationItem/GetList', data);
   },
   Get(id) {
      if (id == 0 || id === null || id === undefined) {
         return ApiService.get('EducationItem/Get');
      } else {
         return ApiService.get(`EducationItem/Get/${id}`);
      }
   },

   GetAsSelectList() {
      return ApiService.get('EducationItem/GetAsSelectList');
   },
   Update(data) {
      if (data.id == 0) {
         return ApiService.post(`EducationItem/Create`, data);
      } else {
         return ApiService.post(`EducationItem/Update`, data);
      }
   },
   Delete(id) {
      return ApiService.post(`EducationItem/Delete/${id}`);
   }
};

export default EducationItemService;
