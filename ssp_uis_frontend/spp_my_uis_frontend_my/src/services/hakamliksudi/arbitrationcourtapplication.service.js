import ApiService from "../api.service";
const ArbitrationCourtApplicationService = {
  GetList(data) {
    return ApiService.post("/ArbitrationCourtApplication/GetList", data);
  },
  GetAsSelectList() {
    return ApiService.get(`/ArbitrationCourtApplication/GetAsSelectList`);
  },
  Get(id) {
    if (id == 0 || id === null || id === undefined) {
      return ApiService.get("/ArbitrationCourtApplication/Get");
    } else {
      return ApiService.get(`/ArbitrationCourtApplication/Get/${id}`);
    }
  },
  Update(data) {
    if (data.id == 0) {
      return ApiService.post(`/ArbitrationCourtApplication/Create`, data);
    } else {
      return ApiService.post(`/ArbitrationCourtApplication/Update`, data);
    }
  },
  DeleteFile(id) {
    return ApiService.post(`/ArbitrationCourtApplication/DeleteFile/${id}`);
  },
  DownloadFile(id) {
    return ApiService.get(`/ArbitrationCourtApplication/DownloadFile/${id}`);
  },
  UploadFile(data) {
    return ApiService.formData(`/ArbitrationCourtApplication/UploadFile`,data);
  },
 
};
export default ArbitrationCourtApplicationService;
