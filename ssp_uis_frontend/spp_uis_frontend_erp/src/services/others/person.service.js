import ApiService from '../api.service';

const PersonService = {
   GetByPassportData(Seria, Number, DateOfBirth) {
      return ApiService.get(`/Person/GetByPassportData?Seria=${Seria}&Number=${Number}&DateOfBirth=${DateOfBirth}`);
   },
   GetChildFromGsp(Seria, Number, DateOfBirth) {
      return ApiService.get(`/Person/GetChildFromGsp?Seria=${Seria}&Number=${Number}&DateOfBirth=${DateOfBirth}`);
   },
   GetRelativesByPassportData(Seria, Number, DateOfBirth) {
      return ApiService.get(
         `/Person/GetRelativesByPassportData?Seria=${Seria}&Number=${Number}&DateOfBirth=${DateOfBirth}`
      );
   },
   GetByPassportDataFromDigital(document, birthDate) {
      return ApiService.get(
         `/Person/GetByPassportDataFromDigital?transaction_id=3&is_consent=Y&langId=1&document=${document}&birth_date=${birthDate}&is_photo=Y`
      );
   },
   UploadFile(data) {
      return ApiService.formData(`/Person/UploadFile`, data);
   },
   DownloadFile(fileId) {
      return ApiService.get(`/Person/DownloadFile/${fileId}`);
   },
   DeleteFile(fileId) {
      return ApiService.post(`/Person/DeleteFile/${fileId}`);
   },
   GetBirthInfoFromFHDYO(data) {
      return ApiService.post(`/DigitizationCenter/GetBirthInfoFromFHDYO`, data);
   },
   GetDeathInfoByPinflFromFHDYO(pnfl) {
      // 32802611050040;
      return ApiService.get(`/DigitizationCenter/GetDeathInfoByPinflFromFHDYO?pinfl=${pnfl}`);
   },
   GetMehnatHistory(data) {
      return ApiService.post(`/DigitizationCenter/GetMehnatHistory`, data);
   },
   GetFromGSP(data) {
      return ApiService.post(`/Person/GetFromGSP`, data);
   },
   //    CreatePersonLog(data) {
   // PassportDate=2016-08-22%20%2000%3A00%3A00&PassportExpiration=2026-08-21%2000%3A00%3A00&PassportDivName=qwerewrwe&PersonId=2745&EmployeeId=1057';
   //       return ApiService.post(`/Person/CreatePersonLog/Id=${id}&Pinfl=${Pinfl}&PassportSeria=${PassportSeria}&PassportNumber=${PassportNumber}&`, data);
   //    },
   GetByEmployeeId(id) {
      return ApiService.get(`Person/GetByEmployeeId?EmployeeId=${id}`);
   }
};
export default PersonService;
