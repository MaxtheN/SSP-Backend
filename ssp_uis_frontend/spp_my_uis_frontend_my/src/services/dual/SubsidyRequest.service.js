import ApiService from "../api.service";
const SubsidyRequestService = {
    GetList(data) {
        return ApiService.post("Dual/SubsidyRequest/GetList", data);
    },
    GetAsSelectList() {
        return ApiService.get(`Dual/SubsidyRequest/GetAsSelectList`);
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
        return ApiService.post(`Dual/SubsidyRequest/Delete?id=${id}`);
    },
    Revoke(data) {
        return ApiService.post(`Dual/SubsidyRequest/Revoke`, data);
    },
    Send(data) {
        return ApiService.post(`Dual/SubsidyRequest/Send`, data);
    },
    UploadFiles(data) {
        return ApiService.formData(`Dual/SubsidyRequest/UploadFile`, data);
    },
    DownloadFile(fileId) {
        return ApiService.get(`Dual/SubsidyRequest/DownloadFile/${fileId}`);
    },
    DeleteFile(fileId) {
        return ApiService.post(`Dual/SubsidyRequest/DeleteFile/${fileId}`);
    },
    DownloadPdf(id) {
        return ApiService.print(`Dual/SubsidyRequest/DownloadPdf?id2=${id}`);
    },
};
export default SubsidyRequestService;
