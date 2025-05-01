<template>
    <div class="container-fluid py-2 px-6">
        <b-row clas style="display: flex; align-items: flex-end" class="bg-white rounded-sm border border-border-color py-2">
            <b-col sm="12" md="4" class="mt-3">
                <div class="left-pos d-flex align-items-center flex-column flex-sm-row">
                    <div class="logo d-flex mr-2" style="cursor: pointer" @click="$router.push('/')">
                        <b-img src="/images/logo/dark/SSP_UZB.png" width="180" alt="" />
                    </div>

                    <div class="d-flex align-items-center cursor-pointer mr-6 mr-sm-0 ml-2 my-3 my-sm-0" @click="$router.push('/')" style="width: 140px; text-transform: uppercase">
                        <b-img class="mr-2" src="/images/ssp_images/logo_footer_no_text.svg" width="90" alt="" />
                        {{ $t('publiceducation') }}
                    </div>
                </div>
                <div class="d-flex my-1 mt-3 align-items-center justify-content-center justify-content-sm-start" style="cursor: pointer">
                    <a href="https://www.facebook.com/uzchamber" target="_blank" aria-label="Bizning Facebook sahifamizga tashrif buyuring">
                        <img style="margin-right: 15px; height: 45px" src="/img/facebook-icon.svg" alt="Facebook belgi" />
                    </a>
                    <a href="https://t.me/uzchamber" target="_blank" aria-label="Bizning Telegram kanalimizga o'ting">
                        <img style="margin-right: 15px; height: 45px" src="/img/telegram-icon.svg" alt="Telegram belgi" />
                    </a>
                    <a href="https://www.instagram.com/uzchamber/" target="_blank" aria-label="Bizning Instagram sahifamizga tashrif buyuring">
                        <img style="margin-right: 15px; height: 40px" src="/img/instagram-icon.svg" alt="Instagram belgi" />
                    </a>
                </div>
            </b-col>
            <b-col sm="12" md="4" class="mt-3 d-flex align-items-center text-center justify-content-center">
                <div>
                    <h2 style="font-family: Montserrat-extrabold; color: #706e6e" class="mb-3">
                        {{ $t('mobileApp') }}
                    </h2>
                    <span style="color: #706e6e"> {{ $t('AllInfoInPhone') }} </span>
                    <b-row class="mt-3">
                        <b-col>
                            <a
                                target="_blank"
                                href="https://play.google.com/store/apps/details?id=com.ssp.mychamber&pcampaignid=web_share"
                                aria-label="Google Play Marketdan My Chamber ilovasini yuklab olish"
                            >
                                <img style="cursor: pointer" width="100%" src="/images/ssp_images/play_market.svg" alt="Google Play Market logotipi" />
                            </a>
                        </b-col>
                        <b-col class="pl-0">
                            <a target="_blank" href="#" aria-label="App Store'dan My Chamber ilovasini yuklab olish">
                                <img style="cursor: pointer" width="100%" src="/images/ssp_images/appstore.svg" alt="Apple App Store logotipi" />
                            </a>
                        </b-col>
                    </b-row>
                </div>
            </b-col>
            <b-col sm="12" md="4" class="mt-3">
                <div style="text-align: right !important">
                    <span class="footer-text"> (78) 150 60 06 </span>
                </div>
                <div style="text-align: right !important">
                    <span class="footer-text">info@chamber.uz </span>
                </div>
                <div style="text-align: right !important">
                    <span class="footer-text"> {{ $t('addressus') }} </span>
                </div>
            </b-col>
        </b-row>

        <SspLinkBtn v-if="!$route.meta.hideCallCenter" />
    </div>
</template>

<script>
const SspLinkBtn = () => import('@/components/home/SspLinkBtn.vue');

export default {
    components: {
        SspLinkBtn
    }
};
</script>

<style lang="scss" scoped>
.container-footer {
    padding: 1rem;
    margin: 1rem;
    border-radius: 10px;
    border: 1px #d5d7e1 solid;
    background-color: #fff;
}
.orgname1 {
    color: #706e6e;
}
.footer-text {
    color: #706e6e;
    text-align: right;
    font-family: Montserrat;
    font-size: 14px;
    font-style: normal;
    font-weight: 500;
    line-height: normal;
}
</style>
