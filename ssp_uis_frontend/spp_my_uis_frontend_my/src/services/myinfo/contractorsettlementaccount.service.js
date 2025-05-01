import ApiService from '../api.service';

export const ContractorSettlementAccountService = {
    GetList(data) {
        return ApiService.post('/ContractorSettlementAccount/GetList', data);
    },
    Get(id) {
        if (id == 0 || id == undefined || id == null) {
            return ApiService.get('/ContractorSettlementAccount/Get');
        } else {
            return ApiService.get(`/ContractorSettlementAccount/Get/${id}`);
        }
    },
    Update(data) {
        if (data.id == 0) {
            return ApiService.post('/ContractorSettlementAccount/Create', data);
        } else {
            return ApiService.post('/ContractorSettlementAccount/Update', data);
        }
    },
    Delete(id) {
        return ApiService.post(`/ContractorSettlementAccount/Delete/${id}`);
    },
    SetMain(id) {
        return ApiService.post(`/ContractorSettlementAccount/SetMain/${id}`);
    }
};

export default ContractorSettlementAccountService;
