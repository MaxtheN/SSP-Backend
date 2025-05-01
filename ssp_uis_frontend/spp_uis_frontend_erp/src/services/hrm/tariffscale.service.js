import ApiService from '../api.service';
const TariffScaleService = {
   GetList(data) {
      return ApiService.post('hrm/TariffScale/GetList', data);
   },
   Get(id) {
      if (id == 0 || id === null || id === undefined) {
         return ApiService.get('hrm/TariffScale/Get');
      } else {
         return ApiService.get(`hrm/TariffScale/Get/${id}`);
      }
   },
   Update(data) {
      if (data.id == 0) {
         return ApiService.post(`hrm/TariffScale/Create`, data);
      } else {
         return ApiService.post(`hrm/TariffScale/Update`, data);
      }
   },
   Delete(id) {
      return ApiService.post(`hrm/TariffScale/Delete/${id}`);
   },
   GetAsSelectList(typeId) {
      if (typeId) {
         return ApiService.get(`hrm/TariffScale/GetAsSelectList/${typeId}`);
      } else {
         return ApiService.get(`hrm/TariffScale/GetAsSelectList`);
      }
   },
   GetTableAsSelectList(id) {
      return ApiService.get(`hrm/TariffScale/GetTableAsSelectList/${id}`);
   }
};
export default TariffScaleService;
