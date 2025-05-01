import ApiService from '../api.service';
const FinanceIntegrationReportService = {
    GetPayDocsByAccEqualsAndBankDateBetween(data) {
        return ApiService.post(`/financeIntegration/GetPayDocsByAccEqualsAndBankDateBetween`, data);
    }
}

export default FinanceIntegrationReportService