import ApiService from '../api.service';
const SpecialtyService = {
    GetList(data) {
        return ApiService.post('/dualedu/Specialty/GetList', data);
    },
    Get(id) {
        if (id == 0 || id === null || id === undefined) {
            return ApiService.get('/dualedu/Specialty/Get');
        } else {
            return ApiService.get(`/dualedu/Specialty/Get/${id}`);
        }
    },
    Update(data) {
        if (data.id == 0) {
            return ApiService.post(`/dualedu/Specialty/Create`, data);
        } else {
            return ApiService.post(`/dualedu/Specialty/Update`, data);
        }
    },
    GetAsSelectList(instituteId) {
        return ApiService.get(`/dualedu/Specialty/GetAsSelectList?instituteId=${instituteId}`);
    },
    Delete(id) {
        return ApiService.post(`/dualedu/Specialty/Delete/${id}`);
    },
};
export default SpecialtyService;
