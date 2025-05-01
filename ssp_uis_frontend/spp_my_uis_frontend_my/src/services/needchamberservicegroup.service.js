import ApiService from "./api.service";
const NeedChamberServiceGroupService ={
    GetList(data){
        return ApiService.post(`/NeedChamberServiceGroup/GetList`,data)
    },
    Get(id){
        return ApiService.get(`/NeedChamberServiceGroup/Get/${id}`,)
    },

}
export default NeedChamberServiceGroupService