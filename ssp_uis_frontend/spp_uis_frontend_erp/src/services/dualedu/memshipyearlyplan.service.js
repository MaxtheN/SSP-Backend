import ApiService from "../api.service";
const MemshipYearlyPlanService = {
    GetList(data) {
        return ApiService.post('/memship/MemshipYearlyPlan/GetList', data);
    },
    Get(id) {
        if (id == 0 || id === null || id === undefined) {
            return ApiService.get('/memship/MemshipYearlyPlan/Get')
        } else {
            return ApiService.get(`/memship/MemshipYearlyPlan/Get/${id}`)
        }

    },
    Update(data) {
        if (data.id == 0) {
            return ApiService.post(`/memship/MemshipYearlyPlan/Create`, data)
        } else {
            return ApiService.post(`/memship/MemshipYearlyPlan/Update`, data)
        }
    },
    FillTable() {
        return ApiService.get(`/memship/MemshipYearlyPlan/FillTable`)
    },
    Accept(data) {
        return ApiService.post('/memship/MemshipYearlyPlan/Accept', data)
    },
    Cancel(data) {
        return ApiService.post('/memship/MemshipYearlyPlan/Cancel', data)
    },
    Delete(id) {
        return ApiService.post(`/memship/MemshipYearlyPlan/Delete/${id}`)
    },
    UploadFile(files) {
        return ApiService.post('/memship/MemshipYearlyPlan/UploadFile', files)
    },
    DownloadFile(fileId) {
        return ApiService.print(`/memship/MemshipYearlyPlan/DownloadFile/${fileId}`)
    },
    DeleteFile(fileId) {
        return ApiService.post(`/memship/MemshipYearlyPlan/DeleteFile/${fileId}`)
    }
}
export default MemshipYearlyPlanService