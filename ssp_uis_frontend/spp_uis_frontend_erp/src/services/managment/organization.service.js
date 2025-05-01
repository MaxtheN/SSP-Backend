import ApiService from '../api.service';
const OrganizationService = {
   GetList(data) {
      return ApiService.post(`/Organization/GetList`, data);
   },
   Get(id) {
      if (id == 0 || id === null || id === undefined) {
         return ApiService.get(`/Organization/Get`);
      } else {
         return ApiService.get(`/Organization/Get/${id}`);
      }
   },
   Update(data) {
      if (data.id == 0) {
         return ApiService.post(`/Organization/Create`, data);
      } else {
         return ApiService.post('/Organization/Update', data);
      }
   },
   Delete(id) {
      return ApiService.post(`/Organization/Delete/${id}`);
   },
   GetAsSelectList(parentId, authorizedOnly, inspectionOnly, organizationGroupId) {
      var params = {};
      if (parentId) {
         params['parentId'] = parentId;
      }
      if (authorizedOnly) {
         params['authorizedOnly'] = authorizedOnly;
      }
      if (inspectionOnly) {
         params['inspectionOnly'] = inspectionOnly;
      }
      if (organizationGroupId) {
         params['organizationGroupId'] = organizationGroupId;
      }
      let url = `/Organization/GetAsSelectList`;
      let joinSymbol = '?';
      if (Object.keys(params).length == 0) {
         return ApiService.get(url);
      } else {
         Object.keys(params).forEach((e) => {
            url = `${url}${joinSymbol}${e}=${params[e]}`;
            joinSymbol = '&';
         });
         return ApiService.get(url);

         // if (parentId === undefined) {
         //   url = `${url}?authorizedOnly=${authorizedOnly}&inspectionOnly=${inspectionOnly}`;
         //   return ApiService.get(url);
         // } else {
         //   url = `${url}?parentId=${parentId}&authorizedOnly=${authorizedOnly}&inspectionOnly=${inspectionOnly}`;
         //   return ApiService.get(url);
         // }

         // return ApiService.get(
         //   `/Organization/GetAsSelectList?parentId=${parentId}&authorizedOnly=${authorizedOnly}&inspectionOnly=${inspectionOnly}`
         // );
      }
   },
   GetByInn(inn) {
      return ApiService.get(`/Organization/GetByInn/${inn}`);
   },
   AsSelectListOrgSettlementAccount() {
      return ApiService.get(`/Organization/AsSelectListOrgSettlementAccount`);
   },
   SaveAsExecel(data) {
      return ApiService.printtemp(`/Organization/SaveAsExecel`, data);
   },
   GetAsSelectListOrgSettlementAccount(data) {
      return ApiService.get(`/Organization/AsSelectListOrgSettlementAccount`, data);
   },
   UpdateStructure(data) {
      let query = `?organizationId=${data.organizationId}`;
      if (data.structureId) {
         query += `&structureId=${data.structureId}`;
      }
      return ApiService.post(`/Organization/UpdateStructure` + query);
   },
   UploadFile(data) {
      return ApiService.formData(`/Organization/UploadFile`, data);
   },
   DownloadFile(fileId) {
      return ApiService.get(`/Organization/DownloadFile/${fileId}`);
   },
   DeleteFile(fileId) {
      return ApiService.post(`/Organization/DeleteFile/${fileId}`);
   }
};
export default OrganizationService;
