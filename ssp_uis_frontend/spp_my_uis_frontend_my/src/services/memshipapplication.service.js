import ApiService from "./api.service";
const MemshipApplicationService = {
  GetList(data) {
    return ApiService.post("/MemshipApplication/GetList", data);
  },
  GetAsSelectList() {
    return ApiService.get(`/MemshipApplication/GetAsSelectList`);
  },
  Get(id) {
    if (id == 0 || id === null || id === undefined) {
      return ApiService.get("/MemshipApplication/Get");
    } else {
      return ApiService.get(`/MemshipApplication/Get/${id}`);
    }
  },
  Update(data) {
    if (data.id == 0) {
      return ApiService.post(`/MemshipApplication/Create`, data);
    } else {
      return ApiService.post(`/MemshipApplication/Update`, data);
    }
  },
  CanCreate() {
    return ApiService.get(`/MemshipApplication/CanCreate`);
  },
  Delete(id) {
    return ApiService.post(`/MemshipApplication/Delete/${id}`);
  },
  Revoke(data) {
    return ApiService.post(`/MemshipApplication/Revoke`, data);
  },
  Send(data) {
    return ApiService.post(`/MemshipApplication/Send`, data);
  },
  UploadFiles(data) {
    return ApiService.formData(`/MemshipApplication/UploadFile`, data);
  },
	DownloadFile(fileId) {
		return ApiService.get(`/MemshipApplication/DownloadFile/${fileId}`);
	},
	DeleteFile(fileId) {
		return ApiService.post(`/MemshipApplication/DeleteFile/${fileId}`);
	},
  DownloadPdf(id) {
    return ApiService.print(`/MemshipApplication/DownloadPdf?id2=${id}`);
  },
};
export default MemshipApplicationService;
