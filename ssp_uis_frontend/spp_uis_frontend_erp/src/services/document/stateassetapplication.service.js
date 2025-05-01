import ApiService from '../api.service';
const StateAssetApplicationService = {
   GetList(data) {
      return ApiService.post('/StateAssetApplication/GetList', data);
   },
   GetAsSelectList() {
      return ApiService.get(`/StateAssetApplication/GetAsSelectList`);
   },
   Get(id) {
      if (id == 0 || id === null || id === undefined) {
         return ApiService.get('/StateAssetApplication/Get');
      } else {
         return ApiService.get(`/StateAssetApplication/Get/${id}`);
      }
   },
   Update(data) {
      if (data.id == 0) {
         return ApiService.post(`/StateAssetApplication/Create`, data);
      } else {
         return ApiService.post(`/StateAssetApplication/Update`, data);
      }
   },
   Reject(data) {
      return ApiService.post(`/StateAssetApplication/Reject`, data);
   },
   Send(id) {
      return ApiService.post(`/StateAssetApplication/Send?id=${id}`);
   },
   Sign(id) {
      return ApiService.post(`/StateAssetApplication/Sign?id=${id}`);
   },
   PrintStateAssetApplicationPdf(id) {
      return ApiService.get(`/StateAssetApplication/PrintStateAssetApplicationPdf?Id=${id}`);
   },
   SaveAsExecel(data) {
      return ApiService.printtemp(`/StateAssetApplication/SaveAsExecel`, data);
   },
   GetStateAssetApplicationAsHtml(id) {
      return ApiService.get(`/StateAssetApplication/GetStateAssetApplicationAsHtml?Id=${id}`);
   },
   GetMfyStateAssetApplication(StateAssetApplicationId2) {
      return ApiService.get(`/StateAssetApplication/GetMfyStateAssetApplication/${StateAssetApplicationId2}`);
   },
   GetJobStatus(id) {
      return ApiService.get(`/StateAssetApplication/GetJobStatus/${id}`);
   },
   SendToDavaktivCustom(id) {
      return ApiService.get(`/StateAssetApplication/SendToDavaktivCustom/${id}`);
   }
};
export default StateAssetApplicationService;
