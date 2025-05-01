import ApiService from '@/services/api.service';

// eslint-disable-next-line import/prefer-default-export
export const changeDocument = {
   DownloadTemplate(url, fileName, format = '.docx') {
      ApiService.print(url).then((res) => {
         this.forceFileDownload(res, fileName, format);
      });
   },
   UploadFile(url, event) {
      const formData = new FormData();
      formData.append('files', event.target.files[0]);
      return ApiService.post(url, formData);
   },
   Update(url, data) {
      return ApiService.post(url, data);
   },
   forceFileDownload(response, name, format) {
      var blob = new Blob([response.data]);
      const url = window.URL.createObjectURL(blob);
      const link = document.createElement('a');
      link.href = url;
      link.setAttribute('download', name + format); //or any other extension
      document.body.appendChild(link);
      link.click();
   }
};
