<template>
   <b-overlay :show="show">
      <b-container max-width="1400">
         <b-row class="justify-content-center">
            <b-col md="8" sm="12">
               <b-card>
                  <DocTabs pdf-title="ClaimApplication" view-title="Info">
                     <template #pdf>
                        <WIframe
                           :src="IframeSrc"
                           style="height: 100vh"
                           :show="Application.application && Application.application.id2"
                        />
                     </template>
                     <template #view>
                        <ClaimApplicationFormView :application="Application" />
                     </template>
                  </DocTabs>
               </b-card>
            </b-col>
            <b-col md="4" sm="12">
               <b-button
                  target="_blank"
                  v-if="Application && Application.files?.length"
                  @click="Print"
                  class="mt-2 w-100"
                  size="xl"
                  variant="primary"
               >
                  <b-spinner style="margin-right: 10px" v-if="downloadLoading" small></b-spinner>
                  <feather-icon icon="DownloadIcon"></feather-icon>
                  {{ $t('Load') }}
               </b-button>
               <b-button
                  v-if="Application.canAccept"
                  @click="Accept"
                  class="mt-2 w-100"
                  size="xl"
                  variant="outline-success"
               >
                  <feather-icon icon="CheckIcon"></feather-icon>
                  {{ $t('Accept') }}
               </b-button>

               <!-- <b-button
                  v-if="Application.canReject"
                  @click="OpenReject"
                  class="mt-2 w-100"
                  size="xl"
                  variant="outline-danger"
               >
                  <feather-icon icon="XCircleIcon"></feather-icon>
                  {{ $t('Reject') }}
               </b-button> -->
               <b-modal v-model="RejectModal" :title="$t('Reject')" hide-footer>
                  <h5 class="mb-2">{{ $t('WantReject') }}</h5>

                  <div class="form-group">
                     <form-input required v-model="filter.message" :label="$t('message')" />
                  </div>

                  <div class="d-flex justify-content-end">
                     <b-button
                        class="mt-2 mr-2"
                        @click="RejectModal = !RejectModal"
                        style="width: 100%"
                        size="xl"
                        variant="danger"
                        >{{ $t('no') }}</b-button
                     >

                     <b-button
                        class="mt-2"
                        :disabled="!filter.message"
                        @click="Reject"
                        style="width: 100%"
                        size="xl"
                        variant="success"
                        >{{ $t('yes') }}</b-button
                     >
                  </div>
               </b-modal>

               <b-button
                  v-if="Application.canCancel"
                  @click="Cancel"
                  class="mt-2 w-100"
                  size="xl"
                  variant="outline-warning"
               >
                  <feather-icon icon="XIcon"></feather-icon>
                  {{ $t('CancelApproval') }}
               </b-button>

               <b-button
                  v-if="Application.id && Application.canCreateMediationPlan"
                  :to="{
                     name: 'EditMediationPlan',
                     params: { id: 0 },
                     query: { applicationId: Application.applicationId }
                  }"
                  class="mt-2 w-100"
                  size="xl"
                  variant="outline-primary"
               >
                  <feather-icon icon="VideoIcon"></feather-icon>
                  {{ $t('MediationPlan') }}
               </b-button>
               <b-button
                  v-if="Application.canEmployeeAttechment"
                  :to="{ name: 'Ijrochitayinlash', params: { id: Application.id } }"
                  class="mt-2 w-100 text-danger"
                  size="xl"
                  variant="outline-primary"
                  >{{ $t('ijrochiga biriktirish') }}
               </b-button>

               <b-form-checkbox class="mt-1" v-model="forMediation">{{ $t('forMediation') }}</b-form-checkbox>

               <b-button
                  v-if="
                     $can('ApplicationForCourtEdit', 'permissions') &&
                     Application.canCreateWhithOutMediation &&
                     forMediation
                  "
                  :to="{
                     name: 'EditApplicationForCourt',
                     params: { id: 0 },
                     query: {
                        docNumber: Application.application.docNumber,
                        applicationID: Application.application.id
                     }
                  }"
                  class="mt-2 w-100"
                  size="xl"
                  variant="success"
                  ><feather-icon icon="FileIcon"></feather-icon>
                  {{ $t('ApplicationForCourt') }}
               </b-button>
               <!-- cancel message -->
               <b-alert
                  class="mt-1 p-1"
                  show
                  variant="danger"
                  v-if="Application.message || Application.application.message"
               >
                  <p class="px-1">
                     {{ Application.message || Application.application.message }}
                  </p>
               </b-alert>
            </b-col>
         </b-row>
      </b-container>

      <Chat v-if="Application && Application.id" :table-id="Application.tableId || 101" :document-id="Application.id" />
   </b-overlay>
