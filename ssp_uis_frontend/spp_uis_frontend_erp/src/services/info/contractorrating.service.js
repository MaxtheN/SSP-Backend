import ApiService from '../api.service';
const ContractorRatingService = {
   GetList(data) {
      return ApiService.post(`memship/ContractorRating/GetList`, data);
   },
   Get(id) {
      if (id == 0 || id === null || id === undefined) {
         return ApiService.get(`memship/ContractorRating/Get`);
      } else {
         return ApiService.get(`memship/ContractorRating/Get/${id}`);
      }
   },
   Update(data) {
      if (data.id == 0) {
         return ApiService.post(`memship/ContractorRating/Create`, data);
      } else {
         return ApiService.post(`memship/ContractorRating/Update`, data);
      }
   },
   Delete(id) {
      return ApiService.post(`memship/ContractorRating/Delete/${id}`);
   },
   GetAsSelectList() {
      return ApiService.get(`memship/ContractorRating/GetAsSelectList`);
   },
   SaveAsExecel(data) {
      return ApiService.printtemp(`memship/ContractorRating/SaveAsExecel`, data);
   }
};
export default ContractorRatingService;
