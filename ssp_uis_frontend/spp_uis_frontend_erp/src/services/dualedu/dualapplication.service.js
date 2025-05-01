import ApiService from '../api.service';
const DualApplicationService = {
   GetList(data) {
      return ApiService.post('Dual/DualApplication/GetList', data);
   },
   Get(id) {
      if (id == 0 || id === null || id === undefined) {
         return ApiService.get('Dual/DualApplication/Get');
      } else {
         return ApiService.get(`Dual/DualApplication/Get/${id}`);
      }
   },
   Reject(data) {
      return ApiService.post(`Dual/DualApplication/Reject`, data);
   },
   Accept(data) {
      return ApiService.post(`Dual/DualApplication/Accept`, data);
   },
   Cancel(data) {
      return ApiService.post(`Dual/DualApplication/Cancel`, data);
   },
   SaveExcelDualApplication(data) {
      return ApiService.printtemp(`/Dual/DualApplication/SaveExcelDualApplication`, data);
   }
};
export default DualApplicationService;
