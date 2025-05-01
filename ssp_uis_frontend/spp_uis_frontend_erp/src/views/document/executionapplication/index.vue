<template>
   <form-table-hrm
      :items="items"
      responsive
      :fields="fields"
      :filter.sync="filter"
      searchable
      :busy="isBusy"
      :deleteLoading="deleteLoading"
      :actions="{
         create: {
            name: 'EditExecutionApplication',
            permission: 'ExecutionApplicationView'
         },
         edit: {
            name: 'EditExecutionApplication',
            permission: 'ExecutionApplicationView'
         },
         delete: {
            name: 'EditExecutionApplication',
            permission: 'ExecutionApplicationView'
         }
      }"
      @row-dblclicked="DbClick"
      @request="Refresh"
      @row-delete="Delete"
   >
      <template #filter>
         <b-row>
            <b-col cols="auto">
               <b-button
                  class="mt-2"
                  variant="primary"
                  @click="$router.push({ name: 'EditExecutionApplication', params: { id: 0 } })"
               >
                  <feather-icon icon="PlusIcon"></feather-icon>
                  {{ $t('create') }}</b-button
               >
            </b-col>
            <b-col cols="12" md="2">
               <form-picker
                  v-model="filter.fromDocDate"
                  :label="$t('startOn')"
                  @change="Refresh"
                  :placeholder="$t('docOn')"
               />
            </b-col>
            <b-col cols="12" md="2">
               <form-picker
                  v-model="filter.toDocDate"
                  :label="$t('endDate')"
                  @change="Refresh"
                  :placeholder="$t('docOn')"
               />
            </b-col>
            <b-col></b-col>
            <b-col cols="12" md="4">
               <b-input-group class="mt-2">
                  <b-form-input v-model="filter.search" @keyup.enter="Refresh" :placeholder="$t('search')" />
                  <b-input-group-append>
                     <b-button @click="Refresh" variant="primary">
                        <feather-icon icon="SearchIcon" />
                     </b-button>
                  </b-input-group-append>
               </b-input-group>
            </b-col>
         </b-row>
      </template>

      <template #cell(totalPaymentAmount)="{ item }">
         {{ currency(item.totalPaymentAmount) }}
      </template>
      <template #cell(totalAverageSalary)="{ item }">{{ currency(item.totalAverageSalary) }}</template>
      <template #cell(actions)="{ item }">
         <div style="display: flex">
            <!-- view -->
            <b-link
               :to="{ name: 'EditExecutionApplication', params: { id: item.id }, query: { isview: true } }"
               style="margin-right: 10px; cursor: pointer"
               v-b-tooltip.hover.top="$t('Edit')"
            >
               <feather-icon icon="EyeIcon"></feather-icon>
            </b-link>

            <!-- edit -->
            <b-link
               v-if="item.canEdit"
               :to="{ name: 'EditExecutionApplication', params: { id: item.id } }"
               style="margin-right: 10px; cursor: pointer"
               v-b-tooltip.hover.top="$t('Edit')"
            >
               <feather-icon icon="EditIcon"></feather-icon>
            </b-link>

            <!-- accept -->
            <b-link
               v-if="item.canAccept"
               style="margin-right: 10px; cursor: pointer"
               @click="Accept(item)"
               class="text-success cursor-pointer"
               v-b-tooltip.hover.top="$t('Approve')"
            >
               <feather-icon icon="CheckCircleIcon"></feather-icon>
            </b-link>

            <!-- sign -->
            <b-link
               v-if="item.canSign"
               @click="OpenSign(item)"
               style="margin-right: 10px; cursor: pointer"
               class="text-success cursor-pointer"
               v-b-tooltip.hover.top="$t('Sign')"
            >
               <EIconSVG style="color: blue" />
            </b-link>
            <b-modal v-model="EImzoModal" size="lg" :title="$t('EImzo')" hide-footer>
               <b-card-text>
                  <just-sign :data-to-sign="item" v-if="!SignLoading" @sign="loginESP($event)" />
                  <div
                     style="height: 600px"
                     v-if="SignLoading"
                     class="d-flex justify-content-center align-items-center"
                  >
                     <b-spinner label="Spinning"></b-spinner>
                  </div>
               </b-card-text>
            </b-modal>

            <!-- cancel -->
            <b-link
               v-if="item.canCancel"
               style="margin-right: 10px; cursor: pointer"
               class="text-danger cursor-pointer mr-1"
               v-b-tooltip.hover.top="$t('Cancel')"
               @click="Cancel(item)"
            >
               <feather-icon icon="XCircleIcon"></feather-icon>
            </b-link>
         </div>
      </template>
   </form-table-hrm>
</template>

