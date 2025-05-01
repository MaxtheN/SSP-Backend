<template>
   <b-overlay :show="show">
      <b-row class="justify-content-center">
         <b-col sm="12" md="8">
            <b-card>
               <DocTabs pdf-title="MemshipCertificate" view-title="Info" :value="1">
                  <template #pdf>
                     <WIframe :src="IframeSrc" style="height: 100vh" :show="iframeLoaded" />
                  </template>
                  <template #view>
                     <MemshipCertificateFormView :certificate="Data" />
                  </template>
               </DocTabs>
            </b-card>
         </b-col>
         <b-col sm="6" md="3" lg="3" class="text-center">
            <div>
               <b-button
                  v-if="Data.canCancel"
                  class="mr-2 mb-1"
                  @click="OpenCancel"
                  style="width: 100%"
                  size="xl"
                  variant="danger"
               >
                  <feather-icon icon="XCircleIcon"></feather-icon>
                  {{ $t('Cancel') }}
               </b-button>
               <b-button
                  v-if="Data.id == 0"
                  class="mr-2 mt-1"
                  @click="SaveData"
                  style="width: 100%"
                  size="xl"
                  variant="success"
               >
                  <feather-icon icon="CheckCircleIcon"></feather-icon>
                  {{ $t('create') }}
               </b-button>
               <b-button
                  v-if="Data.id == 0"
                  class="mr-2 mt-1"
                  @click="GoBack"
                  style="width: 100%"
                  size="xl"
                  variant="danger"
               >
                  <feather-icon icon="ArrowLeftIcon"></feather-icon>
                  {{ $t('goBack') }}
               </b-button>
               <b-modal v-model="cancelModal" :title="$t('Cancel')" hide-footer>
                  <b-card-text>
                     <h5>{{ $t('WantCancel') }}</h5>
                  </b-card-text>
                  <div>
                     <b-col sm="12" md="12" class="text-left">
                        <label for>{{ $t('message') }}</label>
                        <b-form-input v-model="filter.message" :placeholder="$t('message')"></b-form-input>
                     </b-col>
                  </div>
                  <div class="d-flex justify-content-end mt-1">
                     <b-button class="mr-1" @click="cancelModal != cancelModal" size="xl" variant="danger">{{
                        $t('no')
                     }}</b-button>
                     <b-button @click="Cancel" size="xl" variant="success">
                        {{ $t('yes') }}
                     </b-button>
                  </div>
               </b-modal>

               <b-card v-if="historyData.length" tag="article" class="mb-2">
                  <b-card-title class="text-danger">{{ $t('CLAIM_APPLICATION_CANCEL2') }}</b-card-title>
                  <b-card-text> {{ historyData[0].message || Data?.cancelReasen }} </b-card-text>
                  <b-button @click="PrintCancelFile(Data.id2, Data.files[0]?.id)" class="" variant="primary">
                     <feather-icon icon="PrinterIcon"></feather-icon>
                     {{ $t('download') }}
                  </b-button>
               </b-card>

               <!-- korxona info -->

               <b-card v-if="showInfoCard" tag="article" class="mb-2">
                  <b-overlay :show="infoShow">
                     <b-card-title class="text-success text-left">{{ $t('soliqContractorByTin') }}</b-card-title>
                     <b-card-text class="text-left">
                        <span style="font-weight: 900">{{ $t('amount') }}</span> : {{ currency(organInfo.amount) }}
                     </b-card-text>
                     <b-card-text class="text-left">
                        <span style="font-weight: 900">{{ $t('companyName') }}</span> : {{ organInfo.contractor }}
                     </b-card-text>
                     <b-card-text class="text-left">
                        <span style="font-weight: 900">{{ $t('contractorCategory') }}</span> :
                        {{ organInfo.contractorType }}
                     </b-card-text>
                  </b-overlay>
               </b-card>

               <!-- Muddatni uzaytirish -->
               <b-button
                  v-if="Data.canProlong"
                  class="mr-2 mb-1"
                  @click="MuddatOpen"
                  style="width: 100%"
                  size="xl"
                  variant="success"
               >
                  {{ $t('Muddatini uzaytirish') }}
               </b-button>
               <b-modal v-model="MuddatModal" :title="$t('Cancel')" hide-footer>
                  <b-card-text>
                     <h5>{{ $t('Muddatini uzaytirish') }}</h5>
                  </b-card-text>
                  <div>
                     <b-col sm="12" md="12" class="text-left">
                        <form-picker v-model="prolon.newExpireOn" :placeholder="$t('Sana')" />
                     </b-col>
                     <b-col sm="12" md="12" class="text-left">
                        <b-form-input v-model="prolon.details" :placeholder="$t('message')"></b-form-input>
                     </b-col>
                  </div>

                  <div class="d-flex justify-content-end mt-1">
                     <b-button class="mr-1" @click="MuddatModal = !MuddatModal" size="xl" variant="danger">{{
                        $t('no')
                     }}</b-button>
                     <b-button @click="SendProlon" :disabled="!prolon.newExpireOn" size="xl" variant="success">
                        {{ $t('yes') }}
                     </b-button>
                  </div>
               </b-modal>
            </div>
         </b-col>
      </b-row>
   </b-overlay>
</template>

<script>
import {
   BOverlay,
   BCard,
   BCardTitle,
   BRow,
   BCol,
   BFormInput,
   BButton,
   BTable,
   BLink,
   BFormGroup,
   VBTooltip,
   BModal,
   VBModal,
   BCardText,
   BInputGroup,
   BInputGroupAppend,
   BTr,
   BTd,
   BFormTextarea,
   BFormCheckbox,
   BIcon,
   BBadge
} from 'bootstrap-vue';
import axios from 'axios';
// ProlongExpireOn

