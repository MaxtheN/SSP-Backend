<template>
   <div>
      <form-table-hrm
         :items="items"
         :busy="isBusy"
         :actions="{
            create: {
               name: 'EditAppointEmployee',
               permission: 'AppointEmployeeCreate'
            },
            edit: {
               name: 'EditAppointEmployee',
               permission: 'AppointEmployeeEdit'
            }
         }"
         :fields="fields"
         :filter.sync="filter"
         searchable
         @request="Refresh"
         @row-dblclicked="DbClick"
      >
         <template #cell(employees)="{ item }">
            {{ item.employees.join(', ') }}
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
         <template #filter>
            <b-row align-v="center">
               <b-col>
                  <b-button variant="primary" :to="{ name: 'EditAppointEmployee', params: { id: 0 } }">
                     <feather-icon icon="PlusIcon"></feather-icon>
                     {{ $t('create') }}
                  </b-button>
               </b-col>

               <b-col sm="12" md="3">
                  <form-select
                     :options="EmpAppointOrderTypeList"
                     v-model="filter.empAppointOrderTypeId"
                     label="empAppointOrderType"
                     @change="Refresh"
                  ></form-select>
               </b-col>
               <b-col cols="12" md="2">
                  <form-picker
                     v-model="filter.startOn"
                     :label="$t('startOn')"
                     @change="Refresh"
                     :placeholder="$t('docOn')"
                  />
               </b-col>
               <b-col cols="12" md="2">
                  <form-picker
                     v-model="filter.endOn"
                     :label="$t('endDate')"
                     @change="Refresh"
                     :placeholder="$t('docOn')"
                  />
               </b-col>
               <b-col cols="12" md="3" sm="12" class="d-flex no-wrap">
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
         </template>

         <template #cell(actions)="{ item }">
            <div class="text-left" style="text-wrap: nowrap">
               <!-- edit -->
               <b-link
                  v-if="$can('AppointEmployeeEdit', 'permissions') && item.canEdit"
                  :to="{ name: 'EditAppointEmployee', params: { id: item.id } }"
                  v-b-tooltip.hover.top="$t('Edit')"
                  class="mr-1 cursor-pointer"
               >
                  <feather-icon icon="EditIcon"></feather-icon>
               </b-link>
               <!-- delete -->
               <b-link
                  v-if="$can('AppointEmployeeDelete', 'permissions') && item.canDelete"
                  class="text-danger mr-1 cursor-pointer"
                  @click="Delete(item)"
               >
                  <feather-icon icon="TrashIcon"></feather-icon>
               </b-link>
               <!-- view -->
               <b-link
                  :to="{ name: 'ViewAppointEmployee', params: { id: item.id } }"
                  v-b-tooltip.hover.top="$t('View')"
                  v-if="$can('AppointEmployeeView', 'permissions') || $can('AppointEmployeeSignerView', 'permissions')"
                  class="mr-1 cursor-pointer"
               >
                  <feather-icon icon="EyeIcon"></feather-icon>
               </b-link>
               <!-- accept -->
               <b-link
                  v-if="
                     ($can('AppointEmployeeSign', 'permissions') && item.canSign) ||
                     ($can('AppointEmployeeWithoutSigner', 'permissions') && item.canWithOutSigner)
                  "
                  @click="OpenSign(item, 'accept')"
                  class="mr-1 text-success cursor-pointer"
                  v-b-tooltip.hover.top="$t('Sign')"
               >
                  <feather-icon icon="CheckCircleIcon"></feather-icon>
               </b-link>
               <!-- cancel -->
               <b-link
                  v-if="$can('AppointEmployeeCancel', 'permissions') && item.canCancel"
                  class="text-danger cursor-pointer mr-1"
                  v-b-tooltip.hover.top="$t('Cancel')"
                  @click="Cancel(item)"
               >
                  <feather-icon icon="XCircleIcon"></feather-icon>
               </b-link>
               <!-- SignUpdate -->
               <b-link v-if="item.canSignAnyway" class="text-success mr-1 cursor-pointer" @click="SignUpdate(item.id)">
                  <feather-icon icon="CheckSquareIcon"></feather-icon>
               </b-link>
               <!-- document history -->
               <HistoryModalButton :id="item.id" :table-id="item.tableId || 85" />
            </div>
         </template>
         <template #cell(status)="{ item }">
            <b-badge :variant="getColor(item)">{{ item.status }}</b-badge>
         </template>
      </form-table-hrm>
      <!-- sign dialog -->
      <b-modal v-model="EImzoModal" size="lg" :title="$t('EImzo')" hide-footer>
         <b-card-text v-if="selectedItem">
            <just-sign :data-to-sign="selectedItem" v-if="!SignLoading" @sign="loginESP($event)" />
            <div style="height: 600px" v-if="SignLoading" class="d-flex justify-content-center align-items-center">
               <b-spinner label="Spinning"></b-spinner>
            </div>
         </b-card-text>
      </b-modal>
   </div>
</template>

