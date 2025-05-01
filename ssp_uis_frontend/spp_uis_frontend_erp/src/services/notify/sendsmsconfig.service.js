import ApiService from '../api.service';
const SendSmsConfigService = {
    GetList(data) {
        return ApiService.post('Notify/SendSmsConfig/GetList', data);
    },
    Get(id) {
        if (id == 0 || id === null || id === undefined) {
            return ApiService.get('Notify/SendSmsConfig/Get');
        } else {
            return ApiService.get(`Notify/SendSmsConfig/Get/${id}`);
        }
    },
    Update(data) {
        if (data.id == 0) {
            return ApiService.post(`Notify/SendSmsConfig/Create`, data);
        } else {
            return ApiService.post(`Notify/SendSmsConfig/Update`, data);
        }
    },
    GetAsSelectList() {
        return ApiService.get('Notify/SendSmsConfig/GetAsSelectList');
    },
    Delete(id) {
        return ApiService.post(`Notify/SendSmsConfig/Delete/${id}`);
    },
};
export default SendSmsConfigService;
