<template>
   <div>
      <form-table-hrm
         :items="items"
         :fields="fields"
         :filter.sync="filter"
         searchable
         :busy="isBusy"
         :actions="{
            create: {
               name: 'EditEmployeeLeaveOrder',
               permission: 'EmployeeLeaveOrderCreate',
               query: {
                  employeeSickLeaveTypeId: page == 'bolaparvarishi' ? 1 : 2
               }
            },
            edit: {
               name: handleName,
               permission: 'EmployeeLeaveOrderEdit'
            },
            delete: {
               name: 'EditEmployeeLeaveOrder',
               permission: 'EmployeeLeaveOrderDelete'
            }
         }"
         @request="Refresh"
      >
         <template #cell(actions)="{ item }">
            <div class="text-center" style="text-wrap: nowrap">
               <!-- edit -->
               <b-link
                  v-if="$can('EmployeeLeaveOrderEdit', 'permissions') && item.canEdit"
                  :to="{
                     name: handleName,
                     params: { id: item.id }
                  }"
                  v-b-tooltip.hover.top="$t('Edit')"
                  class="mr-1 cursor-pointer"
               >
                  <feather-icon icon="EditIcon"></feather-icon>
               </b-link>
               <!-- view -->
               <b-link
                  :to="{
                     name: handleView,
                     params: { id: item.id }
                  }"
                  v-b-tooltip.hover.top="$t('View')"
                  v-if="
                     $can('EmployeeLeaveOrderView', 'permissions') ||
                     $can('EmployeeLeaveOrderSignerView', 'permissions')
                  "
                  class="mr-1 cursor-pointer"
               >
                  <feather-icon icon="EyeIcon"></feather-icon>
               </b-link>
               <!-- delete -->
               <b-link
                  v-if="$can('EmployeeLeaveOrderDelete', 'permissions') && item.canDelete"
                  class="text-danger mr-1 cursor-pointer"
                  @click="Delete(item)"
                  v-b-tooltip.hover.top="$t('Delete')"
               >
                  <feather-icon icon="TrashIcon"></feather-icon>
               </b-link>
               <!-- Sign -->
               <b-link
                  v-if="$can('EmployeeLeaveOrderSign', 'permissions') && item.canSign"
                  @click="OpenSign(item, 'Sign')"
                  class="mr-1 text-success cursor-pointer"
                  v-b-tooltip.hover.top="$t('Approve')"
               >
                  <feather-icon icon="CheckCircleIcon"></feather-icon>
               </b-link>
               <!-- Cancel -->
               <template v-if="$can('EmployeeLeaveOrderCancel', 'permissions') && item.canCancel">
                  <b-link
                     class="text-danger cursor-pointer mr-1"
                     v-b-tooltip.hover.top="$t('Cancel')"
                     @click="Cancel(item)"
                  >
                     <feather-icon icon="XCircleIcon"></feather-icon>
                  </b-link>
               </template>

               <!-- document history -->
               <HistoryModalButton :id="item.id" :table-id="item.tableId || 89" />
            </div>
         </template>
         <template #status>
            <b-row>
               <b-col>
                  <status-select
                     class="mt-1"
                     v-model="filter.statusId"
                     @input="ChangeStatus"
                     :filter="[1, 2, 3, 4, 27, 24]"
                  />
               </b-col> </b-row
         ></template>
      </form-table-hrm>

      <!-- sign dialog -->
      <b-modal v-model="EImzoModal" size="md" :title="$t('enterEImzo')" hide-footer>
         <b-card-text v-if="selectedItem">
            <just-sign :data-to-sign="selectedItem" v-if="!SignLoading" @sign="loginESP($event)" />
            <div style="height: 400px" v-if="SignLoading" class="d-flex justify-content-center align-items-center">
               <b-spinner label="Spinning"></b-spinner>
            </div>
         </b-card-text>
      </b-modal>
   </div>
</template>

<script>
import StatusSelect from '@/views/components/document/StatusSelect.vue';
import { BCard, VBTooltip, BModal, VBModal, BLink, BCardText, BSpinner, BRow, BCol } from 'bootstrap-vue';
import FormTableHrm from '@/components/forms/form-table-hrm.vue';
import EmployeeLeaveOrderService from '@/services/hrm/employeeleaveorder.service';
import eimzoMixin from '@/mixins/eimzo';
import justSign from '@/components/justSign.vue';
import HistoryModalButton from '@/views/components/document/HistoryModalButton.vue';

