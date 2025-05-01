import ApiService from './api.service'
const ApplicationForCourtService = {
    GetList(data){
        return ApiService.post(`/ApplicationForCourt/GetList`,data)
    },
    Get(id){
        return ApiService.get(`/ApplicationForCourt/Get/${id}`)
    },
    DownloadPdf(id2){
        return ApiService.get(`/ApplicationForCourt/GetByPlanId?id2=${id2}`)
    }
}
export default ApplicationForCourtService