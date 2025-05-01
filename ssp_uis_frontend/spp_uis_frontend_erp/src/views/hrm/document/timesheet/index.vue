<template>
   <form-table-hrm
      :items="items"
      :fields="fields"
      :filter.sync="filter"
      searchable
      :busy="isBusy"
      :actions="{
         create: {
            name: 'EditTimesheet',
            permission: 'TimesheetCreate'
         },
         edit: {
            name: 'EditTimesheet',
            permission: 'TimesheetEdit'
         },
         delete: {
            name: 'EditTimesheet',
            permission: 'TimesheetDelete'
         }
      }"
      @row-dblclicked="DbClick"
      @request="Refresh"
   >
      <template #filter>
         <b-row>
            <b-col cols="2">
               <b-button
                  class="mt-2"
                  variant="primary"
                  @click="$router.push({ name: 'EditTimesheet', params: { id: 0 } })"
               >
                  <feather-icon icon="PlusIcon"></feather-icon>
                  {{ $t('create') }}</b-button
               >
            </b-col>
            <b-col sm="12" md="6"></b-col>
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
            <b-col md="12" class="mb-1"></b-col>
            <!-- <b-col cols="12" md="6" class="mt-2">
               <status-select v-model="filter.statusId" @input="Refresh" :filter="[8, 30, 2, 23, 25]" />
            </b-col> -->
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
            <b-col sm="12" md="3">
               <form-select
                  :options="TimeSheetTypeList"
                  @change="Refresh"
                  v-model="filter.timesheetTypeId"
                  label="timesheetType"
               />
            </b-col>
            <b-col sm="12" md="3">
               <label for>{{ $t('month') }}</label>
               <date-picker
                  v-model="filter.monthOn"
                  size="sm"
                  class="w-100"
                  lang="ru"
                  :placeholder="$t('MM.YYYY')"
                  value-type="DD.MM.YYYY"
                  type="month"
                  format="MM.YYYY"
                  @change="Refresh"
               />
            </b-col>
         </b-row>
      </template>

      <template #cell(actions)="{ item }">
         <div class="text-center" style="text-wrap: nowrap">
            <!-- edit -->
            <b-link
               v-if="$can('TimesheetEdit', 'permissions') && item.canEdit"
               :to="{ name: 'EditTimesheet', params: { id: item.id } }"
               v-b-tooltip.hover.top="$t('Edit')"
               class="mr-1 cursor-pointer"
            >
               <feather-icon icon="EditIcon"></feather-icon>
            </b-link>
            <!-- delete -->
            <b-link
               v-if="$can('TimesheetDelete', 'permissions') && item.canDelete"
               class="text-danger mr-1 cursor-pointer"
               @click="Delete(item)"
               v-b-tooltip.hover.top="$t('Delete')"
            >
               <feather-icon icon="TrashIcon"></feather-icon>
            </b-link>
            <!-- accept -->
            <b-link
               v-if="$can('TimesheetAccept', 'permissions') && item.canAccept"
               @click="Accept(item)"
               class="mr-1 text-success cursor-pointer"
               v-b-tooltip.hover.top="$t('Approve')"
            >
               <feather-icon icon="CheckCircleIcon"></feather-icon>
            </b-link>
            <!-- cancel -->
            <template v-if="$can('TimesheetCancel', 'permissions') && item.canCancel">
               <b-link
                  class="text-danger cursor-pointer mr-1"
                  v-b-tooltip.hover.top="$t('Cancel')"
                  @click="Cancel(item)"
               >
                  <feather-icon icon="XCircleIcon"></feather-icon>
               </b-link>
            </template>
         </div>
      </template>
   </form-table-hrm>
</template>

<script>
import {
   BCard,
   BLink,
   VBTooltip,
   BInputGroup,
   BRow,
   BCol,
   BFormInput,
   BInputGroupAppend,
   BButton
} from 'bootstrap-vue';
import StatusSelect from '@/views/components/document/StatusSelect.vue';
import ManualService from '@/services/others/manual.service';
import FormTableHrm from '@/components/forms/form-table-hrm.vue';
import TimesheetService from '@/services/hrm/timesheet.service';

export default {
   components: {
      StatusSelect,
      BInputGroup,
      BFormInput,
      BInputGroupAppend,
      BButton,
      BRow,
      BCol,
      BLink,
      BCard,
      FormTableHrm
   },
   directives: {
      'b-tooltip': VBTooltip
   },
   data() {
      return {
         items: [],
         MonthList: [],
         TimeSheetTypeList: [],
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
               tdClass: 'text-center'
            },
            {
               key: 'department',
               label: this.$t('Department'),
               thClass: 'text-center',
               tdClass: 'text-center'
            },
            {
               key: 'timesheetType',
               label: this.$t('timesheetType'),
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
               tdClass: 'text-center'
            }
         ],
         filter: {
            search: '',
            sortBy: '',
            monthOn: '',
            orderType: 'asc',
            page: 1,
            pageSize: 20,
            perPageOptions: [10, 20, 50, 100],
            timesheetTypeId: null,
            startOn: '',
            endOn: '',
            total: 0
         },
         isBusy: false
      };
   },
   created() {
      ManualService.TimesheetTypeSelectList().then((res) => {
         this.TimeSheetTypeList = res.data;
      });
      ManualService.GetMonthSelectList().then((res) => {
         this.MonthList = res.data;
      });
   },
   methods: {
      DbClick(item) {
         this.$router.push({
            name: 'EditTimesheet',
            params: { id: item.id }
         });
      },
      Delete(item) {
         this.$swal.fire({
            icon: 'info',
            title: this.$t('WantDelete'),
            showLoaderOnConfirm: true,
            preConfirm: () => {
               return TimesheetService.Delete(item.id)
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
               return TimesheetService.Accept({
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
               return TimesheetService.Cancel({
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
         TimesheetService.GetList({
            ...this.filter,
            monthOn: this.filter.monthOn + ''
         })
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
