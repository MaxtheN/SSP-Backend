import ApiService from '../api.service';

const EmployeeTurnstileReportService = {
   GetEmployeeTurnstileReport(data) {
      return ApiService.post('/EmployeeTurnstileReport/GetEmployeeTurnstileReport', data);
   },
   GetEmployeeTurnstileReportById(data) {
      return ApiService.post('/EmployeeTurnstileReport/GetEmployeeTurnstileReportById', data);
   },
   SaveEmployeeTurnstileReportAsExcel(data) {
      return ApiService.printtemp(`/EmployeeTurnstileReport/SaveEmployeeTurnstileReportAsExcel`, data);
   },
   SaveAsExcelGetStaffCountByGenderReport(data) {
      return ApiService.printtemp(`/Report/SaveAsExcelGetStaffCountByGenderReport`, data);
   },

   GetEmployeeTurnstileTimeReport(data) {
      return ApiService.post(`/EmployeeTurnstileReport/GetEmployeeTurnstileTimeReport`, data);
   },
   SaveAsExcelGetStateEmploymentReport(data) {
      return ApiService.printtemp(`/Report/SaveAsExcelGetStateEmploymentReport`, data);
   },
   SaveEmployeeTurnstileReportById(data) {
      return ApiService.printtemp(`/EmployeeTurnstileReport/SaveEmployeeTurnstileReportById`, data);
   }
};

export default EmployeeTurnstileReportService;
