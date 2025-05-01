import ApiService from "./api.service";
const PrtnCreditDemandService = {
  GetList(data) {
    return ApiService.post("/PrtnCreditDemand/GetList", data);
  },
  Get(id) {
    if (id == 0 || id === null || id === undefined) {
      return ApiService.get("/PrtnCreditDemand/Get");
    } else {
      return ApiService.get(`/PrtnCreditDemand/Get/${id}`);
    }
  },
  Update(data) {
    if (data.id == 0) {
      return ApiService.post(`/PrtnCreditDemand/Create`, data);
    } else {
      return ApiService.post(`/PrtnCreditDemand/Update`, data);
    }
  },
  Delete(id) {
    return ApiService.post(`/PrtnCreditDemand/Delete/${id}`);
  },
 
};
export default PrtnCreditDemandService;
