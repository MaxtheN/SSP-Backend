import ApiService from "../api.service";
const TaxBenefitService = {
  GetList(data) {
    return ApiService.post("hrm/TaxBenefit/GetList", data);
  },
  Get(id) {
    if (id == 0 || id === null || id === undefined) {
      return ApiService.get("hrm/TaxBenefit/Get");
    } else {
      return ApiService.get(`hrm/TaxBenefit/Get/${id}`);
    }
  },
  Update(data) {
    if (data.id == 0) {
      return ApiService.post(`hrm/TaxBenefit/Create`, data);
    } else {
      return ApiService.post(`hrm/TaxBenefit/Update`, data);
    }
  },
  Delete(id) {
    return ApiService.post(`hrm/TaxBenefit/Delete/${id}`);
  },
  GetAsSelectList(data) {
    return ApiService.post(`hrm/TaxBenefit/GetAsSelectList`, data);
  },
};
export default TaxBenefitService;