<script>
import {
   BFormInput,
   BInputGroup,
   BInputGroupAppend,
   BCard,
   BCardText,
   BSpinner,
   VBTooltip,
   BBadge,
   BButton,
   BLink,
   BModal,
   VBModal,
   BRow,
   BCol
} from 'bootstrap-vue';
import StatusSelect from '@/views/components/document/StatusSelect.vue';
import FormTableHrm from '@/components/forms/form-table-hrm.vue';
import AppointEmployeeService from '@/services/hrm/appointemployee.service';
import HistoryModalButton from '@/views/components/document/HistoryModalButton.vue';
import justSign from '@/components/justSign.vue';
import eimzoMixin from '@/mixins/eimzo';
import ManualService from '@/services/others/manual.service';
const DefaultFilter = {
   search: '',
   sortBy: '',
   orderType: 'asc',
   statusId: null,
   statusIds: [],
   startOn: '',
   endOn: '',
   page: 1,
   pageSize: 20,
   perPageOptions: [10, 20, 50, 100, 300],
   total: 0
};

export default {
   components: {
      BFormInput,
      BInputGroup,
      BInputGroupAppend,
      BCard,
      StatusSelect,
      BRow,
      BCol,
      FormTableHrm,
      BCardText,
      BButton,
      BBadge,
      BLink,
      BModal,
      HistoryModalButton,
      justSign,
      BSpinner
   },
   directives: {
      'b-tooltip': VBTooltip,
      'b-modal': VBModal
   },
   mixins: [eimzoMixin],
   data() {
      return {
         organizarionList: [],
         items: [],
         EmpAppointOrderTypeList: [],
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
               label: this.$t('docOn'),
               thClass: 'text-center',
               tdClass: 'text-center',
               sortable: true
            },
            {
               key: 'employees',
               label: this.$t('employee')
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
            sortBy: '',
            orderType: 'asc',
            statusId: null,
            statusIds: [],
            page: 1,
            pageSize: 20,
            perPageOptions: [10, 20, 50, 100, 300],
            total: 0
         },
         isBusy: false,
         EImzoModal: false,
         selectedItem: null,
         SignLoading: false,
         activeSignAction: null
      };
   },
   created() {
      if (JSON.parse(localStorage.getItem('filterData6'))) {
         this.filter = JSON.parse(localStorage.getItem('filterData6'));
      } else {
         this.filter = DefaultFilter;
      }
      ManualService.EmpAppointOrderTypeSelectList()
         .then((res) => {
            this.EmpAppointOrderTypeList = res.data;
         })
         .catch((error) => {
            this.showApiError(error);
         });
   },
   methods: {
      SignUpdate(id) {
         this.$swal.fire({
            icon: 'info',
            title: this.$t('Agree'),
            showLoaderOnConfirm: true,
            preConfirm: () => {
               return AppointEmployeeService.SignUpdate(id)
                  .then(() => {
                     this.makeToast(this.$t('SignMessage'), 'success');
                     this.Refresh();
                  })
                  .catch(this.SwalError);
            },
            allowOutsideClick: () => !this.$swal.isLoading()
         });
      },
      DbClick(item) {
         this.$router.push({
            name: 'EditAppointEmployee',
            params: { id: item.id, isView: true }
         });
      },
      loginESP(item) {
         const isPinfl = this.isPinfl(item);
         if (this.activeSignAction == 'accept') {
            this.Sign(item.key);
         }
      },
      OpenSign(item, type) {
         this.activeSignAction = type;
         this.EImzoModal = true;
         this.selectedItem = item;
      },
      Accept(key, isPinfl) {
         this.SignLoading = true;
         AppointEmployeeService.Accept({
            id: this.selectedItem.id,
            signedData: key,
            isPinfl: isPinfl
         })
            .then(() => {
               this.makeToast(this.$t('AcceptSuccess'), 'success');
               this.Refresh();
               this.EImzoModal = false;
            })
            .catch((err) => {
               this.showApiError(err);
            })
            .finally(() => {
               this.SignLoading = false;
            });
      },
      Delete(item) {
         this.$swal.fire({
            icon: 'info',
            title: this.$t('WantDelete'),
            showLoaderOnConfirm: true,
            preConfirm: () => {
               return AppointEmployeeService.Delete(item.id)
                  .then(() => {
                     this.makeToast(this.$t('DeleteSuccess'), 'success');
                     this.Refresh();
                  })
                  .catch(this.SwalError);
            },
            allowOutsideClick: () => !this.$swal.isLoading()
         });
      },
      Sign(key) {
         this.SignLoading = true;
         AppointEmployeeService.Sign({
            id: this.selectedItem.id,
            signedData: key
         })
            .then(() => {
               this.makeToast(this.$t('AcceptSuccess'), 'success');
               this.Refresh();
               this.EImzoModal = false;
            })
            .catch((err) => {
               this.showApiError(err);
            })
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
               return AppointEmployeeService.Cancel({
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
      },
      Refresh() {
         this.isBusy = true;
         AppointEmployeeService.GetList(this.filter)
            .then((res) => {
               this.items = res.data.rows;
               this.filter.total = res.data.total;
            })
            .finally(() => {
               this.isBusy = false;
            });
      },
      ChangeStatus() {
         if (this.filter.statusId == null) {
            this.filter.statusIds = [];
            this.Refresh();
         } else {
            this.filter.statusIds[0] = this.filter.statusId;
            this.Refresh();
         }
      }
   },
   watch: {
      filter: {
         handler(newValue, oldValue) {
            localStorage.setItem('filterData6', JSON.stringify(newValue));
            this.filter = newValue;
            if (newValue.districtId) {
               this.GetDistrict();
            }
         },
         deep: true
      }
   }
};
</script>
