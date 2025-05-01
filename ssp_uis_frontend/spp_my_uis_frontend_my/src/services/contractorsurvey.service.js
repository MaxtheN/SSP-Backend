import ApiService from './api.service';
const ContractorSurveyService = {
    Create(data) {
        return ApiService.post('/ContractorSurvey/Create', data);
    }
};
export default ContractorSurveyService;
