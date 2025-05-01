import ApiService from "../api.service";
const ContractorCategoryCriterionService = {
  GetList(data) {
    return ApiService.post("/Memship/ContractorCategoryCriterion/GetList", data);
  },
  GetAsSelectList() {
    return ApiService.get(`/Memship/ContractorCategoryCriterion/GetAsSelectList`);
  },
  Get(id) {
    if (id == 0 || id === null || id === undefined) {
      return ApiService.get("/Memship/ContractorCategoryCriterion/Get");
    } else {
      return ApiService.get(`/Memship/ContractorCategoryCriterion/Get/${id}`);
    }
  },
  Update(data) {
    if (data.id == 0) {
      return ApiService.post(`/Memship/ContractorCategoryCriterion/Create`, data);
    } else {
      return ApiService.post(`/Memship/ContractorCategoryCriterion/Update`, data);
    }
  },
  Accept(data) {
    return ApiService.post(`/Memship/ContractorCategoryCriterion/Accept`, data);
  },
  Cancel(data) {
    return ApiService.post(`/Memship/ContractorCategoryCriterion/Cancel`,data);
  },
  Delete(id) {
    return ApiService.post(`/Memship/ContractorCategoryCriterion/Delete/${id}`);
  }
};
export default ContractorCategoryCriterionService;
