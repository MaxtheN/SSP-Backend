<template>
   <form-table-hrm
      :items="items"
      :fields="fields"
      :filter.sync="filter"
      searchable
      :busy="isBusy"
      :actions="{
         create: {
            name: 'EditAppealApplication',
            permission: 'AppealApplicationCreate'
         },
         edit: {
            name: 'EditAppealApplication',
            permission: 'AppealApplicationEdit'
         },
         delete: {
            name: 'EditAppealApplication',
            permission: 'AppealApplicationDelete'
         }
      }"
      @request="Refresh"
   >
      <!-- filtes -->
      <template #filter>
         <b-row>
            <b-col cols="12" md="3">
               <div>
                  <label for>{{ $t('innOrPinfl') }}</label>
                  <b-input-group>
                     <b-form-input
                        type="text"
                        v-model="filter.contractorInn"
                        debounce="300"
                        v-mask="['##############']"
                        @keyup.enter="Refresh"
                        :placeholder="$t('innOrPinfl')"
                     />
                     <b-input-group-append>
                        <b-button @click="Refresh" size="sm" variant="primary">
                           <feather-icon icon="SearchIcon" />
                        </b-button>
                     </b-input-group-append>
                  </b-input-group>
               </div>
            </b-col>
            <b-col cols="12" md="3">
               <form-select
                  :options="RegionList"
                  :reduce="(item) => item.value"
                  label="region"
                  v-model="filter.regionId"
                  @input="ChangeRegion"
               />
            </b-col>
            <b-col cols="12" md="3">
               <form-select
                  :options="DistrictList"
                  :reduce="(item) => item.value"
                  label="Region"
                  v-model="filter.districtId"
                  @input="ChangeDistrict"
               />
            </b-col>
            <b-col cols="12" md="3" class="d-flex align-items-center justify-content-between">
               <b-button @click="Print" class="ml-3" :disabled="printLoding" variant="primary">
                  <feather-icon icon="PrinterIcon"></feather-icon>
                  {{ $t('Print') }}
               </b-button>

               <template v-if="$can('AppealApplicationCreate', 'permissions')">
                  <b-button variant="primary" :to="{ name: 'EditAppealApplication', params: { id: 0 } }">
                     <feather-icon icon="PlusIcon"></feather-icon>
                     {{ $t('create') }}
                  </b-button>
               </template>
            </b-col>
            <b-col cols="12" md="2">
               <div>
                  <label for>{{ $t('startdate') }}</label>
                  <form-picker v-model="filter.fromDocDate" :placeholder="$t('startdate')" @input="Refresh" />
               </div>
            </b-col>
            <b-col cols="12" md="2">
               <div>
                  <label for>{{ $t('enddate') }}</label>
                  <form-picker v-model="filter.toDocDate" :placeholder="$t('enddate')" @input="Refresh" />
               </div>
            </b-col>
            <b-col sm="12" md="2">
               <form-select
                  :options="AppealFormatTypeSelectList"
                  v-model="filter.appealFormatTypeId"
                  :label="$t('appealFormatType')"
                  rules="required"
                  @input="Refresh"
               ></form-select>
            </b-col>
            <b-col sm="12" md="2">
               <form-select
                  :options="AppealTypeSelectList"
                  v-model="filter.appealTypeId"
                  :label="$t('appealType')"
                  rules="required"
                  @input="Refresh"
               ></form-select>
            </b-col>
            <b-col sm="12" md="3">
               <form-select
                  :options="appealTypeArriveList"
                  v-model="filter.appealTypeArriveId"
                  :label="$t('AppealTypeArrive')"
                  rules="required"
                  @input="Refresh"
               ></form-select>
            </b-col>
         </b-row>
         <b-row align-v="center">
            <b-col cols="12" md="12">
               <StatusSelect v-model="filter.statusId" @input="Refresh" :filter="[1, 8, 37, 39, 38, 24]" />
            </b-col>
            <b-col cols="4" class="d-flex no-wrap" v-if="!isMobileDevice()">
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
         <div class="text-center" style="text-wrap: nowrap">
            <!-- view -->
            <b-link
               v-if="$can('AppealApplicationView', 'permissions')"
               :to="{
                  name: 'EditAppealApplication',
                  params: { id: item.id },
                  query: { isView: true, inn: item.contractorInn }
               }"
               v-b-tooltip.hover.top="$t('Edit')"
               class="mr-1 cursor-pointer"
            >
               <feather-icon icon="EyeIcon"></feather-icon>
            </b-link>
            <!-- edit -->
            <b-link
               v-if="$can('AppealApplicationEdit', 'permissions') && item.canEdit"
               :to="{ name: 'EditAppealApplication', params: { id: item.id } }"
               v-b-tooltip.hover.top="$t('Edit')"
               class="mr-1 cursor-pointer"
            >
               <feather-icon icon="EditIcon"></feather-icon>
            </b-link>

            <!-- delete -->
            <b-link
               v-if="$can('AppealApplicationDelete', 'permissions') && item.canDelete"
               class="text-danger mr-1 cursor-pointer"
               @click="Delete(item)"
               v-b-tooltip.hover.top="$t('Delete')"
            >
               <feather-icon icon="TrashIcon"></feather-icon>
            </b-link>
            <!-- accept -->
            <b-link
               v-if="$can('AppealApplicationAccept', 'permissions') && item.canAccept"
               @click="Accept(item)"
               class="mr-1 text-success cursor-pointer"
               v-b-tooltip.hover.top="$t('Approve')"
            >
               <feather-icon icon="CheckCircleIcon"></feather-icon>
            </b-link>
            <!-- Reject -->
            <template v-if="$can('AppealApplicationReject', 'permissions') && item.canReject">
               <b-link
                  class="text-danger cursor-pointer mr-1"
                  v-b-tooltip.hover.top="$t('Reject')"
                  @click="Reject(item)"
               >
                  <feather-icon icon="XSquareIcon"></feather-icon>
               </b-link>
            </template>
            <!-- document history -->
            <HistoryModalButton :id="item.id" :table-id="item.tableId || 138" />
         </div>
      </template>

      <template #cell(personName)="{ item }"> {{ item.personName ? item.personName : item.personFullName }} </template>
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
   VBTooltip,
   BFormInput,
   BInputGroup,
   BInputGroupAppend,
   BRow,
   BCol,
   BButtonGroup,
   BButton
} from 'bootstrap-vue';
import FormTableHrm from '@/components/forms/form-table-hrm.vue';

