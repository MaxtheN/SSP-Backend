<template>
   <form-table-hrm
      :items="items"
      :fields="fields"
      :filter.sync="filter"
      searchable
      :busy="isBusy"
      :actions="{
         edit: {
            name: 'EditMemshipPaymentOrder',
            permission: 'MemshipPaymentOrderEdit'
         },
         delete: {
            name: 'EditMemshipPaymentOrder',
            permission: 'MemshipPaymentOrderDelete'
         }
      }"
      @row-dblclicked="DbClick"
      @request="Refresh"
   >
      <!-- filter -->
      <template #filter>
         <b-row class="order-2">
            <b-col cols="12" md="6" class="d-flex align-items-center justify-content-start mb-1 mb-md-0 order-left">
               <template
                  v-if="
                     $can('MemshipPaymentOrderCreate', 'permissions') ||
                     $can('ServicePaymentOrderCreate', 'permissions')
                  "
               >
                  <b-button
                     variant="primary"
                     :to="{ name: 'EditMemshipPaymentOrder', params: { id: 0 }, query: $route.query.applicationTypeId }"
                  >
                     <feather-icon icon="PlusIcon"></feather-icon>
                     {{ $t('create') }}
                  </b-button>
               </template>
            </b-col>
            <b-col md="2"></b-col>
            <b-col cols="12" md="4" class="order-right">
               <b-input-group>
                  <b-form-input v-model="filter.search" @keyup.enter="Refresh" :placeholder="$t('search')" />
                  <b-input-group-append>
                     <b-button @click="Refresh" variant="primary">
                        <feather-icon icon="SearchIcon" />
                     </b-button>
                  </b-input-group-append>
               </b-input-group>
            </b-col>
         </b-row>
         <b-row class="order-1">
            <b-col cols="12" md="3">
               <div>
                  <label for>{{ $t('startdate') }}</label>
                  <form-picker v-model="filter.fromDocDate" :placeholder="$t('startdate')" @input="Refresh" />
               </div>
            </b-col>
            <b-col cols="12" md="3">
               <div>
                  <label for>{{ $t('enddate') }}</label>
                  <form-picker v-model="filter.toDocDate" :placeholder="$t('enddate')" @input="Refresh" />
               </div>
            </b-col>
            <b-col sm="12" md="3">
               <form-select
                  disabled
                  :options="ApplicationTypeIdConstComp"
                  v-model="filter.applicationTypeId"
                  label="applicationType"
               ></form-select>
            </b-col>

            <!-- memship -->
            <template v-if="filter.applicationTypeId == 3">
               <b-col sm="12" md="3" class="mb-1">
                  <form-input :value="filter.memshipContractNumber" disabled :label="$t('MemshipContract')">
                     <b-input-group-append>
                        <b-button variant="primary" @click="dialog = true">
                           <feather-icon icon="PlusIcon"></feather-icon>
                        </b-button>
                     </b-input-group-append>
                  </form-input>
               </b-col>

               <b-modal size="xl" :title="$t('MemshipContract')" v-model="dialog" hide-footer>
                  <MemshipContractList
                     :selectable="true"
                     @row-selected="memshipSelect"
                     hide-status
                     hide-contract-type
                     :memship-contract-type-id="1"
                     :status-id="21"
                  />
               </b-modal>
            </template>
            <b-col cols="12" v-if="filter.applicationTypeId == 3" class="text-right mt-1">
               <b-button @click="Print" :disabled="PrintLoading" variant="primary">
                  <feather-icon icon="PrinterIcon"></feather-icon>
                  {{ $t('Print') }}
               </b-button>
            </b-col>
            <!-- memship end -->

            <!-- service  -->
            <template v-if="filter.applicationTypeId == 7">
               <b-col sm="12" md="3" class="mb-1">
                  <form-input :value="filter.serviceContractNumber" disabled :label="$t('ServiceContract')">
                     <b-input-group-append>
                        <b-button variant="primary" @click="dialog = true">
                           <feather-icon icon="PlusIcon"></feather-icon>
                        </b-button>
                     </b-input-group-append>
                  </form-input>
               </b-col>
               <b-col cols="12" md="8">
                  <StatusSelect v-model="filter.statusId" @input="Refresh" :filter="[1, 2, 24]" />
               </b-col>

               <b-modal size="xl" :title="$t('ServiceContract')" v-model="dialog" hide-footer>
                  <ServiceContractList :selectable="true" @row-selected="serviceSelect" hide-status :status-id="21" />
               </b-modal>
            </template>
            <!-- service end -->
         </b-row>
      </template>

      <template #cell(actions)="{ item }">
         <!-- view -->
         <div class="text-center" style="text-wrap: nowrap">
            <b-link
               v-if="item.applicationTypeId == 3"
               :to="{ name: 'EditMemshipPaymentOrder', params: { id: item.id }, query: { view: true } }"
               v-b-tooltip.hover.top="$t('View')"
               class="mr-1 cursor-pointer"
            >
               <feather-icon icon="EyeIcon"></feather-icon
            ></b-link>

            <!-- edit -->
            <b-link
               v-if="item.applicationTypeId == 3 ? item.canEdit : item.srvCanAccept"
               :to="{ name: 'EditMemshipPaymentOrder', params: { id: item.id } }"
               v-b-tooltip.hover.top="$t('Edit')"
               class="mr-1 cursor-pointer"
            >
               <feather-icon icon="EditIcon"></feather-icon>
            </b-link>
            <!-- delete -->
            <b-link
               v-if="item.applicationTypeId == 3 && item.canDelete"
               class="text-danger mr-1 cursor-pointer"
               @click="Delete(item)"
               v-b-tooltip.hover.top="$t('Delete')"
            >
               <feather-icon icon="TrashIcon"></feather-icon>
            </b-link>
            <!-- accept -->
            <b-link
               v-if="item.applicationTypeId == 3 ? item.canAccept : item.srvCanAccept"
               @click="Accept(item)"
               class="mr-1 text-success cursor-pointer"
               v-b-tooltip.hover.top="$t('Approve')"
            >
               <feather-icon icon="CheckCircleIcon"></feather-icon>
            </b-link>
            <!-- cancel -->
            <template v-if="item.applicationTypeId == 3 ? item.canCancel : item.srvCanCancel">
               <b-link
                  class="text-danger cursor-pointer mr-1"
                  v-b-tooltip.hover.top="$t('Cancel')"
                  @click="Cancel(item)"
               >
                  <feather-icon icon="XCircleIcon"></feather-icon>
               </b-link>
            </template>
            <!-- history -->
            <HistoryModalButton v-if="item.applicationTypeId == 3" :id="item.id" :table-id="114" />
         </div>
      </template>
      <template #cell(amount)="{ item }">{{ currency(item.amount) }}</template>
      <template #cell(contractor)="{ item }"> {{ item.contractorInn }}-{{ item.contractor }} </template>
      <template #cell(status)="{ item }">
         <b-badge :variant="getColor(item)">{{ item.status }}</b-badge>
      </template>
   </form-table-hrm>
