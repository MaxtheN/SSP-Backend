import ApiService from './api.service';
const ApplicationService = {
    GetList(data) {
        return ApiService.post('/Application/GetList', data);
    },
    GetAsSelectList() {
        return ApiService.get(`/Application/GetAsSelectList`);
    },
    Get(id) {
        if (id == 0 || id === null || id === undefined) {
            return ApiService.get('/Application/GetPrtnApplication');
        } else {
            return ApiService.get(`/Application/GetPrtnApplication/${id}`);
        }
    },
    Update(data) {
        if (data.id == 0) {
            return ApiService.post(`/Application/CreatePrtnApplication`, data);
        } else {
            return ApiService.post(`/Application/UpdatePrtnApplication`, data);
        }
    },
    CanCreateApplication(inn) {
        return ApiService.get(`/Application/CanCreateApplication?inn=${inn}`);
    },
    Delete(id) {
        return ApiService.post(`/Application/Delete/${id}`);
    },
    Revoke(id) {
        return ApiService.post(`/Application/Revoke/${id}`);
    },
    GetApplicationAsHtml(id) {
        return ApiService.get(`/Application/GetApplicationAsHtml?Id=${id}`);
    },
    GetMfyApplication(id) {
        return ApiService.get(`/Application/GetMfyApplication/${id}`);
    },
    GetApplicationVacancies() {
        return ApiService.get(`/Application/GetApplicationVacancies`);
    },
    GetMemshipPayments() {
        return ApiService.get(`/Application/GetMemshipPayments`);
    },
    GetBenefits() {
        return ApiService.get(`/Application/GetBenefits`);
    },
    GetApplicationNotification() {
        return ApiService.get(`/Application/GetApplicationNotification`);
    },
    IsRead(typeName, id) {
        return ApiService.post(`/Application/IsRead/${typeName}?id=${id}`);
    }
};
export default ApplicationService;
