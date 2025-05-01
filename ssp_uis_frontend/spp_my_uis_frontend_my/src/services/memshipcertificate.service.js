import ApiService from "./api.service";
const MemshipCertificateService = {
  GetList(data) {
    return ApiService.post("/MemshipCertificate/GetList", data);
  },
  GetAsSelectList() {
    return ApiService.get(`/MemshipCertificate/GetAsSelectList`);
  },
  Get(id) {
    if (id == 0 || id === null || id === undefined) {
      return ApiService.get("/MemshipCertificate/Get");
    } else {
      return ApiService.get(`/MemshipCertificate/Get/${id}`);
    }
  },
  Print(id) {
    return ApiService.printtemp(`/MemshipCertificate/Print/${id}`);
  },
  DownloadPdf(id) {
    return ApiService.print(`/MemshipCertificate/DownloadPdf?id2=${id}`);
  },
  GetCertificateAsHtml(id) {
    return ApiService.get(`/MemshipCertificate/GetCertificateAsHtml?Id2=${id}`);
  },
 
};
export default MemshipCertificateService;
