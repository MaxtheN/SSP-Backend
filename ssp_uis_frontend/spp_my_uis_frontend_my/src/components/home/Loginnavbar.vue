<template>
    <div class="container-fluid pt-2">
        <b-navbar toggleable="lg" class="border border-border-color py-2 px-4 rounded-sm w-100 text-black cutom-navbar bg-white position-sticky" style="height: auto">
            <b-navbar-brand href="#" class="d-flex align-items-center">
                <b-img src="/images/logo.png" class="d-block d-lg-none" width="35" alt="logo" />
                <!-- <span class="cabinet-sidebar__title pl-2 d-none d-lg-block">
                    {{ $t('ssp') }}
                </span> -->

                <div class="d-none d-lg-block">
                    <b-img v-if="lang == 'uz_latn'" src="/images/logo/dark/SSP_UZB.png" alt="logo" width="210" />
                    <b-img v-else-if="lang == 'uz_cyrl'" src="/images/logo/dark/SSP_CRL.png" alt="logo" width="210" />
                    <b-img v-else-if="lang == 'ru'" src="/images/logo/dark/SSP_RU.png" alt="logo" width="273" />
                    <b-img v-else src="/images/logo/dark/SSP_UZB.png" alt="logo" width="210" />
                </div>
                <div class="border-left px-3 py-1 ml-4 d-none">
                    <b-img src="/img/Logo_SSP.svg" width="65" :alt="$t('publiceducation')" />
                    <span class="pl-2">{{ $t('publiceducation') }}</span>
                </div>
            </b-navbar-brand>

            <div class="ml-auto d-flex">
                <div class="mr-4 d-none d-lg-block">
                    <OrganizationBtn v-if="isToken" />
                </div>
                <b-button variant="light" @click="$router.push({ path: '/myinfo' })" v-if="isToken" class="d-flex border align-items-center rounded-lg mr-3 justify-content-center">
                    <b-img src="/images/ssp_images/Profile.svg" height="24px" width="24px" alt="shaxsiy kabinet" />
                </b-button>
            </div>
            <div class="mr-3 d-none d-lg-block" style="font-size: 18px">
                <span class="">
                    <b-icon-telephone style="color: #188ef6"></b-icon-telephone
                    ><a class="text-primary ml-1" style="text-decoration: none; font-weight: 500" href="tel:+998 78 150 60 60"> (78) 150 60 06</a>
                </span>
                <span class=""> | <a class="text-primary ml-1" style="text-decoration: none; font-weight: 500" href="tel:1094"> 1094</a> </span>
            </div>
            <LanguageSwitcher />
        </b-navbar>
    </div>
</template>

<script>
import OrganizationBtn from '../../layout/cabinet/header/OrganizationBtn.vue';
import isMobileMixin from '@/mixins/isMobileMixin';
import LanguageSwitcher from '@/layout/cabinet/header/LanguageSwitcher.vue';

export default {
    components: {
        LanguageSwitcher,
        OrganizationBtn
    },
    data() {
        return {
            isToken: false,
            lang: localStorage.getItem('locale')
        };
    },
    created() {
        if (localStorage.getItem('user_info')) {
            this.isToken = true;
        } else {
            this.isToken = false;
        }
    },
    mixins: [isMobileMixin]
};
</script>

<style lang="scss" scoped>
.cabinet-sidebar__title {
    font-size: 1rem;
    font-weight: 500;
}
.cutom-navbar {
    z-index: 1025;
}
</style>
