import ApiService from '../api.service';

const ElectionMemberService = {
   GetList(data) {
      return ApiService.post('hrm/ElectionMember/GetList', data);
   },
   Get(id) {
      if (id == 0 || id === null || id === undefined) {
         return ApiService.get('hrm/ElectionMember/Get');
      } else {
         return ApiService.get(`hrm/ElectionMember/Get/${id}`);
      }
   },
   Update(data) {
      if (data.id == 0) {
         return ApiService.post(`hrm/ElectionMember/Create`, data);
      } else {
         return ApiService.post(`hrm/ElectionMember/Update`, data);
      }
   },
   Delete(id) {
      return ApiService.post(`hrm/ElectionMember/Delete/${id}`);
   },
   GetAsSelectList() {
      return ApiService.get(`hrm/ElectionMember/GetAsSelectList`);
   }
};

export default ElectionMemberService;
