import ApiService from "../api.service";
const TaxBenefitTypeService = {
  GetList(data) {
    return ApiService.post("hrm/TaxBenefitType/GetList", data);
  },
  Get(id) {
    if (id == 0 || id === null || id === undefined) {
      return ApiService.get("hrm/TaxBenefitType/Get");
    } else {
      return ApiService.get(`hrm/TaxBenefitType/Get/${id}`);
    }
  },
  Update(data) {
    if (data.id == 0) {
      return ApiService.post(`hrm/TaxBenefitType/Create`, data);
    } else {
      return ApiService.post(`hrm/TaxBenefitType/Update`, data);
    }
  },
  Delete(id) {
    return ApiService.post(`hrm/TaxBenefitType/Delete/${id}`);
  },
  GetAsSelectList(data) {
    return ApiService.get(`hrm/TaxBenefitType/GetAsSelectList`, data);
  },
};
export default TaxBenefitTypeService;