</template>

<script>
import {
   BOverlay,
   BCard,
   BRow,
   BCol,
   BSpinner,
   BButton,
   BLink,
   BIcon,
   BBadge,
   BContainer,
   BModal,
   BFormCheckbox,
   BAlert
} from 'bootstrap-vue';
import axios from 'axios';
import ClaimApplicationService from '@/services/document/claimapplication.service';
import DocTabs from '@/views/components/document/DocTabs.vue';
import WIframe from '@/components/WIframe.vue';
import ClaimApplicationFormView from '@/views/components/claim/ClaimApplicationFormView.vue';
const Chat = () => import('@/views/components/DocumentChat/Chat.vue');

export default {
   components: {
      BOverlay,
      BCard,
      BRow,
      BCol,
      BButton,
      BLink,
      BSpinner,
      BIcon,
      BBadge,
      BContainer,
      BModal,
      DocTabs,
      WIframe,
      ClaimApplicationFormView,
      Chat,
      BFormCheckbox,
      BAlert
   },
   data() {
      return {
         show: false,
         forMediation: false,
         axios,
         downloadLoading: false,
         RejectModal: false,
         Application: {},
         filter: {
            message: '',
            id: 0
         }
      };
   },
   computed: {
      IframeSrc() {
         return (
            axios.defaults.baseURL +
            `ClaimApplication/DownloadPdf?id2=${this.Application?.application?.id2}&lang=${this.getPdfLang()}`
         );
      }
   },
   created() {
      this.GetApplication();
   },
   methods: {
      Print() {
         ClaimApplicationService.DownloadFile(this.Application.files[0].id).then((res) => {
            this.forceFileDownload(res, this.Application.files[0].fileName, this.Application.files[0].fileExtension);
         });
      },
      GetApplication() {
         this.show = true;
         ClaimApplicationService.Get(this.$route.params.id)
            .then((res) => {
               this.Application = res.data;
            })
            .catch((error) => {
               this.showApiError(error);
            })
            .finally(() => {
               this.show = false;
            });
      },
      Cancel() {
         this.$swal.fire({
            icon: 'question',
            title: this.$t('WantCancel'),
            input: 'textarea',
            inputPlaceholder: this.$t('Cancel'),
            showLoaderOnConfirm: true,
            preConfirm: (msg) => {
               return ClaimApplicationService.Cancel({
                  id: this.Application.id,
                  message: msg
               })
                  .then(() => {
                     this.makeToast(this.$t('CancelMessage'), 'success');
                     this.GetApplication();
                  })
                  .catch(this.SwalError);
            },
            allowOutsideClick: () => !this.$swal.isLoading()
         });
      },

      OpenReject() {
         this.RejectModal = true;
         this.filter = {
            message: this.filter.message,
            id: this.Application.id
         };
      },
      Reject() {
         ClaimApplicationService.Reject(this.filter)
            .then((res) => {
               this.makeToast(this.$t('RejectMessage'), 'success');
               this.GetApplication();
            })
            .catch(this.SwalError)
            .finally(() => {
               this.RejectModal = false;
            });
      },
      Accept() {
         this.$swal.fire({
            icon: 'question',
            title: this.$t('WantAccept'),
            showLoaderOnConfirm: true,
            preConfirm: () => {
               return ClaimApplicationService.Accept({
                  id: this.Application.id,
                  message: ''
               })
                  .then(() => {
                     this.makeToast(this.$t('AcceptMessage'), 'success');
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
