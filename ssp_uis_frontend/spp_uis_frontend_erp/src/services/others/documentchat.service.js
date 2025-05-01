import ApiService from '../api.service';
const DocumentChatService = {
  GetList(data) {
    return ApiService.post("public/DocumentChat/GetList", data);
  },
  Create(data) {
    return ApiService.post(`public/DocumentChat/Create`, data);
  },
  Delete(id) {
    return ApiService.post(`public/DocumentChat/Delete/${id}`);
  },
};
export default DocumentChatService;
