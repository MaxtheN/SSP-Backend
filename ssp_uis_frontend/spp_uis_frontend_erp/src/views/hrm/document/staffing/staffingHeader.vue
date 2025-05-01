<template>
   <form-table-hrm
      :items="items"
      :fields="fields"
      :filter.sync="filter"
      searchable
      :busy="isBusy"
      :actions="{
         create: {
            name: 'EditStaffing',
            permission: 'StaffingCreate'
         },
         edit: {
            name: 'EditStaffing',
            permission: 'StaffingEdit'
         }
      }"
      @row-dblclicked="DbClick"
      @request="Refresh"
   >
      <template #filter>
         <b-row align-v="center" class="justify-content-between">
            <b-col cols="12" md="5">
               <form-select
                  :options="OrganisationList"
                  v-model="filter.organizationId"
                  :label="$t('organization')"
                  @change="Refresh"
               />
            </b-col>
            <b-col sm="12" md="2">
               <form-picker
                  type="year"
                  format="YYYY"
                  @change="Refresh"
                  v-model="filter.financeYear"
                  :label="$t('docyear')"
               />
            </b-col>
            <b-col cols="12" md="4" sm="12" class="d-flex justify-content-end">
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
         <b-row>
            <b-col cols="12" md="5">
               <StatusSelect v-model="statusId" :filter="[16, 25, 23]" />
            </b-col>
            <!-- <b-col cols="12" md="3" class="d-flex align-items-center justify-content-between mb-1">
               <b-button variant="primary" :to="{ name: 'EditAppealApplication', params: { id: 0 } }">
                  <feather-icon icon="PlusIcon"></feather-icon>
                  {{ $t('create') }}
               </b-button>
            </b-col> -->
         </b-row>
      </template>

      <template #cell(actions)="{ item }">
         <div class="text-right" style="text-wrap: nowrap">
            <!-- edit -->
            <b-link
               v-if="$can('StaffingView', 'permissions')"
               :to="{ name: 'EditStaffing', params: { id: item.id, isView: true } }"
               v-b-tooltip.hover.top="$t('View')"
               class="mr-1 cursor-pointer"
            >
               <feather-icon icon="EyeIcon"></feather-icon>
            </b-link>
            <!-- Receieved -->
            <b-link
               v-if="$can('StaffingReceieved', 'permissions') && item.canReceived"
               @click="Receieved(item)"
               class="mr-1 text-success cursor-pointer"
               v-b-tooltip.hover.top="$t('Receieved')"
            >
               <feather-icon icon="CheckCircleIcon"></feather-icon>
            </b-link>
            <!-- reject -->
            <b-link
               v-if="$can('StaffingReject', 'permissions') && item.canReject"
               @click="Reject(item)"
               class="mr-1 text-danger cursor-pointer"
               v-b-tooltip.hover.top="$t('Reject')"
            >
               <feather-icon icon="XCircleIcon"></feather-icon>
            </b-link>
         </div>
      </template>
   </form-table-hrm>
</template>

<script>
import {
   BInputGroup,
   BCard,
   BCardText,
   VBTooltip,
   BBadge,
   BButton,
   BLink,
   BModal,
   VBModal,
   BInputGroupAppend,
   BRow,
   BCol,
   BFormInput
} from 'bootstrap-vue';
import FormTableHrm from '@/components/forms/form-table-hrm.vue';
import StaffingService from '@/services/hrm/staffing.service';
import OrganizationService from '@/services/managment/organization.service';
import ManualService from '@/services/others/manual.service';
import StatusSelect from '@/views/components/document/StatusSelect.vue';

const DefaultFilter = {
   financeYear: null,
   organizationId: null,

   search: '',
   sortBy: '',
   orderType: 'asc',
   page: 1,
   pageSize: 20,
   perPageOptions: [10, 20, 50, 100],
   total: 0,
   statusIds: []
};

export default {
   components: {
      StatusSelect,
      BInputGroup,
      BInputGroupAppend,
      BCard,
      BFormInput,
      BCol,
      BRow,
      FormTableHrm,
      BCardText,
      BButton,
      BBadge,
      BLink,
      BModal
   },
   directives: {
      'b-tooltip': VBTooltip,
      'b-modal': VBModal
   },
   data() {
      return {
         items: [],
         statusId: null,
         OrganisationList: [],
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
               key: 'staffingType',
               label: this.$t('staffingType'),
               thClass: 'text-center',
               tdClass: 'text-center',
               sortable: true
            },
            {
               key: 'financeYear',
               label: this.$t('financeYear'),
               thClass: 'text-center',
               tdClass: 'text-center',
               sortable: true
            },
            {
               key: 'docSum',
               label: this.$t('summary')
            },
            {
               key: 'organization',
               label: this.$t('organization')
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
            financeYear: null,
            organizationId: null,

            search: '',
            sortBy: '',
            orderType: 'asc',
            page: 1,
            pageSize: 20,
            perPageOptions: [10, 20, 50, 100],
            total: 0,
            statusIds: []
         },
         isBusy: false
      };
   },
   watch: {
      filter: {
         handler(newValue, oldValue) {
            localStorage.setItem('filterData4', JSON.stringify(newValue));
            this.filter = newValue;
            if (newValue.districtId) {
               this.GetDistrict();
            }
         },
         deep: true
      },

      statusId(newval) {
         if (newval == null) {
            this.filter.statusIds = [];
            this.Refresh();
         } else {
            this.filter.statusIds = [];
            this.filter.statusIds.push(newval);
            this.Refresh();
         }
      }
   },
   created() {
      if (JSON.parse(localStorage.getItem('filterData4'))) {
         this.filter = JSON.parse(localStorage.getItem('filterData4'));
      } else {
         this.filter = DefaultFilter;
      }

      ManualService.OrganizationAsSelectListByGroup([1, 3]).then((res) => {
         this.OrganisationList = res.data;
      });
   },
   methods: {
      DbClick(item) {
         if (this.$can('StaffingView', 'permissions')) {
            this.$router.push({
               name: 'EditStaffing',
               params: { id: item.id, isView: true }
            });
         }
      },
      Reject(item) {
         this.$swal.fire({
            icon: 'question',
            title: this.$t('WantReject'),
            showLoaderOnConfirm: true,
            input: 'text',
            inputPlaceholder: this.$t('RejectMessage'),
            preConfirm: (msg) => {
               return StaffingService.Reject({
                  id: item.id,
                  message: msg
               })
                  .then(() => {
                     this.makeToast(this.$t('RejectSuccess'), 'success');
                     this.Refresh();
                  })
                  .catch(this.SwalError);
            },
            allowOutsideClick: () => !this.$swal.isLoading()
         });
      },
      Receieved(item) {
         this.$swal.fire({
            icon: 'question',
            title: this.$t('WantReceieved'),
            showLoaderOnConfirm: true,
            preConfirm: () => {
               return StaffingService.Receieved({
                  id: item.id,
                  message: ''
               })
                  .then(() => {
                     this.makeToast(this.$t('ReceievedSuccess'), 'success');
                     this.Refresh();
                  })
                  .catch(this.SwalError);
            },
            allowOutsideClick: () => !this.$swal.isLoading()
         });
      },
      Refresh() {
         this.isBusy = true;
         StaffingService.GetListForHeader(this.filter)
            .then((res) => {
               this.items = res.data.rows;
               this.filter.total = res.data.total;
            })
            .catch((error) => {
               this.makeToast(error.response.data.errors, 'danger');
            })
            .finally(() => {
               this.isBusy = false;
            });
      }
   }
};
</script>
