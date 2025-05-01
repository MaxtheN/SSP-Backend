import Vue from 'vue'
import FeatherIcon from '@core/components/feather-icon/FeatherIcon.vue'
import 'vue2-datepicker/index.css';

import FormSelect from '@/components/forms/form-select.vue';
import FormInput from '@/components/forms/form-input.vue';
import FormTextarea from '@/components/forms/form-textarea.vue';
import FormInputHrm from '@/components/forms/form-input-hrm.vue';
import FormCurrencyInput from '@/components/forms/form-currency-input.vue';
import FormPicker from '@/components/forms/form-picker.vue';
import DatePicker from 'vue2-datepicker';
import vSelect from 'vue-select';

Vue.component('date-picker', DatePicker);
Vue.component('form-input', FormInput);
Vue.component('form-textarea', FormTextarea);
Vue.component('form-input-hrm', FormInputHrm);
Vue.component('form-currency-input', FormCurrencyInput);
Vue.component('form-picker', FormPicker);
Vue.component('form-select', FormSelect);
Vue.component('v-select', vSelect);

Vue.component(FeatherIcon.name, FeatherIcon)
