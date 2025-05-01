import ApiService from './api.service';
const ClaimApplicationService = {
    GetList(data) {
        return ApiService.post('/ClaimApplication/GetList', data);
    },
    Get(id) {
        if (id == 0 || id === null || id === undefined) {
            return ApiService.get('/ClaimApplication/Get');
        } else {
            return ApiService.get(`/ClaimApplication/Get/${id}`);
        }
    },
    Update(data) {
        if (data.id == 0) {
            return ApiService.post(`/ClaimApplication/Create`, data);
        } else {
            return ApiService.post(`/ClaimApplication/Update`, data);
        }
    },
    Delete(id) {
        return ApiService.post(`/ClaimApplication/Delete/${id}`);
    },
    Revoke(id) {
        return ApiService.post(`/ClaimApplication/Revoke/${id}`);
    },
    DownloadPdf(id) {
        return ApiService.print(`/ClaimApplication/DownloadPdf?id2=${id}`);
    },
    UploadFile(data) {
        return ApiService.formData('/ClaimApplication/UploadFile', data);
    },
    DownloadFile(fileId) {
        return ApiService.get(`/ClaimApplication/DownloadFile/${fileId}`);
    },
    DeleteFile(fileId) {
        return ApiService.post(`/ClaimApplication/DeleteFile/${fileId}`);
    },
    CanCreate() {
        return ApiService.get(`/ClaimApplication/CanCreate`);
    },
    GetListDocNumbersByClaimAppTypeId(data) {
        return ApiService.post(`/ClaimApplication/GetListDocNumbersByClaimAppTypeId`, data);
    }
};
export default ClaimApplicationService;
