import ApiService from '../api.service';
const SrvApplicationYearlyPlanService = {
   GetList(data) {
      return ApiService.post('/srv/SrvApplicationYearlyPlan/GetList', data);
   },
   Get(id) {
      if (id == 0 || id === null || id === undefined) {
         return ApiService.get('/srv/SrvApplicationYearlyPlan/Get');
      } else {
         return ApiService.get(`/srv/SrvApplicationYearlyPlan/Get/${id}`);
      }
   },
   Update(data) {
      if (data.id == 0) {
         return ApiService.post(`/srv/SrvApplicationYearlyPlan/Create`, data);
      } else {
         return ApiService.post(`/srv/SrvApplicationYearlyPlan/Update`, data);
      }
   },
   ConvertToCellTables(regionId) {
      return ApiService.get(`/srv/SrvApplicationYearlyPlan/ConvertToCellTables`, {
         params: { regionId: regionId }
      });
   },
   Accept(data) {
      return ApiService.post(`/srv/SrvApplicationYearlyPlan/Accept`, data);
   },
   Cancel(data) {
      return ApiService.post(`/srv/SrvApplicationYearlyPlan/Cancel`, data);
   },
   DownloadPdf(id) {
      return ApiService.get(`/srv/SrvApplicationYearlyPlan/DownloadPdf?id=${id}&lang=ru`);
   },
   UploadFile(files) {
      return ApiService.formData(`/srv/SrvApplicationYearlyPlan/UploadFile`, files);
   },
   DeleteFile(id) {
      return ApiService.post(`/srv/SrvApplicationYearlyPlan/DeleteFile/${id}`);
   },
   DownloadFile(id) {
      return ApiService.get(`/srv/SrvApplicationYearlyPlan/DownloadFile/${id}`);
   }
};
export default SrvApplicationYearlyPlanService;
