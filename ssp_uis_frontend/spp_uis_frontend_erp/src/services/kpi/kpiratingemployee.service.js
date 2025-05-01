import ApiService from '../api.service';

const KpiRatingEmployeeService = {
   GetList(data) {
      return ApiService.post('/kpi/KpiRatingEmployee/GetList', data);
   },
   Get(id) {
      if (id == 0 || id === null || id === undefined) {
         return ApiService.get('/kpi/KpiRatingEmployee/Get');
      } else {
         return ApiService.get(`/kpi/KpiRatingEmployee/Get/${id}`);
      }
   },
   Update(data) {
      if (data.id == 0) {
         return ApiService.post(`/kpi/KpiRatingEmployee/Create`, data);
      } else {
         return ApiService.post(`/kpi/KpiRatingEmployee/Update`, data);
      }
   },
   Delete(id) {
      return ApiService.post(`/kpi/KpiRatingEmployee/Delete/${id}`);
   },
   Accept(data) {
      return ApiService.post(`/kpi/KpiRatingEmployee/Accept`, data);
   },
   Cancel(data) {
      return ApiService.post(`/kpi/KpiRatingEmployee/Cancel`, data);
   },
   GetAsSelectList() {
      return ApiService.get(`/kpi/KpiRatingEmployee/GetAsSelectList`);
   },
   FillTable(id) {
      return ApiService.post(`/kpi/KpiRatingEmployee/FillTable?planId=${id}`);
   }
   // FillIndicatorEmployee(id) {
   //    return ApiService.get(`/kpi/KpiPlanForEmployee/FillIndicators?employeeManageId=${id}`);
   // }
};

export default KpiRatingEmployeeService;
