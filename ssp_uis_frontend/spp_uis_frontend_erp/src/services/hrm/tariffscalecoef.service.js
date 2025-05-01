import ApiService from '../api.service';
const TariffScaleCoefService = {
   GetList(data) {
      return ApiService.post('hrm/TariffScaleCoef/GetList', data);
   },
   Get(id) {
      if (id == 0 || id === null || id === undefined) {
         return ApiService.get('hrm/TariffScaleCoef/Get');
      } else {
         return ApiService.get(`hrm/TariffScaleCoef/Get/${id}`);
      }
   },
   Update(data) {
      if (data.id == 0) {
         return ApiService.post(`hrm/TariffScaleCoef/Create`, data);
      } else {
         return ApiService.post(`hrm/TariffScaleCoef/Update`, data);
      }
   },
   Delete(id) {
      return ApiService.post(`hrm/TariffScaleCoef/Delete/${id}`);
   },
   GetAsSelectList() {
      return ApiService.get(`hrm/TariffScaleCoef/GetAsSelectList`);
   },
   GetTableAsSelectList(tariffScaleId, tariffScaleTableId) {
      return ApiService.get(
         `hrm/TariffScaleCoef/GetTableAsSelectList?tariffScaleId=${tariffScaleId}&tariffScaleTableId=${tariffScaleTableId}`
      );
   }
};
export default TariffScaleCoefService;
