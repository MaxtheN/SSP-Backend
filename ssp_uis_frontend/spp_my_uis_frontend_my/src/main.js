import Vue from 'vue';
import App from './App.vue';
import store from './store';
import axios from 'axios';
import VueMask from 'v-mask';
import 'vue2-datepicker/index.css';
import router from './router';
import { VueMaskDirective } from 'v-mask';

import 'vue-select/dist/vue-select.css';
import i18n from './lang/index.js';
import VueToast from 'vue-toast-notification';
import ApiService from './services/api.service';
import VueNumber from 'vue-number-animation';
import '@/plugins/vee-validate';

Vue.use(VueNumber);

import { BootstrapVue, BootstrapVueIcons } from 'bootstrap-vue';

import './assets/styles/core.scss';

Vue.use(BootstrapVue);
Vue.use(BootstrapVueIcons);

import { ModalPlugin } from 'bootstrap-vue';
import 'vue-toast-notification/dist/theme-sugar.css';
import globalMixin from './mixins/global';
Vue.use(VueToast, {
    position: 'top'
});
Vue.use(VueMask);
Vue.directive('mask', VueMaskDirective);
Vue.use(ModalPlugin);
Vue.mixin(globalMixin);
axios.defaults.baseURL = 'https://sspmyuis.apptest.uz/api/';
// axios.defaults.baseURL = 'https://mytest.chamber.uz/api';

// axios.defaults.baseURL = 'https://my-api.chamber.uz/';

https: if (window.location.href.indexOf('https://my.chamber.uz') > -1) {
    axios.defaults.baseURL = 'https://my-api.chamber.uz/';
}
if (window.location.href.indexOf('http://my.chamber.uz') > -1) {
    axios.defaults.baseURL = 'http://my-api.chamber.uz/';
}

var lang = localStorage.getItem('locale');
if (lang == 'ru') {
    axios.defaults.params = {
        __lang: 'ru',
        langId: 1
    };
} else if (lang == 'uz_latn') {
    axios.defaults.params = {
        __lang: 'uz-latn',
        langId: 3
    };
} else {
    axios.defaults.params = {
        __lang: 'uz-cyrl',
        langId: 2
    };
}

const requestInterceptor = (request) => {
    request.withCredentials = true;
    return request;
};
localStorage.getItem('locale') === null ? localStorage.setItem('locale', 'uz_latn') : '';
axios.interceptors.request.use((request) => requestInterceptor(request));

Vue.config.productionTip = false;
ApiService.mount401Interceptor();

new Vue({
    router,
    store,
    i18n,
    render: (h) => h(App)
}).$mount('#app');
