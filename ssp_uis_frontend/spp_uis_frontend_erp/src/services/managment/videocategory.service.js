import ApiService from '../api.service';

const VideoCategoryService = {
   GetList(data) {
      return ApiService.post('VideoCategory/GetList', data);
   },
   Get(id) {
      if (id == 0 || id === null || id === undefined) {
         return ApiService.get('VideoCategory/Get');
      } else {
         return ApiService.get(`VideoCategory/Get/${id}`);
      }
   },
   Update(data) {
      if (data.id == 0) {
         return ApiService.post(`VideoCategory/Create`, data);
      } else {
         return ApiService.post(`VideoCategory/Update`, data);
      }
   },
   Delete(id) {
      return ApiService.post(`VideoCategory/Delete/${id}`);
   },
   GetAsSelectList() {
      return ApiService.get(`VideoCategory/GetAsSelectList`);
   }
};
export default VideoCategoryService;
