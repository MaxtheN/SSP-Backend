<template>
   <div>
      <form-table-hrm
         :items="items"
         :fields="fields"
         :filter.sync="filter"
         searchable
         :busy="isBusy"
         :actions="{}"
         :selectable="selectable"
         select-mode="single"
         show-empty
         @request="Refresh"
         @row-dblclicked="(e) => $emit('row-selected', e)"
         @row-selected="(e) => (selectedRows = e)"
      >
         <template #filter>
            <b-row>
               <b-col v-if="isOrganization" cols="12" md="12">
                  <form-select
                     :options="OrganisationList"
                     v-model="filter.organizationId"
                     :label="$t('organization')"
                     @change="Refresh"
                  />
               </b-col>
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
               <b-col cols="12" md="3">
                  <v-select
                     class="mt-2"
                     :options="items"
                     :placeholder="$t(`employee`)"
                     v-bind="$attrs"
                     :reduce="(e) => e.id"
                     label="employee"
                     :filterable="false"
                     @option:selected="onSelected"
                  >
                  </v-select>
               </b-col>
               <b-col cols="12" md="3" v-if="!hideIsOnlyWorkingEmployee">
                  <b-form-checkbox
                     class="mt-2"
                     v-model="filter.isOnlyWorkingEmployee"
                     @input="Refresh"
                     name="check-button"
                     switch
                  >
                     Faqat ishlaydigan xodimlar
                  </b-form-checkbox>
               </b-col>
            </b-row>
         </template>
      </form-table-hrm>
      <b-row v-if="selectable && selectedRows.length > 0">
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

import EmployeeManageService from '@/services/hrm/employeemanage.service';
import DepartmentService from '@/services/info/department.service';
import EmployeeService from '@/services/info/employee.service';
import EmployeeSelect2 from '@/views/components/employee/EmployeeSelect2.vue';
import FormTableHrm from '@/components/forms/form-table-hrm.vue';
import StaffingService from '@/services/hrm/staffing.service';
import ManualService from '@/services/others/manual.service';
import { extend } from 'vee-validate';

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
      EmployeeSelect2,
      FormTableHrm
   },
   emits: ['row-selected'],

   props: {
      isOrganization: {
         type: Boolean,
         default: false
      },

      employeeManageId: {
         type: Number,
         default: null
      },
      employeeId: {
         type: Number,
         default: null
      },
      organizationId: {
         type: Number,
         default: null
      },
      departmentId: {
         type: Number,
         default: null
      },
      hideIsOnlyWorkingEmployee: {
         type: Boolean,
         default: false
      },
      isOnlyWorkingEmployee: {
         type: Boolean,
         default: false
      },
      selectable: {
         type: Boolean,
         default: false
      }
   },
   directives: {
      'b-tooltip': VBTooltip
   },
   data() {
      return {
         selectedRows: [],
         items: [],
         OrganisationList: [],
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
               key: 'docId',
               label: this.$t('docId'),
               thClass: 'text-center',
               tdClass: 'text-center',
               sortable: true
            },
            {
               key: 'empAppointOrderType',
               label: this.$t('empAppointOrderType'),
               thClass: 'text-center',
               tdClass: 'text-center'
            },
            {
               key: 'department',
               label: this.$t('Department'),
               thClass: 'text-center',
               tdClass: 'text-center',
               sortable: true
            },
            {
               key: 'position',
               label: this.$t('position'),
               thClass: 'text-center',
               tdClass: 'text-center'
            },
            {
               key: 'employee',
               label: this.$t('employee'),
               thClass: 'text-center',
               tdClass: 'text-center'
            },
            {
               key: 'employmentType',
               label: this.$t('employmentType'),
               thClass: 'text-center',
               tdClass: 'text-center',
               sortable: true
            },
            {
               key: 'organization',
               label: this.$t('organization'),
               thClass: 'text-center',
               tdClass: 'text-center'
            },
            {
               key: 'workSchedule',
               label: this.$t('workSchedule'),
               thClass: 'text-center',
               tdClass: 'text-center'
            },
            {
               key: 'employmentRate',
               label: this.$t('employeeRate'),
               thClass: 'text-center',
               tdClass: 'text-center'
            },
            {
               key: 'startOn',
               label: this.$t('startdate'),
               thClass: 'text-center',
               tdClass: 'text-center'
            },
            {
               key: 'endOn',
               label: this.$t('enddate'),
               thClass: 'text-center',
               tdClass: 'text-center'
            }
         ],
         filter: {
            search: '',
            sortBy: '',
            orderType: 'asc',
            page: 1,
            pageSize: 20,
            perPageOptions: [10, 20, 50, 100],
            total: 0,
            organizationId: 0,
            employeeId: null,
            departmentId: this.departmentId,
            positionId: null,
            isOnlyWorkingEmployee: true
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
      this.Refresh();
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
      ManualService.OrganizationAsSelectListByGroup([1, 3]).then((res) => {
         this.OrganisationList = res.data;
      });
   },
   watch: {
      employeeId: {
         handler(e) {
            if (e) {
               this.filter.employeeId = e;
            }
         },
         immediate: true
      },
      organizationId: {
         handler(e) {
            if (e) {
               this.filter.organizationId = e;
            }
         },
         immediate: true
      },
      isOnlyWorkingEmployee: {
         handler(e) {
            this.filter.isOnlyWorkingEmployee = e || true;
         },
         immediate: true
      },
      // eslint-disable-next-line func-names
      'filter.organizationId': function (newVal) {
         this.filter.departmentId = null;
         this.GetDepartment(newVal);
      },
      // eslint-disable-next-line func-names
      'filter.departmentId': function (newVal) {
         this.filter.positionId = null;
         StaffingService.GetStaffingPositionClassification(
            null,
            null,
            newVal,
            this.filter.organizationId || this.organizationId
         ).then((res) => {
            this.PositionList = res.data;
         });
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

         EmployeeManageService.GetList(this.filter)
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
      },
      GetDepartment(id) {
         if (id) {
            DepartmentService.GetAsSelectList(id, {})
               .then((res) => {
                  if (Array.isArray(res.data)) {
                     this.DepartmentList = res.data;
                  }
               })
               .catch((error) => {
                  this.makeToast(error.response.data.errors, 'danger');
               });
         }
      },
      onSelected(e) {
         this.filter.employeeId = e.employeeId;
         this.Refresh();
      }
   }
};
</script>

<style>
.table.b-table > tbody .b-table-row-selected.table-active td {
   background-color: #8effc3 !important;
}
</style>
