<template>
   <b-overlay :show="show">
      <b-container max-width="2000">
         <b-row :class="{ 'justify-content-center': 1 }">
            <b-col md="8" cols="12">
               <b-card>
                  <DocTabs pdf-title="JoinAntiCorruptionCertificate" view-title="Info">
                     <template #pdf>
                        <WIframe :src="IframeSrc" style="height: 100vh" :show="Data && Data.id2" />
                     </template>
                     <template #view>
                        <JoinAntiCorruptionCertificateFormView :data="Data" />
                     </template>
                  </DocTabs>
               </b-card>
            </b-col>
         </b-row>
      </b-container>
   </b-overlay>
</template>

<script>
import { BOverlay, BCard, BRow, BCol, BButton, BLink, BIcon, BBadge, BContainer } from 'bootstrap-vue';
import axios from 'axios';
import JoinAntiCorruptionCertificateService from '@/services/corruption/joinanticorruptioncertificate.service';
import DocTabs from '@/views/components/document/DocTabs.vue';
import WIframe from '@/components/WIframe.vue';
import JoinAntiCorruptionCertificateFormView from '@/views/components/corruption/JoinAntiCorruptionCertificateFormView.vue';

export default {
   components: {
      BOverlay,
      BCard,
      BRow,
      BCol,
      BButton,
      BLink,
      BIcon,
      BBadge,
      BContainer,
      DocTabs,
      WIframe,
      JoinAntiCorruptionCertificateFormView
   },
   data() {
      return {
         iframeLoaded: false,
         axios,
         show: false,
         Data: {}
      };
   },
   computed: {
      IframeSrc() {
         return (
            axios.defaults.baseURL +
            `Corruption/JoinAntiCorruptionCertificate/DownloadPdf?id2=${this.Data.id2}&lang=${this.getPdfLang()}`
         );
      }
   },
   created() {
      this.GetData();
   },
   methods: {
      GetData() {
         this.show = true;
         JoinAntiCorruptionCertificateService.Get(this.$route.params.id)
            .then((res) => {
               this.Data = res.data;
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
