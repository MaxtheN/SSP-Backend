import ApiService from './api.service';

const QueizManualService = {
    AnswerTypeSelectList() {
        return ApiService.get('/QuizManual/AnswerTypeSelectList');
    },
    InspectionTypeSelectList() {
        return ApiService.get('/QuizManual/InspectionTypeSelectList');
    },
    QuestionnaireTypeSelectList() {
        return ApiService.get('/QuizManual/QuestionnaireTypeSelectList');
    }
};

export default QueizManualService;
