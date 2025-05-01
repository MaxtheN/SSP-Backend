import axios from 'axios';

export const API_URL_CHAMBER = 'https://api.chamber.uz';

export const apiInstanceChamber = axios.create({
    baseURL: API_URL_CHAMBER + '/api/',
    headers: {
        Accept: 'application/json',
        'Content-Type': 'application/json'
    }
});

const ApiService = {
    removeHeader() {
        axios.defaults.headers.common = {};
    },
    setHeader() {
        axios.defaults.headers.common['Authorization'] = `Bearer ${localStorage.getItem('auth_token')}`;
    },
    get(resource) {
        return axios.get(resource);
    },

    post(resource, data) {
        return axios.post(resource, data);
    },
    formData(resource, data) {
        return axios.post(resource, data, {
            headers: {
                'Content-Type': 'multipart/form-data'
            }
        });
    },
    token(resource, data) {
        return axios.post(resource, data, {
            headers: {
                'Content-Type': 'application/json;charset=utf-8'
            }
        });
    },
    put(resource, data) {
        return axios.put(resource, data);
    },

    delete(resource) {
        return axios.delete(resource);
    },
    print(resourse) {
        return axios.get(resourse, {
            responseType: 'blob'
        });
    },
    printtemp(resourse, data) {
        return axios.post(resourse, data, {
            responseType: 'blob'
        });
    },
    customRequest(data) {
        return axios(data);
    },

    mount401Interceptor() {
        this._401interceptor = axios.interceptors.response.use(
            (response) => {
                return response;
            },
            async (error) => {
                if (error.request.status == 401) {
                    localStorage.clear();
                    // window.location.href = "/"
                }
                if (error.request.status == 403) {
                    window.location.href = '/';
                }
                throw error;
            }
        );
    },

    unmount401Interceptor() {
        axios.interceptors.response.eject(this._401interceptor);
    }
};

export default ApiService;
