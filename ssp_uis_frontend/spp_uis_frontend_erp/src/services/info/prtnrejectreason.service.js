import ApiService from "../api.service";
const PrtnRejectReasonService = {
  GetList(data) {
    return ApiService.post(`/PrtnRejectReason/GetList`, data);
  },
  Get(id) {
    if (id == 0 || id === null || id === undefined) {
      return ApiService.get(`/PrtnRejectReason/Get`);
    } else {
      return ApiService.get(`/PrtnRejectReason/Get/${id}`);
    }
  },
  Update(data) {
    if (data.id == 0) {
      return ApiService.post(`/PrtnRejectReason/Create`, data);
    } else {
      return ApiService.post(`/PrtnRejectReason/Update`, data);
    }
  },
  Delete(id) {
    return ApiService.post(`/PrtnRejectReason/Delete/${id}`);
  },
  GetAsSelectList() {
    return ApiService.get(`/PrtnRejectReason/GetAsSelectList`);
  },
};
export default PrtnRejectReasonService;
