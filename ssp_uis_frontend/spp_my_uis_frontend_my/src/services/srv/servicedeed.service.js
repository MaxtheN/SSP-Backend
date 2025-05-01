import ApiService from '../api.service';
const ServiceDeedService = {
    GetList(data) {
        return ApiService.post('/srv/ServiceDeed/GetList', data);
    },
    GetAsSelectList() {
        return ApiService.get(`/srv/ServiceDeed/GetAsSelectList`);
    },
    Get(id) {
        if (id == 0 || id === null || id === undefined) {
            return ApiService.get('/srv/ServiceDeed/Get');
        } else {
            return ApiService.get(`/srv/ServiceDeed/Get/${id}`);
        }
    },
    Update(data) {
        if (data.id == 0) {
            return ApiService.post(`/srv/ServiceDeed/Create`, data);
        } else {
            return ApiService.post(`/srv/ServiceDeed/Update`, data);
        }
    },
    GetBySrvContractId(srvContractId) {
        return ApiService.get(`/srv/ServiceDeed/GetBySrvContractId?srvContractId=${srvContractId}`);
    },
    Signing(data) {
        return ApiService.post('/srv/ServiceDeed/Signing', data);
    },
    Signed(data) {
        return ApiService.post('/srv/ServiceDeed/Signed', data);
    }
    // SignWebImzo(data) {
    //     return ApiService.post('/srv/ServiceDeed/SignWebImzo', data);
    // }
};
export default ServiceDeedService;