</template>

<script>
import {
   BCard,
   BBadge,
   BLink,
   BTooltip,
   VBTooltip,
   BRow,
   BCol,
   BModal,
   BInputGroup,
   BButton,
   BFormInput,
   BInputGroupAppend
} from 'bootstrap-vue';

import HistoryModalButton from '@/views/components/document/HistoryModalButton.vue';
import StatusSelect from '@/views/components/document/StatusSelect.vue';
import FormTableHrm from '@/components/forms/form-table-hrm.vue';
import MemshipPaymentOrderService from '@/services/dualedu/memshippaymentorder.service';
import ApplicationMixin from '@/mixins/application';

const MemshipContractList = () => import('@/views/document/memshipcontract/index.vue');
const ServiceContractList = () => import('@/views/srv/ServiceContract/index.vue');

export default {
   components: {
      StatusSelect,
      BCard,
      BBadge,
      BLink,
      BTooltip,
      BRow,
      BCol,
      BModal,
      BInputGroup,
      BInputGroupAppend,
      BButton,
      BFormInput,
      FormTableHrm,
      MemshipContractList,
      ServiceContractList,
      HistoryModalButton
   },
   mixins: [ApplicationMixin],
   directives: {
      'b-tooltip': VBTooltip
   },
   data() {
      return {
         items: [],
         PrintLoading: false,
         filter: {
            applicationTypeId: +this.$route.query.applicationTypeId || 3,
            search: '',
            sortBy: '',
            statusId: null,
            orderType: 'asc',
            page: 1,
            pageSize: 20,
            perPageOptions: [10, 20, 50, 100],
            total: 0
         },
         dialog: false,
         isBusy: false
      };
   },
   watch: {
      '$route.query': function (val) {
         this.filter.applicationTypeId = +val.applicationTypeId || 3;
         this.Refresh();
      }
   },
   computed: {
      ApplicationTypeIdConstComp() {
         return this.ApplicationTypeIdConst.filter((e) => [3, 7].includes(e.value));
      },
      fields() {
         return [
            {
               key: 'id',
               label: this.$t('id'),
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
               label: this.$t('ondate'),
               thClass: 'text-center',
               tdClass: 'text-center',
               sortable: true
            },
            {
               key: this.$route.query?.applicationTypeId == 3 ? 'memshipContractNumber' : 'serviceContractId',
               label:
                  this.$route.query?.applicationTypeId == 3
                     ? this.$t('memshipContractNumber')
                     : this.$t('contractNumber'),
               thClass: 'text-center',
               tdClass: 'text-center',
               sortable: true
            },
            {
               key: this.$route.query?.applicationTypeId == 3 ? 'memshipContractDocOn' : 'serviceContractDocOn',
               label:
                  this.$route.query?.applicationTypeId == 3
                     ? this.$t('memshipContractDocOn')
                     : this.$t('contractDocOn'),
               thClass: 'text-center',
               tdClass: 'text-center',
               sortable: true
            },
            {
               key: 'contractor',
               label: this.$t('contractor'),
               thStyle: {
                  minWidth: '300px'
               }
            },
            {
               key: 'amount',
               label: this.$t('amount'),
               thClass: 'text-center',
               thStyle: {
                  minWidth: '200px'
               },
               tdClass: 'text-center',
               sortable: true
            },
            {
               key: 'bankName',
               label: this.$t('bankName'),
               thClass: 'text-center',
               tdClass: 'text-center',
               sortable: true
            },
            {
               key: 'status',
               label: this.$t('status'),
               thClass: 'text-center',
               tdClass: 'text-center',
               sortable: true
            },
            {
               key: 'actions',
               label: this.$t('actions'),
               thClass: 'text-center',

               tdClass: 'text-center'
            }
         ];
      }
   },
   methods: {
      memshipSelect(item) {
         this.dialog = false;
         this.filter.memshipContractId = item.id;
         this.filter.memshipContractNumber = item.docNumber;
         this.Refresh();
      },
      serviceSelect(item) {
         this.dialog = false;
         this.filter.serviceContractId = item.id;
         this.filter.serviceContractNumber = item.docNumber;
         this.Refresh();
      },
      DbClick(item) {
         this.$router.push({
            name: 'EditMemshipPaymentOrder',
            params: { id: item.id }
         });
      },
      Delete(item) {
         this.$swal.fire({
            icon: 'info',
            title: this.$t('WantDelete'),
            showLoaderOnConfirm: true,
            preConfirm: () => {
               return MemshipPaymentOrderService.Delete(item.id)
                  .then(() => {
                     this.makeToast(this.$t('DeleteSuccess'), 'success');
                     this.Refresh();
                  })
                  .catch(this.SwalError);
            },
            allowOutsideClick: () => !this.$swal.isLoading()
         });
      },
      Accept(item) {
         this.$swal.fire({
            icon: 'info',
            title: this.$t('WantAccept'),
            showLoaderOnConfirm: true,
            preConfirm: () => {
               return MemshipPaymentOrderService.Accept({
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
            input: 'text',
            inputPlaceholder: this.$t('RejectMessage'),
            preConfirm: (msg) => {
               return MemshipPaymentOrderService.Cancel({
                  id: item.id,
                  message: msg
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
      Refresh() {
         this.isBusy = true;
         MemshipPaymentOrderService.GetList(this.filter)
            .then((res) => {
               this.items = res.data.rows;
               this.filter.total = res.data.total;
            })
            .finally(() => {
               this.isBusy = false;
            });
      },
      Print() {
         this.PrintLoading = true;
         MemshipPaymentOrderService.SaveAsExcelForGetListMemshipPaymentOrder(this.filter)
            .then((res) => {
               this.forceFileDownload(res, this.$t('MemshipPaymentOrder'));
            })
            .catch((error) => {
               this.PrintLoading = false;
               this.showApiError(error);
            })
            .finally(() => {
               this.PrintLoading = false;
            });
      }
   }
};
</script>

<style scoped>
@media only screen and (max-width: 768px) {
   .order-1 {
      order: 1;
   }
   .order-2 {
      order: 2;
   }
}
</style>
