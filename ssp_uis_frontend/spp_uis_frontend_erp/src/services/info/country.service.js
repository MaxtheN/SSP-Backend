import ApiService from "../api.service";

const CountryService = {
  GetList(data) {
    return ApiService.post(`/Country/GetList`, data);
  },
  Get(id) {
    if (id == 0 || id === null || id === undefined) {
      return ApiService.get(`/Country/Get`);
    } else {
      return ApiService.get(`/Country/Get/${id}`);
    }
  },
  Update(data) {
    if (data.id == 0) {
      return ApiService.post(`/Country/Create`, data);
    } else {
      return ApiService.post(`/Country/Update`, data);
    }
  },
  Delete(id) {
    return ApiService.post(`/Country/Delete/${id}`);
  },
  GetAsSelectList() {
    return ApiService.get(`/Country/GetAsSelectList`);
  },
  GetAsSelectListForBojXona(data) {
    return ApiService.post(`/Country/GetAsSelectListForBojXona`,data);
  },
  SaveAsExecel(data) {
    return ApiService.printtemp(`/Country/SaveAsExecel`, data);
  },
};
export default CountryService;
