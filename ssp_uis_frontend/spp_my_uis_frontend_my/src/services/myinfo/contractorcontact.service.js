import ApiService from '../api.service';

export const ContractorContactService = {
    GetList(data) {
        return ApiService.post('/ContractorContact/GetList', data);
    },
    Get(id) {
        if (id == 0 || id == undefined || id == null) {
            return ApiService.get('/ContractorContact/Get');
        } else {
            return ApiService.get(`/ContractorContact/Get/${id}`);
        }
    },
    Update(data) {
        if (data.id == 0) {
            return ApiService.post('/ContractorContact/Create', data);
        } else {
            return ApiService.post('/ContractorContact/Update', data);
        }
    },
    Delete(id) {
        return ApiService.post(`/ContractorContact/Delete/${id}`);
    }
};

export default ContractorContactService;
