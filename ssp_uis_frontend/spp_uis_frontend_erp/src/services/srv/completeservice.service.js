import ApiService from "../api.service";
const CompleteService = {
    GetList(data) {
        return ApiService.post("/srv/CompleteService/GetList", data)
    },
    Get(id) {
        if (id == 0 || id === undefined || id === null) {
            return ApiService.get("/srv/CompleteService/Get")
        } else {
            return ApiService.get(`/srv/CompleteService/Get/${id}`)
        }
    },
    Update(data) {
        if (data.id == 0) {
            return ApiService.post("/srv/CompleteService/Create", data)
        } else {
            return ApiService.post("/srv/CompleteService/Update", data)
        }
    },
    Delete(id) {
        return ApiService.post(`/srv/CompleteService/Delete/${id}`)
    }
}
export default CompleteService