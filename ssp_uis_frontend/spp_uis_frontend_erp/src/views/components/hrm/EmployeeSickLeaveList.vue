<template>
   <div>
      <form-table-hrm
         :items="items"
         :fields="fields"
         :filter.sync="filter"
         :busy="isBusy"
         :actions="{}"
         select-mode="single"
         show-empty
         @request="Refresh"
         @row-dblclicked="(e) => $emit('row-selected', e)"
         @row-selected="(e) => (selectedRows = e)"
      >
         <template #filter>
            <b-row>
               <b-col cols="12" md="3">
                  <form-select
                     v-model="filter.departmentId"
                     label="Department"
                     @input="Refresh"
                     :options="DepartmentList"
                  ></form-select>
               </b-col>
               <b-col cols="12" md="3">
                  <form-select
                     v-model="filter.positionId"
                     label="position"
                     valueid="positionId"
                     valuename="positionName"
                     @input="Refresh"
                     :options="PositionListFilter(filter.departmentId)"
                  ></form-select>
               </b-col>
            </b-row>
         </template>
      </form-table-hrm>
      <b-row v-if="selectedRows.length > 0">
         <b-col cols="12" class="text-center">
            <b-button
               :disabled="selectedRows.length < 1"
               variant="outline-success"
               @click="(e) => $emit('row-selected', selectedRows[0])"
            >
               <feather-icon icon="PlusIcon"></feather-icon> {{ $t('Add') }}
            </b-button>
         </b-col>
      </b-row>
   </div>
</template>

<script>
import {
   BSpinner,
   BButton,
   BPagination,
   BTable,
   BCol,
   BRow,
   BCard,
   BTooltip,
   BBadge,
   BInputGroup,
   BFormInput,
   BInputGroupAppend,
   VBTooltip,
   BModal,
   BLink,
   BCardText,
   BFormCheckbox
} from 'bootstrap-vue';
import EmployeeSickLeaveService from '@/services/hrm/employeesickleave.service';
import DepartmentService from '@/services/info/department.service';
import EmployeeService from '@/services/info/employee.service';
import StaffingService from '@/services/hrm/staffing.service';
import FormTableHrm from '@/components/forms/form-table-hrm.vue';
export default {
   components: {
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
      BLink,
      BFormCheckbox,
      BCardText,
      FormTableHrm
   },
   emits: ['row-selected'],
   // eslint-disable-next-line vue/require-prop-types
   props: ['employeeid'],
   directives: {
      'b-tooltip': VBTooltip
   },
   data() {
      return {
         selectedRows: [],
         items: [],
         DepartmentList: [],
         EmployeeList: [],
         PositionList: [],
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
               key: 'details',
               label: this.$t('details'),
               thClass: 'text-center',
               tdClass: 'text-center',
               sortable: true
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
               tdClass: 'text-center'
            }
         ],
         filter: {
            search: '',
            sortBy: '',
            orderType: 'asc',
            page: 1,
            employeeSickLeaveTypeId: null,
            pageSize: 20,
            employeeid: null,
            perPageOptions: [10, 20, 50, 100],
            total: 0
         },
         isBusy: false
      };
   },
   computed: {
      firstNumber() {
         return (this.filter.page - 1) * this.filter.pageSize + 1;
      },
      lastNumber() {
         if (this.filter.total < this.filter.pageSize) {
            return this.filter.total;
         } else {
            if (this.filter.page * this.filter.pageSize > this.filter.total) {
               return this.filter.total;
            } else {
               return this.filter.page * this.filter.pageSize;
            }
         }
      },
      PositionListFilter() {
         return (departmentId) => {
            return departmentId ? this.PositionList.filter((e) => e.departmentId == departmentId) : this.PositionList;
         };
      }
   },
   created() {
      this.filter.employeeSickLeaveTypeId = this.$props.employeeid;
      DepartmentService.GetAsSelectList(null, {})
         .then((res) => {
            if (Array.isArray(res.data)) {
               this.DepartmentList = res.data;
            }
         })
         .catch((error) => {
            this.makeToast(error.response.data.errors, 'danger');
         });

      EmployeeService.GetAsSelectList({})
         .then((res) => {
            const { rows } = res.data;
            if (Array.isArray(rows)) {
               this.EmployeeList = rows;
            }
         })
         .catch((error) => {
            this.makeToast(error.response.data.errors, 'danger');
         });

      StaffingService.GetStaffingPositionClassification(null, null, null, this.organizationId).then((res) => {
         this.PositionList = res.data;
      });
      this.Refresh();
   },
   watch: {
      employeeId: {
         handler(e) {
            if (e) {
               console.log(e);
            }
         },
         immediate: true
      }
   },

   methods: {
      SortChange(data) {
         this.filter.sortBy = data.sortBy;
         this.filter.orderType = data.sortDesc ? 'desc' : 'asc';
         this.Refresh();
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
      rowClass(item, type) {
         if (item && type === 'row') {
            if (item.id == this.employeeManageId) {
               return 'b-table-row-selected table-active';
            } else {
               return '';
            }
         } else {
            return null;
         }
      }
   }
};
</script>

<style>
.table.b-table > tbody .b-table-row-selected.table-active td {
   background-color: #8effc3 !important;
}
</style>
