import ApiService from '../api.service';

const UserService = {
   GetList(data) {
      return ApiService.post(`/User/GetList`, data);
   },
   Get(id) {
      if (id == 0 || id === null || id === undefined) {
         return ApiService.get(`/User/Get`);
      } else {
         return ApiService.get(`/User/Get/${id}`);
      }
   },
   GetByPassportData(data) {
      return ApiService.get(
         `/User/GetByPassportData?Seria=${data.Seria}&Number=${data.Number}&DateOfBirth=${data.DateOfBirth}`
      );
   },
   Update(data) {
      if (data.id == 0) {
         return ApiService.post(`/User/Create`, data);
      } else {
         return ApiService.post('/User/Update', data);
      }
   },
   GetByPassportData(OrganizationId, Seria, Number, DateOfBirth) {
      return ApiService.get(
         `/User/GetByPassportData?OrganizationId=${OrganizationId}&Seria=${Seria}&Number=${Number}&DateOfBirth=${DateOfBirth}`
      );
   },
   GetByPassportDataFromDigital(document, birthDate) {
      return ApiService.get(
         `/Person/GetByPassportDataFromDigital?transaction_id=3&is_consent=Y&langId=1&document=${document}&birth_date=${birthDate}&is_photo=Y`
      );
   },
   CreateByEmployee(data) {
      return ApiService.post(`/User/CreateByEmployee`, data);
   },
   CheckUserName(data) {
      return ApiService.post(`/User/CreateByEmployee`, CheckUserName);
   },
   SaveAsExecel(data) {
      return ApiService.post(`/User/SaveAsExecel`, data);
   },
   Delete(id) {
      return ApiService.post(`/User/Delete/${id}`);
   },
   GetAsSelectList(data = {}) {
      return ApiService.post(`/User/GetAsSelectList`, data);
   }
};
export default UserService;
