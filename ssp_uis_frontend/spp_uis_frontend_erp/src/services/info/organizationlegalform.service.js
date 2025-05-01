import ApiService from "../api.service";
const OrganizationLegalFormService = {
  GetList(data) {
    return ApiService.post(`/OrganizationLegalForm/GetList`, data);
  },
  Get(id) {
    if (id == 0 || id === null || id === undefined) {
      return ApiService.get(`/OrganizationLegalForm/Get`);
    } else {
      return ApiService.get(`/OrganizationLegalForm/Get/${id}`);
    }
  },
  Update(data) {
    if (data.id == 0) {
      return ApiService.post(`/OrganizationLegalForm/Create`, data);
    } else {
      return ApiService.post(`/OrganizationLegalForm/Update`, data);
    }
  },
  Delete(id) {
    return ApiService.post(`/OrganizationLegalForm/Delete/${id}`);
  },
  GetAsSelectList() {
    return ApiService.get(`/OrganizationLegalForm/GetAsSelectList`);
  },
  AsSelectListOrgSettlementAccount() {
    return ApiService.get(`/OrganizationLegalForm/AsSelectListOrgSettlementAccount`);
  },
};
export default OrganizationLegalFormService;
