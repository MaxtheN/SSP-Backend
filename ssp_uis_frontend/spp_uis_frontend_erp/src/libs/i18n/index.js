import Vue from 'vue'
import VueI18n from 'vue-i18n'
import uzLatn from "./locales/uz_latn.json";
import uzCyrl from "./locales/uz_cyrl.json";
import ru from "./locales/ru.json";
Vue.use(VueI18n)
const locale = localStorage.getItem("locale") || "uz_latn";
const messages = {
  uz_latn: {
    ...uzLatn
  },
  uz_cyrl: {
    ...uzCyrl,
  },
  ru: {
    ...ru,
  },
};
const i18n = new VueI18n({
  locale,
  messages,
  fallbackLocale: 'uz_latn',
  silentTranslationWarn: false
});

export default i18n;