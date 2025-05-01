import ApiService from '../api.service';
const PrtnCreditDemandService = {
   GetList(data) {
      return ApiService.post('/PrtnCreditDemand/GetList', data);
   },
   GetAsSelectList() {
      return ApiService.get('/PrtnCreditDemand/GetAsSelectList');
   },
   Get(id) {
      return ApiService.get(`/PrtnCreditDemand/Get/${id}`);
   },
   SaveAsExcel(data) {
      return ApiService.printtemp('/PrtnCreditDemand/SaveAsExecel', data);
   },
};
export default PrtnCreditDemandService;
