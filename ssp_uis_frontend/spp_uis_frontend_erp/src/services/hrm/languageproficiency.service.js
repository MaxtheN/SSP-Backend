import ApiService from '../api.service';

const LanguageProficiencyService = {
   GetList(data) {
      return ApiService.post('hrm/LanguageProficiency/GetList', data);
   },
   Get(id) {
      if (id == 0 || id === null || id === undefined) {
         return ApiService.get('hrm/LanguageProficiency/Get');
      } else {
         return ApiService.get(`hrm/LanguageProficiency/Get/${id}`);
      }
   },
   Update(data) {
      if (data.id == 0) {
         return ApiService.post(`hrm/LanguageProficiency/Create`, data);
      } else {
         return ApiService.post(`hrm/LanguageProficiency/Update`, data);
      }
   },
   Delete(id) {
      return ApiService.post(`hrm/LanguageProficiency/Delete/${id}`);
   },
   GetAsSelectList() {
      return ApiService.get(`hrm/LanguageProficiency/GetAsSelectList`);
   }
};

export default LanguageProficiencyService;
