import ApiService from "../api.service";
const DocumentHeldForSignService = {
  GetHrmDocumentList(data) {
    return ApiService.post("HrmSigner/GetData", data);
  },
};
export default DocumentHeldForSignService;
