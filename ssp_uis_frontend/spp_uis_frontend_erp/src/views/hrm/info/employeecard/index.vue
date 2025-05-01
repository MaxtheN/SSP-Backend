<template>
   <div>
      <!-- search -->
      <b-card>
         <b-card-text>
            <b-row class="align-items-baseline">
               <b-col sm="4">
                  <form-input-hrm
                     v-model="pinfl"
                     placeholder="XXXX XXXX XXXX XX"
                     mask="#### #### #### ##"
                     :label="$t('pinfl')"
                  />
               </b-col>
               <b-col class="col-auto">
                  <b-button variant="primary" @click="GetEmployeeCard" :disabled="searchLoading">
                     <b-spinner v-if="searchLoading" small /> {{ $t('search') }}
                  </b-button>
               </b-col>
            </b-row>
         </b-card-text>
      </b-card>

      <!-- info -->
      <b-overlay :show="searchLoading">
         <b-card>
            <b-tabs v-model="tab" class="nav-tabs nav-justified">
               <b-tab :title="this.$t('Showing')" active> </b-tab>
               <b-tab :title="this.$t('orders')"></b-tab>
            </b-tabs>
         </b-card>

         <template v-if="EmployeeInfo">
            <EmployeeInfo v-if="tab == 0" :employee-info="EmployeeInfo" />
            <EmployeeOrder v-if="tab == 1" :employee-info="EmployeeInfo" />
         </template>
      </b-overlay>
   </div>
</template>

<script>
import ReportService from '@/services/report/report.service';
import { BCard, BCardText, BRow, BCol, BButton, BSpinner, BOverlay, BAvatar, BTab, BTabs } from 'bootstrap-vue';

const EmployeeInfo = () => import('./components/EmployeeInfo.vue');
const EmployeeOrder = () => import('./components/EmployeeOrder.vue');

export default {
   components: {
      BCard,
      BTab,
      BCardText,
      BRow,
      BCol,
      BButton,
      BSpinner,
      BOverlay,
      BAvatar,
      BTabs,
      EmployeeInfo,
      EmployeeOrder
   },
   data() {
      return {
         showTabs: false,
         pictureId: null,
         pinfl: this.$route.query?.pinfl || '',
         searchLoading: false,
         EmployeeInfo: null,
         lang: localStorage.getItem('locale') || 'ru',
         userImg: '',
         tab: 0
      };
   },
   mounted() {
      if (this.$route.query?.pinfl && this.$route.query?.pinfl.length == 14) {
         this.GetEmployeeCard();
      }
   },

   methods: {
      GetEmployeeCard() {
         this.searchLoading = true;
         this.EmployeeInfo = null;
         ReportService.GetEmployeeCard({
            pinfl: this.pinfl?.replace(/\s/g, '')
         })
            .then((res) => {
               this.EmployeeInfo = res.data;
               this.showTabs = true;
            })
            .catch((e) => {
               this.showApiError(e);
               this.showTabs = false;
            })
            .finally(() => {
               this.searchLoading = false;
            });
      }
   }
};
</script>