import MemshipCertificateService from '@/services/document/memshipcertificate.service';
import WIframe from '@/components/WIframe.vue';
import DocTabs from '@/views/components/document/DocTabs.vue';
import MemshipCertificateFormView from '@/views/components/memship/MemshipCertificateFormView.vue';
import ManualService from '@/services/others/manual.service';
export default {
   components: {
      BCardTitle,
      BOverlay,
      BCard,
      BRow,
      BCol,
      BFormInput,
      BButton,
      BTable,
      BLink,
      BFormGroup,
      BModal,
      BCardText,
      BInputGroup,
      BInputGroupAppend,
      BTr,
      BTd,
      BFormTextarea,
      BFormCheckbox,
      BIcon,
      BBadge,
      WIframe,
      DocTabs,
      MemshipCertificateFormView
   },
   directives: {
      'b-tooltip': VBTooltip,
      'b-modal': VBModal
   },
   data() {
      return {
         prolon: {
            id: null,
            newExpireOn: '',
            details: ''
         },
         infoShow: true,
         organInfo: {},
         axios,
         IframeSrc: '',
         historyData: [],
         show: false,
         Data: {},
         filter: {
            id: 0,
            message: ''
         },
         showInfoCard: false,
         MuddatModal: false,
         cancelModal: false,
         loading: false,
         iframeLoaded: false
      };
   },

   async created() {
      this.OpenHistory();
      this.getData();
   },
   methods: {
      async getData() {
         this.show = true;
         try {
            if (this.$route.query.isList) {
               await MemshipCertificateService.GetByMemshipContractId(this.$route.query.contractId)
                  .then((res) => {
                     this.show = false;
                     this.Data = res.data;
                     this.GetFromSoliq(res.data.contractorInn);
                     this.getIframePdf(this.Data);
                  })
                  .catch((err) => {
                     this.showApiError(err);
                  });
            } else {
               await MemshipCertificateService.Get(this.$route.params.id)
                  .then((res) => {
                     this.show = false;
                     this.Data = res.data;
                     this.GetFromSoliq(res.data.contractorInn);
                     this.getIframePdf(this.Data);
                  })
                  .catch((err) => {
                     this.showApiError(err);
                  });
            }
         } catch (e) {
            console.log(e);
         }
      },
      OpenCancel() {
         this.cancelModal = true;
         this.filter = {
            id: this.Data.id,
            message: ''
         };
      },
      MuddatOpen() {
         this.MuddatModal = true;
      },
      SendProlon() {
         MemshipCertificateService.ProlongExpireOn({ ...this.prolon, id: this.Data.id })
            .then(() => {
               this.makeToast(this.$t('SaveSuccess'), 'success');
               this.MuddatModal = false;
               this.getData();
            })
            .catch((err) => {
               this.showApiError(err);
            });
      },
      Cancel() {
         MemshipCertificateService.Cancel({
            id: this.Data.id,
            message: this.filter.message
         })
            .then((res) => {
               this.makeToast(this.$t('CancelSuccess'), 'success');
               this.$router.push({ name: 'MemshipCertificate' });
            })
            .catch((err) => {
               this.showApiError(err);
            })
            .finally(() => {
               this.cancelModal = false;
            });
      },
      Print(id) {
         MemshipCertificateService.PrintCertificatePdf(id).then((res) => {
            this.forceFileDownload(res, this.$t('MemshipCertificate'));
         });
      },
      PrintCancelFile(id2, fileId) {
         MemshipCertificateService.DownloadFile(id2, fileId).then((res) => {
            this.forceFileDownload(res, this.Data.files[0].fileName, this.Data.files[0].fileExtension);
         });
      },
      GoBack() {
         this.$router.push({ name: 'MemshipCertificate' });
      },
      getIframePdf() {
         this.iframeLoaded = false;
         if (this.Data.id == 0) {
            MemshipCertificateService.DownloadPdfCopy({
               docNumber: this.Data.docNumber,
               contractorId: this.Data.contractorId,
               // memshipContractId: this.$route.query.contractId,
               memshipContractId: this.Data.memshipContractId,
               docOn: this.Data.docOn
            })
               .then((res) => {
                  this.IframeSrc = URL.createObjectURL(res.data);
               })
               .catch((err) => {
                  this.showApiError(err);
               })
               .finally(() => {
                  this.iframeLoaded = true;
               });
         } else {
            MemshipCertificateService.DownloadPdf(this.Data.id2, this.getPdfLang())
               .then((res) => {
                  this.IframeSrc = URL.createObjectURL(res.data);
               })
               .catch((err) => {
                  this.showApiError(err);
               })
               .finally(() => {
                  this.iframeLoaded = true;
               });
         }
      },
      SaveData() {
         MemshipCertificateService.Update(this.Data)
            .then(() => {
               this.makeToast(this.$t('SaveSuccess'), 'success');
               this.$router.push({ name: 'MemshipCertificate' });
            })
            .catch((err) => {
               this.showApiError(err);
            });
      },
      OpenHistory() {
         ManualService.GetListByDocumentId(98, this.$route.params.id)
            .then((res) => {
               this.historyData = res.data.filter((item) => item.statusId == 24);
            })
            .catch((error) => {
               this.showApiError(error);
            })
            .finally(() => {
               this.isBusy = false;
            });
      },
      GetFromSoliq(inn) {
         MemshipCertificateService.GetFromSoliq(inn).then((res) => {
            this.organInfo = res.data;
            this.infoShow = false;
         });
      }
   }
};
</script>
