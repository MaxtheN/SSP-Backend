import ApiService from '../api.service';
const DepartmentService = {
   GetList(data) {
      return ApiService.post(`hrm/Department/GetList`, data);
   },
   Get(id) {
      if (id == 0 || id === null || id === undefined) {
         return ApiService.get(`hrm/Department/Get`);
      } else {
         return ApiService.get(`hrm/Department/Get/${id}`);
      }
   },
   Update(data) {
      if (data.id == 0) {
         return ApiService.post(`hrm/Department/Create`, data);
      } else {
         return ApiService.post(`hrm/Department/Update`, data);
      }
   },
   Delete(id) {
      return ApiService.post(`hrm/Department/Delete/${id}`);
   },
   GetAsSelectList(organizationId, data) {
      if (organizationId) {
         return ApiService.post(`hrm/Department/GetAsSelectList?organizationId=${organizationId}`, data);
      } else {
         return ApiService.post(`hrm/Department/GetAsSelectList`, data);
      }
   }
};
export default DepartmentService;
