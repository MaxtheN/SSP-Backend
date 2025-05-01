import ApiService from "../api.service";

const SubsidyRequestService = {
  GetList(data) {
    return ApiService.post("Dual/SubsidyRequest/GetList", data);
  },
  Get(id) {
    if (id == 0 || id === null || id === undefined) {
      return ApiService.get("Dual/SubsidyRequest/Get");
    } else {
      return ApiService.get(`Dual/SubsidyRequest/Get/${id}`);
    }
  },
  Update(data) {
    if (data.id == 0) {
      return ApiService.post(`Dual/SubsidyRequest/Create`, data);
    } else {
      return ApiService.post(`Dual/SubsidyRequest/Update`, data);
    }
  },
  Delete(id) {
    return ApiService.post(`Dual/SubsidyRequest/Delete/${id}`);
  },
  Accept(data) {
    return ApiService.post(`Dual/SubsidyRequest/Accept`, data);
  },
  Cancel(data) {
    return ApiService.post(`Dual/SubsidyRequest/Cancel`, data);
  },
  Reject(data) {
    return ApiService.post(`Dual/SubsidyRequest/Reject`, data);
  },
  UploadFile(data) {
    return ApiService.formData(`Dual/SubsidyRequest/UploadFile`, data);
  },
  DownloadFile(fileId) {
    return ApiService.get(`Dual/SubsidyRequest/DownloadFile/${fileId}`);
  },
  DeleteFile(fileId) {
    return ApiService.post(`Dual/SubsidyRequest/DeleteFile/${fileId}`);
  },
};
export default SubsidyRequestService;
