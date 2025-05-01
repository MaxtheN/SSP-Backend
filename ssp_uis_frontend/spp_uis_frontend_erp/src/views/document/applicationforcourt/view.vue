<template>
   <div class="container">
      <b-overlay :show="loader" spinner-variant="primary" spinner-type="grow" rounded="sm">
         <b-row>
            <b-col md="8" sm="12">
               <b-card style="min-height: 40vh">
                  <DocTabs pdf-title="ApplicationForCourt" view-title="Info">
                     <template #pdf>
                        <WIframe :src="IframeSrcFileQrCode" style="height: 100vh" show v-if="IframeSrcFileQrCode" />
                     </template>
                     <template #view>
                        <ApplicationForCourtFormView is-component />
                     </template>
                  </DocTabs>
               </b-card>
            </b-col>
            <b-col md="4" sm="12">
               <b-row>
                  <b-col md="12" sm="12">
                     <a
                        v-if="data.files[0].id"
                        :href="IframeSrcFileQrCode"
                        target="_blank"
                        class="btn btn-warning mt-2 w-100"
                        role="button"
                        size="xl"
                     >
                        <b-spinner style="margin-right: 10px" v-if="downloadLoading" small></b-spinner>
                        <feather-icon icon="DownloadIcon"></feather-icon>
                        {{ $t('applicationforcourt') }}
                     </a>
                  </b-col>
                  <b-col md="6" sm="12">
                     <b-button
                        @click="DownloadClaimApplication"
                        :disabled="tempData && tempData.claimApplicationFileIds?.ids.length == 0"
                        class="mt-2 w-100"
                        size="xl"
                        variant="primary"
                     >
                        <b-spinner style="margin-right: 10px" v-if="downloadLoading" small></b-spinner>
                        <feather-icon icon="DownloadIcon"></feather-icon>
                        {{ $t('ClaimApplication') }}
                     </b-button>
                  </b-col>

                  <b-col md="6" sm="12">
                     <a
                        :disabled="tempData && !tempData.claimApplicationIntegrationFileUrl.claimApplicationLink"
                        :href="tempData.claimApplicationIntegrationFileUrl?.claimApplicationLink"
                        target="_blank"
                        class="btn btn-warning mt-2 w-100"
                        role="button"
                        size="xl"
                     >
                        <b-spinner style="margin-right: 10px" v-if="downloadLoading" small></b-spinner>
                        <feather-icon icon="DownloadIcon"></feather-icon>
                        {{ $t('claimApplicationLink') }}
                     </a>
                  </b-col>

                  <b-col md="6" sm="12">
                     <b-button
                        @click="DownloadMediation"
                        :disabled="tempData && tempData.mediationFileIds?.ids.length == 0"
                        class="mt-2 w-100"
                        size="xl"
                        variant="primary"
                     >
                        <b-spinner style="margin-right: 10px" v-if="downloadLoading" small></b-spinner>
                        <feather-icon icon="DownloadIcon"></feather-icon>
                        {{ $t('Mediation') }}
                     </b-button>
                  </b-col>
                  <b-col md="6" sm="12">
                     <a
                        :disabled="tempData && !tempData.claimApplicationIntegrationFileUrl.mediationLink"
                        :href="tempData.claimApplicationIntegrationFileUrl.mediationLink"
                        target="_blank"
                        class="btn btn-warning mt-2 w-100"
                        role="button"
                        size="xl"
                     >
                        <b-spinner style="margin-right: 10px" v-if="downloadLoading" small></b-spinner>
                        <feather-icon icon="DownloadIcon"></feather-icon>
                        {{ $t('mediationLink') }}
                     </a>
                  </b-col>
                  <b-col md="6" sm="12">
                     <b-button
                        target="_blank"
                        :href="IframeSrcFile"
                        :disabled="data && !data.files?.length"
                        class="mt-2 w-100"
                        size="xl"
                        variant="primary"
                     >
                        <b-spinner style="margin-right: 10px" v-if="downloadLoading" small></b-spinner>
                        <feather-icon icon="DownloadIcon"></feather-icon>
                        {{ $t('LoadDoc') }}
                     </b-button>
                  </b-col>
                  <b-col md="6" sm="12">
                     <a
                        :disabled="tempData && !tempData.claimApplicationIntegrationFileUrl.mediationPLanLink"
                        :href="tempData.claimApplicationIntegrationFileUrl.mediationPLanLink"
                        target="_blank"
                        class="btn btn-warning mt-2 w-100"
                        role="button"
                        size="xl"
                     >
                        <b-spinner style="margin-right: 10px" v-if="downloadLoading" small></b-spinner>
                        <feather-icon icon="DownloadIcon"></feather-icon>
                        {{ $t('mediationPlanLink') }}
                     </a>
                  </b-col>
                  <b-col sm="12" md="12" class="mt-1"><div style="border-bottom: 1px solid #000"></div> </b-col>
                  <b-col md="6" sm="12" class="text-primary mt-1">{{ $t('Biriktirilgan hujjatlar') }}</b-col>
                  <b-col md="6" sm="12" class="text-warning mt-1">{{ $t('Tizimda shakllangan PDF') }}</b-col>
                  <b-col md="6" sm="12">
                     <b-button
                        v-if="$can('ApplicationForCourtAccept', 'permissions') && data.canAccept"
                        @click="OpenSign(data)"
                        class="mt-2 w-100"
                        size="xl"
                        variant="success"
                     >
                        <b-spinner style="margin-right: 10px" v-if="downloadLoading" small></b-spinner>
                        <feather-icon icon="CheckSquareIcon"></feather-icon>
                        {{ $t('Accept') }}</b-button
                     >
                  </b-col>
                  <b-col md="6" sm="12">
                     <!-- reject -->
                     <b-button
                        v-if="$can('ApplicationForCourtCancel', 'permissions') && data.canCancel"
                        @click="NotAccept(data)"
                        class="mt-2 w-100"
                        size="xl"
                        variant="danger"
                     >
                        <b-spinner style="margin-right: 10px" v-if="downloadLoading" small></b-spinner>
                        <feather-icon icon="XCircleIcon"></feather-icon>
                        {{ $t('Reject') }}
                     </b-button>
                  </b-col>
               </b-row>
            </b-col>
         </b-row>
         <!-- sign -->
         <b-modal v-model="EImzoModal" size="lg" :title="$t('enterEImzo')" hide-footer>
            <b-card-text v-if="selectedItem">
               <just-sign :data-to-sign="selectedItem" v-if="!SignLoading" @sign="loginESP($event)" />
               <div style="height: 600px" v-if="SignLoading" class="d-flex justify-content-center align-items-center">
                  <b-spinner label="Spinning"></b-spinner>
               </div>
            </b-card-text>
         </b-modal>
      </b-overlay>
   </div>
