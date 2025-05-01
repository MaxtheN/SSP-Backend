import ApiService from "../api.service";
const IdentityDocumentService = {
  GetList(data) {
    return ApiService.post(`/IdentityDocument/GetList`, data);
  },
  Get(id) {
    if (id == 0 || id === null || id === undefined) {
      return ApiService.get(`/IdentityDocument/Get`);
    } else {
      return ApiService.get(`/IdentityDocument/Get/${id}`);
    }
  },
  Update(data) {
    if (data.id == 0) {
      return ApiService.post(`/IdentityDocument/Create`, data);
    } else {
      return ApiService.post(`/IdentityDocument/Update`, data);
    }
  },
  Delete(id) {
    return ApiService.post(`/IdentityDocument/Delete/${id}`);
  },
  GetAsSelectList() {
    return ApiService.get(`/IdentityDocument/GetAsSelectList`);
  },
};
export default IdentityDocumentService;