<script>
import {
   BCard,
   BBadge,
   BButton,
   BRow,
   BCol,
   BInputGroup,
   BInputGroupAppend,
   BFormInput,
   BLink,
   VBTooltip,
   BModal,
   BCardText,
   BSpinner
} from 'bootstrap-vue';
import FormTableHrm from '@/components/forms/form-table-hrm.vue';
import JustSign from '@/components/justSign.vue';
import ExecutionApplicationService from '@/services/document/executionapplication.service';
import EIconSVG from '@/components/EIconSVG.vue';
export default {
   components: {
      BLink,
      EIconSVG,
      JustSign,
      BCardText,
      BModal,
      BCard,
      BButton,
      BRow,
      BCol,
      BInputGroup,
      BFormInput,
      BInputGroupAppend,
      BBadge,
      FormTableHrm,
      BSpinner
   },
   directives: {
      'b-tooltip': VBTooltip
   },
   data() {
      return {
         EImzoModal: false,
         selectedItem: null,
         SignLoading: false,
         items: [],
         fields: [
            {
               key: 'id',
               label: this.$t('id'),
               thClass: 'text-center',
               tdClass: 'text-center',
               sortable: true
            },
            {
               key: 'id',
               label: this.$t('T/R'),
               thClass: 'text-center',
               tdClass: 'text-center',
               sortable: true
            },
            {
               key: 'docNumber',
               label: this.$t('docnumber'),
               thClass: 'text-center',
               tdClass: 'text-center',
               sortable: true
            },
            {
               key: 'docOn',
               label: this.$t('docOn'),
               thClass: 'text-center',
               tdClass: 'text-center',
               sortable: true
            },
            {
               key: 'organization',
               label: this.$t('organization'),
               thClass: 'text-center',
               thStyle: {
                  minWidth: '250px'
               },
               tdClass: 'text-center',
               sortable: true
            },
            {
               key: 'region',
               label: this.$t('region'),
               thClass: 'text-center',
               tdClass: 'text-center',
               sortable: true
            },
            {
               key: 'district',
               label: this.$t('district'),
               thClass: 'text-center',
               tdClass: 'text-center',
               sortable: true
            },
            {
               key: 'year',
               label: this.$t('docyear'),
               thClass: 'text-center',
               tdClass: 'text-center',
               sortable: true
            },
            {
               key: 'totalNewVacanciesCount',
               label: this.$t('LDYTEIOS'),
               thClass: 'text-center',
               tdClass: 'text-center',
               thStyle: {
                  minWidth: '200px'
               },
               sortable: true
            },
            {
               key: 'totalPaymentAmount',
               label: this.$t('HIS'),
               thClass: 'text-center',
               thStyle: {
                  minWidth: '150px'
               },
               tdClass: 'text-center',
               sortable: true
            },
            {
               key: 'totalAverageSalary',
               label: this.$t('BOIH'),
               thClass: 'text-center',
               tdClass: 'text-center',
               thStyle: {
                  minWidth: '150px'
               },
               sortable: true
            },
            {
               key: 'status',
               label: this.$t('status'),
               thClass: 'text-center',
               tdClass: 'text-center'
            },
            {
               key: 'actions',
               label: this.$t('actions'),
               thClass: 'text-center',
               tdClass: 'text-center'
            }
         ],
         SignData: {
            signedData: '',
            signedAt: new Date(),
            id: null,
            statusId: null,
            message: ''
         },
         filter: {
            employeeId: null,
            startOn: '',
            endOn: '',
            empAppointOrderTypeId: null,
            statusIds: [],
            search: '',
            sortBy: '',
            page: 1,
            orderType: '',
            pageSize: 20,
            perPageOptions: [10, 20, 50, 100],
            total: 0
         },
         isBusy: false,
         deleteLoading: false
      };
   },
   methods: {
      DbClick(item) {
         this.$router.push({
            name: 'EditExecutionApplication',
            params: { id: item.id }
         });
      },
      Delete(item) {
         this.deleteLoading = true;
         ExecutionApplicationService.Delete(item.id)
            .then(() => {
               this.makeToast(this.$t('DeleteSuccess'), 'success');
               this.Refresh();
            })
            .catch((error) => {
               this.showApiError(error);
            })
            .finally(() => {
               this.deleteLoading = false;
            });
      },
      Refresh() {
         this.isBusy = true;
         ExecutionApplicationService.GetList(this.filter)
            .then((res) => {
               this.items = res.data.rows;
               this.filter.total = res.data.total;
            })
            .finally(() => {
               this.isBusy = false;
            });
      },
      Accept(item) {
         this.$swal.fire({
            icon: 'info',
            title: this.$t('WantAccept'),
            showLoaderOnConfirm: true,
            preConfirm: () => {
               return ExecutionApplicationService.Accept({
                  id: item.id,
                  message: ''
               })
                  .then(() => {
                     this.makeToast(this.$t('AcceptSuccess'), 'success');
                     this.Refresh();
                  })
                  .catch(this.SwalError);
            },
            allowOutsideClick: () => !this.$swal.isLoading()
         });
      },
      Cancel(item) {
         this.$swal.fire({
            icon: 'question',
            title: this.$t('WantCancel'),
            showLoaderOnConfirm: true,
            preConfirm: () => {
               return ExecutionApplicationService.Cancel({
                  message: '',
                  id: item.id
               })
                  .then(() => {
                     this.makeToast(this.$t('CancelMessage'), 'success');
                     this.Refresh();
                  })
                  .catch(this.SwalError);
            },
            allowOutsideClick: () => !this.$swal.isLoading()
         });
      },

      loginESP(item) {
         this.Sign(item.key);
         console.log(item);
      },
      OpenSign(item) {
         this.SignData.id = item.id;
         this.SignData.statusId = 27;
         this.EImzoModal = true;
         // this.ClearFilter();
      },
      ClearFilter() {
         this.SignData = {
            signedData: '',
            message: '',
            id: null,
            prtnRejectReasonId: 0,
            files: []
         };
      },
      Sign(key) {
         this.SignLoading = true;
         this.SignData.signedData = key;

         ExecutionApplicationService.Sign(this.SignData)
            .then(() => {
               this.makeToast(this.$t('SignMessage'), 'success');
               this.SignLoading = false;
               this.SignModal = false;
               this.EImzoModal = false;
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
