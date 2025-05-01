import Vue from 'vue';
import VueI18n from 'vue-i18n';
import ru from 'vee-validate/dist/locale/ru.json';
import uz_cyrl from '../plugins/vee-validate/locales/uz_cyrl.json';
import uz_latn from '../plugins/vee-validate/locales/uz_latn.json';
Vue.use(VueI18n);

function getVeeValidateLocale(locale) {
    const veeValidateLocales = { uz_cyrl: uz_cyrl, uz_latn: uz_latn, ru: ru, default: uz_latn };

    return veeValidateLocales[locale] || veeValidateLocales['default'];
}

const SUPPORT_LOCALES = [localStorage.getItem('locale') || 'uz_latn'];

function loadLocaleMessages() {
    const messages = {};

    // Vite yordamida barcha JSON fayllarni dinamik yuklash
    const locales = import.meta.glob('./translate/*.json');

    // Fayllarni yuklab olish va qayta ishlash
    Object.keys(locales).forEach((key) => {
        const matched = key.match(/([A-Za-z0-9-_]+)\./i);

        if (matched && matched.length > 1) {
            const locale = matched[1];

            if (SUPPORT_LOCALES.includes(locale)) {
                // Faylni yuklab olish va Promise qaytarish
                locales[key]().then((module) => {
                    // Faylni messages obyektiga saqlash
                    messages[locale] = { ...module };

                    // Vee-validate uchun validatsiya xabarlarini yuklash
                    messages[locale].validation = getVeeValidateLocale(locale).messages;
                });
            }
        }
    });

    return messages;
}

const i18n = new VueI18n({
    locale: SUPPORT_LOCALES[0],
    messages: loadLocaleMessages(),
    silentTranslationWarn: true
});

export default i18n;
