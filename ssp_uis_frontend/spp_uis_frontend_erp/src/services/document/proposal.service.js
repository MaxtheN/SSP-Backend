import ApiService from '../api.service';
const ProposalService = {
   GetList(data) {
      return ApiService.post('/Proposal/GetList', data);
   },
   GetAsSelectList() {
      return ApiService.get(`/Proposal/GetAsSelectList`);
   },
   GetContractorFromSoliq(inn) {
      return ApiService.get(`/Proposal/GetContractorFromSoliq?inn=${inn}`);
   },
   Get(id) {
      if (id == 0 || id === null || id === undefined) {
         return ApiService.get('/Proposal/Get');
      } else {
         return ApiService.get(`/Proposal/Get/${id}`);
      }
   },
   Update(data) {
      if (data.id == 0) {
         return ApiService.post(`/Proposal/Create`, data);
      } else {
         return ApiService.post(`/Proposal/Update`, data);
      }
   },

   Delete(id) {
      return ApiService.post(`/Proposal/Delete/${id}`);
   },

   UploadFiles(data) {
      return ApiService.post(`/Proposal/UploadFiles`, data);
   },
   SaveAsExcel(data) {
      return ApiService.printtemp(`/Proposal/SaveAsExcel`, data);
   }
};
export default ProposalService;
