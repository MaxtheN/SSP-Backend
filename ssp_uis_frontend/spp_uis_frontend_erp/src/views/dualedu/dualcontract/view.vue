<template>
   <b-overlay :show="show">
      <b-row class="justify-content-center">
         <b-col cols="10">
            <b-card>
               <WIframe
                  v-if="Data && Data.id2"
                  :src="IframeSrcFromBase64"
                  style="height: 100vh"
                  :show="Data && Data.id2"
               />
            </b-card>
         </b-col>
      </b-row>
   </b-overlay>
</template>
<script>
import { BOverlay, BCard, BCardText, BRow, BCol, BTable, BButton, BLink, BSpinner, BModal } from 'bootstrap-vue';

import axios from 'axios';
import DualContractService from '@/services/dualedu/dualcontract.service';
const WIframe = () => import('@/components/WIframe.vue');

export default {
   components: {
      BOverlay,
      BCard,
      BCardText,
      BRow,
      BCol,
      BButton,
      BTable,
      BModal,
      BLink,
      BSpinner,
      WIframe
   },
   props: {
      page: {
         type: String,
         default: 'index'
      }
   },
   data() {
      return {
         show: false,
         Data: {},
         IframeSrcFromBase64: ''
      };
   },
   async created() {
      this.Refresh();
   },
   computed: {
      IframeSrc() {
         return (
            axios.defaults.baseURL +
            `DualContract/DualContract/DownloadPdf?id2=${this.Data?.id2}&lang=${this.getPdfLang()}`
         );
      }
   },
   methods: {
      Refresh() {
         this.show = true;

         DualContractService.Get(this.$route.params.id)
            .then((res) => {
               this.Data = res.data;
               DualContractService.DownloadPdf(this.Data?.id2).then((res2) => {
                  this.IframeSrcFromBase64 = URL.createObjectURL(res2.data);
               });
            })
            .catch((error) => {
               this.showApiError(error);
            })
            .finally(() => {
               this.show = false;
            });
      }
   }
};
</script>
