import ApiService from '../api.service';

const AcademicDegreeService = {
   GetList(data) {
      return ApiService.post('hrm/AcademicDegree/GetList', data);
   },
   Get(id) {
      if (id == 0 || id === null || id === undefined) {
         return ApiService.get('hrm/AcademicDegree/Get');
      } else {
         return ApiService.get(`hrm/AcademicDegree/Get/${id}`);
      }
   },
   Update(data) {
      if (data.id == 0) {
         return ApiService.post(`hrm/AcademicDegree/Create`, data);
      } else {
         return ApiService.post(`hrm/AcademicDegree/Update`, data);
      }
   },
   Delete(id) {
      return ApiService.post(`hrm/AcademicDegree/Delete/${id}`);
   },
   GetAsSelectList() {
      return ApiService.get(`hrm/AcademicDegree/GetAsSelectList`);
   }
};

export default AcademicDegreeService;
