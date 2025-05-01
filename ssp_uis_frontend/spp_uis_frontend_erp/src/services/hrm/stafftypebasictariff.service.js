import ApiService from '../api.service';

const StaffTypeBasicTariffService = {
   GetList(data) {
      return ApiService.post('hrm/StaffTypeBasicTariff/GetList', data);
   },
   Get(id) {
      if (id == 0 || id === null || id === undefined) {
         return ApiService.get('hrm/StaffTypeBasicTariff/Get');
      } else {
         return ApiService.get(`hrm/StaffTypeBasicTariff/Get/${id}`);
      }
   },
   Update(data) {
      if (data.id == 0) {
         return ApiService.post(`hrm/StaffTypeBasicTariff/Create`, data);
      } else {
         return ApiService.post(`hrm/StaffTypeBasicTariff/Update`, data);
      }
   },
   Delete(id) {
      return ApiService.post(`hrm/StaffTypeBasicTariff/Delete/${id}`);
   },
   GetAsSelectList() {
      return ApiService.get(`hrm/StaffTypeBasicTariff/GetAsSelectList`);
   },
   Create(data) {
      if (data.id == 0) {
         return ApiService.post(`hrm/StaffTypeBasicTariff/CreateWithUser`, data);
      } else {
         return ApiService.post(`hrm/StaffTypeBasicTariff/UpdateWithUser`, data);
      }
   }
};
export default StaffTypeBasicTariffService;