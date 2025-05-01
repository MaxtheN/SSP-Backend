import ApiService from './api.service';
const MemshipContractService = {
    GetList(data) {
        return ApiService.post('/Memship/MemshipContract/GetList', data);
    },
    GetAsSelectList() {
        return ApiService.get(`/Memship/MemshipContract/GetAsSelectList`);
    },
    Get(id) {
        if (id == 0 || id === null || id === undefined) {
            return ApiService.get('/Memship/MemshipContract/Get');
        } else {
            return ApiService.get(`/Memship/MemshipContract/Get/${id}`);
        }
    },
    Sign(data) {
        return ApiService.post(`/Memship/MemshipContract/Sign`, data);
    },
    DownloadPdf(id) {
        return ApiService.print(`Memship/MemshipContract/DownloadPdf?id2=${id}`);
    }
    // SignWebImzo(data) {
    //   return ApiService.post(`/Memship/MemshipContract/SignWebImzo`, data);
    // },
};
export default MemshipContractService;
