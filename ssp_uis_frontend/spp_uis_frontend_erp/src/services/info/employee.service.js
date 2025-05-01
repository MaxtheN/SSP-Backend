import ApiService from '../api.service';
const EmployeeService = {
   GetList(data) {
      return ApiService.post('hrm/Employee/GetList', data);
   },
   GetListAttestation(data) {
      return ApiService.post('hrm/Employee/GetListWithAttestation', data);
   },
   GetAttestationList(id) {
      return ApiService.get(`hrm/Employee/GetAttestationList/${id}`);
   },
   Get(id) {
      if (id == 0 || id === null || id === undefined) {
         return ApiService.get('hrm/employee/Get');
      } else {
         return ApiService.get(`hrm/employee/Get/${id}`);
      }
   },
   Update(data) {
      if (data.id == 0) {
         return ApiService.post(`hrm/employee/Create`, data);
      } else {
         return ApiService.post(`hrm/employee/Update`, data);
      }
   },
   Delete(id) {
      return ApiService.post(`hrm/employee/Delete/${id}`);
   },
   GetByPassportData(Seria, Number, DateOfBirth) {
      return ApiService.get(
         `hrm/Employee/GetByPassportData?Seria=${Seria}&Number=${Number}&DateOfBirth=${DateOfBirth}`
      );
   },
   GetByPassportDataFromDigital(document, birthDate) {
      return ApiService.get(
         `/Person/GetByPassportDataFromDigital?transaction_id=3&is_consent=Y&langId=1&document=${document}&birth_date=${birthDate}&is_photo=Y`
      );
   },
   GetAsSelectList(data) {
      return ApiService.post(`hrm/Employee/GetAsSelectList`, data);
   },
   Create(data) {
      if (data.id == 0) {
         return ApiService.post(`hrm/Employee/CreateWithUser`, data);
      } else {
         return ApiService.post(`hrm/Employee/UpdateWithUser`, data);
      }
   },
   // CreateWithUser(data) {
   //   return ApiService.post(`/Employee/CreateWithUser`, data);
   // },
   SaveAsExecel1(data) {
      return ApiService.printtemp(`hrm/Employee/SaveAsExecel`, data);
   },
   SaveAsExecelKadir(data) {
      return ApiService.printtemp(`hrm/Employee/SaveAsExecelKadir`, data);
   },
   MakeHr(id) {
      return ApiService.post(`hrm/Employee/MakeHr/${id}`);
   },
   MakeInspector(id) {
      return ApiService.post(`hrm/Employee/MakeInspector/${id}`);
   },
   SaveAsExecel(data) {
      return ApiService.printtemp(`hrm/Employee/GetListWithAttestationExcel`, data);
   },

   DownloadEmployeeCv(pinfl) {
      return ApiService.print(`hrm/Employee/DownloadEmployeeCv?pinfl=${pinfl}`);
   },
   AddOrUpdatePersonFiles(data) {
      return ApiService.post(`hrm/Employee/AddOrUpdatePersonFiles`, data);
   },
   GetWorkYear(data) {
      return ApiService.post(`EmployeeWorkSchedule/GetWorkYear`, data);
   },
   GetWorkYearFromMehnat(data) {
      return ApiService.post(`EmployeeWorkSchedule/GetWorkYearFromMehnat`, data);
   },
   GetTotalWorkYears(data) {
      return ApiService.post(`EmployeeWorkSchedule/GetTotalWorkYears`, data);
   },
   GetWorkYearFromEmpManage(data) {
      return ApiService.post(`EmployeeWorkSchedule/GetWorkYearFromEmpManage`, data);
   }
};
export default EmployeeService;
