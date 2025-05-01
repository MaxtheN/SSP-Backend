import ApiService from '../api.service';

const StaffingService = {
   GetList(data) {
      return ApiService.post('hrm/Staffing/GetList', data);
   },
   GetListForHeader(data) {
      return ApiService.post('hrm/Staffing/GetListForHeader', data);
   },
   GetListForOwnOrg(data) {
      return ApiService.post('/hrm/Staffing/GetListForOwnOrg', data);
   },
   GetAllStaffingPositionsForQuantity(positionId, departmentId, organizationid) {
      if (organizationid) {
         return ApiService.post(
            `hrm/Staffing/GetAllStaffingPositionsForQuantity?positionId=${positionId}&departmentId=${departmentId}&organizationid=${organizationid}`
         );
      } else {
         return ApiService.post(
            `hrm/Staffing/GetAllStaffingPositionsForQuantity?positionId=${positionId}&departmentId=${departmentId}`
         );
      }
   },
   GetStaffingPositionClassification(date, positionClassificationId, departmentId, organizationid) {
      if (organizationid) {
         return ApiService.get(`hrm/Staffing/GetAllStaffingPositions`, {
            params: {
               date,
               positionClassificationId,
               departmentId,
               organizationid
            }
         });
      } else {
         return ApiService.get(`hrm/Staffing/GetAllStaffingPositions`, {
            params: {
               date,
               positionClassificationId,
               departmentId
            }
         });
      }
   },
   FillStaffingPositions(staffingTemplateId) {
      return ApiService.get(`hrm/Staffing/FillStaffingPositions?staffingTemplateId=${staffingTemplateId}`);
   },
   GetStaffingPosition(data) {
      return ApiService.post(`hrm/Staffing/GetStaffingPosition`, data);
   },
   FillIndicator(data) {
      return ApiService.post(`hrm/Staffing/FillIndicator`, data);
   },
   RecalcStaffingCalcKindTables(data) {
      return ApiService.post(`hrm/Staffing/RecalcStaffingCalcKindTables`, data);
   },
   Get(id) {
      if (id == 0 || id === null || id === undefined) {
         return ApiService.get('hrm/Staffing/Get');
      } else {
         return ApiService.get(`hrm/Staffing/Get/${id}`);
      }
   },
   Update(data) {
      if (data.id == 0) {
         return ApiService.post(`hrm/Staffing/Create`, data);
      } else {
         return ApiService.post(`hrm/Staffing/Update`, data);
      }
   },
   Delete(id) {
      return ApiService.post(`hrm/Staffing/Delete/${id}`);
   },
   GetAsSelectList(data) {
      return ApiService.post(`hrm/Staffing/GetAsSelectList`, data);
   },
   Accept(data) {
      return ApiService.post(`hrm/Staffing/Accept`, data);
   },
   Cancel(data) {
      return ApiService.post(`hrm/Staffing/Cancel`, data);
   },
   Send(id) {
      return ApiService.post(`hrm/Staffing/Send/${id}`);
   },
   Reject(data) {
      return ApiService.post(`hrm/Staffing/Reject`, data);
   },
   Revoke(data) {
      return ApiService.post(`hrm/Staffing/Revoke`, data);
   },
   Receieved(data) {
      return ApiService.post(`hrm/Staffing/Receieved`, data);
   },
   Create(data) {
      if (data.id == 0) {
         return ApiService.post(`hrm/Staffing/CreateWithUser`, data);
      } else {
         return ApiService.post(`hrm/Staffing/UpdateWithUser`, data);
      }
   },
   GetAllStaffingPositionsAll() {
      return ApiService.get('/hrm/Staffing/GetAllStaffingPositions');
   },
   GetClone(id) {
      return ApiService.get(`hrm/Staffing/GetClone/${id}`);
   },
   SendToArchive(id) {
      return ApiService.post(`/hrm/Staffing/SendToArchive/${id}`);
   },
   RecallFromArchive(id) {
      return ApiService.post(`/hrm/Staffing/RecallFromArchive/${id}`);
   }
};
export default StaffingService;
