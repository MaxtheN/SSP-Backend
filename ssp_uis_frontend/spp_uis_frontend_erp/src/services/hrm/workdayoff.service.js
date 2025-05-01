import ApiService from "../api.service";
const WorkDayOffService = {
  GetList(data) {
    return ApiService.post("hrm/WorkDayOff/GetList", data);
  },
  Get(id) {
    if (id == 0 || id === null || id === undefined) {
      return ApiService.get("hrm/WorkDayOff/Get");
    } else {
      return ApiService.get(`hrm/WorkDayOff/Get/${id}`);
    }
  },
  Update(data) {
    if (data.id == 0) {
      return ApiService.post(`hrm/WorkDayOff/Create`, data);
    } else {
      return ApiService.post(`hrm/WorkDayOff/Update`, data);
    }
  },
  Delete(id) {
    return ApiService.post(`hrm/WorkDayOff/Delete/${id}`);
  },
  Accept(data) {
    return ApiService.post(`hrm/WorkDayOff/Accept`,data);
  },
  Cancel(data) {
    return ApiService.post(`hrm/WorkDayOff/Cancel`,data);
  },
  GetAsSelectList(data) {
    return ApiService.post(`hrm/WorkDayOff/GetAsSelectList`, data);
  },
};
export default WorkDayOffService;
