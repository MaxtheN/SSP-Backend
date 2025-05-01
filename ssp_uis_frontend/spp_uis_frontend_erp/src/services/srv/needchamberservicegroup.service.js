import ApiService from "../api.service";
const NeedChamberServiceGroupService = {
    GetList(data) {
        return ApiService.post("/public/NeedChamberServiceGroup/GetList", data)
    },
    Get(id) {
        if (id == 0 || id === null || id === undefined) {
            return ApiService.get("/public/NeedChamberServiceGroup/Get")
        } else {
            return ApiService.get(`/public/NeedChamberServiceGroup/Get/${id}`)
        }
    },
    Update(data) {
        if (data.id == 0) {
            return ApiService.post("/public/NeedChamberServiceGroup/Create", data)
        } else {
            return ApiService.post("/public/NeedChamberServiceGroup/Update", data)
        }
    },
    GetAsSelectList() {
        return ApiService.get("/public/NeedChamberServiceGroup/GetAsSelectList")
    },
    Delete(id) {
        return ApiService.post(`/public/NeedChamberServiceGroup/Delete/${id}`)
    }
}

export default NeedChamberServiceGroupService;