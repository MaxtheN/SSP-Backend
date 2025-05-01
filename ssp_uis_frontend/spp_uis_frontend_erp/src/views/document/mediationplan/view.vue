<template>
   <div class="container">
      <b-overlay :show="loader" spinner-variant="primary" spinner-type="grow" rounded="sm">
         <b-card style="min-height: 80vh">
            <DocTabs pdf-title="MediationPlan" view-title="Info">
               <template #pdf>
                  <WIframe :src="IframeSrc" style="height: 100vh" :show="data && data.id2" />
               </template>
               <template #view>
                  <MediationPlanFormView is-component :view="true" />
               </template>
            </DocTabs>
         </b-card>
      </b-overlay>
   </div>
</template>

<script>
import { BCard, BOverlay } from 'bootstrap-vue';
import MediationPlanService from '@/services/document/mediationplan.service';
import DocTabs from '@/views/components/document/DocTabs.vue';
import WIframe from '@/components/WIframe.vue';
import MediationPlanFormView from './edit.vue';

export default {
   components: { BCard, BOverlay, DocTabs, WIframe, MediationPlanFormView },
   data() {
      return {
         loader: false,
         IframeSrc: '',
         data: {}
      };
   },
   created() {
      this.loader = true;
      MediationPlanService.Get(this.$route.params.id)
         .then(async (res) => {
            this.data = res.data;
            const { id2 } = res.data;
            const { data } = await MediationPlanService.DownloadPdf(id2, this.getPdfLang());
            this.IframeSrc = URL.createObjectURL(data);
         })
         .finally(() => {
            this.loader = false;
         });
   }
};
</script>
