import ApiService from "../api.service";

const EmployeeSickLeaveService = {
  GetList(data) {
    return ApiService.post("hrm/EmployeeSickLeave/GetList", data);
  },
  Get(id) {
    if (id == 0 || id === null || id === undefined) {
      return ApiService.get("hrm/EmployeeSickLeave/Get");
    } else {
      return ApiService.get(`hrm/EmployeeSickLeave/Get/${id}`);
    }
  },
  Update(data) {
    if (data.id == 0) {
      return ApiService.post(`hrm/EmployeeSickLeave/Create`, data);
    } else {
      return ApiService.post(`hrm/EmployeeSickLeave/Update`, data);
    }
  },
  Accept(data) {
    return ApiService.post(`hrm/EmployeeSickLeave/Accept`,data);
  },
  Cancel(data) {
    return ApiService.post(`hrm/EmployeeSickLeave/Cancel`,data);
  },
  Delete(id) {
    return ApiService.post(`hrm/EmployeeSickLeave/Delete/${id}`);
  },
  GetAsSelectList(data) {
    return ApiService.post(`hrm/EmployeeSickLeave/GetAsSelectList`, data);
  }
};
export default EmployeeSickLeaveService;
