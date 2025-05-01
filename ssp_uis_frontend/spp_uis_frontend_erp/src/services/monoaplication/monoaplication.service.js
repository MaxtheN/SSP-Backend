import ApiService from '../api.service';
const MonoAplicationService = {
   GetList(data) {
      return ApiService.post('/MonoApplication/GetList', data);
   },
   Get(id) {
      return ApiService.get(`/MonoApplication/Get/${id}`);
   },
   GetByMonoAppId(appId) {
      return ApiService.get(`/MonoApplication/GetByMonoAppId?appId=${appId}`);
   }
};

export default MonoAplicationService;