import AppealApplicationService from '@/services/appeal/AppealApplication.service';
import AppealDescriptionService from '@/services/appeal/AppealDescription.service';
import AppealTypeArriveService from '@/services/appeal/AppealTypeArrive.service';
import RegionService from '@/services/info/region.service';
import ManualService from '@/services/others/manual.service';
import DistrictService from '@/services/info/district.service';
import StatusSelect from '@/views/components/document/StatusSelect.vue';

import HistoryModalButton from '@/views/components/document/HistoryModalButton.vue';
export default {
   components: {
      BCard,
      HistoryModalButton,
      BBadge,
      BLink,
      BFormInput,
      BInputGroup,
      BInputGroupAppend,
      BRow,
      BCol,
      BButton,
      BButtonGroup,
      FormTableHrm,
      StatusSelect
   },
   directives: {
      'b-tooltip': VBTooltip
   },
   data() {
      return {
         printLoding: false,
         items: [],
         RegionList: [],
         DistrictList: [],
         appealTypeArriveList: [],
         AppealFormatTypeSelectList: [],
         AppealTypeSelectList: [],
         AppealDescriptionSelectList: [],
         fields: [
            {
               key: 'actions',
               label: this.$t('actions'),
               thClass: 'text-center',
               tdClass: 'text-center',
               stickyColumn: true
            },
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
               key: 'personName',
               label: this.$t('fullName')
            },
            {
               key: 'contractorInn',
               label: this.$t('companyInn'),
               thClass: 'text-center',
               tdClass: 'text-center',
               sortable: true
            },
            {
               key: 'contractor',
               label: this.$t('companyName'),
               sortable: true,
               thStyle: {
                  minWidth: '300px'
               }
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
               key: 'appealType',
               label: this.$t('appealType'),
               thClass: 'text-center',
               tdClass: 'text-center',
               sortable: true
            },
            {
               key: 'appealFormatType',
               label: this.$t('appealFormatType'),
               thClass: 'text-center',
               tdClass: 'text-center',
               sortable: true
            },
            {
               key: 'phoneNumber',
               label: this.$t('phone'),
               thClass: 'text-center',
               tdClass: 'text-center',
               sortable: true
            },
            {
               key: 'appealTypeArrive',
               label: this.$t('AppealTypeArrive'),
               thClass: 'text-center',
               tdClass: 'text-center',
               sortable: true,
               thStyle: {
                  minWidth: '300px'
               }
            },
            {
               key: 'status',
               label: this.$t('status'),
               thClass: 'text-center',
               sortable: true,
               stickyColumn: !this.isMobileDevice(),
               thStyle: {
                  right: !this.isMobileDevice() ? '0' : ''
               },
               tdClass: 'r-0'
            }
         ],
         filter: {
            statusId: null,
            regionId: null,
            personFullName: '',
            fromDocOn: '',
            toDocOn: '',
            contractorInn: '',
            districtId: null,
            appealFormatTypeId: null,
            appealTypeId: null,
            appealTypeArriveId: null,
            appealDescriptionId: null,
            search: '',
            sortBy: '',
            orderType: 'asc',
            page: 1,
            pageSize: 20,
            perPageOptions: [10, 20, 50, 100],
            total: 0
         },
         isBusy: false
      };
   },
   created() {
      RegionService.GetAsSelectList(211)
         .then((res) => {
            this.RegionList = res.data;
         })
         .catch(this.showApiError)
         .finally(() => {
            this.saveLoading = false;
         });
      ManualService.AppealTypeSelectList()
         .then((res) => {
            this.AppealTypeSelectList = res.data;
         })
         .catch(this.showApiError)
         .finally(() => {
            this.show = false;
         });
      ManualService.AppealFormatTypeSelectList()
         .then((res) => {
            this.AppealFormatTypeSelectList = res.data;
         })
         .catch(this.showApiError)
         .finally(() => {
            this.show = false;
         });
      AppealTypeArriveService.GetAsSelectList().then((res) => {
         this.appealTypeArriveList = res.data;
      });

      AppealDescriptionService.GetAsSelectList()
         .then((res) => {
            this.AppealDescriptionSelectList = res.data;
         })
         .catch(this.showApiError)
         .finally(() => {
            this.show = false;
         });
   },
   methods: {
      Print() {
         this.printLoding = true;
         AppealApplicationService.PrinAppealApplicationExcel(this.filter).then((res) => {
            this.forceFileDownload(res, this.$t('AppealApplication'));
            this.printLoding = false;
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
      },
      Delete(item) {
         this.$swal.fire({
            icon: 'info',
            title: this.$t('WantDelete'),
            showLoaderOnConfirm: true,
            preConfirm: () => {
               return AppealApplicationService.Delete(item.id)
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
               return AppealApplicationService.Accept({
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
               return AppealApplicationService.Reject({
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
      Refresh() {
         this.isBusy = true;
         AppealApplicationService.GetList(this.filter)
            .then((res) => {
               this.items = res.data.rows;
               this.filter.total = res.data.total;
            })
            .finally(() => {
               this.isBusy = false;
            });
      }
   }
};
</script>
<style>
.r-0 {
   right: 0;
}
</style>
