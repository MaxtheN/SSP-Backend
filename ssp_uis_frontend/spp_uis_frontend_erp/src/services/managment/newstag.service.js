import ApiService from '../api.service';

const NewsTagService = {
   GetList(data) {
      return ApiService.post('newstag/GetList', data);
   },
   Get(id) {
      if (id == 0 || id === null || id === undefined) {
         return ApiService.get('newstag/Get');
      } else {
         return ApiService.get(`newstag/Get/${id}`);
      }
   },
   Update(data) {
      if (data.id == 0) {
         return ApiService.post(`newstag/Create`, data);
      } else {
         return ApiService.post(`newstag/Update`, data);
      }
   },
   Delete(id) {
      return ApiService.post(`newstag/Delete/${id}`);
   },
   GetAsSelectList() {
      return ApiService.get(`newstag/GetAsSelectList`);
   }
};
export default NewsTagService;
