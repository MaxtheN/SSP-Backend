import ApiService from '../api.service';
const ClaimApplicationService = {
   GetList(data) {
      return ApiService.post('/ClaimApplication/GetList', data);
   },
   Get(id) {
      if (id == 0 || id === null || id === undefined) {
         return ApiService.get('/ClaimApplication/Get');
      } else {
         return ApiService.get(`/ClaimApplication/Get/${id}`);
      }
   },
   Reject(data) {
      return ApiService.post(`/ClaimApplication/Reject`, data);
   },
   Accept(data) {
      return ApiService.post(`/ClaimApplication/Accept`, data);
   },
   Cancel(data) {
      return ApiService.post(`/ClaimApplication/Cancel`, data);
   },
   DownloadFile(fileId) {
      return ApiService.print(`/ClaimApplication/DownloadFile/${fileId}`);
   },
   EmployeeAttachment(data) {
      return ApiService.post('/ClaimApplication/EmployeeAttachment', data);
   },
   GetForInfo(id) {
      return ApiService.get(`/ClaimApplication/GetForInfo/${id}`);
   },
   GetForInfoID(id, stepId) {
      return ApiService.get(`/ClaimApplication/GetForInfo/${id}?stepId=${stepId}`);
   },
   UpdateClaimApplicationTable(id, newAddress) {
      return ApiService.post(`ClaimApplication/UpdateClaimApplicationTable?id=${id}&newAddress=${newAddress}`);
   }
};
export default ClaimApplicationService;
