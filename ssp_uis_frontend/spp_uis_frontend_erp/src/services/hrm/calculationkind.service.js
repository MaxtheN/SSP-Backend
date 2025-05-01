import ApiService from '../api.service';

const CalculationKindService = {
   GetList(data) {
      return ApiService.post('hrm/CalculationKind/GetList', data);
   },
   Get(id) {
      if (id == 0 || id === null || id === undefined) {
         return ApiService.get('hrm/CalculationKind/Get');
      } else {
         return ApiService.get(`hrm/CalculationKind/Get/${id}`);
      }
   },
   Update(data) {
      if (data.id == 0) {
         return ApiService.post(`hrm/CalculationKind/Create`, data);
      } else {
         return ApiService.post(`hrm/CalculationKind/Update`, data);
      }
   },
   Delete(id) {
      return ApiService.post(`hrm/CalculationKind/Delete/${id}`);
   },
   GetAsSelectList() {
      return ApiService.get(`hrm/CalculationKind/GetAsSelectList`);
   },
   Create(data) {
      if (data.id == 0) {
         return ApiService.post(`hrm/CalculationKind/CreateWithUser`, data);
      } else {
         return ApiService.post(`hrm/CalculationKind/UpdateWithUser`, data);
      }
   }
};
export default CalculationKindService;
