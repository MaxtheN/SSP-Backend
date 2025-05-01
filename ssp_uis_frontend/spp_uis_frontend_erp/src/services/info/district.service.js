import ApiService from "../api.service";
const DistrictService = {
  GetList(data) {
    return ApiService.post(`/District/GetList`, data);
  },
  Get(id) {
    if (id == 0 || id === null || id === undefined) {
      return ApiService.get(`/District/Get`);
    } else {
      return ApiService.get(`/District/Get/${id}`);
    }
  },
  Update(data) {
    if (data.id == 0) {
      return ApiService.post(`/District/Create`, data);
    } else {
      return ApiService.post(`/District/Update`, data);
    }
  },
  Delete(id) {
    return ApiService.post(`/District/Delete/${id}`);
  },
  GetAsSelectList(id) {
    return ApiService.get(`/District/GetAsSelectList/${id}`);
  },
  SaveAsExecel(data) {
    return ApiService.printtemp(`/District/SaveAsExecel`, data);
  },
};
export default DistrictService;
