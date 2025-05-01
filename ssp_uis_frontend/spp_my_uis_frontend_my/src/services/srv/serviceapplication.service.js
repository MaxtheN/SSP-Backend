import ApiService from '../api.service';
const ServiceApplicationService = {
    GetList(data) {
        return ApiService.post('/srv/ServiceApplication/GetList', data);
    },
    GetAsSelectList() {
        return ApiService.get(`/srv/ServiceApplication/GetAsSelectList`);
    },
    Get(id) {
        if (id == 0 || id === null || id === undefined) {
            return ApiService.get('/srv/ServiceApplication/Get');
        } else {
            return ApiService.get(`/srv/ServiceApplication/Get/${id}`);
        }
    },
    Update(data) {
        if (data.id == 0) {
            return ApiService.post(`/srv/ServiceApplication/Create`, data);
        } else {
            return ApiService.post(`/srv/ServiceApplication/Update`, data);
        }
    },
    Delete(id) {
        return ApiService.post(`/srv/ServiceApplication/Delete/${id}`);
    },
    Cancel(data) {
        return ApiService.post(`/srv/ServiceApplication/Cancel`, data);
    },
    Accept(data) {
        return ApiService.post(`/srv/ServiceApplication/Accept`, data);
    },
    Send(data) {
        return ApiService.post(`/srv/ServiceApplication/Send`, data);
    },
    UploadFiles(data) {
        return ApiService.formData(`/srv/ServiceApplication/UploadFile`, data);
    },
    DownloadFile(fileId) {
        return ApiService.get(`/srv/ServiceApplication/DownloadFile/${fileId}`);
    },
    DeleteFile(fileId) {
        return ApiService.post(`/srv/ServiceApplication/DeleteFile/${fileId}`);
    },
    DownloadPdf(id2, lang) {
        return ApiService.print(`/srv/ServiceApplication/DownloadPdf?id2=${id2}&lang=${lang}`);
    },
    GetStatistics() {
        return ApiService.get(`/srv/ServiceApplication/GetStatistics`);
    },
};
export default ServiceApplicationService;
