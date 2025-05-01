import ApiService from '../api.service';
const ServiceInfoService = {
   GetList(data) {
      return ApiService.post('/Report/GetSrvServiceInfo', data);
   },
   SaveAsExcelSrvServiceGetInfo(data) {
      return ApiService.printtemp('/Report/SaveAsExcelSrvServiceGetInfo ', data);
   },
   FinanceIntegration(data) {
      return ApiService.post(`/financeIntegration/GetPayDocsByAccEqualsAndBankDateBetween`, data);
   }
};
export default ServiceInfoService;
