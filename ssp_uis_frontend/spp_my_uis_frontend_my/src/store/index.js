import Vue from 'vue';
import Vuex from 'vuex';
Vue.use(Vuex);
export default new Vuex.Store({
    state: {
        isMobileMenuOpen: false,
        user_info: localStorage.getItem('user_info') || false
    },
    mutations: {
        setUserInfo(state, payload) {
            state.user_info = payload;
        },
        TOGGLE_MOBILE_MENU(state) {
            state.isMobileMenuOpen = !state.isMobileMenuOpen;
        },
        CLOSE_MOBILE_MENU(state) {
            state.isMobileMenuOpen = false;
        },
        OPEN_MOBILE_MENU(state) {
            state.isMobileMenuOpen = true;
        }
    },
    actions: {
        toggleMobileMenu({ commit }) {
            commit('TOGGLE_MOBILE_MENU');
        },
        closeMobileMenu({ commit }) {
            commit('CLOSE_MOBILE_MENU');
        },
        openMobileMenu({ commit }) {
            commit('OPEN_MOBILE_MENU');
        }
    },
    getters: {
        isMobileMenuOpen: (state) => state.isMobileMenuOpen
    }
});
