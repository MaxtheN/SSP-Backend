import ApiService from "../api.service";
const OrderToSendBusinessTripService = {
    GetList(data) {
        return ApiService.post("hrm/OrderToSendBusinessTrip/GetList", data);
    },
    GetListForSigner(data) {
        return ApiService.post("hrm/OrderToSendBusinessTrip/GetListForSigner", data);
    },
    Get(id) {
        if (id == 0 || id === null || id === undefined) {
            return ApiService.get("hrm/OrderToSendBusinessTrip/Get");
        } else {
            return ApiService.get(`hrm/OrderToSendBusinessTrip/Get/${id}`);
        }
    },
    Update(data) {
        if (data.id == 0) {
            return ApiService.post(`hrm/OrderToSendBusinessTrip/Create`, data);
        } else {
            return ApiService.post(`hrm/OrderToSendBusinessTrip/Update`, data);
        }
    },
    Delete(id) {
        return ApiService.post(`hrm/OrderToSendBusinessTrip/Delete/${id}`);
    },
    Accept(data) {
        return ApiService.post(`hrm/OrderToSendBusinessTrip/Accept`, data);
    },
    Sign(data) {
        return ApiService.post(`hrm/OrderToSendBusinessTrip/Sign`, data);
    },
    Cancel(data) {
        return ApiService.post(`hrm/OrderToSendBusinessTrip/Cancel`, data);
    },
    GetAsSelectList(data) {
        return ApiService.get(`hrm/OrderToSendBusinessTrip/GetAsSelectList`, { params: data });
    },
};
export default OrderToSendBusinessTripService;