</template>

<script>
import { BCard, BOverlay, BCol, BRow, BButton, BCardText, BSpinner, BModal, VBModal } from 'bootstrap-vue';
import ApplicationForCourtService from '@/services/document/applicationforcourt.service';
import justSign from '@/components/justSign.vue';
import DocTabs from '@/views/components/document/DocTabs.vue';
import WIframe from '@/components/WIframe.vue';
import ApplicationForCourtFormView from './edit.vue';
import ClaimApplicationService from '@/services/document/claimapplication.service';
import axios from 'axios';
import MediationService from '@/services/document/mediation.service';

export default {
   components: {
      BCardText,
      BCard,
      justSign,
      BModal,
      VBModal,
      BOverlay,
      DocTabs,
      WIframe,
      ApplicationForCourtFormView,
      BCol,
      BButton,
      BSpinner,
      BRow
   },
   data() {
      return {
         loader: false,
         downloadLoading: false,
         IframeSrcFileQrCode: null,
         data: {},
         EImzoModal: false,
         selectedItem: null,
         tempData: {}
      };
   },
   computed: {
      IframeSrc() {
         return (
            axios.defaults.baseURL + `ApplicationForCourt/DownloadPdf?id2=${this.data?.id2}&lang=${this.getPdfLang()}`
         );
      },
      IframeSrcFile() {
         return axios.defaults.baseURL + `ApplicationForCourt/DownloadFile/${this.data?.files[0]?.id}`;
      }
   },
   async created() {
      this.loader = true;
      await ApplicationForCourtService.Get(this.$route.params.id)
         .then(async (res) => {
            this.data = res.data;
            if (this.data?.files[0]?.id) {
               ApplicationForCourtService.DownloadFileWithQrCode(this.data?.files[0]?.id).then((res2) => {
                  this.IframeSrcFileQrCode = URL.createObjectURL(res2.data);
               });
            }
         })
         .finally(() => {
            this.loader = false;
         });
      ApplicationForCourtService.GetForFiles(this.data.mediationId)
         .then(async (res) => {
            this.tempData = res.data;
         })
         .finally(() => {
            this.loader = false;
         });
   },
   methods: {
      DownloadClaimApplication() {
         this.tempData.claimApplicationFileIds?.ids.forEach((item, index) => {
            const fileType = this.tempData.claimApplicationFileIds.fileNames[index];
            ClaimApplicationService.DownloadFile(item).then((res) => {
               this.forceFileDownload(res, 'claimApplication', fileType);
            });
         });
      },

      DownloadMediation() {
         this.tempData.mediationFileIds?.ids.forEach((item, index) => {
            const fileType = this.tempData.mediationFileIds.fileNames[index];
            MediationService.DownloadFile(item).then((res) => {
               this.forceFileDownload(res, 'mediation', fileType);
            });
         });
      },

      OpenSign(item) {
         this.EImzoModal = true;
         this.selectedItem = item;
      },
      Accept(key, isPinfl) {
         this.SignLoading = true;
         if (this.selectedItem) {
            const body = {
               id: this.selectedItem.id,
               signedData: key,
               isPinfl: isPinfl
            };
            //xodim
            // this.filter.isEmployee;
            if (false) {
               ApplicationForCourtService.AcceptForEmployee(body)
                  .then(() => {
                     this.makeToast(this.$t('ApproveMessage'), 'success');
                     this.EImzoModal = false;
                     this.Refresh();
                  })
                  .catch((error) => {
                     this.showApiError(error);
                  })
                  .finally(() => {
                     this.SignLoading = false;
                  });
            } else {
               ApplicationForCourtService.Accept(body)
                  .then(() => {
                     this.makeToast(this.$t('ApproveMessage'), 'success');
                     this.EImzoModal = false;
                     this.Refresh();
                  })
                  .catch((error) => {
                     this.showApiError(error);
                  })
                  .finally(() => {
                     this.SignLoading = false;
                  });
            }
         }
      },
      NotAccept(item) {
         this.$swal.fire({
            icon: 'info',
            title: this.$t('WantNotAccept'),
            showLoaderOnConfirm: true,
            preConfirm: () => {
               return ApplicationForCourtService.NotAccept(item.id)
                  .then(() => {
                     this.makeToast(this.$t('NotAcceptSuccess'), 'success');
                     this.GetApplication();
                  })
                  .catch(this.SwalError);
            },
            allowOutsideClick: () => !this.$swal.isLoading()
         });
      }
   }
};
</script>
