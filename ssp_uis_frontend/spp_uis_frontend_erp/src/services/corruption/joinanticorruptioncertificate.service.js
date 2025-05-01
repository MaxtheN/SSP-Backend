import ApiService from '../api.service';
const JoinAntiCorruptionCertificateService = {
   GetList(data) {
      return ApiService.post('/Corruption/JoinAntiCorruptionCertificate/GetList', data);
   },
   Get(id) {
      if (id == 0 || id === null || id === undefined) {
         return ApiService.get('/Corruption/JoinAntiCorruptionCertificate/Get');
      } else {
         return ApiService.get(`/Corruption/JoinAntiCorruptionCertificate/Get/${id}`);
      }
   },
   DownloadPdf(id) {
      return ApiService.get(`/Corruption/JoinAntiCorruptionCertificate/DownloadPdf?id=${id}&lang=ru`);
   },
};
export default JoinAntiCorruptionCertificateService;
