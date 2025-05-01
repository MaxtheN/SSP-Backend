import ApiService from "../api.service";
const DualApplicationService = {
  GetList(data) {
    return ApiService.post("Dual/DualApplication/GetList", data);
  },
  GetAsSelectList() {
    return ApiService.get(`Dual/DualApplication/GetAsSelectList`);
  },
  Get(id) {
    if (id == 0 || id === null || id === undefined) {
      return ApiService.get("Dual/DualApplication/Get");
    } else {
      return ApiService.get(`Dual/DualApplication/Get/${id}`);
    }
  },
  Update(data) {
    if (data.id == 0) {
      return ApiService.post(`Dual/DualApplication/Create`, data);
    } else {
      return ApiService.post(`Dual/DualApplication/Update`, data);
    }
  },
  CanCreate() {
    return ApiService.get(`Dual/DualApplication/CanCreate`);
  },
  Send(data) {
    return ApiService.post(`Dual/DualApplication/Send`, data);
  },
  Revoke(data) {
    return ApiService.post(`Dual/DualApplication/Revoke`, data);
  },
};
export default DualApplicationService;
