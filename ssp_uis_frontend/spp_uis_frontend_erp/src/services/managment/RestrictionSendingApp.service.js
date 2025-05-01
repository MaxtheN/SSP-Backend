import ApiService from '../api.service';

const RestrictionSendingAppService = {
   GetList(data) {
      return ApiService.post('public/RestrictionSendingApp/GetList', data);
   },

   Get(id) {
      if (id == 0 || id === null || id === undefined) {
         return ApiService.get('public/RestrictionSendingApp/Get');
      } else {
         return ApiService.get(`public/RestrictionSendingApp/Get/${id}`);
      }
   },
   Update(data) {
      if (data.id == 0) {
         return ApiService.post(`public/RestrictionSendingApp/Create`, data);
      } else {
         return ApiService.post(`public/RestrictionSendingApp/Update`, data);
      }
   },
   Delete(id) {
      return ApiService.post(`public/RestrictionSendingApp/Delete/${id}`);
   },
   
     

};
export default RestrictionSendingAppService;
