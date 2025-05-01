import ApiService from '../api.service';
const SendSmsService = {
    Send(data) {
        return ApiService.post('SendSms/Send', data);
    }
};
export default SendSmsService;
