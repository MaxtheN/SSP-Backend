import ApiService from './api.service'

const LandingPageService = {
    GetPageData(){
        return ApiService.get('/LandingPage/GetPageData')
    }
}
export default LandingPageService