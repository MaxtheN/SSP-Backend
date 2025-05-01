import ApiService from "./api.service";
const StateAssetApplicationService = {
  GetList(data) {
    return ApiService.post("/StateAssetApplication/GetList", data);
  },
  GetAsSelectList() {
    return ApiService.get(`/StateAssetApplication/GetAsSelectList`);
  },
  Get(id) {
    if (id == 0 || id === null || id === undefined) {
      return ApiService.get("/StateAssetApplication/Get");
    } else {
      return ApiService.get(`/StateAssetApplication/Get/${id}`);
    }
  },
  Print(id) {
    return ApiService.post(`/StateAssetApplication/Print/${id}`);
  },
  Update(data) {
    if (data.id == 0) {
      return ApiService.post(`/StateAssetApplication/Create`, data);
    } else {
      return ApiService.post(`/StateAssetApplication/Update`, data);
    }
  },
  Revoke(id) {
    return ApiService.post(`/StateAssetApplication/Revoke/${id}`);
  },
  Delete(id){
    return ApiService.delete(`/StateAssetApplication/Delete/${id}`);
  },
  CanCreate(){
    return ApiService.get("/StateAssetApplication/CanCreate");
  },
  GetAsHtml(){
    return ApiService.get("/StateAssetApplication/GetAsHtml");
  },
  GetAsHtmlPost(data){
    return ApiService.post("/StateAssetApplication/GetAsHtml",data);
  },
  GetAsPdf(id2){
    return ApiService.print(`/StateAssetApplication/GetAsPdf/${id2}`);
  },
 
};
export default StateAssetApplicationService;
