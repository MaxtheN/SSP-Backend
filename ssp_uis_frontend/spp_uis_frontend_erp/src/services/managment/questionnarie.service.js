import ApiService from '../api.service';

const QuestionnarieService = {
   GetList(data) {
      return ApiService.post('Questionnarie/GetList', data);
   },
   Get(id) {
      if (id == 0 || id === null || id === undefined) {
         return ApiService.get('Questionnarie/Get');
      } else {
         return ApiService.get(`Questionnarie/Get/${id}`);
      }
   },
   Update(data) {
      if (data.id == 0) {
         return ApiService.post(`Questionnarie/Create`, data);
      } else {
         return ApiService.post(`Questionnarie/Update`, data);
      }
   },
   Delete(id) {
      return ApiService.post(`Questionnarie/Delete/${id}`);
   },
   GetAsSelectList() {
      return ApiService.get(`Questionnarie/GetAsSelectList`);
   }
};
export default QuestionnarieService;
