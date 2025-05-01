import ApiService from "./api.service";
const ReportService = {
  GetFreeAreaFromBandlik(data) {
    return ApiService.post(`/Report/GetFreeAreaFromBandlik`, data);
  },
};
export default ReportService;
