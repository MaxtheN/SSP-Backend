import ApiService from '../api.service';
const JoinAntiCorruptionApplicationService = {
   GetList(data) {
      return ApiService.post('/Corruption/JoinAntiCorruptionApplication/GetList', data);
   },
   Get(id) {
      if (id == 0 || id === null || id === undefined) {
         return ApiService.get('/Corruption/JoinAntiCorruptionApplication/Get');
      } else {
         return ApiService.get(`/Corruption/JoinAntiCorruptionApplication/Get/${id}`);
      }
   },
   CanCreate() {
      return ApiService.get(`/Corruption/JoinAntiCorruptionApplication/CanCreate`);
   },
   Delete(id) {
      return ApiService.post(`/Corruption/JoinAntiCorruptionApplication/Delete`, null, { params: { id } });
   },
   DownloadPdf(id) {
      return ApiService.post(`/Corruption/JoinAntiCorruptionApplication/DownloadPdf`, null, { params: { id } });
   },
   Reject(data) {
      return ApiService.post(`/Corruption/JoinAntiCorruptionApplication/Reject`, data);
   },
   Accept(data) {
      return ApiService.post(`/Corruption/JoinAntiCorruptionApplication/Accept`, data);
   },
   Cancel(data) {
      return ApiService.post(`/Corruption/JoinAntiCorruptionApplication/Cancel`, data);
   }
};
export default JoinAntiCorruptionApplicationService;
