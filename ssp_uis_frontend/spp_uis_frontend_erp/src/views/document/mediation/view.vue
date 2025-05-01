<template>
   <div class="container">
      <b-overlay :show="loader" spinner-variant="primary" spinner-type="grow" rounded="sm">
         <b-row>
            <b-col md="8" sm="12">
               <b-card style="min-height: 80vh">
                  <DocTabs pdf-title="Mediation" view-title="Info">
                     <template #pdf>
                        <WIframe :src="IframeSrc" style="height: 100vh" :show="data && data.id2" />
                     </template>
                     <template #view>
                        <MediationFormView is-component />
                     </template>
                  </DocTabs>
               </b-card>
            </b-col>
            <b-col md="4" sm="12">
               <b-button
                  target="_blank"
                  v-if="data && data.files?.length"
                  :href="IframeSrcFile"
                  class="mt-2 w-100"
                  size="xl"
                  variant="primary"
               >
                  <b-spinner style="margin-right: 10px" v-if="downloadLoading" small></b-spinner>
                  <feather-icon icon="DownloadIcon"></feather-icon>
                  {{ $t('Load') }}
               </b-button>
            </b-col>
         </b-row>
      </b-overlay>
   </div>
</template>

<script>
import axios from 'axios';
import { BCard, BOverlay, BRow, BCol, BButton, BSpinner } from 'bootstrap-vue';
import MediationService from '@/services/document/mediation.service';
import DocTabs from '@/views/components/document/DocTabs.vue';
import WIframe from '@/components/WIframe.vue';
import MediationFormView from './edit.vue';

export default {
   components: { BCard, BRow, BCol, BOverlay, DocTabs, WIframe, MediationFormView, BButton, BSpinner, axios },
   data() {
      return {
         loader: false,
         downloadLoading: false,
         IframeSrc: '',
         data: {}
      };
   },

   computed: {
      IframeSrcFile() {
         return axios.defaults.baseURL + `Mediation/DownloadFile/${this.data?.files[0]?.id}`;
      }
   },

   created() {
      this.loader = true;
      MediationService.Get(this.$route.params.id)
         .then(async (res) => {
            this.data = res.data;
            const { id2 } = res.data;
            const { data } = await MediationService.DownloadPdf(id2, this.getPdfLang());
            this.IframeSrc = URL.createObjectURL(data);
         })
         .finally(() => {
            this.loader = false;
         });
   }
};
</script>
