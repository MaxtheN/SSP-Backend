import ApiService from '../api.service';

const VideoLessonService = {
   GetList(data) {
      return ApiService.post('VideoLesson/GetList', data);
   },
   Get(id) {
      if (id == 0 || id === null || id === undefined) {
         return ApiService.get('VideoLesson/Get');
      } else {
         return ApiService.get(`VideoLesson/Get/${id}`);
      }
   },
   Update(data) {
      if (data.id == 0) {
         return ApiService.post(`VideoLesson/Create`, data);
      } else {
         return ApiService.post(`VideoLesson/Update`, data);
      }
   },
   Delete(id) {
      return ApiService.post(`VideoLesson/Delete/${id}`);
   },
   GetAsSelectList() {
      return ApiService.get(`VideoLesson/GetAsSelectList`);
   }
};
export default VideoLessonService;
