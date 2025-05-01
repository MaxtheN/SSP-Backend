<template>
    <Swiper
        :autoplay="{ delay: 2000, disableOnInteraction: false }"
        :pagination="{ clickable: true }"
        :modules="modules"
        :loop-additional-slides="true"
        :slides-per-view="1"
        :loop="true"
        :speed="800"
    >
        <template #wrapper-start>
            <swiper-slide
                ><div class="my-sidebar-body-right">
                    <b-img class="login_img-right" src="/images/ssp_images/logobgimg-small.png" alt="login bg" />
                    <div class="info_title">
                        <b-navbar-brand href="#" class="d-md-block d-none">
                            <b-img src="/img/Logo_SSP.svg" width="30" :alt="$t('publiceducation')" />
                            <span class="pl-2 text-white">{{ $t('publiceducation') }}</span>
                        </b-navbar-brand>
                        <p class="subtitle text-white" v-html="$t('welcomeMessage')"></p>
                    </div></div></swiper-slide
            ><swiper-slide
                ><div class="my-sidebar-body-right">
                    <div class="text-center h-100 w-100"><b-img class="login_img-right" src="/images/ssp_images/imgslayd2.png" alt="login bg" /></div>
                    <div class="info_title">
                        <b-navbar-brand href="#" class="d-md-block d-none">
                            <b-img src="/images/logo.png" width="45" :alt="$t('ssp')" />
                            <span class="pl-2 text-white">{{ $t('ssp') }}</span>
                        </b-navbar-brand>
                        <p class="subtitle text-white" v-html="$t('welcomeMessage2')"></p>
                    </div></div
            ></swiper-slide>
        </template>
    </Swiper>
</template>
<script>
// import Swiper core and required modules
import { Pagination, Scrollbar, Autoplay } from 'swiper/modules';

// Import Swiper Vue.js components
import { Swiper, SwiperSlide } from 'swiper/vue';

// Import Swiper styles
import 'swiper/css';
import 'swiper/css/pagination';
import 'swiper/css/scrollbar';
import 'swiper/css/autoplay';

// Import Swiper styles
export default {
    components: {
        Swiper,
        SwiperSlide
    },
    setup() {
        return {
            modules: [Pagination, Scrollbar, Autoplay]
        };
    }
};
</script>
<style scoped lang="scss">
@import url('../../assets/styles/landing_style.scss');
.swiper {
    height: 100%;
}
</style>
