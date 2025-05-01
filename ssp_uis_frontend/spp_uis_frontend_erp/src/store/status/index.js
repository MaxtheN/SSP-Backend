import ManualService from '@/services/others/manual.service';

export default {
   namespaced: true,
   state: {
      statusList: [],
      statusListLoading: false
   },
   getters: {
      statusList: (state) => state.statusList,
      statusListLoading: (state) => state.statusListLoading,
      filteredStatusList:
         (state) => (ids = []) => {
            return state.statusList.filter((s) => ids.includes(s.value));
         }
   },
   mutations: {
      setStatusList(state, val) {
         state.statusList = val;
      },
      setStatusListLoading(state, val) {
         state.statusListLoading = val;
      }
   },
   actions: {
      fetchStatusList({ commit, state }) {
         if (state.statusList.length == 0) {
            commit('setStatusListLoading', true);
            ManualService.StatusSelectList()
               .then((res) => {
                  if (Array.isArray(res.data)) {
                     commit('setStatusList', res.data);
                  }
               })
               .finally(() => {
                  commit('setStatusListLoading', false);
               });
         }
      }
   }
};
