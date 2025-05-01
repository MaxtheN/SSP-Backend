import ApiService from '../api.service';

const ItemOfExpenseService = {
   GetList(data) {
      return ApiService.post('hrm/ItemOfExpense/GetList', data);
   },
   Get(id) {
      if (id == 0 || id === null || id === undefined) {
         return ApiService.get('hrm/ItemOfExpense/Get');
      } else {
         return ApiService.get(`hrm/ItemOfExpense/Get/${id}`);
      }
   },
   Update(data) {
      if (data.id == 0) {
         return ApiService.post(`hrm/ItemOfExpense/Create`, data);
      } else {
         return ApiService.post(`hrm/ItemOfExpense/Update`, data);
      }
   },
   Delete(id) {
      return ApiService.post(`hrm/ItemOfExpense/Delete/${id}`);
   },
   GetAsSelectList() {
      return ApiService.get(`hrm/ItemOfExpense/GetAsSelectList`);
   },
   Create(data) {
      if (data.id == 0) {
         return ApiService.post(`hrm/ItemOfExpense/CreateWithUser`, data);
      } else {
         return ApiService.post(`hrm/ItemOfExpense/UpdateWithUser`, data);
      }
   }
};
export default ItemOfExpenseService;
