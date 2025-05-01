<template>
   <form-select
      :options="EmployeeListComp"
      v-model="valueComp"
      valueid="id"
      valuename="employee"
      label="employeeManage"
      :loading="isBusy"
      :selectable="(option) => !disabledEmployeManageIds.includes(option.id)"
      v-bind="$attrs"
      v-on="$listeners"
      @option:selected="rowSelect"
   ></form-select>
</template>
<script>
import EmployeeManageService from '@/services/hrm/employeemanage.service';

export default {
   props: {
      value: {
         type: Number,
         default: null
      },
      employeeId: {
         type: Number,
         default: null
      },
      directorId: {
         type: Number,
         default: null
      },
      hrId: {
         type: Number,
         default: null
      },
      positionId: {
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
      isEmployee: {
         type: Boolean,
         default: null
      },
      allSigner: {
         type: Array,
         default: () => []
      },
      disabledEmployeManageIds: {
         type: Array,
         default: () => []
      }
   },
   emits: ['update:data', 'input'],
   data() {
      return {
         EmployeeList: [],
         isBusy: false,
         filter: {
            search: '',
            sortBy: '',
            orderType: 'asc',
            page: 1,
            pageSize: 500,
            perPageOptions: [10, 20, 50, 100],
            organizationId: 0,
            employeeId: null,
            departmentId: null,
            positionId: null,
            isOnlyWorkingEmployee: true
         }
      };
   },
   computed: {
      valueComp: {
         get() {
            return this.value;
         },
         set(value) {
            this.$emit('input', value);
         }
      },
      EmployeeListComp() {
         let list = [...this.EmployeeList];
         if (this.isEmployee) {
            if (this.directorId) {
               list = list.filter((x) => x.id != this.directorId);
            } else if (this.hrId) {
               list = list.filter((x) => x.id != this.hrId);
            }
         }

         return list;
      }
   },
   methods: {
      Refresh() {
         this.isBusy = true;
         this.EmployeeLis = [];
         EmployeeManageService.GetList(this.filter)
            .then((res) => {
               this.EmployeeList = res.data.rows;
            })
            .finally(() => {
               this.isBusy = false;
            });
      },
      rowSelect(e) {
         this.$emit('update:data', e);
      }
   },
   watch: {
      filter: {
         handler(e) {
            this.Refresh();
         },
         immediate: true,
         deep: true
      },
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
      departmentId: {
         handler(e) {
            this.filter.departmentId = e;
         },
         immediate: true
      },
      positionId: {
         handler(e) {
            this.filter.positionId = e;
         },
         immediate: true
      }
   }
};
</script>
