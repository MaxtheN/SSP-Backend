import ApiService from '../api.service';

const ContractorActivityGroupService = {
    GetList(data) {
        return ApiService.post('hrm/ContractorActivityGroup/GetList', data);
    },
    Get(id) {
        if (id == 0 || id === null || id === undefined) {
            return ApiService.get('hrm/ContractorActivityGroup/Get');
        } else {
            return ApiService.get(`hrm/ContractorActivityGroup/Get/${id}`);
        }
    },
    Update(data) {
        if (data.id == 0) {
            return ApiService.post(`hrm/ContractorActivityGroup/Create`, data);
        } else {
            return ApiService.post(`hrm/ContractorActivityGroup/Update`, data);
        }
    },
    Delete(id) {
        return ApiService.post(`hrm/ContractorActivityGroup/Delete/${id}`);
    },
    GetAsSelectList() {
        return ApiService.get(`hrm/ContractorActivityGroup/GetAsSelectList`);
    },
    Create(data) {
        if (data.id == 0) {
            return ApiService.post(`hrm/ContractorActivityGroup/CreateWithUser`, data);
        } else {
            return ApiService.post(`hrm/ContractorActivityGroup/UpdateWithUser`, data);
        }
    }
};
export default ContractorActivityGroupService;
