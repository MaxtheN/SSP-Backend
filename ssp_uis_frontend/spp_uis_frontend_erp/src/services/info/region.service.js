import ApiService from "../api.service";

const RegionService = {
  GetList(data) {
    return ApiService.post(`/Region/GetList`, data);
  },
  Get(id) {
    if (id == 0 || id === null || id === undefined) {
      return ApiService.get(`/Region/Get`);
    } else {
      return ApiService.get(`/Region/Get/${id}`);
    }
  },
  Update(data) {
    if (data.id == 0) {
      return ApiService.post(`/Region/Create`, data);
    } else {
      return ApiService.post(`/Region/Update`, data);
    }
  },
  Delete(id) {
    return ApiService.post(`/Region/Delete/${id}`);
  },
  GetAsSelectList(id) {
    return ApiService.get(`/Region/GetAsSelectList`);
  },
  SaveAsExecel(data) {
    return ApiService.printtemp(`/Region/SaveAsExecel`, data);
  },
};
export default RegionService;
