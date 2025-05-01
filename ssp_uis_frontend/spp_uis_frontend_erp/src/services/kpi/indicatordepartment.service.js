import ApiService from '../api.service';

const IndicatorDepartmentService = {
   GetList(data) {
      return ApiService.post(`/kpi/IndicatorDepartment/GetList`, data);
   },
   Get(id) {
      if (id == 0 || id == null || id == undefined) {
         return ApiService.get(`/kpi/IndicatorDepartment/Get`);
      } else {
         return ApiService.get(`/kpi/IndicatorDepartment/Get/${id}`);
      }
   },

   GetAsSelectList() {
      return ApiService.get(`/kpi/IndicatorDepartment/GetAsSelectList`);
   },

   Update(data) {
      if (data.id == 0) {
         return ApiService.post(`/kpi/IndicatorDepartment/Create`, data);
      } else {
         return ApiService.post(`/kpi/IndicatorDepartment/Update`, data);
      }
   },
   Delete(id) {
      return ApiService.post(`/kpi/IndicatorDepartment/Delete/${id}`);
   }
};

export default IndicatorDepartmentService;
