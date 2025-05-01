import ApiService from '../api.service';

const DualContractService = {
   GetList(data) {
      return ApiService.post('/DualContract/DualContract/GetList', data);
   },
   Get(id) {
      return ApiService.get(`/DualContract/DualContract/Get/${id}`);
   },
   DownloadPdf(id2) {
      return ApiService.print(`/DualContract/DualContract/DownloadPdf?id2=${id2}&lang=uz`);
   }
};

export default DualContractService;
