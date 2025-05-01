<template>
   <form-table-hrm
      :items="items"
      :fields="fields"
      :filter.sync="filter"
      searchable
      :busy="isBusy"
      :actions="{
         create: {
            name: 'EditEmployeeSickLeave',
            permission: 'EmployeeSickLeaveCreate'
         },
         edit: {
            name: 'EditEmployeeSickLeave',
            permission: 'EmployeeSickLeaveEdit'
         },
         delete: {
            name: 'EditEmployeeSickLeave',
            permission: 'EmployeeSickLeaveDelete'
         }
      }"
      @row-dblclicked="DbClick"
      @request="Refresh"
      @row-delete="Delete"
   >
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
      <template #cell(actions)="{ item }">
         <div class="text-center" style="text-wrap: nowrap">
            <!-- edit -->
            <b-link
               v-if="$can('EditEmployeeSickLeave', 'permissions') && item.canEdit"
               :to="{ name: 'EditEmployeeSickLeave', params: { id: item.id } }"
               v-b-tooltip.hover.top="$t('Edit')"
               class="mr-1 cursor-pointer"
            >
               <feather-icon icon="EditIcon"></feather-icon>
            </b-link>
            <!-- delete -->
            <b-link
               v-if="$can('EmployeeSickLeaveDelete', 'permissions') && item.canDelete"
               class="text-danger mr-1 cursor-pointer"
               @click="Delete(item)"
               v-b-tooltip.hover.top="$t('Delete')"
            >
               <feather-icon icon="TrashIcon"></feather-icon>
            </b-link>
            <!-- accept -->
            <b-link
               v-if="$can('EmployeeSickLeaveAccept', 'permissions') && item.canAccept"
               @click="Accept(item)"
               class="mr-1 text-success cursor-pointer"
               v-b-tooltip.hover.top="$t('Approve')"
            >
               <feather-icon icon="CheckCircleIcon"></feather-icon>
            </b-link>
            <!-- cancel -->
            <template v-if="$can('EmployeeSickLeaveCancel', 'permissions') && item.canCancel">
               <b-link
                  class="text-danger cursor-pointer mr-1"
                  v-b-tooltip.hover.top="$t('Cancel')"
                  @click="Cancel(item)"
               >
                  <feather-icon icon="XCircleIcon"></feather-icon>
               </b-link>
            </template>
         </div> </template
   ></form-table-hrm>
</template>

<script>
import { BCard, BLink, VBTooltip, BRow, BCol } from 'bootstrap-vue';
import FormTableHrm from '@/components/forms/form-table-hrm.vue';
import EmployeeSickLeaveService from '@/services/hrm/employeesickleave.service';
import StatusSelect from '@/views/components/document/StatusSelect.vue';

const DefaultFilter = {
   search: '',
   sortBy: '',
   orderType: 'asc',
   page: 1,
   statusId: null,
   statusIds: [],
   pageSize: 20,
   perPageOptions: [10, 20, 50, 100],
   total: 0
};

export default {
   components: {
      BCard,
      BRow,
      BCol,
      StatusSelect,
      BLink,
      FormTableHrm
   },
   directives: {
      'b-tooltip': VBTooltip
   },
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
               label: this.$t('docOn'),
               thClass: 'text-center',
               tdClass: 'text-center'
            },
            {
               key: 'organization',
               label: this.$t('organization'),
               thClass: 'text-center',
               tdClass: 'text-center'
            },
            {
               key: 'employeeSickLeaveType',
               label: this.$t('employeeSickLeaveType'),
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
            orderType: 'asc',
            page: 1,
            statusId: null,
            statusIds: [],
            pageSize: 20,
            perPageOptions: [10, 20, 50, 100],
            total: 0
         },
         isBusy: false
      };
   },
   computed() {
      if (JSON.parse(localStorage.getItem('filterData11'))) {
         this.filter = JSON.parse(localStorage.getItem('filterData11'));
      } else {
         this.filter = DefaultFilter;
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
      DbClick(item) {
         this.$router.push({
            name: 'EditEmployeeSickLeave',
            params: { id: item.id }
         });
      },
      Delete(item) {
         this.$swal.fire({
            icon: 'info',
            title: this.$t('WantDelete'),
            showLoaderOnConfirm: true,
            preConfirm: () => {
               return EmployeeSickLeaveService.Delete(item.id)
                  .then(() => {
                     this.makeToast(this.$t('DeleteSuccess'), 'success');
                     this.Refresh();
                  })
                  .catch(this.SwalError);
            },
            allowOutsideClick: () => !this.$swal.isLoading()
         });
      },
      Refresh() {
         this.isBusy = true;
         EmployeeSickLeaveService.GetList(this.filter)
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
               return EmployeeSickLeaveService.Accept({
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
               return EmployeeSickLeaveService.Cancel({
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
      }
   },
   watch: {
      filter: {
         handler(newValue, oldValue) {
            localStorage.setItem('filterData11', JSON.stringify(newValue));
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

<style lang="scss">
.v-ellipsis {
   .vs__selected-options {
      flex-wrap: nowrap;
      max-width: calc(100% - 40px);
      /* change this to `- 40px` if you're supporting a `clearable` field; I was not */
   }

   .vs__selected {
      display: block;
      white-space: nowrap;
      text-overflow: ellipsis;
      max-width: 100%;
      overflow: hidden;
   }
}
</style>
