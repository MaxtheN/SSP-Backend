import ApiService from '../api.service';
const MonoApplicationService = {
    GetList(data) {
        return ApiService.post('/MonoApplication/GetList', data);
    },
    GetByMonoAppId(id) {
        return ApiService.get(`/MonoApplication/GetByMonoAppId?appId=${id}`);
    },
    GetAsSelectList() {
        return ApiService.get(`/MonoApplication/GetAsSelectList`);
    },
    Get(id) {
        if (id == 0 || id === null || id === undefined) {
            return ApiService.get('/MonoApplication/Get');
        } else {
            return ApiService.get(`/MonoApplication/Get/${id}`);
        }
    },
    Update(data) {
        if (data.id == 0) {
            return ApiService.post(`/MonoApplication/Create`, data);
        } else {
            return ApiService.post(`/MonoApplication/Update`, data);
        }
    },
    Delete(id) {
        return ApiService.post(`/MonoApplication/Delete/${id}`);
    },
    CanCreate() {
        return ApiService.get(`/MonoApplication/CanCreate`);
    },
    DownloadPdf(id2, lang) {
        return ApiService.print(`/MonoApplication/DownloadPdf?id2=${id2}&lang=${lang}`);
    },
    UploadFile(data) {
        return ApiService.formData('/MonoApplication/UploadFile', data);
    },
    DownloadFile(fileId) {
        return ApiService.get(`/MonoApplication/DownloadFile/${fileId}`);
    },
    DeleteFile(fileId) {
        return ApiService.post(`/MonoApplication/DeleteFile/${fileId}`);
    }
};
export default MonoApplicationService;
