import ApiService from '../api.service';
const PrtnCertificateService = {
   GetList(data) {
      return ApiService.post('/PrtnCertificate/GetList', data);
   },
   GetAsSelectList() {
      return ApiService.get(`/PrtnCertificate/GetAsSelectList`);
   },
   Get(id) {
      if (id == 0 || id === null || id === undefined) {
         return ApiService.get('/PrtnCertificate/Get');
      } else {
         return ApiService.get(`/PrtnCertificate/Get/${id}`);
      }
   },
   GetByPrtnContractId(id) {
      return ApiService.get(`/PrtnCertificate/GetByPrtnContractId/${id}`);
   },
   Update(data) {
      if (data.id == 0) {
         return ApiService.post(`/PrtnCertificate/Create`, data);
      } else {
         return ApiService.post(`/PrtnCertificate/Update`, data);
      }
   },
   Create(data) {
      return ApiService.post(`/PrtnCertificate/Create`, data);
   },
   Delete(id) {
      return ApiService.post(`/PrtnCertificate/Delete/${id}`);
   },
   Cancel(data) {
      return ApiService.post(`/PrtnCertificate/Cancel`, data);
   },
   Send(id) {
      return ApiService.post(`/PrtnCertificate/Send?id=${id}`);
   },
   Sign(id) {
      return ApiService.post(`/PrtnCertificate/Sign?id=${id}`);
   },
   GetCertificateAsHtml(id) {
      return ApiService.get(`/PrtnCertificate/GetCertificateAsHtml?Id2=${id}`);
   },
   GetHtmlTemplate(data) {
      return ApiService.post(`/PrtnCertificate/GetHtmlTemplate`, data);
   },
   PrintCertificatePdf(id, lang) {
      return ApiService.print(`/PrtnCertificate/PrintCertificatePdf?Id2=${id}&lang=${lang}`);
   },
   SaveAsExecel(data) {
      return ApiService.printtemp(`/PrtnCertificate/SaveAsExecel`, data);
   },
   PrinGraphExcel(data) {
      return ApiService.printtemp(`/PrtnCertificate/PrinGraphExcel`, data);
   },
   GetFromSoliq(inn) {
      return ApiService.get(`/Contractor/GetFromSoliq/${inn}`);
   },
   GetFromSoliq(inn) {
      return ApiService.get(`/Contractor/GetFromSoliq/${inn}`);
   },
   GetFromDavAkiv(inn) {
      return ApiService.get(`/Contractor/GetFromDavAkiv/${inn}`);
   },
   GetByInnFromSoliq(inn) {
      return ApiService.get(`/Contractor/GetByInnFromSoliq/${inn}`);
   },
   GetPdfTemplateByPrtnContractId(id, lang) {
      return ApiService.get(`/PrtnCertificate/GetPdfTemplateByPrtnContractId/${id}?lang=${lang}`);
   },
   GetPdf(id) {
      return ApiService.get(`/PrtnCertificate/GetPdf/${id}`);
   }
};
export default PrtnCertificateService;
