import ApiService from '../api.service';
const DualEducationTypeService = {
    GetList(data) {
        return ApiService.post('/dualedu/DualEducationType/GetList', data);
    },
    Get(id) {
        if (id == 0 || id === null || id === undefined) {
            return ApiService.get('/dualedu/DualEducationType/Get');
        } else {
            return ApiService.get(`/dualedu/DualEducationType/Get/${id}`);
        }
    },
    Update(data) {
        if (data.id == 0) {
            return ApiService.post(`/dualedu/DualEducationType/Create`, data);
        } else {
            return ApiService.post(`/dualedu/DualEducationType/Update`, data);
        }
    },
    GetAsSelectList() {
        return ApiService.get('/dualedu/DualEducationType/GetAsSelectList');
    },
    Delete(id) {
        return ApiService.post(`/dualedu/DualEducationType/Delete/${id}`);
    },
};
export default DualEducationTypeService;
