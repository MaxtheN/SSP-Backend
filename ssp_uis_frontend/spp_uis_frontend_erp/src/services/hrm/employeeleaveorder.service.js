import ApiService from "../api.service";
const EmployeeLeaveOrderService = {
  GetList(data) {
    return ApiService.post("hrm/EmployeeLeaveOrder/GetList", data);
  },
  GetListAll(data) {
    return ApiService.post("hrm/EmployeeLeaveOrder/GetListAll", data);
  },
  GetListForSigner(data) {
    return ApiService.post("hrm/EmployeeLeaveOrder/GetListForSigner", data);
  },
  Get(id) {
    if (id == 0 || id === null || id === undefined) {
      return ApiService.get("hrm/EmployeeLeaveOrder/Get");
    } else {
      return ApiService.get(`hrm/EmployeeLeaveOrder/Get/${id}`);
    }
  },
  Update(data) {
    if (data.id == 0) {
      return ApiService.post(`hrm/EmployeeLeaveOrder/Create`, data);
    } else {
      return ApiService.post(`hrm/EmployeeLeaveOrder/Update`, data);
    }
  },
  Accept(data) {
    return ApiService.post(`hrm/EmployeeLeaveOrder/Accept`, data);
  },
  Sign(data) {
    return ApiService.post(`hrm/EmployeeLeaveOrder/Sign`, data);
  },
  Cancel(data) {
    return ApiService.post(`hrm/EmployeeLeaveOrder/Cancel`, data);
  },
  Delete(id) {
    return ApiService.post(`hrm/EmployeeLeaveOrder/Delete/${id}`);
  },
  GetAsSelectList(data) {
    return ApiService.get(`hrm/EmployeeLeaveOrder/GetAsSelectList`, { params: data });
  },
  GetCalculatedDays(data) {
    return ApiService.get(`hrm/EmployeeLeaveOrder/GetCalculatedDays`, { params: data });
  },
  GetTableAsSelectList(data) {
    return ApiService.get(`hrm/EmployeeLeaveOrder/GetTableAsSelectList`, { params: data });
  },
};
export default EmployeeLeaveOrderService;
