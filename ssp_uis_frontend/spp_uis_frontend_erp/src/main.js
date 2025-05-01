import Vue from 'vue';
import i18n from '@/libs/i18n';
import router from '@/router';
import store from './store';
import App from './App.vue';

// Global Components
import '@/global-components';
// axios install
import '@/services/setup.servie';

// 3rd party plugins
import '@/libs/acl';
import '@/libs/portal-vue';
import '@/libs/toastification';
import '@/libs/sweet-alerts';
import '@/libs/vue-select';
import '@/libs/vee-validate/index';
import VueMask from 'v-mask';
import GlobalMixin from '@/@core/mixins/global';
import VueCurrencyFilter from 'vue-currency-filter';
import Global from '@/mixins/global';
import isMobile from '@/mixins/isMobile';

Vue.mixin(GlobalMixin);
Vue.mixin(Global);
Vue.mixin(isMobile);

Vue.use(VueMask);
Vue.use(VueCurrencyFilter, {
   symbol: '',
   thousandsSeparator: '.',
   fractionCount: 2,
   fractionSeparator: ',',
   symbolPosition: 'front',
   symbolSpacing: true,
   avoidEmptyDecimals: undefined
});

// * Shall remove it if not using font-icons of feather-icons - For form-wizard
import '@core/assets/fonts/feather/iconfont.css';

// import core styles
import "@core/scss/core.scss";

// import assets styles
import "@/assets/scss/style.scss";

Vue.config.productionTip = false;

new Vue({
   router,
   store,
   i18n,
   render: (h) => h(App)
}).$mount('#app');
