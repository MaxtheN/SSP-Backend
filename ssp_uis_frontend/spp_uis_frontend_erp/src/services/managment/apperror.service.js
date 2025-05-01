import ApiService from '@/services/api.service';

const AppError = {
  GetList(data) {
    return ApiService.post("/AppError/GetList", data);
  },
  Get(id) {
    if (id == 0 || id === null || id === undefined) {
      return ApiService.get(`/AppError/Get`);
    } else {
      return ApiService.get(`/AppError/Get/${id}`);
    }
  },
};

export default AppError