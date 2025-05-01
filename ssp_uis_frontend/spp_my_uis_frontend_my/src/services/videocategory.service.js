import ApiService from "./api.service";
const VideoCategoryService = {
  GetList(data) {
    return ApiService.post(`/videocategory/GetList`, data);
  },
  Get(id) {
    return ApiService.get(`/videocategory/Get/${id}`);
  },
  GetAsSelectList() {
    return ApiService.get(`/videocategory/GetAsSelectList`);
  },
};
export default VideoCategoryService;
