import ApiService from '../api.service';

const HrmReportService = {
   GetHrmEmployeeActivityReport(data) {
      return ApiService.post(`/HrmReport/GetHrmEmployeeActivityReport`, data);
   },
   GetHrmEmployeeActivityReportByRegion(data) {
      return ApiService.post(`/HrmReport/GetHrmEmployeeActivityReportByRegion`, data);
   },
   SaveHrmReportAsExcel(data) {
      return ApiService.printtemp(`/HrmReport/SaveHrmReportAsExcel`, data);
   },
   GetReportDocumentsForHrm(data) {
      return ApiService.post(`/Report/GetReportDocumentsForHrm`, data);
   }
};
export default HrmReportService;
