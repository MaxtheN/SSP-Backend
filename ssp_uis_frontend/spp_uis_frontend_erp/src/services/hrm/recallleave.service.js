import ApiService from "../api.service";
const RecallLeaveService = {
  GetList(data) {
    return ApiService.post("hrm/RecallLeave/GetList", data);
  },
  GetListForSigner(data) {
    return ApiService.post("hrm/RecallLeave/GetListForSigner", data);
  },
  Get(id) {
    if (id == 0 || id === null || id === undefined) {
      return ApiService.get("hrm/RecallLeave/Get");
    } else {
      return ApiService.get(`hrm/RecallLeave/Get/${id}`);
    }
  },
  Update(data) {
    if (data.id == 0) {
      return ApiService.post(`hrm/RecallLeave/Create`, data);
    } else {
      return ApiService.post(`hrm/RecallLeave/Update`, data);
    }
  },
  Accept(data) {
    return ApiService.post(`hrm/RecallLeave/Accept`, data);
  },
  Sign(data) {
    return ApiService.post(`hrm/RecallLeave/Sign`, data);
  },
  Cancel(data) {
    return ApiService.post(`hrm/RecallLeave/Cancel`, data);
  },
  Delete(id) {
    return ApiService.post(`hrm/RecallLeave/Delete/${id}`);
  },
  GetAsSelectList() {
    return ApiService.get(`hrm/RecallLeave/GetAsSelectList`);
  },
};
export default RecallLeaveService;
