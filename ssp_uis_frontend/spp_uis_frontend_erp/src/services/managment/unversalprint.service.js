import ApiService from '../api.service';

const UnversalPrintService = {
   SelectList() {
      return ApiService.get('/DocumentPrint/SelectList');
   },
   GenerateWord(id) {
      return ApiService.get(`/DocumentPrint/GenerateWord?tableId=${id}`, {
         responseType: 'blob'
      });
   },
   UploadFile(data) {
      return ApiService.post('/DocumentPrint/UploadFile', data);
   },
   SaveTemplate(data) {
      return ApiService.post('/DocumentPrint/SaveTemplate', data);
   }
};

export default UnversalPrintService;
