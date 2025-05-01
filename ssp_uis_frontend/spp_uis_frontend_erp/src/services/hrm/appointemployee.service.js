import ApiService from '../api.service';
const AppointEmployeeService = {
   GetList(data) {
      return ApiService.post('hrm/AppointEmployee/GetList', data);
   },
   CheckEmploymentRateBeforSave(employeeId, startOn, employeeRate, fromPositionId) {
      return ApiService.get(
         `hrm/AppointEmployee/CheckEmploymentRateBeforSave?employeeId=${employeeId}&startOn=${startOn}&employeeRate=${employeeRate}&fromPositionId=${fromPositionId}`
      );
   },
   Get(id) {
      if (id == 0 || id === null || id === undefined) {
         return ApiService.get('hrm/AppointEmployee/Get');
      } else {
         return ApiService.get(`hrm/AppointEmployee/Get/${id}`);
      }
   },
   Update(data) {
      if (data.id == 0) {
         return ApiService.post(`hrm/AppointEmployee/Create`, data);
      } else {
         return ApiService.post(`hrm/AppointEmployee/Update`, data);
      }
   },
   Delete(id) {
      return ApiService.post(`hrm/AppointEmployee/Delete/${id}`);
   },
   Accept(data) {
      return ApiService.post(`hrm/AppointEmployee/Accept`, data);
   },
   Sign(data) {
      return ApiService.post(`hrm/AppointEmployee/Sign`, data);
   },
   Cancel(data) {
      return ApiService.post(`hrm/AppointEmployee/Cancel`, data);
   },
   GetAsSelectList(data) {
      return ApiService.post(`hrm/AppointEmployee/GetAsSelectList`, data);
   },
   SignUpdate(id) {
      return ApiService.post(`hrm/AppointEmployee/SignUpdate?id=${id}`);
   }
};
export default AppointEmployeeService;
