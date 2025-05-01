import ApiService from "../api.service";
const RelativeDegreeService = {
  GetList(data) {
    return ApiService.post(`/RelativeDegree/GetList`, data);
  },
  Get(id) {
    if (id == 0 || id === null || id === undefined) {
      return ApiService.get(`/RelativeDegree/Get`);
    } else {
      return ApiService.get(`/RelativeDegree/Get/${id}`);
    }
  },
  Update(data) {
    if (data.id == 0) {
      return ApiService.post(`/RelativeDegree/Create`, data);
    } else {
      return ApiService.post(`/RelativeDegree/Update`, data);
    }
  },
  Delete(id) {
    return ApiService.post(`/RelativeDegree/Delete/${id}`);
  },
  GetAsSelectList() {
    return ApiService.get(`/RelativeDegree/GetAsSelectList`);
  },
};
export default RelativeDegreeService;
