import ApiService from "../api.service";
const ClaimThemeService = {
  GetList(data) {
    return ApiService.post(`ClaimTheme/GetList`, data);
  },
  Get(id) {
    if (id == 0 || id === null || id === undefined) {
      return ApiService.get(`ClaimTheme/Get`);
    } else {
      return ApiService.get(`ClaimTheme/Get/${id}`);
    }
  },
  Update(data) {
    if (data.id == 0) {
      return ApiService.post(`ClaimTheme/Create`, data);
    } else {
      return ApiService.post(`ClaimTheme/Update`, data);
    }
  },
  Delete(id) {
    return ApiService.post(`ClaimTheme/Delete/${id}`);
  },
  GetAsSelectList() {
    return ApiService.get(`ClaimTheme/GetAsSelectList`);
  }
};
export default ClaimThemeService;
