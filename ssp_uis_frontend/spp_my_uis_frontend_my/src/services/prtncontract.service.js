import ApiService from "./api.service";
const PrtnContractService = {
  GetList(data) {
    return ApiService.post("/PrtnContract/GetList", data);
  },
  GetAsSelectList() {
    return ApiService.get(`/PrtnContract/GetAsSelectList`);
  },
  Get(id) {
    if (id == 0 || id === null || id === undefined) {
      return ApiService.get("/PrtnContract/Get");
    } else {
      return ApiService.get(`/PrtnContract/Get/${id}`);
    }
  },
  Print(id) {
    return ApiService.post(`/PrtnContract/Print/${id}`);
  },
  Sign(data) {
    return ApiService.post(`/PrtnContract/Sign/`,data);
  },
  Reject(data) {
    return ApiService.post(`/PrtnContract/Reject`,data);
  },
  GetPrtnContractAsHtml(id){
    return ApiService.get(`/PrtnContract/GetPrtnContractAsHtml?Id2=${id}`);
  }
 
};
export default PrtnContractService;
