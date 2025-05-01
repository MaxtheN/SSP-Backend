import ApiService from '../api.service';

const KpiPlanForEmployeeService = {
   GetList(data) {
      return ApiService.post('/kpi/KpiPlanForEmployee/GetList', data);
   },
   Get(id) {
      if (id == 0 || id === null || id === undefined) {
         return ApiService.get('/kpi/KpiPlanForEmployee/Get');
      } else {
         return ApiService.get(`/kpi/KpiPlanForEmployee/Get/${id}`);
      }
   },
   Update(data) {
      if (data.id == 0) {
         return ApiService.post(`/kpi/KpiPlanForEmployee/Create`, data);
      } else {
         return ApiService.post(`/kpi/KpiPlanForEmployee/Update`, data);
      }
   },
   Delete(id) {
      return ApiService.post(`/kpi/KpiPlanForEmployee/Delete/${id}`);
   },
   Accept(data) {
      return ApiService.post(`/kpi/KpiPlanForEmployee/Accept`, data);
   },
   Cancel(data) {
      return ApiService.post(`/kpi/KpiPlanForEmployee/Cancel`, data);
   },
   GetAsSelectList() {
      return ApiService.post(`/kpi/KpiPlanForEmployee/GetAsSelectList`);
   },
   FillTable(id) {
      return ApiService.post(`/kpi/KpiPlanForEmployee/FillTable?organizationId=${id}`);
   },
   FillTableByDepartment(id) {
      return ApiService.post(`/kpi/KpiPlanForEmployee/FillTableByDepartment?organizationId=${id}`);
   }
};

export default KpiPlanForEmployeeService;
