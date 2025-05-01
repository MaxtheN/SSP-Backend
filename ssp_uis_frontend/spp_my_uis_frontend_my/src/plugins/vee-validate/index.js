/* eslint-disable camelcase */
import Vue from "vue";
import { ValidationObserver, extend, configure } from "vee-validate";
import * as rules from "vee-validate/dist/rules";

import i18n from "../../lang/index";

Object.keys(rules).forEach((rule) => {
	extend(rule, rules[rule]);
});

configure({
	defaultMessage: (field, values) => {
		values._field_ = i18n.t(`${field}`);

		return i18n.t(`validation.${values._rule_}`, values);
	},
});

const validatorPhone = {
	message(field) {
		return `Поле ${field} недействительно`;
	},
	validate(value) {
		const pRegExp = (value + "").replace(/\D/g, "");
		const validPhone = pRegExp.length == 12;
		return validPhone;
	},
};

extend("validatorPhone", validatorPhone);

// Register it globally
Vue.component("validation-observer", ValidationObserver);
