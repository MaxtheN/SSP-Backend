import ApiService from '../api.service';

const SignCriterionService = {
   GetList(data) {
      return ApiService.post('SignCriterion/GetList', data);
   },
   Get(id) {
      if (id == 0 || id === null || id === undefined) {
         return ApiService.get('SignCriterion/Get');
      } else {
         return ApiService.get(`SignCriterion/Get/${id}`);
      }
   },
   Update(data) {
      if (data.id == 0) {
         return ApiService.post(`SignCriterion/Create`, data);
      } else {
         return ApiService.post(`SignCriterion/Update`, data);
      }
   },
   Delete(id) {
      return ApiService.post(`SignCriterion/Delete/${id}`);
   },
   GetAsSelectList() {
      return ApiService.get(`SignCriterion/GetAsSelectList`);
   }
};
export default SignCriterionService;
