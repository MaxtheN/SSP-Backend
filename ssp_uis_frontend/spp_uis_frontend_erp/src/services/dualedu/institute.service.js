import ApiService from '../api.service';
const InstituteService = {
    GetList(data) {
        return ApiService.post('/dualedu/Institute/GetList', data);
    },
    Get(id) {
        if (id == 0 || id === null || id === undefined) {
            return ApiService.get('/dualedu/Institute/Get');
        } else {
            return ApiService.get(`/dualedu/Institute/Get/${id}`);
        }
    },
    Update(data) {
        if (data.id == 0) {
            return ApiService.post(`/dualedu/Institute/Create`, data);
        } else {
            return ApiService.post(`/dualedu/Institute/Update`, data);
        }
    },
    GetAsSelectList() {
        return ApiService.get('/dualedu/Institute/GetAsSelectList');
    },
    Delete(id) {
        return ApiService.post(`/dualedu/Institute/Delete/${id}`);
    },
};
export default InstituteService;
