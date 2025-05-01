import ApiService from "./api.service";

const AdditionalAgreementService = {
    GetList(data) {
        return ApiService.post("/Memship/AdditionalAgreement/GetList", data);
    },
    Get(id) {
        return ApiService.get(`/Memship/AdditionalAgreement/Get/${id}`);
    },
    Sign(data) {
        return ApiService.post(`/Memship/AdditionalAgreement/Sign`, data);
    },
    DownloadPdf(id2) {
        return ApiService.print(`/Memship/AdditionalAgreement/DownloadPdf?id2=${id2}`);
    }
};
export default AdditionalAgreementService;
