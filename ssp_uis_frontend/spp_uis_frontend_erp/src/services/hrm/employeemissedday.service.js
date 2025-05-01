import ApiService from '../api.service';

const EmployeeMissedDayService = {
   GetList(data) {
      return ApiService.post('hrm/EmployeeMissedDay/GetList', data);
   },
   Get(id) {
      if (id == 0 || id === null || id === undefined) {
         return ApiService.get('hrm/EmployeeMissedDay/Get');
      } else {
         return ApiService.get(`hrm/EmployeeMissedDay/Get/${id}`);
      }
   },
   Update(data) {
      if (data.id == 0) {
         return ApiService.post(`hrm/EmployeeMissedDay/Create`, data);
      } else {
         return ApiService.post(`hrm/EmployeeMissedDay/Update`, data);
      }
   },
   Delete(id) {
      return ApiService.post(`hrm/EmployeeMissedDay/Delete/${id}`);
   },
   GetAsSelectList() {
      return ApiService.get(`hrm/EmployeeMissedDay/GetAsSelectList`);
   },
   CreateByEmployee(data) {
      return ApiService.post(`hrm/EmployeeMissedDay/CreateByEmployee`, data);
   },
   Approve(data) {
      return ApiService.post(`hrm/EmployeeMissedDay/Approve`, data);
   },
   CancelApprove(data) {
      return ApiService.post(`hrm/EmployeeMissedDay/CancelApprove`, data);
   }
};

export default EmployeeMissedDayService;
