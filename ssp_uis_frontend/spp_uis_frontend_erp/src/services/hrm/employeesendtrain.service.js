import ApiService from "../api.service";

const EmployeeSendTrainService = {
  GetList(data) {
    return ApiService.post("hrm/EmployeeSendTrain/GetList", data);
  },
  GetListForSigner(data) {
    return ApiService.post("hrm/EmployeeSendTrain/GetListForSigner", data);
  },
  Get(id) {
    if (id == 0 || id === null || id === undefined) {
      return ApiService.get("hrm/EmployeeSendTrain/Get");
    } else {
      return ApiService.get(`hrm/EmployeeSendTrain/Get/${id}`);
    }
  },
  Update(data) {
    if (data.id == 0) {
      return ApiService.post(`hrm/EmployeeSendTrain/Create`, data);
    } else {
      return ApiService.post(`hrm/EmployeeSendTrain/Update`, data);
    }
  },
  Accept(data) {
    return ApiService.post(`hrm/EmployeeSendTrain/Accept`, data);
  },
  Sign(data) {
    return ApiService.post(`hrm/EmployeeSendTrain/Sign`, data);
  },
  Cancel(data) {
    return ApiService.post(`hrm/EmployeeSendTrain/Cancel`, data);
  },
  Delete(id) {
    return ApiService.post(`hrm/EmployeeSendTrain/Delete/${id}`);
  },
  GetAsSelectList(data) {
    return ApiService.post(`hrm/EmployeeSendTrain/GetAsSelectList`, data);
  }
};
export default EmployeeSendTrainService;
