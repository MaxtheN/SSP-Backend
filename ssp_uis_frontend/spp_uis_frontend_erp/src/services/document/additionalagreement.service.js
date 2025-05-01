import ApiService from '../api.service';

const AdditionalAgreementService = {
   GetList(data) {
      return ApiService.post(`/AdditionalAgreement/GetList`, data);
   },
   Get(id) {
      if (id == 0 || id === null || id === undefined) {
         return ApiService.get(`/AdditionalAgreement/Get`);
      } else {
         return ApiService.get(`/AdditionalAgreement/Get/${id}`);
      }
   },
   Update(data) {
      if (data.id == 0) {
         return ApiService.post(`/AdditionalAgreement/Create`, data);
      } else {
         return ApiService.post(`/AdditionalAgreement/Update`, data);
      }
   },
   Delete(id) {
      return ApiService.post(`/AdditionalAgreement/Delete/${id}`);
   },
   GetAsSelectList() {
      return ApiService.post(`/AdditionalAgreement/GetAsSelectList`);
   },
   GetByMemshipContractId(id) {
      return ApiService.get(`/AdditionalAgreement/GetByMemshipContractId/${id}`);
   },
   Delete(id) {
      return ApiService.post(`/AdditionalAgreement/Delete/${id}`);
   },
   Sign(data) {
      return ApiService.post(`/AdditionalAgreement/Sign`, data);
   },
   Reject(data) {
      return ApiService.post(`/AdditionalAgreement/Reject`, data);
   }
};
export default AdditionalAgreementService;
