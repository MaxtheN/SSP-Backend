import Vue from 'vue';
import VueSweetalert2 from 'vue-sweetalert2';
import '@core/scss/vue/libs/vue-sweetalert.scss';
import i18n from './i18n';

Vue.use(VueSweetalert2, {
   showCancelButton: true,
   reverseButtons: true,
   confirmButtonText: i18n.t('accept'),
   cancelButtonText: i18n.t('back'),
   confirmButtonColor: '#41b882',
   cancelButtonColor: '#ff7674'
});
