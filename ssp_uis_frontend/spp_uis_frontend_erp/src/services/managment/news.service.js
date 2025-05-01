import ApiService from '../api.service';

const NewsService = {
   GetList(data) {
      return ApiService.post('news/GetList', data);
   },
   GetListByTag(tag) {
      return ApiService.post(`news/GetListByTag/${tag}`);
   },
   GetNewsImage(newsImageId) {
      return ApiService.get(`news/GetNewsImage/${newsImageId}`);
   },
   UploadNewsImage(data) {
      return ApiService.formData(`news/UploadNewsImage`, data);
   },
   Get(id) {
      if (id == 0 || id === null || id === undefined) {
         return ApiService.get('news/Get');
      } else {
         return ApiService.get(`news/Get/${id}`);
      }
   },
   Update(data) {
      if (data.id == 0) {
         return ApiService.post(`news/Create`, data);
      } else {
         return ApiService.post(`news/Update`, data);
      }
   },
   Delete(id) {
      return ApiService.post(`news/Delete/${id}`);
   },
   GetAsSelectList() {
      return ApiService.get(`news/GetAsSelectList`);
   }
};
export default NewsService;
