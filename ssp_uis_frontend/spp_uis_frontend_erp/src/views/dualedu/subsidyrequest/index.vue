<template>
   <form-table-hrm
      :items="items"
      :fields="fields"
      :filter.sync="filter"
      searchable
      :busy="isBusy"
      :actions="{
         delete: {
            name: 'EditSubsidyRequest',
            permission: 'SubsidyRequestDelete'
         }
      }"
      @row-dblclicked="DbClick"
      @request="Refresh"
   >
      <template #filter>
         <b-row align-h="between">
            <b-col sm="12" md="2">
               <div>
                  <form-select
                     :options="RegionList"
                     v-model="filter.regionId"
                     @input="ChangeRegion"
                     :label="$t('Oblast')"
                  ></form-select>
                  <!-- :disabled="!$can('ApplicationViewByRegion', 'permissions')" -->
               </div>
            </b-col>
            <b-col sm="12" md="2">
               <div>
                  <label for>{{ $t('Region') }}</label>
                  <v-select
                     :options="DistrictList"
                     :reduce="(item) => item.value"
                     :placeholder="$t('ChooseBelow')"
                     label="text"
                     v-model="filter.districtId"
                     @input="ChangeDistrict"
                     class="w-100"
                  ></v-select>
               </div>
            </b-col>
            <b-col sm="12" md="2">
               <form-picker v-model="filter.fromDocDate" @change="Refresh" :label="$t('startdate')" />
            </b-col>
            <b-col sm="12" md="2">
               <form-picker v-model="filter.toDocDate" @change="Refresh" :label="$t('enddate')" />
            </b-col>

            <b-col sm="12" md="2">
               <div>
                  <label for>{{ $t('inn') }}</label>
                  <b-form-input v-model="filter.contractorInn" @input="Refresh" :placeholder="$t('inn')"></b-form-input>
               </div>
            </b-col>

            <b-col sm="12" md="2">
               <form-picker type="year" format="YYYY" @change="Refresh" v-model="filter.year" :label="$t('docyear')" />
            </b-col>
            <b-col sm="12" md="2">
               <form-select
                  :options="MonthList"
                  v-model="filter.monthOn"
                  @change="Refresh"
                  requitred-star
                  label="month"
               />
            </b-col>
            <b-col cols="12" lg="3" class="text-right mt-2">
               <b-input-group class="text-right">
                  <b-form-input v-model="filter.search" :placeholder="$t('search')" />
                  <b-input-group-append>
                     <b-button @click="Refresh" variant="primary">
                        <feather-icon icon="SearchIcon" />
                     </b-button>
                  </b-input-group-append>
               </b-input-group>
            </b-col>
         </b-row>
         <b-row align-h="between" align-v="center">
            <!-- <b-col cols="12" md="7" class="mt-2">
               <status-select v-model="filter.statusId" @input="Refresh" :filter="[8, 30, 2, 23, 25]" />
            </b-col> -->
            <!-- <b-col cols="12" lg="3" class="text-right mt-2">
               <b-input-group class="text-right">
                  <b-form-input v-model="filter.search" :placeholder="$t('search')" />
                  <b-input-group-append>
                     <b-button @click="Refresh" variant="primary">
                        <feather-icon icon="SearchIcon" />
                     </b-button>
                  </b-input-group-append>
               </b-input-group>
            </b-col> -->
         </b-row>
      </template>

      <template #cell(actions)="{ item }">
         <div class="text-center" style="text-wrap: nowrap">
            <!-- view -->
            <b-link
               v-if="$can('SubsidyRequestView', 'permissions')"
               :to="{ name: 'ViewSubsidyRequest', params: { id: item.id } }"
               v-b-tooltip.hover.top="$t('Edit')"
               class="mr-1 cursor-pointer"
            >
               <feather-icon icon="EyeIcon"></feather-icon>
            </b-link>
            <!-- delete -->
            <b-link
               v-if="$can('SubsidyRequestDelete', 'permissions') && item.canDelete"
               class="text-danger mr-1 cursor-pointer"
               @click="Delete(item)"
               v-b-tooltip.hover.top="$t('Delete')"
            >
               <feather-icon icon="TrashIcon"></feather-icon>
            </b-link>
            <!-- accept -->
            <b-link
               v-if="$can('SubsidyRequestAccept', 'permissions') && item.canAccept"
               @click="Accept(item)"
               class="mr-1 text-success cursor-pointer"
               v-b-tooltip.hover.top="$t('Approve')"
            >
               <feather-icon icon="CheckCircleIcon"></feather-icon>
            </b-link>
            <!-- cancel -->
            <template v-if="$can('SubsidyRequestCancel', 'permissions') && item.canCancel">
               <b-link
                  class="text-warning cursor-pointer mr-1"
                  v-b-tooltip.hover.top="$t('Cancel')"
                  @click="Cancel(item)"
               >
                  <feather-icon icon="XCircleIcon"></feather-icon>
               </b-link>
            </template>
            <!-- Reject -->
            <template v-if="$can('SubsidyRequestReject', 'permissions') && item.canReject">
               <b-link
                  class="text-danger cursor-pointer mr-1"
                  v-b-tooltip.hover.top="$t('Reject')"
                  @click="Reject(item)"
               >
                  <feather-icon icon="XSquareIcon"></feather-icon>
               </b-link>
            </template>
         </div>
      </template>
      <template #cell(month)="{ item }">{{ $t('month' + item.month) }}</template>
   </form-table-hrm>
</template>

