<template>
   <b-overlay :show="show">
      <b-container max-width="2000">
         <b-row class="justify-content-center">
            <b-col md="8" cols="12">
               <b-card v-if="Application && Application.application">
                  <DocTabs pdf-title="ServiceApplication" view-title="Info">
                     <template #pdf>
                        <WIframe
                           :src="IframeSrc"
                           style="height: 100vh"
                           :show="Application.application && Application.application.id2"
                        />
                     </template>
                     <template #view>
                        <ServiceApplicationFormView :application="Application" />
                     </template>
                  </DocTabs>
               </b-card>
            </b-col>
            <b-col md="4" cols="12" v-if="Application.canReceived || Application.canAccept || Application.canCancel">
               <b-card>
                  <template v-if="Application.isFree">
                     <!-- Received -->
                     <b-button
                        v-if="Application.canReceived"
                        @click="dialog = true"
                        class="w-100 mb-1"
                        variant="outline-success"
                     >
                        <feather-icon icon="CheckCircleIcon"></feather-icon>
                        {{ $t('Accept') }}
                     </b-button>
                  </template>
                  <template v-else>
                     <!-- accept -->
                     <b-button
                        v-if="Application.canAccept"
                        @click="Accept()"
                        class="w-100 mb-1"
                        variant="outline-success"
                     >
                        <feather-icon icon="CheckCircleIcon"></feather-icon>
                        {{ $t('Accept') }}
                     </b-button>
                  </template>

                  <b-button
                     variant="success"
                     v-if="Application.canCreateContract && !Application.isFree"
                     :to="{ name: 'EditServiceContract', params: { id: 0 }, query: { appId: Application.id } }"
                     v-b-tooltip.hover.top="$t('ServiceContract')"
                     class="w-100 mb-1"
                  >
                     <feather-icon icon="FileTextIcon"></feather-icon>
                     {{ $t('ServiceContract') }}
                  </b-button>

                  <!-- cancel -->
                  <b-button v-if="Application.canCancel" variant="outline-warning" class="w-100 mb-1" @click="Cancel()">
                     <feather-icon icon="XCircleIcon"></feather-icon>
                     {{ $t('Cancel') }}
                  </b-button>
               </b-card>
               <b-alert v-if="Application.message" variant="danger" show>
                  <p class="px-1">{{ Application.message }}</p>
               </b-alert>
            </b-col>
         </b-row>
      </b-container>
      <b-modal size="lg" no-enforce-focus :title="$t('NeedChamberService')" tabindex="-1" v-model="dialog" hide-footer>
         <template v-for="(priceTable, i) in groups">
            <table v-if="priceTable.tables.length > 0" class="priceTable mt-2 w-100" :key="i + 'group'">
               <thead>
                  <tr>
                     <th class="text-center" colspan="2">{{ priceTable.group }}</th>
                  </tr>
               </thead>
               <tbody>
                  <tr v-for="(tab, j) in priceTable.tables" :key="j + 'tab' + i">
                     <td>{{ tab.needChamberService }}</td>
                     <td class="text-center w-70px">
                        <b-form-checkbox v-model="tab.isCompleted" />
                     </td>
                  </tr>
               </tbody>
            </table>
         </template>

         <b-button @click="Received()" class="w-100 mb-1 mt-1" variant="outline-success">
            <feather-icon icon="CheckCircleIcon"></feather-icon>
            {{ $t('Accept') }}
         </b-button>
      </b-modal>

      <Chat v-if="Application && Application.id" :table-id="Application.tableId || 117" :document-id="Application.id" />
   </b-overlay>
</template>

<script>
import {
   VBModal,
   VBTooltip,
   BOverlay,
   BCard,
   BRow,
   BCol,
   BButton,
   BLink,
   BModal,
   BIcon,
   BBadge,
   BContainer,
   BFormCheckbox,
   BAlert
} from 'bootstrap-vue';

import axios from 'axios';
import ServiceApplicationService from '@/services/srv/ServiceApplication.service';
import DocTabs from '@/views/components/document/DocTabs.vue';
import WIframe from '@/components/WIframe.vue';
import ServiceApplicationFormView from '@/views/components/srv/ServiceApplicationFormView.vue';
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
      BModal,
      BFormCheckbox,
      ServiceApplicationFormView,
      Chat,
      BAlert
   },
   directives: {
      'b-tooltip': VBTooltip,
      'b-modal': VBModal
   },
   data() {
      return {
         iframeLoaded: false,
         show: false,
         dialog: false,
         Application: {},
         groups: []
      };
   },
   computed: {
      IframeSrc() {
         return (
            axios.defaults.baseURL +
            `srv/ServiceApplication/DownloadPdf?id2=${this.Application?.application?.id2}&lang=${this.getPdfLang()}`
         );
      }
   },
   created() {
      this.GetApplication();
   },
   methods: {
      GetApplication() {
         this.show = true;
         ServiceApplicationService.Get(this.$route.params.id)
            .then((res) => {
               this.Application = res.data;
               this.groups = res.data.groups.map((e) => ({
                  id: e.id,
                  groupId: e.groupId,
                  group: e.group,
                  tables: e.tables.map((j) => ({ ...j, id: j.id, isCompleted: false }))
               }));
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
            showLoaderOnConfirm: true,
            input: 'text',
            inputPlaceholder: this.$t('CancelMessage'),
            preConfirm: (msg) => {
               return ServiceApplicationService.Cancel({
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
      Accept() {
         this.$swal.fire({
            icon: 'info',
            title: this.$t('WantAccept'),
            showLoaderOnConfirm: true,
            input: 'text',
            inputPlaceholder: this.$t('AcceptMessage'),
            preConfirm: (msg) => {
               return ServiceApplicationService.Accept({
                  id: this.Application.id,
                  message: msg
               })
                  .then(() => {
                     this.makeToast(this.$t('AcceptSuccess'), 'success');
                     this.GetApplication();
                  })
                  .catch(this.SwalError);
            },
            allowOutsideClick: () => !this.$swal.isLoading()
         });
      },
      Received() {
         this.$swal.fire({
            icon: 'info',
            title: this.$t('WantAccept'),
            showLoaderOnConfirm: true,
            input: 'text',
            inputPlaceholder: this.$t('AcceptSuccess'),
            preConfirm: (msg) => {
               if (this.Application.isFree) {
                  return ServiceApplicationService.AcceptForFree({
                     id: this.Application.id,
                     message: msg
                  })
                     .then(() => {
                        this.makeToast(this.$t('AcceptSuccess'), 'success');
                        this.GetApplication();
                        this.dialog = false;
                        this.$router.push({ name: 'ServiceApplication' });
                     })
                     .catch(this.SwalError);
               } else {
                  return ServiceApplicationService.Accept({
                     id: this.Application.id,
                     message: msg,
                     groups: []
                  })
                     .then(() => {
                        this.makeToast(this.$t('AcceptSuccess'), 'success');
                        this.GetApplication();
                        this.dialog = false;
                        this.$router.push({ name: 'ServiceApplication' });
                     })
                     .catch(this.SwalError);
               }
            },
            allowOutsideClick: () => !this.$swal.isLoading()
         });
      }
   }
};
</script>
<style lang="scss">
.priceTable {
   thead {
      tr {
         background-color: #f0f0f0;
      }
   }

   tr th,
   tr td {
      padding: 7px;
      border-collapse: collapse;
      border: 1px solid #f5f5f5;
   }

   tr:nth-child(even) {
      background-color: #f7f7f7;
   }

   .w-70px {
      width: 70px;
   }
}
</style>
