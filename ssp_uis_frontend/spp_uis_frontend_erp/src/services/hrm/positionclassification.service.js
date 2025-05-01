import ApiService from '../api.service';

const PositionClassificationService = {
    GetList(data) {
        return ApiService.post('hrm/PositionClassification/GetList', data);
    },
    Get(id) {
        if (id == 0 || id === null || id === undefined) {
            return ApiService.get('hrm/PositionClassification/Get');
        } else {
            return ApiService.get(`hrm/PositionClassification/Get/${id}`);
        }
    },
    Update(data) {
        if (data.id == 0) {
            return ApiService.post(`hrm/PositionClassification/Create`, data);
        } else {
            return ApiService.post(`hrm/PositionClassification/Update`, data);
        }
    },
    Delete(id) {
        return ApiService.post(`hrm/PositionClassification/Delete/${id}`);
    },
    GetAsSelectList() {
        return ApiService.get(`hrm/PositionClassification/GetAsSelectList`);
    },
    Create(data) {
        if (data.id == 0) {
            return ApiService.post(`hrm/PositionClassification/CreateWithUser`, data);
        } else {
            return ApiService.post(`hrm/PositionClassification/UpdateWithUser`, data);
        }
    }
};
export default PositionClassificationService;
