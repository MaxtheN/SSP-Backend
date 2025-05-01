import ApiService from '../api.service';

const UniteOfMeasureService = {
   GetList(data) {
      return ApiService.post(`/UniteOfMeasure/GetList`, data);
   },
   Get(id) {
      if (id == 0 || id == null || id == undefined) {
         return ApiService.get(`/UniteOfMeasure/Get`);
      } else {
         return ApiService.get(`/UniteOfMeasure/Get/${id}`);
      }
   },

   GetAsSelectList() {
      return ApiService.get(`/UniteOfMeasure/GetAsSelectList`);
   },

   Update(data) {
      if (data.id == 0) {
         return ApiService.post(`/UniteOfMeasure/Create`, data);
      } else {
         return ApiService.post(`/UniteOfMeasure/Update`, data);
      }
   },
   Delete(id) {
      return ApiService.post(`/UniteOfMeasure/Delete/${id}`);
   }
};

export default UniteOfMeasureService;