export default {
   components: {
      BRow,
      BCol,
      StatusSelect,
      BCard,
      BLink,
      BModal,
      BCardText,
      BSpinner,
      VBModal,
      FormTableHrm,
      justSign,
      HistoryModalButton
   },
   props: {
      page: {
         type: String,
         default: 'index'
      }
   },
   directives: {
      'b-tooltip': VBTooltip,
      'b-modal': VBModal
   },
   mixins: [eimzoMixin],
   data() {
      return {
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
               key: 'details',
               label: this.$t('details')
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
         ],
         filter: {
            search: '',
            statusId: null,
            statusIds: [],
            sortBy: '',
            orderType: 'asc',
            page: 1,
            pageSize: 20,
            employeeSickLeaveTypeId: null,
            perPageOptions: [10, 20, 50, 100],
            total: 0
         },
         isBusy: false,
         selectedItem: null,
         EImzoModal: false,
         SignLoading: false,
         activeSignAction: null
      };
   },

   created() {
      this.employeeSickLeaveTypeId();
   },

   computed: {
      handleName() {
         let a = 'EditEmployeeLeaveOrder';
         if (this.$props.page === 'bolaparvarishi') {
            a = 'EditBolaparvarishi';
         } else if (this.$props.page === 'homiladorliktatili') {
            a = 'EditHomiladorliktatili';
         } else {
            a = 'EditEmployeeLeaveOrder';
         }
         return a;
      },

      handleView() {
         let a = 'ViewEmployeeLeaveOrder';
         if (this.$props.page === 'bolaparvarishi') {
            a = 'ViewBolaparvarishi';
         } else if (this.$props.page === 'homiladorliktatili') {
            a = 'ViewHomiladorliktatili';
         } else {
            a = 'ViewEmployeeLeaveOrder';
         }
         return a;
      }
   },
   watch: {
      '$route.name': function () {
         this.Refresh();
      }
   },
   methods: {
      ChangeStatus() {
         if (this.filter.statusId == null) {
            this.filter.statusIds = [];
            this.Refresh();
         } else {
            this.filter.statusIds[0] = this.filter.statusId;
            this.Refresh();
         }
      },
      employeeSickLeaveTypeId() {
         if (this.$props.page === 'bolaparvarishi') {
            this.filter.employeeSickLeaveTypeId = 1;
            console.log('saslas');
         } else if (this.$props.page === 'homiladorliktatili') {
            this.filter.employeeSickLeaveTypeId = 2;
            console.log('homila');
         } else {
            this.filter.employeeSickLeaveTypeId = null;
         }
      },

      Refresh() {
         this.isBusy = true;
         if (this.$route.name == 'EmployeeLeaveOrderForSigner') {
            EmployeeLeaveOrderService.GetListForSigner(this.filter)
               .then((res) => {
                  this.items = res.data.rows;
                  this.filter.total = res.data.total;
               })
               .finally(() => {
                  this.isBusy = false;
               });
         } else {
            EmployeeLeaveOrderService.GetList(this.filter)
               .then((res) => {
                  this.items = res.data.rows;
                  this.filter.total = res.data.total;
               })
               .finally(() => {
                  this.isBusy = false;
               });
         }
      },
      Delete(item) {
         this.$swal.fire({
            icon: 'info',
            title: this.$t('WantDelete'),
            showLoaderOnConfirm: true,
            preConfirm: () => {
               return EmployeeLeaveOrderService.Delete(item.id)
                  .then(() => {
                     this.makeToast(this.$t('DeleteSuccess'), 'success');
                     this.Refresh();
                  })
                  .catch(this.SwalError);
            },
            allowOutsideClick: () => !this.$swal.isLoading()
         });
      },
      loginESP(item) {
         const isPinfl = this.isPinfl(item);
         if (this.activeSignAction == 'Sign') {
            this.Sign(item.key);
         }
      },
      OpenSign(item, type) {
         this.activeSignAction = type;
         this.EImzoModal = true;
         this.selectedItem = item;
      },
      Sign(signedData, isPinfl) {
         this.SignLoading = true;

         EmployeeLeaveOrderService.Sign({
            id: this.selectedItem.id,
            signedData: signedData,
            isPinfl: isPinfl
         })
            .then(() => {
               this.makeToast(this.$t('AcceptSuccess'), 'success');
               this.Refresh();
               this.EImzoModal = false;
            })
            .catch(this.showApiError)
            .finally(() => {
               this.SignLoading = false;
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
               return EmployeeLeaveOrderService.Cancel({
                  id: item.id,
                  message: msg,
                  signedData: 'cancel'
               })
                  .then(() => {
                     this.makeToast(this.$t('CancelMessage'), 'success');
                     this.Refresh();
                  })
                  .catch(this.SwalError);
            },
            allowOutsideClick: () => !this.$swal.isLoading()
         });
      }
   }
};
</script>
