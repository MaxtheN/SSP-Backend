import ApiService from "./api.service";
const PrtnCertificateService = {
  GetList(data) {
    return ApiService.post("/PrtnCertificate/GetList", data);
  },
  GetAsSelectList() {
    return ApiService.get(`/PrtnCertificate/GetAsSelectList`);
  },
  Get(id) {
    if (id == 0 || id === null || id === undefined) {
      return ApiService.get("/PrtnCertificate/Get");
    } else {
      return ApiService.get(`/PrtnCertificate/Get/${id}`);
    }
  },
  Print(id) {
    return ApiService.post(`/PrtnCertificate/Print/${id}`);
  },
  PrintCertificatePdf(id) {
    return ApiService.get(`/PrtnCertificate/PrintCertificatePdf/${id}`);
  },
  GetCertificateAsHtml(id) {
    return ApiService.get(`/PrtnCertificate/GetCertificateAsHtml?Id2=${id}`);
  },
 
};
export default PrtnCertificateService;
