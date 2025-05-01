import ApiService from '../api.service';

const SettlementAccountSourceService = {
   GetList(data) {
      return ApiService.post('hrm/SettlementAccountSource/GetList', data);
   },
   Get(id) {
      if (id == 0 || id === null || id === undefined) {
         return ApiService.get('hrm/SettlementAccountSource/Get');
      } else {
         return ApiService.get(`hrm/SettlementAccountSource/Get/${id}`);
      }
   },
   Update(data) {
      if (data.id == 0) {
         return ApiService.post(`hrm/SettlementAccountSource/Create`, data);
      } else {
         return ApiService.post(`hrm/SettlementAccountSource/Update`, data);
      }
   },
   Delete(id) {
      return ApiService.post(`hrm/SettlementAccountSource/Delete/${id}`);
   },
   GetAsSelectList() {
      return ApiService.get(`hrm/SettlementAccountSource/GetAsSelectList`);
   },
   Create(data) {
      if (data.id == 0) {
         return ApiService.post(`hrm/SettlementAccountSource/CreateWithUser`, data);
      } else {
         return ApiService.post(`hrm/SettlementAccountSource/UpdateWithUser`, data);
      }
   }
};
export default SettlementAccountSourceService;
