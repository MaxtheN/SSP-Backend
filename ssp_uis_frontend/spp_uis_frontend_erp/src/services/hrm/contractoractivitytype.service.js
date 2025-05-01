import ApiService from '../api.service';

const ContractorActivityTypeService = {
    GetList(data) {
        return ApiService.post('hrm/ContractorActivityType/GetList', data);
    },
    Get(id) {
        if (id == 0 || id === null || id === undefined) {
            return ApiService.get('hrm/ContractorActivityType/Get');
        } else {
            return ApiService.get(`hrm/ContractorActivityType/Get/${id}`);
        }
    },
    Update(data) {
        if (data.id == 0) {
            return ApiService.post(`hrm/ContractorActivityType/Create`, data);
        } else {
            return ApiService.post(`hrm/ContractorActivityType/Update`, data);
        }
    },
    Delete(id) {
        return ApiService.post(`hrm/ContractorActivityType/Delete/${id}`);
    },
    GetAsSelectList() {
        return ApiService.get(`hrm/ContractorActivityType/GetAsSelectList`);
    },
    Create(data) {
        if (data.id == 0) {
            return ApiService.post(`hrm/ContractorActivityType/CreateWithUser`, data);
        } else {
            return ApiService.post(`hrm/ContractorActivityType/UpdateWithUser`, data);
        }
    }
};
export default ContractorActivityTypeService;
