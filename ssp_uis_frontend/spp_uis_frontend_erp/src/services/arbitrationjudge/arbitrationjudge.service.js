import ApiService from '../api.service';
const ArbitrationJudgeService = {
   GetList(data) {
      return ApiService.post('/Arbitration/ArbitrationJudge/GetList', data);
   },
   GetAsSelectList() {
      return ApiService.get('/Arbitration/ArbitrationJudge/GetAsSelectList');
   },
   Get(id) {
      if (id == 0) {
         return ApiService.get('/Arbitration/ArbitrationJudge/Get');
      } else {
         return ApiService.get(`/Arbitration/ArbitrationJudge/Get/${id}`);
      }
   },
   Update(data) {
      if (data.id == 0) {
         return ApiService.post(`/Arbitration/ArbitrationJudge/Create`, data);
      } else {
         return ApiService.post(`/Arbitration/ArbitrationJudge/Update`, data);
      }
   },

   Delete(id) {
      return ApiService.post(`/Arbitration/ArbitrationJudge/Delete/${id}`);
   }
};

export default ArbitrationJudgeService;
