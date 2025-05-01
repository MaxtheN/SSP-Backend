import ApiService from '../api.service';
const ServiceContractService = {
    GetList(data) {
        return ApiService.post('/srv/ServiceContract/GetList', data);
    },
    GetAsSelectList() {
        return ApiService.get(`/srv/ServiceContract/GetAsSelectList`);
    },
    Get(id) {
        if (id == 0 || id === null || id === undefined) {
            return ApiService.get('/srv/ServiceContract/Get');
        } else {
            return ApiService.get(`/srv/ServiceContract/Get/${id}`);
        }
    },
    Update(data) {
        if (data.id == 0) {
            return ApiService.post(`/srv/ServiceContract/Create`, data);
        } else {
            return ApiService.post(`/srv/ServiceContract/Update`, data);
        }
    },
    GetByApplicationId(applicationId) {
        return ApiService.get(`/srv/ServiceContract/GetByApplicationId?applicationId=${applicationId}`);
    },
    Signing(data) {
        return ApiService.post('/srv/ServiceContract/Signing', data);
    },
    Signed(data) {
        return ApiService.post('/srv/ServiceContract/Signed', data);
    }
    // SignWebImzo(data) {
    //     return ApiService.post('/srv/ServiceContract/SignWebImzo', data);
    // }
};
export default ServiceContractService;
