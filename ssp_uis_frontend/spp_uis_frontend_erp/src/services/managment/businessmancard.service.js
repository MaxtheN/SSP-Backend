import ApiService from '../api.service';

const BusinessmanCardService = {
   GetByInn(inn) {
      return ApiService.get(`BusinessmanCard/GetByInn/${inn}`);
   },
   GetFromInvestmentByInn(data) {
      return ApiService.post('/Contractor/GetFromInvestment', data);
   }
};
export default BusinessmanCardService;
