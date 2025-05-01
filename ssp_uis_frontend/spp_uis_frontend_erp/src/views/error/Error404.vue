<template>
   <!-- Error page-->
   <div class="misc-wrapper">
      <b-link class="brand-logo">
         <img width="200px" src="@/assets/images/SSP_UZB.png" alt="" />
      </b-link>

      <div class="misc-inner p-2 p-sm-3">
         <div class="w-100 text-center">
            <h2 class="mb-1">Page Not Found 🕵🏻‍♀️</h2>
            <p class="mb-2">Oops! 😖 The requested URL was not found on this server.</p>

            <b-button variant="primary" class="mb-2 btn-sm-block" :to="{ path: '/' }"> Back to home </b-button>

            <!-- image -->
            <b-img fluid :src="imgUrl" alt="Error page" />
         </div>
      </div>
   </div>
   <!-- / Error page-->
</template>

<script>
import { BLink, BButton, BImg } from 'bootstrap-vue';
import store from '@/store/index';
import downImgLight from '@/assets/images/pages/error.svg';
import downImgDark from '@/assets/images/pages/error-dark.svg';

export default {
   components: {
      BLink,
      BButton,
      BImg
   },
   computed: {
      imgUrl() {
         if (store.state.appConfig.layout.skin === 'dark') {
            return downImgDark;
         }
         return downImgLight;
      }
   }
};
</script>

<style lang="scss">
@import '@core/scss/vue/pages/page-misc.scss';
</style>
