<template>
   <div>
      <validation-observer ref="ValidationDTO1">
         <b-card>
            <b-row>
               <b-col sm="12" md="4">
                  <form-input :label="$t('docnumber')" v-model="Data.docNumber" />
               </b-col>
               <b-col sm="12" md="4">
                  <form-picker v-model="Data.docDate" :label="$t('docOn')" :placeholder="$t('docOn')"></form-picker>
               </b-col>

               <b-col md="4">
                  <b-form-checkbox class="mt-2" v-model="Data.forAllEmployee">
                     {{ $t('Barcha Xodimlar uchun') }}</b-form-checkbox
                  >
               </b-col>
               <b-col sm="12" md="4" v-if="Data.forAllEmployee">
                  <form-input :label="$t('details')" v-model="Data.details" />
               </b-col>

               <b-col sm="12" md="2" v-if="Data.forAllEmployee">
                  <form-picker v-model="Data.satrtOn" :label="$t('startOn')" :placeholder="$t('startOn')"></form-picker>
               </b-col>
               <b-col sm="12" md="2" v-if="Data.forAllEmployee">
                  <form-picker v-model="Data.endOn" :label="$t('endDate')" :placeholder="$t('endDate')"></form-picker>
               </b-col>
               <b-col md="12" v-if="Data.forAllEmployee" class="text-right">
                  <b-button :disabled="saveLoading" @click="Save" variant="success">
                     <feather-icon icon="CheckIcon"></feather-icon>
                     {{ $t('Save') }}
                  </b-button>
               </b-col>
            </b-row>
         </b-card>
      </validation-observer>
      <b-card v-if="!Data.forAllEmployee">
         <validation-observer ref="ValidationDTO">
            <b-row>
               <b-col md="4">
                  <form-select
                     required-star
                     :options="DepartmentList"
                     v-model="Data.departmentId"
                     :label="$t('department')"
                     @change="(e) => (tabrow.departmentId = e)"
                  ></form-select>
               </b-col>
               <b-col md="4">
                  <EmployeeManageSelect
                     required
                     :selectable="true"
                     v-model="tabrow.employeeManageId"
                     :departmentId="tabrow.departmentId"
                     :employee="tabrow.employee"
                     :employee-id="tabrow.employeeId"
                     :label="$t('employeeManage')"
                     @update:data="onUpdateEmployeeManage"
                  />
               </b-col>
               <b-col md="4">
                  <form-select
                     required-star
                     @change="missedType"
                     :options="missedDaysList"
                     v-model="tabrow.missedDaysTypeId"
                     :label="$t('missedDaysTypeId')"
                  ></form-select>
               </b-col>
               <b-col md="2">
                  <form-picker
                     required
                     v-model="tabrow.startAt"
                     :label="$t('startOn')"
                     :placeholder="$t('startOn')"
                  ></form-picker>
               </b-col>
               <b-col md="2">
                  <form-picker
                     required
                     v-model="tabrow.endAt"
                     :label="$t('endDate')"
                     :placeholder="$t('endDate')"
                  ></form-picker>
               </b-col>
               <!-- <b-col md="2">
                  <b-form-checkbox class="mt-2" v-model="tabrow.withoutReason"> {{ $t('Sabali') }}</b-form-checkbox>
               </b-col> -->
               <b-col sm="12" md="3">
                  <form-input :label="$t('details')" v-model="tabrow.details" />
               </b-col>
               <b-col></b-col>
               <b-col md="1"
                  ><b-button @click="addTabrow" block size="md" class="mt-2" variant="primary">
                     <feather-icon icon="PlusIcon"></feather-icon> </b-button
               ></b-col>
               <b-col md="12">
                  <b-table
                     class="mt-2"
                     :items="Data.tables"
                     :fields="tabrowFields"
                     responsive
                     no-border-collapse
                     show-empty
                     :empty-text="$t('NotFound')"
                  >
                     <template #cell(id)="{ item, index }">{{ index + 1 }}</template>
                     <template #cell(actions)="{ item, index }">
                        <b-link @click="EditTabrow(item, index)" style="margin-right: 5px; cursor: pointer">
                           <feather-icon icon="EditIcon"></feather-icon>
                        </b-link>
                        <b-link class="text-danger" @click="() => Data.tables.splice(index, 1)" style="cursor: pointer">
                           <feather-icon icon="TrashIcon"></feather-icon>
                        </b-link>
                     </template>
                  </b-table>
               </b-col>
               <b-col md="12" class="text-right">
                  <b-button :disabled="saveLoading" @click="Save" variant="success">
                     <feather-icon icon="CheckIcon"></feather-icon>
                     {{ $t('Save') }}
                  </b-button>
               </b-col>
            </b-row>
         </validation-observer>
      </b-card>
   </div>
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
   BFormCheckbox,
   BButton,
   BCol,
   BButtonGroup,
   BTable
} from 'bootstrap-vue';
import ManualService from '@/services/others/manual.service';
import { ValidationObserver } from 'vee-validate';
import DepartmentService from '@/services/info/department.service';
import EmployeeMissedDayService from '@/services/hrm/employeemissedday.service';
import EmployeeManageSelect from '@/views/components/hrm/EmployeeManageSelect.vue';

