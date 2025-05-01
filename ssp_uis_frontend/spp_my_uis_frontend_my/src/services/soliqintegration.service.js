import ApiService from "./api.service";
const SoliqIntegrationService = {
    GetByPinfl(pinfl) {
        return ApiService.get(`/Soliq/GetByPinfl/${pinfl}`)
    },
    GetCompanyCriteries() {
        return ApiService.get(`/Soliq/GetCompanyCriteries`)
    },
}
export default SoliqIntegrationService;