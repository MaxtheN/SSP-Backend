import ApiService from '../api.service';

const PlannedCalculationService = {
   GetList(data) {
      return ApiService.post('hrm/PlannedCalculation/GetList', data);
   },
   Get(id) {
      if (id == 0 || id === null || id === undefined) {
         return ApiService.get('hrm/PlannedCalculation/Get');
      } else {
         return ApiService.get(`hrm/PlannedCalculation/Get/${id}`);
      }
   },
   Update(data) {
      if (data.id == 0) {
         return ApiService.post(`hrm/PlannedCalculation/Create`, data);
      } else {
         return ApiService.post(`hrm/PlannedCalculation/Update`, data);
      }
   },
   Delete(id) {
      return ApiService.post(`hrm/PlannedCalculation/Delete/${id}`);
   },
   GetAsSelectList(data) {
      return ApiService.post(`hrm/PlannedCalculation/GetAsSelectList`, data);
   },
   Create(data) {
      if (data.id == 0) {
         return ApiService.post(`hrm/PlannedCalculation/CreateWithUser`, data);
      } else {
         return ApiService.post(`hrm/PlannedCalculation/UpdateWithUser`, data);
      }
   }
};
export default PlannedCalculationService;
