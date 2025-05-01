import ApiService from '../api.service';

const ContractorService = {
   GetList(data) {
      return ApiService.post(`/Contractor/GetList`, data);
   },
   GetListForEmployee(data) {
      return ApiService.post(`/Contractor/GetListForEmloyee`, data);
   },
   Get(id) {
      if (id == 0 || id === null || id === undefined) {
         return ApiService.get(`/Contractor/Get`);
      } else {
         return ApiService.get(`/Contractor/Get/${id}`);
      }
   },
   Update(data) {
      if (data.id == 0) {
         return ApiService.post(`/Contractor/Create`, data);
      } else {
         return ApiService.post(`/Contractor/UpdateContractorDatas`, data);
      }
   },
   Delete(id) {
      return ApiService.post(`/Contractor/Delete/${id}`);
   },
   GetAsSelectList() {
      return ApiService.post(`/Contractor/GetAsSelectList`);
   },
   GetGTDFromBojxona(data) {
      return ApiService.post(`/Contractor/GetGTDFromBojxona`, data);
   },
   SaveAsExcel(data) {
      return ApiService.printtemp(`/Contractor/SaveAsExecelFromBojxona`, data);
   },
   SearchByInnPnfl(data) {
      return ApiService.get(`/Contractor/SearchByInnPnfl?innpnfl=${data}`);
   },
   GetAsPagedSelectList(data) {
      return ApiService.post(`/Contractor/GetAsPagedSelectList`, data);
   }
};
export default ContractorService;
