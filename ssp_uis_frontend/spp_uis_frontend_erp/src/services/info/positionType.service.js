import ApiService from "../api.service";
const PositionTypeService = {
  GetList(data) {
    return ApiService.post(`/PositionType/GetList`, data);
  },
  Get(id) {
    if (id == 0 || id === null || id === undefined) {
      return ApiService.get(`hrm/PositionType/Get`);
    } else {
      return ApiService.get(`hrm/PositionType/Get/${id}`);
    }
  },
  Update(data) {
    if (data.id == 0) {
      return ApiService.post(`hrm/PositionType/Create`, data);
    } else {
      return ApiService.post(`hrm/PositionType/Update`, data);
    }
  },
  Delete(id) {
    return ApiService.post(`hrm/PositionType/Delete/${id}`);
  },
  GetAsSelectList() {
    return ApiService.get(`hrm/PositionType/GetAsSelectList`);
  },
  SaveAsExecel(data) {
    return ApiService.printtemp(`hrm/PositionType/SaveAsExecel`, data);
  },
};
export default PositionTypeService;
