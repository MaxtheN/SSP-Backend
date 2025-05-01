import ApiService from '../api.service';

// http://sspuis.apptest.uz/api/hrm/ScientificDegree/GetList

const ScientificDegreeService = {
   GetList(data) {
      return ApiService.post('hrm/ScientificDegree/GetList', data);
   },
   Get(id) {
      if (id == 0 || id === null || id === undefined) {
         return ApiService.get('hrm/ScientificDegree/Get');
      } else {
         return ApiService.get(`hrm/ScientificDegree/Get/${id}`);
      }
   },
   Update(data) {
      if (data.id == 0) {
         return ApiService.post(`hrm/ScientificDegree/Create`, data);
      } else {
         return ApiService.post(`hrm/ScientificDegree/Update`, data);
      }
   },
   Delete(id) {
      return ApiService.post(`hrm/ScientificDegree/Delete/${id}`);
   },
   GetAsSelectList() {
      return ApiService.get(`hrm/ScientificDegree/GetAsSelectList`);
   }
};

export default ScientificDegreeService;
