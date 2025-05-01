import ApiService from '../api.service'

const AppealDescriptionService = {
    GetList(data) {
        return ApiService.post('AppealDescription/GetList', data)
    },
    Get(id) {
        if (id == 0 || id === null || id === undefined) {
            return ApiService.get('AppealDescription/Get')
        } else {
            return ApiService.get(`AppealDescription/Get/${id}`)
        }
    },
    Update(data) {
        if (data.id == 0) {
            return ApiService.post(`AppealDescription/Create`, data)
        } else {
            return ApiService.post(`AppealDescription/Update`, data)
        }

    },
    Delete(id) {
        return ApiService.post(`AppealDescription/Delete/${id}`)
    },
    GetAsSelectList(hasParent) {
        return ApiService.get(`AppealDescription/GetAsSelectList`, {
            params: {
                hasParent
            }
        })
    }
}
export default AppealDescriptionService