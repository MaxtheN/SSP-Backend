import ApiService from '../api.service';

const IndicatorService = {
   GetList(data, tab) {
      if (tab == 0) {
         return ApiService.post(`/kpi/Indicator/GetList`, data);
      } else {
         return ApiService.post(`/kpi/IndicatorDistrict/GetList`, data);
      }
   },
   Get(id, tab) {
      if (tab == 0) {
         if (id == 0 || id == null || id == undefined) {
            return ApiService.get(`/kpi/Indicator/Get`);
         } else {
            return ApiService.get(`/kpi/Indicator/Get/${id}`);
         }
      }
      if (tab == 1) {
         if (id == 0 || id == null || id == undefined) {
            return ApiService.get(`/kpi/IndicatorDistrict/Get`);
         } else {
            return ApiService.get(`/kpi/IndicatorDistrict/Get/${id}`);
         }
      }
   },

   GetAsSelectList(tab) {
      if (true) {
         return ApiService.get(`/kpi/Indicator/GetAsSelectList`);
      } else {
         return ApiService.get(`/kpi/IndicatorDistrict/GetAsSelectList`);
      }
   },

   Update(data, tab) {
      if (tab == 0) {
         if (data.id == 0) {
            return ApiService.post(`/kpi/Indicator/Create`, data);
         } else {
            return ApiService.post(`/kpi/Indicator/Update`, data);
         }
      } else {
         if (data.id == 0) {
            return ApiService.post(`/kpi/IndicatorDistrict/Create`, data);
         } else {
            return ApiService.post(`/kpi/IndicatorDistrict/Update`, data);
         }
      }
   },
   Delete(id, tab) {
      console.log(tab);

      if (tab == 0) {
         return ApiService.post(`/kpi/Indicator/Delete/${id}`);
      } else {
         return ApiService.post(`/kpi/IndicatorDistrict/Delete/${id}`);
      }
   }
};

export default IndicatorService;
