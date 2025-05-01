import ApiService from "../api.service";
const DocumentHeldForEmpService = {
  GetPinflData(data) {
    return ApiService.post("HrmEmp/GetByPinflData", data);
  },
};
export default DocumentHeldForEmpService;
