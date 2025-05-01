import ApiService from "../api.service";
const OrganizationalStructureService = {
  GetList(data) {
    return ApiService.post(`/OrganizationalStructure/GetList`, data);
  },
  GetOrganizationCount() {
    return ApiService.get(`/OrganizationalStructure/GetOrganizationCount`);
  },
  Get(id) {
    if (id == 0 || id === null || id === undefined) {
      return ApiService.get(`/OrganizationalStructure/Get`);
    } else {
      return ApiService.get(`/OrganizationalStructure/Get/${id}`);
    }
  },
  Update(data) {
    if (data.id == 0) {
      return ApiService.post(`/OrganizationalStructure/Create`, data);
    } else {
      return ApiService.post(`/OrganizationalStructure/Update`, data);
    }
  },
  Delete(id) {
    return ApiService.post(`/OrganizationalStructure/Delete/${id}`);
  },
  GetAsSelectList() {
    return ApiService.get(`/OrganizationalStructure/GetAsSelectList`);
  },
  GetCorrCoef() {
    return ApiService.get(`/OrganizationalStructure/GetCorrCoef`);
  },
  GetListForDashbord(data) {
    return ApiService.get(`/OrganizationalStructure/GetListForDashbord`,{params:data});
  },
  GetListForDashbord2() {
    return ApiService.get(`/OrganizationalStructure/GetListForDashbord2`);
  },
  UploadEcxel(data) {
    return ApiService.formData(`/OrganizationalStructure/UploadEcxel`,data);
  },
  GetSecurityInfo() {
    return ApiService.get(`/OrganizationalStructure/GetSecurityInfo`);
  },
};
export default OrganizationalStructureService;
