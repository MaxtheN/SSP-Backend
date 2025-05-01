import ApiService from '../api.service';

const CandidatesConfirmationService = {
   GetList(data) {
      return ApiService.post('hrm/CandidatesConfirmation/GetList', data);
   },
   Get(id) {
      if (id == 0 || id === null || id === undefined) {
         return ApiService.get('/hrm/CandidatesConfirmation/Get');
      } else {
         return ApiService.get(`/hrm/CandidatesConfirmation/Get/${id}`);
      }
   },
   Update(data) {
      if (data.id == 0) {
         return ApiService.post(`/hrm/CandidatesConfirmation/Create`, data);
      } else {
         return ApiService.post(`/hrm/CandidatesConfirmation/Update`, data);
      }
   },
   Delete(id) {
      return ApiService.post(`/hrm/CandidatesConfirmation/Delete/${id}`);
   },
   Accept(data) {
      return ApiService.post('/hrm/CandidatesConfirmation/Accept', data);
   },
   Cancel(data) {
      return ApiService.post('/hrm/CandidatesConfirmation/Cancel', data);
   },
   Send(data) {
      return ApiService.post('/hrm/CandidatesConfirmation/Send', data);
   },
   UploadFile(data) {
      return ApiService.post(`/hrm/CandidatesConfirmation/UploadFile`, data);
   },
   DownloadFile(id) {
      return ApiService.get(`/hrm/CandidatesConfirmation/DownloadFile/${id}`);
   },
   DeleteFile(id) {
      return ApiService.post(`/hrm/CandidatesConfirmation/DeleteFile/${id}`);
   }
};

export default CandidatesConfirmationService;
