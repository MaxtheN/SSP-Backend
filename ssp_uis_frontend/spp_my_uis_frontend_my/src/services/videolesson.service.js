import ApiService from "./api.service";
const VideoLessonService = {
  GetList(data) {
    return ApiService.post(`/videolesson/GetList`, data);
  },
  Get(id) {
    return ApiService.get(`/videolesson/Get/${id}`);
  },
  GetAsSelectList(categoryId) {
    return ApiService.get(`/videolesson/GetAsSelectLesson/${categoryId}`);
  },
};
export default VideoLessonService;
