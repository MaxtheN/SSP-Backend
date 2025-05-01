<template>
   <b-overlay :show="show">
      <b-container max-width="2000">
         <b-row :class="{ 'justify-content-center': 1 }">
            <b-col md="8" cols="12" class="order-left">
               <b-card>
                  <DocTabs pdf-title="JoinAntiCorruptionApplication" view-title="Info">
                     <template #pdf>
                        <WIframe
                           :src="IframeSrc"
                           style="height: 100vh"
                           :show="Application.application && Application.application.id2"
                        />
                     </template>
                     <template #view>
                        <JoinAntiCorruptionAppFormView :application="Application" />
                     </template>
                  </DocTabs>
               </b-card>
            </b-col>
            <b-col md="4" cols="12" class="order-right">
               <!-- reject -->
               <b-button
                  v-if="Application.canReject"
                  class="mt-2 mr-2 w-100"
                  @click="Reject"
                  size="xl"
                  variant="outline-danger"
               >
                  <feather-icon icon="XCircleIcon"></feather-icon>
                  {{ $t('Reject') }}
               </b-button>
               <!-- accept -->
               <b-button
                  v-if="Application.canAccept || Application.canAcceptOmbudsman || Application.canAcceptSSP"
                  class="mt-2 mr-2 w-100"
                  @click="Accept"
                  size="xl"
                  variant="outline-success"
               >
                  <feather-icon icon="CheckCircleIcon"></feather-icon>
                  {{ $t('Accept') }}
               </b-button>
               <!-- cancel -->
               <b-button
                  v-if="Application.canCancel"
                  class="mt-2 mr-2 w-100"
                  @click="Cancel"
                  size="xl"
                  variant="outline-warning"
               >
                  <feather-icon icon="XIcon"></feather-icon>
                  {{ $t('Cancel') }}
               </b-button>
            </b-col>
         </b-row>
      </b-container>

      <Chat v-if="Application && Application.id" :table-id="Application.tableId || 107" :document-id="Application.id" />
   </b-overlay>
</template>

<script>
import { BOverlay, BCard, BRow, BCol, BButton, BLink, BIcon, BBadge, BContainer } from 'bootstrap-vue';
import axios from 'axios';
import JoinAntiCorruptionApplicationService from '@/services/corruption/joinanticorruptionapplication.service';
import DocTabs from '@/views/components/document/DocTabs.vue';
import WIframe from '@/components/WIframe.vue';
import JoinAntiCorruptionAppFormView from '@/views/components/corruption/JoinAntiCorruptionAppFormView.vue';
const Chat = () => import('@/views/components/DocumentChat/Chat.vue');

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
      JoinAntiCorruptionAppFormView,
      Chat
   },
   data() {
      return {
         axios,
         show: false,
         Application: {}
      };
   },
   computed: {
      IframeSrc() {
         return (
            axios.defaults.baseURL +
            `Corruption/JoinAntiCorruptionApplication/DownloadPdf?id2=${
               this.Application?.application?.id2
            }&lang=${this.getPdfLang()}`
         );
      }
   },
   created() {
      this.GetApplication();
   },
   methods: {
      GetApplication() {
         this.show = true;
         JoinAntiCorruptionApplicationService.Get(this.$route.params.id)
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
            icon: 'info',
            title: this.$t('WantCancel'),
            showLoaderOnConfirm: true,
            preConfirm: () => {
               return JoinAntiCorruptionApplicationService.Cancel({
                  id: this.Application.id,
                  message: ''
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
      Reject() {
         this.$swal.fire({
            icon: 'info',
            title: this.$t('WantReject'),
            showLoaderOnConfirm: true,
            input: 'text',
            inputPlaceholder: this.$t('RejectMessage'),
            preConfirm: (msg) => {
               return JoinAntiCorruptionApplicationService.Reject({
                  id: this.Application.id,
                  message: msg
               })
                  .then(() => {
                     this.makeToast(this.$t('RejectSuccess'), 'success');
                     this.GetApplication();
                  })
                  .catch(this.SwalError);
            },
            allowOutsideClick: () => !this.$swal.isLoading()
         });
      },
      Accept() {
         this.$swal.fire({
            icon: 'question',
            title: this.$t('WantAccept'),
            showLoaderOnConfirm: true,
            preConfirm: () => {
               return JoinAntiCorruptionApplicationService.Accept({
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
