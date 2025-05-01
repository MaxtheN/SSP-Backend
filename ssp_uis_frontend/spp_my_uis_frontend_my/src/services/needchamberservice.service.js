import ApiService from "./api.service";
const NeedChamberServiceService = {
    GetList(data) {
        return ApiService.post(`/NeedChamberService/GetList`, data)
    },
    Get(id) {
        return ApiService.get(`/NeedChamberService/Get/${id}`,)
    },
    WihtOfferta() {
        return ApiService.get(`/NeedChamberService/WihtOfferta`,)
    },
    GroupingByFreeServices(data) {
        return ApiService.post(`/NeedChamberService/GroupingByFreeServices`,data)
    }

}
export default NeedChamberServiceService