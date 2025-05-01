import ApiService from "../api.service";
const ClaimOrganizationTypeService = {
  GetList(data) {
    return ApiService.post(`ClaimOrganizationType/GetList`, data);
  },
  Get(id) {
    if (id == 0 || id === null || id === undefined) {
      return ApiService.get(`ClaimOrganizationType/Get`);
    } else {
      return ApiService.get(`ClaimOrganizationType/Get/${id}`);
    }
  },
  Update(data) {
    if (data.id == 0) {
      return ApiService.post(`ClaimOrganizationType/Create`, data);
    } else {
      return ApiService.post(`ClaimOrganizationType/Update`, data);
    }
  },
  Delete(id) {
    return ApiService.post(`ClaimOrganizationType/Delete/${id}`);
  },
  GetAsSelectList() {
    return ApiService.get(`ClaimOrganizationType/GetAsSelectList`);
  }
};
export default ClaimOrganizationTypeService;
