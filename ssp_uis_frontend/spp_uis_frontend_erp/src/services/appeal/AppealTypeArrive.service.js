import ApiService from '../api.service'

const AppealTypeArriveService = {
    GetList(data) {
        return ApiService.post('appeal/AppealTypeArrive/GetList', data)
    },
    Get(id) {
        if (id == 0 || id === null || id === undefined) {
            return ApiService.get('appeal/AppealTypeArrive/Get')
        } else {
            return ApiService.get(`appeal/AppealTypeArrive/Get/${id}`)
        }
    },
    Update(data) {
        if (data.id == 0) {
            return ApiService.post(`appeal/AppealTypeArrive/Create`, data)
        } else {
            return ApiService.post(`appeal/AppealTypeArrive/Update`, data)
        }

    },
    Delete(id) {
        return ApiService.post(`appeal/AppealTypeArrive/Delete/${id}`)
    },
    GetAsSelectList() {
        return ApiService.get(`appeal/AppealTypeArrive/GetAsSelectList`)
    }
}
export default AppealTypeArriveService