const defaultTbarow = {
   employee: '',
   departmentId: null,
   positionId: null,
   employeeManageId: null,
   missedDaysTypeId: null,
   missedDaysType: '',
   missedDays: 0,
   startAt: '',
   endAt: '',
   withoutReason: false,
   details: ''
};
export default {
   components: {
      BCard,
      BBadge,
      BLink,
      EmployeeManageSelect,
      BFormInput,
      BInputGroup,
      BFormCheckbox,
      BInputGroupAppend,
      BRow,
      BCol,
      BButtonGroup,
      BButton,
      BTable,
      ValidationObserver
   },

   data() {
      return {
         missedDaysList: [],

         saveLoading: false,
         tableIndex: null,
         DepartmentList: [],
         Data: {
            docDate: '',
            docNumber: '',
            details: '',
            forAllEmployee: '',
            departmentId: null,
            orgAreasOfActivityId: null,
            satrtOn: '',
            endOn: '',
            tables: []
         },
         tabrow: { ...defaultTbarow },
         tabrowFields: [
            {
               key: 'id',
               label: this.$t('id'),
               sortable: false,
               tdClass: 'text-center',
               thClass: 'text-center'
            },
            {
               key: 'employee',
               label: this.$t('employee'),
               sortable: false,
               tdClass: 'text-center',
               thClass: 'text-center'
            },
            {
               key: 'missedDaysType',
               label: this.$t('missedDaysTypeId'),
               sortable: false,
               tdClass: 'text-center',
               thClass: 'text-center'
            },

            {
               key: 'startAt',
               label: this.$t('startOn'),
               sortable: false,
               tdClass: 'text-center',
               thClass: 'text-center'
            },
            {
               key: 'endAt',
               label: this.$t('endDate'),
               sortable: false,
               tdClass: 'text-center',
               thClass: 'text-center'
            },
            {
               key: 'details',
               label: this.$t('details'),
               sortable: false,
               tdClass: 'text-center',
               thClass: 'text-center'
            },
            {
               key: 'actions',
               label: this.$t('actions'),
               sortable: false,
               tdClass: 'text-center',
               thClass: 'text-center'
            }
         ]
      };
   },
   created() {
      EmployeeMissedDayService.Get(this.$route.params.id).then((res) => {
         this.Data = res.data;
      });
      ManualService.MissedDaysTypeSelectList().then((res) => {
         this.missedDaysList = res.data;
      });
      DepartmentService.GetAsSelectList(null, {})
         .then((res) => {
            if (Array.isArray(res.data)) {
               this.DepartmentList = res.data;
            }
         })
         .catch(this.showApiError);
   },

   methods: {
      addTabrow() {
         this.$refs.ValidationDTO.validate().then((success) => {
            if (success) {
               if (this.tableIndex == null) {
                  this.Data.tables.push(this.tabrow);
                  this.tableIndex = null;
                  this.tabrow = {};
                  this.$refs.ValidationDTO.reset();
               } else {
                  Object.assign(this.Data.tables[this.tableIndex], this.tabrow);
                  this.tabrow = {};
                  this.tableIndex = null;
                  this.$refs.ValidationDTO.reset();
               }
            }
         });
      },
      EditTabrow(item, index) {
         this.tableIndex = index;
         this.tabrow = Object.assign({}, item);
         console.log(this.tabrow);
      },
      onUpdateEmployeeManage(e) {
         this.tabrow.departmentId = e.departmentId;
         this.tabrow.department = e.department;
         this.tabrow.employee = e.employee;
         this.Data.positionId = e.positionId;
      },
      missedType(e) {
         if (e) {
            this.tabrow.missedDaysType = this.missedDaysList.find((item) => item.value == e)['text'];
         }
      },

      Save() {
         this.$refs.ValidationDTO1.validate().then((success) => {
            if (success) {
               this.saveLoading = true;
               EmployeeMissedDayService.Update({
                  ...this.Data,
                  departmentId: this.Data.forAllEmployee ? null : this.Data.departmentId
               })
                  .then(() => {
                     this.makeToast(this.$t('SaveSuccess'), 'success');
                     this.$router.push({ name: 'EmployeeMissedDay' });
                  })
                  .catch((err) => {
                     this.showApiError(err);
                  })
                  .finally(() => {
                     this.saveLoading = false;
                  });
            }
         });
      }
   }
};
</script>

<style lang="scss" scoped></style>
