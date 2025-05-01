import ApiService from '../api.service'

const OblastService = {
    GetAll(lang){
        return ApiService.get(`/Oblast/GetAll?lang=${lang}`)
    }, 
    GetList(Search,SortColumn, OrderType, PageNumber, PageLimit){
        return ApiService.get(`/Oblast/GetList?Search=${Search}&SortColumn=${SortColumn}&OrderType=${OrderType}&PageNumber=${PageNumber}&PageLimit=${PageLimit}`)
    }
}
export default OblastService