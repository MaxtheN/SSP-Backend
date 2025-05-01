import ApiService from '../api.service';

const StaffingIndicatorService = {
   GetList(data) {
      return ApiService.post('hrm/StaffingIndicator/GetList', data);
   },
   Get(id) {
      if (id == 0 || id === null || id === undefined) {
         return ApiService.get('hrm/StaffingIndicator/Get');
      } else {
         return ApiService.get(`hrm/StaffingIndicator/Get/${id}`);
      }
   },
   Update(data) {
      if (data.id == 0) {
         return ApiService.post(`hrm/StaffingIndicator/Create`, data);
      } else {
         return ApiService.post(`hrm/StaffingIndicator/Update`, data);
      }
   },
   Delete(id) {
      return ApiService.post(`hrm/StaffingIndicator/Delete/${id}`);
   },
   GetAsSelectList(data) {
      return ApiService.get(`hrm/StaffingIndicator/GetAsSelectList`, {
         params: data
      });
   },
   Create(data) {
      if (data.id == 0) {
         return ApiService.post(`hrm/StaffingIndicator/CreateWithUser`, data);
      } else {
         return ApiService.post(`hrm/StaffingIndicator/UpdateWithUser`, data);
      }
   }
};
export default StaffingIndicatorService;
