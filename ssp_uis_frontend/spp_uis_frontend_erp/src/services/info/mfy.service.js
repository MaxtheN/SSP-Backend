import ApiService from "../api.service";

const MfyService = {
  GetList(data) {
    return ApiService.post(`/Mfy/GetList`, data);
  },
  Get(id) {
    if (id == 0 || id === null || id === undefined) {
      return ApiService.get(`/Mfy/Get`);
    } else {
      return ApiService.get(`/Mfy/Get/${id}`);
    }
  },
  Update(data) {
    if (data.id == 0) {
      return ApiService.post(`/Mfy/Create`, data);
    } else {
      return ApiService.post(`/Mfy/Update`, data);
    }
  },
  Delete(id) {
    return ApiService.post(`/Mfy/Delete/${id}`);
  },
  GetAsSelectList(regionId, districtId) {
    if (districtId == 0 || districtId === null || districtId === undefined) {
      return ApiService.get(`/Mfy/GetAsSelectList/${regionId}`);
    } else {
      return ApiService.get(
        `/Mfy/GetAsSelectList/${regionId}?districtId=${districtId}`
      );
    }
  },
};
export default MfyService;
