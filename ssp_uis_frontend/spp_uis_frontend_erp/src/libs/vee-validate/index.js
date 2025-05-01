/* eslint-disable camelcase */
import Vue from 'vue';
import { ValidationObserver, extend, localize } from 'vee-validate';
import * as rules from 'vee-validate/dist/rules';
import ru from 'vee-validate/dist/locale/ru.json';
import uz_cyrl from './locales/uz_cyrl.json';
import uz_latn from './locales/uz_latn.json';

localize({
   ru,
   uz_cyrl,
   uz_latn
});

Object.keys(rules).forEach((rule) => {
   extend(rule, rules[rule]);
});


const validatorPhone = {
   message(field) {
      return `Поле ${field} недействительно`;
   },
   validate(value) {
      const pRegExp = (value + '').replace(/\D/g, '');
      const validPhone = pRegExp.length == 12;
      return validPhone;
   }
};

extend('validatorPhone', validatorPhone);

// Register it globally
Vue.component('validation-observer', ValidationObserver);
