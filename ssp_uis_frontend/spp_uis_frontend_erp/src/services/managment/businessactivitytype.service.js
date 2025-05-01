import ApiService from '../api.service';

const BusinessActivityTypeService = {
   GetList(data) {
      return ApiService.post('BusinessActivityType/GetList', data);
   },
   SyncContractorInfo() {
      return ApiService.post('BusinessActivityType/SyncContractorInfo', {});
   }
};
export default BusinessActivityTypeService;
