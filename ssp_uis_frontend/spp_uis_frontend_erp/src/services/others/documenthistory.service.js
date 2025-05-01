import ApiService from "../api.service";

const DocumentHistoryService = {
  GetLastMessage(data) {
    return ApiService.get(
      `DocumentHistory/GetLastMessage?TableId=${data.TableId}&DocumentId=${data.DocumentId}`
    );
  },
};
export default DocumentHistoryService;
