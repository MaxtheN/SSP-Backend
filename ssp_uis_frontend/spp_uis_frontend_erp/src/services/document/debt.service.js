import ApiService from '../api.service';
const MemshipDebtService = {
   GetList(data) {
      return ApiService.post(`/memship/Debt/GetList`, data);
   },
   Get(id) {
      if (id == 0 || id === null || id === undefined) {
         return ApiService.get(`/memship/Debt/Get`);
      } else {
         return ApiService.get(`/memship/Debt/Get/${id}`);
      }
   },
   Update(data) {
      if (data.id == 0) {
         return ApiService.post(`/memship/Debt/Create`, data);
      } else {
         return ApiService.post(`/memship/Debt/Update`, data);
      }
   },
   Delete(id) {
      return ApiService.post(`/memship/Debt/Delete/${id}`);
   }
};
export default MemshipDebtService;
