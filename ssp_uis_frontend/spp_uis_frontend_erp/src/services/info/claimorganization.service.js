import ApiService from "../api.service";
const ClaimOrganizationService = {
  GetList(data) {
    return ApiService.post(`ClaimOrganization/GetList`, data);
  },
  Get(id) {
    if (id == 0 || id === null || id === undefined) {
      return ApiService.get(`ClaimOrganization/Get`);
    } else {
      return ApiService.get(`ClaimOrganization/Get/${id}`);
    }
  },
  Update(data) {
    if (data.id == 0) {
      return ApiService.post(`ClaimOrganization/Create`, data);
    } else {
      return ApiService.post(`ClaimOrganization/Update`, data);
    }
  },
  Delete(id) {
    return ApiService.post(`ClaimOrganization/Delete/${id}`);
  },
  GetAsSelectList() {
    return ApiService.get(`ClaimOrganization/GetAsSelectList`);
  }
};
export default ClaimOrganizationService;
