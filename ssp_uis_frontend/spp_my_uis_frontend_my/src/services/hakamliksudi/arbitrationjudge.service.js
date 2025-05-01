import ApiService from "../api.service";
const ArbitrationJudgeService = {
  GetList(data) {
    return ApiService.post("/Arbitration/ArbitrationJudge/GetList", data);
  },
  GetAsSelectList() {
    return ApiService.get(`/Arbitration/ArbitrationJudge/GetAsSelectList`);
  },
  Get(id) {
    return ApiService.get(`/Arbitration/ArbitrationJudge/Get/${id}`);
  },
};
export default ArbitrationJudgeService;
