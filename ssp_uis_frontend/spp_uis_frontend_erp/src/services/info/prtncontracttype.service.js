import ApiService from "../api.service";
const PrtnContractTypeService = {
  GetList(data) {
    return ApiService.post("/PrtnContractType/GetList", data);
  },
  GetAsSelectList() {
    return ApiService.get(`/PrtnContractType/GetAsSelectList`);
  },
  Get(id) {
    if (id == 0 || id === null || id === undefined) {
      return ApiService.get("/PrtnContractType/Get");
    } else {
      return ApiService.get(`/PrtnContractType/Get/${id}`);
    }
  },
  GetTables(id){
    return ApiService.get(`/PrtnContractType/GetTables/${id}`);
  },
  GetTablesBySignOrganizationType(data = {}) {
    return ApiService.post(
      `/PrtnContractType/GetTablesBySignOrganizationType`,
      data
    );
  },
  Update(data) {
    if (data.id == 0) {
      return ApiService.post(`/PrtnContractType/Create`, data);
    } else {
      return ApiService.post(`/PrtnContractType/Update`, data);
    }
  },
  Delete(id) {
    return ApiService.post(`/PrtnContractType/Delete/${id}`);
  },

  // GetAsSelectList(data) {
  //   return ApiService.post(`/PrtnContractType/GetAsSelectList`, data);
  // },
};
export default PrtnContractTypeService;
