import ApiService from "./api.service";
const PersonService = {
    GetByPassportData(Seria, Number, DateOfBirth) {
        return ApiService.get(`/Person/GetByPassportData?Seria=${Seria}&Number=${Number}&DateOfBirth=${DateOfBirth}`)
    },
    GetByPassportDataFromDigital(Seria, Number, DateOfBirth) {
        return ApiService.get(`/Person/GetByPassportDataFromDigital?transaction_id=3&is_consent=Y&langId=1&document=${Seria + Number}&birth_date=${DateOfBirth}&is_photo=Y`)
    },
    GetRelativesByPassportData(Seria, Number, DateOfBirth) {
        return ApiService.get(`/Person/GetRelativesByPassportData?Seria=${Seria}&Number=${Number}&DateOfBirth=${DateOfBirth}`)
    }
}
export default PersonService;