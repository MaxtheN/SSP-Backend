import ApiService from '../api.service';

const EmployeeManageService = {
   GetList(data) {
      return ApiService.post('hrm/EmployeeManage/GetList', data);
   },
   GetListForUser(data) {
      return ApiService.post('hrm/EmployeeManage/GetListForUser', data);
   },
   Get(id) {
      return ApiService.get('hrm/EmployeeManage/Get/' + id);
   },
   CheckSigners() {
      return ApiService.get('hrm/EmployeeManage/CheckSigners');
   }
};
export default EmployeeManageService;
