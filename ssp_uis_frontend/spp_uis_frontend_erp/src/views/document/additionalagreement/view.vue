<template>
   <b-container style="max-width: 1000px">
      <b-card>
         <DocTabs pdf-title="AdditionalAgreement" view-title="Info">
            <template #pdf>
               <WIframe :src="IframeSrc" style="height: 100vh" :show="data.id2" />
            </template>
            <template #view>
               <AdditionalAgreementFormView :data="data" />
            </template>
         </DocTabs>

         <b-button
            v-if="data.canSign"
            class="mt-2"
            @click="OpenSign"
            style="width: 100%"
            size="xl"
            variant="outline-success"
         >
            <feather-icon icon="CheckIcon"></feather-icon>
            {{ $t('Sign') }}
         </b-button>
         <b-button
            v-if="data.canReject"
            class="mt-2"
            @click="OpenSign('reject')"
            style="width: 100%"
            size="xl"
            variant="danger"
         >
            <feather-icon icon="XIcon"></feather-icon>
            {{ $t('Reject') }}
         </b-button>

         <!-- sign dialog -->
         <b-modal v-model="EImzoModal" size="lg" :title="$t('EImzo')" hide-footer>
            <b-card-text>
               <just-sign :data-to-sign="data" v-if="!SignLoading" @sign="loginESP($event)" />
               <div style="height: 600px" v-if="SignLoading" class="d-flex justify-content-center align-items-center">
                  <b-spinner label="Spinning"></b-spinner>
               </div>
            </b-card-text>
         </b-modal>
      </b-card>
   </b-container>
</template>

<script>
import {
   BModal,
   BSpinner,
   BOverlay,
   BCard,
   BRow,
   BCol,
   BButton,
   BLink,
   BIcon,
   BBadge,
   BContainer,
   BCardText
} from 'bootstrap-vue';
import axios from 'axios';
import AdditionalAgreementService from '@/services/document/additionalagreement.service';
import justSign from '@/components/justSign.vue';
import eimzoMixin from '@/mixins/eimzo';
import WIframe from '@/components/WIframe.vue';
import AdditionalAgreementFormView from '@/views/components/memship/AdditionalAgreementFormView.vue';
import DocTabs from '@/views/components/document/DocTabs.vue';

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
      justSign,
      BModal,
      BSpinner,
      BCardText,
      WIframe,
      AdditionalAgreementFormView,
      DocTabs
   },
   mixins: [eimzoMixin],
   data() {
      return {
         iframeLoaded: false,
         EImzoModal: false,
         SignLoading: false,
         show: false,
         data: {},
         reject: ''
      };
   },
   computed: {
      IframeSrc() {
         return (
            axios.defaults.baseURL + `AdditionalAgreement/DownloadPdf?id2=${this.data.id2}&lang=${this.getPdfLang()}`
         );
      }
   },
   created() {
      this.GetData();
   },
   methods: {
      GetData() {
         this.show = true;
         AdditionalAgreementService.Get(this.$route.params.id)
            .then((res) => {
               this.data = res.data;
            })
            .catch((error) => {
               this.showApiError(error);
            })
            .finally(() => {
               this.show = false;
            });
      },
      OpenSign(reject) {
         this.EImzoModal = true;
         this.reject = reject;
      },
      loginESP(item) {
         const isPinfl = this.isPinfl(item);
         if (this.reject == 'reject') {
            this.Reject(item.key, isPinfl);
         } else {
            this.Sign(item.key, isPinfl);
         }
      },
      Sign(key, isPinfl) {
         this.SignLoading = true;
         AdditionalAgreementService.Sign({
            signedData: key,
            isPinfl: isPinfl,
            id: this.data.id
         })
            .then(() => {
               this.makeToast(this.$t('SignMessage'), 'success');
               this.SignLoading = false;
               this.SignModal = false;
               this.EImzoModal = false;
               this.GetData();
            })
            .catch((error) => {
               this.showApiError(error);
            })
            .finally(() => {
               this.SignLoading = false;
            });
      },
      Reject(key, isPinfl) {
         this.SignLoading = true;
         AdditionalAgreementService.Reject({
            signedData: key,
            isPinfl: isPinfl,
            id: this.data.id
         })
            .then(() => {
               this.makeToast(this.$t('RejectSuccess'), 'success');
               this.SignLoading = false;
               this.SignModal = false;
               this.EImzoModal = false;
               this.reject = '';
               this.GetData();
            })
            .catch((error) => {
               this.showApiError(error);
            })
            .finally(() => {
               this.SignLoading = false;
            });
      }
   }
};
</script>
