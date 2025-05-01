<template>
   <b-row v-if="$route.meta.breadcrumb || $route.meta.pageTitle" class="content-header">
      <!-- Content Left -->
      <b-col
         class="content-header-left"
         cols="12"
         md="9"
         :class="{ 'mb-1': isMobileDevice(), 'mb-2': !isMobileDevice() }"
      >
         <b-row class="breadcrumbs-top">
            <b-col cols="12">
               <h2 class="content-header-title float-left pr-1 mb-0 pb-0" :class="{ h3: isMobileDevice() }">
                  {{ $t($route.meta.pageTitle) }}
               </h2>
            </b-col>
            <b-col cols="12" :class="{ 'mt-2': !isMobileDevice() }">
               <div class="breadcrumb-wrapper">
                  <b-breadcrumb>
                     <b-breadcrumb-item to="/">
                        <feather-icon icon="HomeIcon" size="16" class="align-text-top" />
                     </b-breadcrumb-item>
                     <b-breadcrumb-item
                        v-for="item in $route.meta.breadcrumb"
                        :key="item.text"
                        :active="item.active"
                        :to="{ name: item.to }"
                     >
                        {{ $t(item.text) }}
                     </b-breadcrumb-item>
                  </b-breadcrumb>
               </div>
            </b-col>
         </b-row>
      </b-col>

      <!-- Content Right -->
      <b-col class="content-header-right text-md-right d-md-block d-none mb-1" md="3" cols="12">
         <b-button variant="outline-danger" size="sm" @click="BackButton">
            <feather-icon icon="ArrowLeftIcon"> </feather-icon> {{ $t('back') }}
         </b-button>
      </b-col>
   </b-row>
</template>

<script>
import { BBreadcrumb, BBreadcrumbItem, BRow, BCol, BDropdown, BDropdownItem, BButton } from 'bootstrap-vue';
import Ripple from 'vue-ripple-directive';

export default {
   directives: {
      Ripple
   },
   components: {
      BBreadcrumb,
      BBreadcrumbItem,
      BRow,
      BCol,
      BDropdown,
      BDropdownItem,
      BButton
   },
   methods: {
      BackButton() {
         this.$router.back();
      }
   }
};
</script>
