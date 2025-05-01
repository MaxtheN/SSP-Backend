import ApiService from '../api.service';

const KpiGratingService = {
   GetList(data) {
      return ApiService.post('/kpi/KpiGrating/GetList', data);
   },
   Get(id) {
      if (id == 0 || id === null || id === undefined) {
         return ApiService.get('/kpi/KpiGrating/Get');
      } else {
         return ApiService.get(`/kpi/KpiGrating/Get/${id}`);
      }
   },
   Update(data) {
      if (data.id == 0) {
         return ApiService.post(`/kpi/KpiGrating/Create`, data);
      } else {
         return ApiService.post(`/kpi/KpiGrating/Update`, data);
      }
   },
   Delete(id) {
      return ApiService.post(`/kpi/KpiGrating/Delete/${id}`);
   },
   Accept(data) {
      return ApiService.post(`/kpi/KpiGrating/Accept`, data);
   },
   Cancel(data) {
      return ApiService.post(`/kpi/KpiGrating/Cancel`, data);
   },
   GetAsSelectList() {
      return ApiService.get(`/kpi/KpiGrating/GetAsSelectList`);
   },
   FillIndicator() {
      return ApiService.get(`/kpi/KpiGrating/FillIndicator`);
   }
};

export default KpiGratingService;
