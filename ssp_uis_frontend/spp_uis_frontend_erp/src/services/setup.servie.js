import axios from 'axios';
import ApiService from '@/services/api.service';

axios.defaults.headers.common.langCode = localStorage.getItem('locale') || 'uz_latn';

// axios.defaults.baseURL = 'https://erptest.chamber.uz/api/';
axios.defaults.baseURL = 'https://sspuis.apptest.uz/api/';
// axios.defaults.baseURL = 'https://erp-api.chamber.uz/';

if (window.location.href.indexOf('https://erp.chamber.uz') > -1) {
   axios.defaults.baseURL = 'http://erp-api.chamber.uz/';
}
if (window.location.href.indexOf('https://erp.chamber.uz') > -1) {
   axios.defaults.baseURL = 'https://erp-api.chamber.uz/';
}
// axios.defaults.baseURL = 'https://erp-api.chamber.uz/';

const token = localStorage.getItem('auth_token');
if (token) {
   axios.defaults.headers.common['Authorization'] = `Bearer ${token}`;
}
const requestInterceptor = (request) => {
   request.withCredentials = true;
   return request;
};
axios.interceptors.request.use((request) => requestInterceptor(request));

ApiService.mount401Interceptor();
