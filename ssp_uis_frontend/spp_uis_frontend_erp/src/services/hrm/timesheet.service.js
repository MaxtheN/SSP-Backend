import ApiService from '../api.service';

const TimesheetService = {
   GetList(data) {
      return ApiService.post('hrm/Timesheet/GetList', data);
   },
   Get(id) {
      if (id == 0 || id === null || id === undefined) {
         return ApiService.get('hrm/Timesheet/Get');
      } else {
         return ApiService.get(`hrm/Timesheet/Get/${id}`);
      }
   },
   Update(data) {
      if (data.id == 0) {
         return ApiService.post(`hrm/Timesheet/Create`, data);
      } else {
         return ApiService.post(`hrm/Timesheet/Update`, data);
      }
   },
   Delete(id) {
      return ApiService.post(`hrm/Timesheet/Delete/${id}`);
   },
   GetTable(id) {
      return ApiService.get(`hrm/Timesheet/GetTable/${id}`);
   },
   UpdateTable(data) {
      return ApiService.post(`hrm/Timesheet/UpdateTable`, data);
   },
   Accept(data) {
      return ApiService.post(`hrm/Timesheet/Accept`, data);
   },
   Cancel(data) {
      return ApiService.post(`hrm/Timesheet/Cancel`, data);
   },
   GetAsSelectList(data) {
      return ApiService.post(`hrm/Timesheet/GetAsSelectList`, data);
   },
   Create(data) {
      if (data.id == 0) {
         return ApiService.post(`hrm/Timesheet/CreateWithUser`, data);
      } else {
         return ApiService.post(`hrm/Timesheet/UpdateWithUser`, data);
      }
   },
   FillTimeSheet(data) {
      return ApiService.post(`hrm/Timesheet/FillTimeSheet`, data);
   },
   Clear(data) {
      return ApiService.post(`hrm/Timesheet/Clear`, data);
   },
   SaveAsExecelForTabel(id) {
      return ApiService.printtemp(`hrm/Timesheet/SaveAsExecelForTabel?id=${id}`);
   }
};
export default TimesheetService;
