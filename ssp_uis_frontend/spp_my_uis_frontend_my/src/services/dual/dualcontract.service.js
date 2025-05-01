import ApiService from '../api.service';

const DualContractService = {
    GetList(data) {
        return ApiService.post('/Dual/DualContract/GetList', data);
    },
    Get(id) {
        return ApiService.get(`/Dual/DualContract/Get/${id}`);
    },
    Sign(data) {
        return ApiService.post('/Dual/DualContract/Sign', data);
    },
    // SignWebImzo(data) {
    //     return ApiService.post('/Dual/DualContract/SignWebImzo', data);
    // },
    Reject(data) {
        return ApiService.post('/Dual/DualContract/Reject', data);
    },

    DualContractIntegrationDownloadPdf(id2) {
        return ApiService.print(`/Dual/DualContractIntegration/DownloadContract/${id2}`);
    }
};

export default DualContractService;
