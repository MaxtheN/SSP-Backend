import ApiService from "../api.service";
const ApplicationService = {
  GetList(data) {
    return ApiService.post("/Application/GetList", data);
  },
  GetAsSelectList() {
    return ApiService.get(`/Application/GetAsSelectList`);
  },
  Get(id) {
    if (id == 0 || id === null || id === undefined) {
      return ApiService.get("/Application/Get");
    } else {
      return ApiService.get(`/Application/Get/${id}`);
    }
  },
  Update(data) {
    if (data.id == 0) {
      return ApiService.post(`/Application/Create`, data);
    } else {
      return ApiService.post(`/Application/Update`, data);
    }
  },
  Reject(data) {
    return ApiService.post(`/Application/Reject`, data);
  },
  Send(id) {
    return ApiService.post(`/Application/Send?id=${id}`);
  },
  Sign(id) {
    return ApiService.post(`/Application/Sign?id=${id}`);
  },
  PrintApplicationPdf(id) {
    return ApiService.get(`/Application/PrintApplicationPdf?Id=${id}`);
  },
  SaveAsExecel(data) {
    return ApiService.printtemp(`/Application/SaveAsExecel`, data);
  },
  GetApplicationAsHtml(id) {
    return ApiService.get(`/Application/GetApplicationAsHtml?Id=${id}`);
  },
  GetMfyApplication(applicationId2) {
    return ApiService.get(`/Application/GetMfyApplication/${applicationId2}`);
  },
};
export default ApplicationService;
