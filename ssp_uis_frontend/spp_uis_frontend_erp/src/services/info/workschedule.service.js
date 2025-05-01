import ApiService from "../api.service";
const WorkScheduleService = {
  GetList(data) {
    return ApiService.post(`hrm/WorkSchedule/GetList`, data);
  },
  Get(id) {
    if (id == 0 || id === null || id === undefined) {
      return ApiService.get(`hrm/WorkSchedule/Get`);
    } else {
      return ApiService.get(`hrm/WorkSchedule/Get/${id}`);
    }
  },
  Update(data) {
    if (data.id == 0) {
      return ApiService.post(`hrm/WorkSchedule/Create`, data);
    } else {
      return ApiService.post(`hrm/WorkSchedule/Update`, data);
    }
  },
  Delete(id) {
    return ApiService.post(`hrm/WorkSchedule/Delete/${id}`);
  },
  GetAsSelectList() {
    return ApiService.get(`hrm/WorkSchedule/GetAsSelectList`);
  },
};
export default WorkScheduleService;
