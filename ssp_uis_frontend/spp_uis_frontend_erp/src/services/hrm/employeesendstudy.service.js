import ApiService from '../api.service';

const EmployeeSendStudyService = {
   GetList(data) {
      return ApiService.post('hrm/EmployeeSendStudy/GetList', data);
   },
   GetListForSigner(data) {
      return ApiService.post('hrm/EmployeeSendStudy/GetListForSigner', data);
   },
   Get(id) {
      if (id == 0 || id === null || id === undefined) {
         return ApiService.get('hrm/EmployeeSendStudy/Get');
      } else {
         return ApiService.get(`hrm/EmployeeSendStudy/Get/${id}`);
      }
   },
   Update(data) {
      if (data.id == 0) {
         return ApiService.post(`hrm/EmployeeSendStudy/Create`, data);
      } else {
         return ApiService.post(`hrm/EmployeeSendStudy/Update`, data);
      }
   },
   Accept(data) {
      return ApiService.post(`hrm/EmployeeSendStudy/Accept`, data);
   },
   Sign(data) {
      return ApiService.post(`hrm/EmployeeSendStudy/Sign`, data);
   },
   Cancel(data) {
      return ApiService.post(`hrm/EmployeeSendStudy/Cancel`, data);
   },
   Delete(id) {
      return ApiService.post(`hrm/EmployeeSendStudy/Delete/${id}`);
   },
   GetAsSelectList(data) {
      return ApiService.post(`hrm/EmployeeSendStudy/GetAsSelectList`, data);
   }
};
export default EmployeeSendStudyService;
