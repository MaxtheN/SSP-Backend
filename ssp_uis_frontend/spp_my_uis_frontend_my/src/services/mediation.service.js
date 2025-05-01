import ApiService from './api.service';
const MediationService = {
    GetList(data) {
        return ApiService.post(`/Mediation/GetList`, data);
    },
    Get(id) {
        return ApiService.get(`/Mediation/Get/${id}`);
    },
    DownloadPdf(id2, lang) {
        return ApiService.print(`/Mediation/DownloadPdf?id2=${id2}&lang=${lang}`);
    }
};
export default MediationService;
