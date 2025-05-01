import ApiService from '../api.service';

const CustomJobService = {
   GetList(data) {
      return ApiService.post('CustomJob/GetList', data);
   },
   Get(id) {
      if (id == 0 || id === null || id === undefined) {
         return ApiService.get('CustomJob/Get');
      } else {
         return ApiService.get(`CustomJob/Get/${id}`);
      }
   },
   Update(data) {
      if (data.id == 0) {
         return ApiService.post(`CustomJob/Create`, data);
      } else {
         return ApiService.post(`CustomJob/Update`, data);
      }
   },
   Delete(id) {
      return ApiService.post(`CustomJob/Delete/${id}`);
   },
   Approve(id) {
      return ApiService.post(`CustomJob/Approve`, {
         id: id
      });
   },
   CancelApprove(id) {
      return ApiService.post(`CustomJob/CancelApprove`, {
         id: id
      });
   }
};
export default CustomJobService;
