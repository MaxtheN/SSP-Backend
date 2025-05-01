import ApiService from '../api.service';
const CurrencyService = {
    GetList(data) {
        return ApiService.post('Currency/GetList', data);
    },
    Get(id) {
        if (id == 0 || id === null || id === undefined) {
            return ApiService.get('Currency/Get');
        } else {
            return ApiService.get(`Currency/Get/${id}`);
        }
    },
    Update(data) {
        if (data.id == 0) {
            return ApiService.post(`Currency/Create`, data);
        } else {
            return ApiService.post(`Currency/Update`, data);
        }
    },
    GetAsSelectList() {
        return ApiService.get('Currency/GetAsSelectList');
    },
    Delete(id) {
        return ApiService.post(`Currency/Delete/${id}`);
    },
};
export default CurrencyService;
