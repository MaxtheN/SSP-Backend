import ApiService from '../api.service';

const ExecutionApplicationService = {
   GetList(data) {
      return ApiService.post('/ExecutionApplication/GetList', data);
   },
   Get(id) {
      if (id == 0 || id == null || id == undefined) {
         return ApiService.get('/ExecutionApplication/Get');
      } else {
         return ApiService.get(`/ExecutionApplication/Get/${id}`);
      }
   },
   Update(data) {
      if (data.id == 0) {
         return ApiService.post('/ExecutionApplication/Create', data);
      } else {
         return ApiService.post('/ExecutionApplication/Update', data);
      }
   },
   Accept(data) {
      return ApiService.post(`/ExecutionApplication/Accept`, data);
   },
   Cancel(data) {
      return ApiService.post(`/ExecutionApplication/Cancel`, data);
   },
   GetFromGraph(data) {
      const { year, month } = data;
      return ApiService.get(`/ExecutionApplication/GetFromGraph?year=${year}&month=${month}`);
   },
   DownloadPdf(id2, lang) {
      return ApiService.print(`/MediationPlan/DownloadPdf?id2=${id2}`);
   },
   Sign(data) {
      return ApiService.post('/ExecutionApplication/Sign', data);
   }
};

export default ExecutionApplicationService;
