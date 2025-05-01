import ApiService from "./api.service";
const DocumentChatService = {
  GetList(data) {
    return ApiService.post("/DocumentChat/GetList", data);
  },
  Create(data) {
    return ApiService.post(`/DocumentChat/Create`, data);
  },
  Delete(id) {
    return ApiService.post(`/DocumentChat/Delete/${id}`);
  },
};
export default DocumentChatService;
