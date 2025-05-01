import ApiService from '../api.service';

const PositionCategoryService = {
    GetList(data) {
        return ApiService.post('hrm/PositionCategory/GetList', data);
    },
    Get(id) {
        if (id == 0 || id === null || id === undefined) {
            return ApiService.get('hrm/PositionCategory/Get');
        } else {
            return ApiService.get(`hrm/PositionCategory/Get/${id}`);
        }
    },
    Update(data) {
        if (data.id == 0) {
            return ApiService.post(`hrm/PositionCategory/Create`, data);
        } else {
            return ApiService.post(`hrm/PositionCategory/Update`, data);
        }
    },
    Delete(id) {
        return ApiService.post(`hrm/PositionCategory/Delete/${id}`);
    },
    GetAsSelectList() {
        return ApiService.get(`hrm/PositionCategory/GetAsSelectList`);
    },
    Create(data) {
        if (data.id == 0) {
            return ApiService.post(`hrm/PositionCategory/CreateWithUser`, data);
        } else {
            return ApiService.post(`hrm/PositionCategory/UpdateWithUser`, data);
        }
    }
};
export default PositionCategoryService;
