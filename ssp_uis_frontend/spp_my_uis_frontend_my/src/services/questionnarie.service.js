import ApiService from "./api.service";
const QuestionnarieService = {
    GetList(data) {
        return ApiService.post("/Questionnarie/GetList", data);
    },
    GetAsSelectList() {
        return ApiService.get(`/Questionnarie/GetAsSelectList`);
    },
    Get(id) {
        return ApiService.get(`/Questionnarie/Get/${id}`);
    },
    Create(data) {
        return ApiService.post(`/Questionnarie/Create`, data);
    },
};
export default QuestionnarieService;
