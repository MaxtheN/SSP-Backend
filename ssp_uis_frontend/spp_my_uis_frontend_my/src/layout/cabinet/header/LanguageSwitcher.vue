<template>
    <b-dropdown text="Lang" right variant="light">
        <template #button-content>
            <span>
                <b-icon-globe width="40" height="22" class="text-primary" />
                <span v-if="lang == 'uz_latn'" class="text-primary d-none d-lg-inline" style="font-size: 18px; font-weight: 400">O’zbekcha</span>
                <span v-else-if="lang == 'ru'" class="text-primary d-none d-lg-inline" style="font-size: 18px; font-weight: 400">Русский</span>
                <span v-else-if="lang == 'uz_cyrl'" class="text-primary d-none d-lg-inline" style="font-size: 18px; font-weight: 400">Ўзбекча</span>
                <span v-else class="text-primary d-none d-lg-inline" style="font-size: 18px; font-weight: 400">O’zbekcha</span>
            </span>
        </template>
        <b-dropdown-item :active="lang == 'uz_latn'" @click="ChangeLang('uz_latn')">O’zbekcha</b-dropdown-item>
        <b-dropdown-item :active="lang == 'ru'" @click="ChangeLang('ru')">Русский</b-dropdown-item>
        <b-dropdown-item :active="lang == 'uz_cyrl'" @click="ChangeLang('uz_cyrl')">Ўзбекча</b-dropdown-item>
    </b-dropdown>
</template>

<script>
import { localeChanged } from 'vee-validate';

export default {
    data() {
        return {
            lang: localStorage.getItem('locale')
        };
    },
    methods: {
        ChangeLang(lang) {
            let langId = 0;
            this.lang = lang;
            localStorage.setItem('locale', lang);
            if (lang == 'en') {
                langId = 4;
            }
            if (lang == 'uz_latn') {
                langId = 3;
            }
            if (lang == 'ru') {
                langId = 1;
            }
            if (lang == 'uz_cyrl') {
                langId = 2;
            }
            localStorage.setItem('langId', langId);
            window.location.reload();
        }
    },
    watch: {
        '$i18n.locale': {
            handler() {
                localeChanged();
            },
            immediate: true
        }
    }
};
</script>
