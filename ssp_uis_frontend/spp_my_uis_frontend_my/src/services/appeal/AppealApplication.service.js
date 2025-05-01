import ApiService from '../api.service';
const AppealApplicationService = {
    GetList(data) {
        return ApiService.post('/appeal/AppealApplication/GetList', data);
    },
    Get(id) {
        if (id == 0 || id === null || id === undefined) {
            return ApiService.get('/appeal/AppealApplication/Get');
        } else {
            return ApiService.get(`/appeal/AppealApplication/Get/${id}`);
        }
    },
    Update(data) {
        if (data.id == 0) {
            return ApiService.post(`/appeal/AppealApplication/Create`, data);
        } else {
            return ApiService.post(`/appeal/AppealApplication/Update`, data);
        }
    },
    Delete(id) {
        return ApiService.post(`/appeal/AppealApplication/Delete/${id}`);
    },
    Sign(data) {
        return ApiService.post(`/appeal/AppealApplication/Sign`, data);
    },
    UploadFile(data) {
        return ApiService.formData('/appeal/AppealApplication/UploadFile', data);
    },
    DownloadFile(fileId) {
        return ApiService.get(`/appeal/AppealApplication/DownloadFile/${fileId}`);
    },
    DeleteFile(fileId) {
        return ApiService.post(`/appeal/AppealApplication/DeleteFile/${fileId}`);
    }
    // SignWebImzo(data) {
    //     return ApiService.post('/appeal/AppealApplication/SignWebImzo', data);
    // }
};
export default AppealApplicationService;
