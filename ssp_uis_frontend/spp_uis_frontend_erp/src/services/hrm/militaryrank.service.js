import ApiService from '../api.service';

const MilitaryRankService = {
   GetList(data) {
      return ApiService.post('hrm/MilitaryRank/GetList', data);
   },
   Get(id) {
      if (id == 0 || id === null || id === undefined) {
         return ApiService.get('hrm/MilitaryRank/Get');
      } else {
         return ApiService.get(`hrm/MilitaryRank/Get/${id}`);
      }
   },
   Update(data) {
      if (data.id == 0) {
         return ApiService.post(`hrm/MilitaryRank/Create`, data);
      } else {
         return ApiService.post(`hrm/MilitaryRank/Update`, data);
      }
   },
   Delete(id) {
      return ApiService.post(`hrm/MilitaryRank/Delete/${id}`);
   },
   GetAsSelectList() {
      return ApiService.get(`hrm/MilitaryRank/GetAsSelectList`);
   }
};

export default MilitaryRankService;
