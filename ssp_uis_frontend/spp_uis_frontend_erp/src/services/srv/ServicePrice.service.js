import ApiService from '../api.service';
const ServicePriceService = {
    GetList(data) {
        return ApiService.post('/srv/ServicePrice/GetList', data);
    },
    Get(id) {
        if (id == 0 || id === null || id === undefined) {
            return ApiService.get('/srv/ServicePrice/Get');
        } else {
            return ApiService.get(`/srv/ServicePrice/Get/${id}`);
        }
    },
    Update(data) {
        if (data.id == 0) {
            return ApiService.post(`/srv/ServicePrice/Create`, data);
        } else {
            return ApiService.post(`/srv/ServicePrice/Update`, data);
        }
    },
    GroupingByServicePrice(data) {
        return ApiService.post(`/srv/ServicePrice/GroupingByServicePrice`, data);
    },
    Accept(data) {
        return ApiService.post(`/srv/ServicePrice/Accept`, data);
    },
    Cancel(data) {
        return ApiService.post(`/srv/ServicePrice/Cancel`, data);
    },
    DownloadPdf(id) {
        return ApiService.get(`/srv/ServicePrice/DownloadPdf?id=${id}&lang=ru`);
    },
    UploadFiles(files) {
        return ApiService.formData(`/srv/ServicePrice/UploadFile`, files);
    },
    DeleteFile(id) {
        return ApiService.post(`/srv/ServicePrice/DeleteFile/${id}`);
    },
    DownloadFile(id) {
        return ApiService.get(`/srv/ServicePrice/DownloadFile/${id}`);
    },
    Delete(id) {
        return ApiService.post(`/srv/ServicePrice/Delete/${id}`);
    },
    Clone(id) {
        return ApiService.get(`/srv/ServicePrice/Clone/${id}`);
    },
};
export default ServicePriceService;
