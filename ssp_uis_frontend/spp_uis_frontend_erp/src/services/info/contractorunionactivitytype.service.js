import ApiService from '../api.service';

const ContractorUnionActivityTypeService = {
    GetList(data) {
        return ApiService.post('corruption/ContractorUnionActivityType/GetList', data);
    },
    Get(id) {
        if (id == 0 || id === null || id === undefined) {
            return ApiService.get('corruption/ContractorUnionActivityType/Get');
        } else {
            return ApiService.get(`corruption/ContractorUnionActivityType/Get/${id}`);
        }
    },
    Update(data) {
        if (data.id == 0) {
            return ApiService.post(`corruption/ContractorUnionActivityType/Create`, data);
        } else {
            return ApiService.post(`corruption/ContractorUnionActivityType/Update`, data);
        }
    },
    Delete(id) {
        return ApiService.post(`corruption/ContractorUnionActivityType/Delete/${id}`);
    },
    GetAsSelectList() {
        return ApiService.get(`corruption/ContractorUnionActivityType/GetAsSelectList`);
    },
    Create(data) {
        if (data.id == 0) {
            return ApiService.post(`corruption/ContractorUnionActivityType/CreateWithUser`, data);
        } else {
            return ApiService.post(`corruption/ContractorUnionActivityType/UpdateWithUser`, data);
        }
    }
};
export default ContractorUnionActivityTypeService;
