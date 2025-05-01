import ApiService from "./api.service";
const ProposalService = {
  GetList(data) {
    return ApiService.post("/Proposal/GetList", data);
  },
  GetAsSelectList() {
    return ApiService.get(`/Proposal/GetAsSelectList`);
  },
  Get(id) {
    return ApiService.get(`/Proposal/Get/id=${id}`);
  },
  Create(data) {
    return ApiService.post(`/Proposal/Create`, data);
  },
  Delete(id) {
    return ApiService.post(`/Proposal/Delete?id=${id}`);
  },
  UploadFiles(data){
    return ApiService.formData(`/Proposal/UploadFiles`, data);
  },
  GetContractorFromSoliq(inn) {
    return ApiService.get(`/Proposal/GetContractorFromSoliq?inn=${inn}`);
  }
 
};
export default ProposalService;
