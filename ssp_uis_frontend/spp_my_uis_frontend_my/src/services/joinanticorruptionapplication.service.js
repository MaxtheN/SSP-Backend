import ApiService from "./api.service";
const JoinAntiCorruptionApplicationService = {
	GetList(data) {
		return ApiService.post("/JoinAntiCorruptionApplication/GetList", data);
	},
	Get(id) {
		if (id == 0 || id === null || id === undefined) {
			return ApiService.get("/JoinAntiCorruptionApplication/Get");
		} else {
			return ApiService.get(`/JoinAntiCorruptionApplication/Get/${id}`);
		}
	},
	Update(data) {
		if (data.id == 0) {
			return ApiService.post(`/JoinAntiCorruptionApplication/Create`, data);
		} else {
			return ApiService.post(`/JoinAntiCorruptionApplication/Update`, data);
		}
	},
	Delete(id) {
		return ApiService.post(`/JoinAntiCorruptionApplication/Delete/${id}`);
	},
	CanCreate() {
		return ApiService.get(`/JoinAntiCorruptionApplication/CanCreate`);
	},
    Send(data) {
		return ApiService.post("/JoinAntiCorruptionApplication/Send", data);
	},
	Revoke(data) {
		return ApiService.post("/JoinAntiCorruptionApplication/Revoke", data);
	},
	UploadFile(data) {
		return ApiService.formData("/JoinAntiCorruptionApplication/UploadFile", data);
	},
	DownloadFile(fileId) {
		return ApiService.get(`/JoinAntiCorruptionApplication/DownloadFile/${fileId}`);
	},
    DeleteFile(fileId) {
		return ApiService.post(`/JoinAntiCorruptionApplication/DeleteFile/${fileId}`);
	}
};
export default JoinAntiCorruptionApplicationService;
