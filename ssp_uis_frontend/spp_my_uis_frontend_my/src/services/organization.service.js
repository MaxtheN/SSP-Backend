import ApiService from './api.service';
const OrganizationService = {
    GetList() {
        return ApiService.post(`/Organization/GetList`);
    },
    OrganizationAsSelectListByGroup() {
        return ApiService.get(`/Organization/OrganizationAsSelectListByGroup?groupId=1&groupId=3`);
    },
    DownloadFile(id) {
        return ApiService.get(`/Organization/DownloadFile/${id}`);
    }
};
export default OrganizationService;
