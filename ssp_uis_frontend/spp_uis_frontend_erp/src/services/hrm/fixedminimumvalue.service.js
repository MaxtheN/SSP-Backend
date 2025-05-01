import ApiService from "../api.service";
const FixedMinimumValueService = {
  GetList(data) {
    return ApiService.post("hrm/FixedMinimumValue/GetList", data);
  },
  Get(id) {
    if (id == 0 || id === null || id === undefined) {
      return ApiService.get("hrm/FixedMinimumValue/Get");
    } else {
      return ApiService.get(`hrm/FixedMinimumValue/Get/${id}`);
    }
  },
  Update(data) {
    if (data.id == 0) {
      return ApiService.post(`hrm/FixedMinimumValue/Create`, data);
    } else {
      return ApiService.post(`hrm/FixedMinimumValue/Update`, data);
    }
  },
  Delete(id) {
    return ApiService.post(`hrm/FixedMinimumValue/Delete/${id}`);
  },
  GetAsSelectList(data) {
    return ApiService.post(`hrm/FixedMinimumValue/GetAsSelectList`, data);
  },
};
export default FixedMinimumValueService;
