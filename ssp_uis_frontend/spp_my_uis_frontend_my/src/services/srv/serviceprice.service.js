import ApiService from "../api.service";
const ServicePriceService = {
    GetList(data) {
        return ApiService.post("/srv/ServicePrice/GetList", data);
    },
    GetAsSelectList(data) {
        return ApiService.post(`/srv/ServicePrice/GetAsSelectList`, data);
    },
    Get(id) {
        if (id == 0 || id === null || id === undefined) {
            return ApiService.get("/srv/ServicePrice/Get");
        } else {
            return ApiService.get(`/srv/ServicePrice/Get/${id}`);
        }
    },
    GroupingByServicePrice(data) {
        return ApiService.post(`/srv/ServicePrice/GroupingByServicePrice`, data);
    },
};
export default ServicePriceService;
