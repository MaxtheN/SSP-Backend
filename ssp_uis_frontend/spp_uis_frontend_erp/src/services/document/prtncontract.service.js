import ApiService from '../api.service';
const PrtnContractService = {
   GetList(data) {
      return ApiService.post('/PrtnContract/GetList', data);
   },
   GetAsSelectList() {
      return ApiService.get(`/PrtnContract/GetAsSelectList`);
   },
   Get(id) {
      if (id == 0 || id === null || id === undefined) {
         return ApiService.get('/PrtnContract/Get');
      } else {
         return ApiService.get(`/PrtnContract/Get/${id}`);
      }
   },
   GetByApplicationId(id) {
      return ApiService.get(`/PrtnContract/GetByApplicationId/${id}`);
   },
   Update(data) {
      if (data.id == 0) {
         return ApiService.post(`/PrtnContract/Create`, data);
      } else {
         return ApiService.post(`/PrtnContract/Update`, data);
      }
   },
   Delete(id) {
      return ApiService.post(`/PrtnContract/Delete/${id}`);
   },
   Send(id) {
      return ApiService.post(`/PrtnContract/Send?id=${id}`);
   },
   Reject(data) {
      return ApiService.post(`/PrtnContract/Reject`, data);
   },
   Agree(data) {
      return ApiService.post(`/PrtnContract/Agree`, data);
   },
   Revoke(data) {
      return ApiService.post(`/PrtnContract/Revoke`, data);
   },
   Cancel(data) {
      return ApiService.post(`/PrtnContract/Cancel`, data);
   },
   Sign(data) {
      return ApiService.post(`/PrtnContract/Sign`, data);
   },
   PassExpertise(data) {
      return ApiService.post(`/PrtnContract/PassExpertise`, data);
   },
   NotPassExpertise(data) {
      return ApiService.post(`/PrtnContract/NotPassExpertise`, data);
   },
   ResendExpertise(data) {
      return ApiService.post(`/PrtnContract/ResendExpertise`, data);
   },
   GetHtmlTemplate(data) {
      return ApiService.post(`/PrtnContract/GetHtmlTemplate`, data);
   },
   GetPdfTemplate(applicationId) {
      return ApiService.get(`/PrtnContract/GetPdfTemplate?applicationId=${applicationId}`, {});
   },
   PrintPrtnContractPdf(id) {
      return ApiService.print(`/PrtnContract/PrintPrtnContractPdf?Id2=${id}`);
   },
   GetPrtnContractAsHtml(id) {
      return ApiService.get(`/PrtnContract/GetPrtnContractAsHtml?Id2=${id}`);
   },
   SaveAsExecel(data) {
      return ApiService.printtemp(`/PrtnContract/SaveAsExecel`, data);
   },
   UploadFile(data) {
      return ApiService.formData(`/PrtnContract/UploadFile`, data);
   },
   DownloadFile(id) {
      return ApiService.print(`/PrtnContract/DownloadFile/${id}`);
   },
   DeleteFile(id) {
      return ApiService.post(`/PrtnContract/DeleteFile/${id}`, {});
   },
   SaveAsExcelPrtnContracts(data) {
      return ApiService.printtemp('/Report/SaveAsExcelPrtnContracts', data);
   }
};
export default PrtnContractService;
