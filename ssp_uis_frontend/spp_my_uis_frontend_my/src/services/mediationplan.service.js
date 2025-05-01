import ApiService from './api.service'
const MediationPlanService = {
    GetList(data){
        return ApiService.post(`/MediationPlan/GetList`,data)
    },
    Get(id){
        return ApiService.get(`/MediationPlan/Get/${id}`)
    },
    GetByApplicationId(applicationId){
        return ApiService.get(`/MediationPlan/GetByApplicationId?applicationId=${applicationId}`)
    },
    
}
export default MediationPlanService