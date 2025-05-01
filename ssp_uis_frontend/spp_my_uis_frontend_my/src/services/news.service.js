import { apiInstanceChamber } from './api.service';

const BASE_URL = '/news'

const NewsService = {
  GetList(params) {
    return apiInstanceChamber.get(`${BASE_URL}`, {
      params: {
        ...{
          "filters[publishDate][$lte]": new Date(),
          sort: "publishDate:DESC",
          locale: ['uz', 'ru', 'en'].includes(localStorage.getItem('locale')) ? localStorage.getItem('locale') : 'uz'
        },
        ...params
      }
    });
  },
  Get(id, config) {
    return apiInstanceChamber.get(`${BASE_URL}/${id}`, config);
  },
  updateNewsViewCount(id) {
    return apiInstanceChamber.post(`${BASE_URL}/updateNewsViewCount/${id}`);
  }
};
export default NewsService