<script>
import {
   BLink,
   VBTooltip,
   BButton,
   BPagination,
   BTable,
   BCol,
   BRow,
   BSpinner,
   BCard,
   BTooltip,
   BBadge,
   BInputGroup,
   BFormInput,
   BInputGroupAppend,
   BModal,
   BCardText,
   BTableSimple,
   BThead,
   BTbody,
   BTr,
   BTd,
   BTh,
   BButtonGroup,
   BFormCheckbox,
   BBreadcrumb,
   BBreadcrumbItem,
   BOverlay
} from 'bootstrap-vue';

import RegionService from '@/services/info/region.service';
import DistrictService from '@/services/info/district.service';
import FormTableHrm from '@/components/forms/form-table-hrm.vue';
import SubsidyRequestService from '@/services/dualedu/subsidyrequest.service';
import StatusSelect from '@/views/components/document/StatusSelect.vue';
import ManualService from '@/services/others/manual.service';
export default {
   components: {
      FormTableHrm,
      BButton,
      BPagination,
      BTable,
      BCol,
      BRow,
      BSpinner,
      BCard,
      BTooltip,
      BBadge,
      BInputGroup,
      BFormInput,
      BInputGroupAppend,
      BLink,
      BModal,
      BCardText,
      BTableSimple,
      BThead,
      BTbody,
      BTr,
      BTd,
      BTh,
      BButtonGroup,
      BFormCheckbox,
      BBreadcrumb,
      BBreadcrumbItem,
      BOverlay,
      StatusSelect
   },
   directives: {
      'b-tooltip': VBTooltip
   },

   created() {
      RegionService.GetAsSelectList(211)
         .then((res) => {
            this.RegionList = res.data;
         })
         .catch((error) => {
            this.showApiError(error);
         });
   },
   data() {
      return {
         MonthList: [],
         items: [],
         RegionList: [],
         DistrictList: [],
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
               tdClass: 'text-center text-nowrap',
               sortable: true
            },
            {
               key: 'docOn',
               label: this.$t('docOn'),
               thClass: 'text-center',
               tdClass: 'text-center'
            },
            {
               key: 'contractorInnPinfl',
               label: this.$t('companyInn')
            },
            {
               key: 'contractor',
               label: this.$t('companyName'),
               thStyle: {
                  minWidth: '400px'
               }
            },
            {
               key: 'region',
               label: this.$t('region')
            },
            {
               key: 'district',
               label: this.$t('District')
            },
            {
               key: 'address',
               label: this.$t('address'),
               thStyle: {
                  minWidth: '300px'
               }
            },
            {
               key: 'phone',
               label: this.$t('phoneNumber'),
               tdClass: 'text-nowrap'
            },
            {
               key: 'year',
               label: this.$t('docyear')
            },
            {
               key: 'month',
               label: this.$t('month')
            },
            {
               key: 'totalSubsidyAmount',
               label: this.$t('totalSubsidyAmount'),
               thClass: 'text-center',
               tdClass: 'text-center'
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
               tdClass: 'r-0',
               stickyColumn: true,
               thStyle: {
                  right: '0'
               }
            }
         ],
         filter: {
            search: '',
            year: '',
            statusId: null,
            monthOn: '',
            sortBy: '',
            orderType: 'asc',
            page: 1,
            pageSize: 20,
            perPageOptions: [10, 20, 50, 100],
            total: 0,
            regionId: null,
            districtId: null,
            contractorInn: '',
            fromDocDate: '',
            toDocDate: '',
            phoneNumber: ''
         },
         isBusy: false
      };
   },

   created() {
      ManualService.GetMonthSelectList().then((res) => {
         this.MonthList = res.data;
      });
   },
   methods: {
      DbClick(item) {
         this.$router.push({
            name: 'EditSubsidyRequest',
            params: { id: item.id }
         });
      },
      Delete(item) {
         this.$swal.fire({
            icon: 'info',
            title: this.$t('WantDelete'),
            showLoaderOnConfirm: true,
            preConfirm: () => {
               return SubsidyRequestService.Delete(item.id)
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
               return SubsidyRequestService.Accept({
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
      Reject(item) {
         this.$swal.fire({
            icon: 'info',
            title: this.$t('WantReject'),
            showLoaderOnConfirm: true,
            preConfirm: () => {
               return SubsidyRequestService.Reject({
                  id: item.id,
                  message: ''
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
      Cancel(item) {
         this.$swal.fire({
            icon: 'question',
            title: this.$t('WantCancel'),
            showLoaderOnConfirm: true,
            preConfirm: () => {
               return SubsidyRequestService.Cancel({
                  statusId: item.statusId,
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
      Refresh() {
         this.isBusy = true;
         SubsidyRequestService.GetList(this.filter)
            .then((res) => {
               this.items = res.data.rows;
               this.filter.total = res.data.total;
            })
            .catch((error) => {
               this.showApiError(error);
            })
            .finally(() => {
               this.isBusy = false;
            });
      },
      ChangeRegion() {
         if (this.filter.regionId) {
            this.filter.districtId = null;
            this.GetDistrict();
         }
         this.Refresh();
      },
      GetDistrict() {
         if (this.filter.regionId) {
            DistrictService.GetAsSelectList(this.filter.regionId).then((res) => {
               this.DistrictList = res.data;
            });
         } else {
            this.filter.districtId = null;
            this.DistrictList = [];
            this.Refresh();
         }
      },
      ChangeDistrict() {
         this.Refresh();
      }
   }
};
</script>
<style>
.r-0 {
   right: 0;
}
</style>
