import ApiService from '../api.service';

const QualificationCategoryService = {
    GetList(data) {
        return ApiService.post('hrm/QualificationCategory/GetList', data);
    },
    Get(id) {
        if (id == 0 || id === null || id === undefined) {
            return ApiService.get('hrm/QualificationCategory/Get');
        } else {
            return ApiService.get(`hrm/QualificationCategory/Get/${id}`);
        }
    },
    Update(data) {
        if (data.id == 0) {
            return ApiService.post(`hrm/QualificationCategory/Create`, data);
        } else {
            return ApiService.post(`hrm/QualificationCategory/Update`, data);
        }
    },
    Delete(id) {
        return ApiService.post(`hrm/QualificationCategory/Delete/${id}`);
    },
    GetAsSelectList() {
        return ApiService.get(`hrm/QualificationCategory/GetAsSelectList`);
    },
    Create(data) {
        if (data.id == 0) {
            return ApiService.post(`hrm/QualificationCategory/CreateWithUser`, data);
        } else {
            return ApiService.post(`hrm/QualificationCategory/UpdateWithUser`, data);
        }
    }
};
export default QualificationCategoryService;
