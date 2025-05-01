import ApiService from '../api.service';

const BirthdayService = {
   GetEmployeeBirthDate(data) {
      return ApiService.post('/HrmDashboard/GetEmployeeBithDate', data);
   }
};

export default BirthdayService;
