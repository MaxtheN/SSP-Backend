<template>
   <b-overlay :show="show">
      <b-row>
         <b-col sm="12" md="12" lg="12">
            <b-card>
               <validation-observer ref="ValidationDTO">
                  <b-row>
                     <b-col sm="12" md="4">
                        <div class="form-group">
                           <form-input v-model="Data.docNumber" required :label="$t('docnumber')" />
                        </div>
                     </b-col>
                     <b-col sm="12" md="4">
                        <form-picker
                           v-model="Data.docOn"
                           :label="$t('docdate')"
                           :placeholder="$t('docdate')"
                        ></form-picker>
                     </b-col>
                     <b-col sm="12" md="4">
                        <form-select
                           :options="EmployeeSickLeaveTypeSelectList"
                           v-model="Data.employeeSickLeaveTypeId"
                           :label="$t('employeeSickLeaveType')"
                           :placeholder="$t('employeeSickLeaveType')"
                           required-star
                        ></form-select>
                     </b-col>
                     <b-col sm="12" md="12">
                        <b-form-textarea
                           v-model="Data.details"
                           rows="2"
                           max-rows="6"
                           :label="$t('orderDetails')"
                           :placeholder="$t('orderDetails')"
                        />
                     </b-col>
                  </b-row>
                  <b-row class="mt-2">
                     <b-col class="text-right">
                        <b-button @click="OpenTabrow" size="sm" variant="outline-primary">
                           <feather-icon icon="PlusIcon"></feather-icon>
                           {{ $t('Add') }}
                        </b-button>
                     </b-col>
                  </b-row>

                  <b-table
                     class="mt-2"
                     :fields="TablesField"
                     small
                     responsive
                     :items="Data.tables"
                     bordered
                     hover
                     show-empty
                     :empty-text="$t('NotFound')"
                  >
                     <template #cell(status)="{ item }">
                        <b-badge :variant="item.stateId == '1' ? 'light-danger' : 'light-success'">{{
                           item.state
                        }}</b-badge>
                     </template>
                     <template #cell(isMaternityLeave)="{ item }">
                        <feather-icon icon="CheckCircleIcon" v-if="item.isMaternityLeave" class="text-success" />
                        <feather-icon icon="XCircleIcon" v-else class="text-danger" />
                     </template>
                     <template #cell(actions)="{ item, index }">
                        <div class="text-center">
                           <b-link>
                              <feather-icon
                                 style="margin-right: 5px"
                                 @click="EditTabrow(item)"
                                 icon="EditIcon"
                              ></feather-icon>
                           </b-link>
                           <b-link>
                              <feather-icon @click="DeleteTabrow(index)" icon="Trash2Icon"></feather-icon>
                           </b-link>
                        </div>
                     </template>
                     <template #cell(order)="{ index }">
                        <span>{{ index + 1 }}</span>
                     </template>
                  </b-table>

                  <b-row>
                     <b-col sm="12" md="6" lg="6" class="text-left"></b-col>
                     <b-col sm="12" md="6" lg="6" class="text-right">
                        <b-button :disabled="saveLoading" @click="SaveData" size="sm" variant="outline-success">
                           <feather-icon icon="CheckIcon"></feather-icon>
                           {{ $t('Save') }}
                        </b-button>
                     </b-col>
                  </b-row>
               </validation-observer>
            </b-card>
         </b-col>
      </b-row>
      <!-- table modal -->
      <b-modal
         size="lg"
         v-model="TabrowModal"
         v-if="TabrowModal"
         static
         no-close-on-backdrop
         hide-footer
         :title="$t('employeesickleave')"
      >
         <validation-observer ref="ValidationTabrow">
            <b-row>
               <b-col sm="12" md="6">
                  <form-picker
                     v-model="tabrow.startOn"
                     :label="$t('startdate')"
                     required
                     :placeholder="$t('startdate')"
                  ></form-picker>
               </b-col>
               <b-col sm="12" md="6">
                  <form-picker
                     v-model="tabrow.endOn"
                     :label="$t('enddate')"
                     required
                     :placeholder="$t('enddate')"
                  ></form-picker>
               </b-col>
               <b-col sm="12" md="6">
                  <EmployeeManageSelect
                     v-model="tabrow.employeeManageId"
                     :employee="tabrow.employee"
                     :label="$t('employeeManage')"
                     required-star
                     required
                     @update:data="onUpdateEmployeeManage"
                  />
               </b-col>
               <b-col sm="12" md="6" class="pb-1">
                  <form-picker v-model="tabrow.giveOn" :label="$t('giveOn')" />
               </b-col>
               <b-col sm="12" md="6" class="pb-1">
                  <form-input v-model="tabrow.documentSeria" required :label="$t('documentseries')" />
               </b-col>
               <b-col sm="12" md="6" class="pb-1">
                  <form-input v-model="tabrow.documentNumber" required :label="$t('documentnumber')" />
               </b-col>
               <b-col sm="12" md="6" class="pb-1">
                  <form-input v-model="tabrow.diagnosis" required :label="$t('diagnosis')" />
               </b-col>
               <b-col sm="12" md="6" class="pb-1">
                  <form-input v-model="tabrow.givenOrganization" required :label="$t('givenOrganization')" />
               </b-col>
               <b-col sm="12" md="6" class="pb-1">
                  <form-input v-model="tabrow.yearWorkExp" type="number" :label="$t('yearWorkExp')" />
               </b-col>
               <b-col sm="12" md="6" class="pb-1">
                  <form-input v-model="tabrow.calcPerc" type="number" :label="$t('calcPerc')" />
               </b-col>
               <!-- <b-col sm="12" md="6">
                  <b-form-checkbox v-model="tabrow.isMaternityLeave">{{ $t('isMaternityLeave') }}</b-form-checkbox>
               </b-col> -->
               <b-col sm="12" md="12" class="mt-1">
                  <b-form-textarea
                     v-model="tabrow.details"
                     :placeholder="$t('details')"
                     rows="2"
                     max-rows="6"
                  ></b-form-textarea>
               </b-col>
            </b-row>
         </validation-observer>
         <b-row>
            <b-col class="text-center mt-1">
               <b-button variant="outline-danger" @click="TabrowModal = false" class="mr-1" size="sm">
                  <feather-icon icon="ArrowLeftIcon"></feather-icon>
                  {{ $t('back') }}
               </b-button>
               <b-button variant="outline-success" @click="AddTabrow" size="sm">
                  <feather-icon icon="CheckIcon"></feather-icon>
                  {{ $t('Save') }}
               </b-button>
            </b-col>
         </b-row>
      </b-modal>
   </b-overlay>
</template>
<script>
// service
import ManualService from '@/services/others/manual.service';
import OrganizationService from '@/services/managment/organization.service';
import EmployeeSickLeaveService from '@/services/hrm/employeesickleave.service';
import EmployeeManageSelect from '@/views/components/hrm/EmployeeManageSelect.vue';

// components
import {
   BOverlay,
   BCard,
   BCardBody,
   BRow,
   BCol,
   BFormInput,
   BTabs,
   BTable,
   BTab,
   BButton,
   BLink,
   BFormGroup,
   BModal,
   BCardText,
   BInputGroup,
   BInputGroupAppend,
   BTr,
   BTd,
   BFormCheckbox,
   BFormTextarea
} from 'bootstrap-vue';
import EmployeeManageService from '@/services/hrm/employeemanage.service';

const defaultTableRow = {
   id: 0,
   department: '',
   employee: '',
   details: '',
   startOn: '',
   endOn: '',
   employee: '',
   employeeManageId: null,
   giveOn: null,
   documentSeria: null,
   documentNumber: null,
   diagnosis: null,
   givenOrganization: null,
   yearWorkExp: 0,
   calcPerc: 0,
   isMaternityLeave: false
};

export default {
   components: {
      BOverlay,
      BCard,
      BCardBody,
      BRow,
      BCol,
      BFormInput,
      BTabs,
      BTab,
      BButton,
      BTable,
      BLink,
      BFormGroup,
      BModal,
      BCardText,
      BInputGroup,
      BInputGroupAppend,
      BTr,
      BTd,
      BFormCheckbox,
      BFormTextarea,
      EmployeeManageSelect
   },
   name: 'Edit',
   data() {
      return {
         show: false,
         OrganizationList: [],
         EmployeeSickLeaveTypeSelectList: [],
         loadingButton: false,
         TabrowModal: false,
         saveLoading: false,
         Data: {
            id: null,
            docNumber: null,
            docOn: null,
            details: null,
            organization: null,
            status: null,
            employeeSickLeaveTypeId: null
         },
         tabrow: { ...defaultTableRow },
         TablesField: [
            {
               key: 'department',
               label: this.$t('Department'),
               sortable: true
            },
            {
               key: 'employee',
               label: this.$t('employee'),
               sortable: true
            },
            {
               key: 'giveOn',
               label: this.$t('giveOn')
            },
            {
               key: 'documentSeria',
               label: this.$t('documentSeria')
            },
            {
               key: 'documentNumber',
               label: this.$t('documentnumber')
            },
            {
               key: 'diagnosis',
               label: this.$t('diagnosis')
            },
            {
               key: 'givenOrganization',
               label: this.$t('givenOrganization')
            },
            {
               key: 'yearWorkExp',
               label: this.$t('yearWorkExp')
            },
            {
               key: 'calcPerc',
               label: this.$t('calcPerc'),
               sortable: true
            },
            {
               key: 'isMaternityLeave',
               label: this.$t('isMaternityLeave'),
               sortable: true,
               thClass: 'text-center',
               tdClass: 'text-center'
            },
            {
               key: 'startOn',
               label: this.$t('startdate')
            },
            {
               key: 'endOn',
               label: this.$t('enddate')
            },
            {
               key: 'actions',
               label: this.$t('actions'),
               thClass: 'text-center',
               tdClass: 'text-center',
               sortable: true
            }
         ]
      };
   },

   created() {
      this.show = true;
      EmployeeSickLeaveService.Get(this.$route.params.id)
         .then((res) => {
            this.Data = res.data;
         })
         .catch((error) => {
            this.makeToast(error.response.data.errors, 'danger');
         })
         .finally(() => {
            this.show = false;
         });

      ManualService.EmployeeSickLeaveTypeSelectList()
         .then((res) => {
            this.EmployeeSickLeaveTypeSelectList = res.data;
         })
         .catch((error) => {
            this.showApiError(error);
         });

      OrganizationService.GetAsSelectList()
         .then((res) => {
            this.OrganizationList = res.data;
         })
         .catch((error) => {
            this.showApiError(error);
         });
   },
   watch: {
      '$route.query': {
         handler(val) {
            if (val) {
               if (val.employeeManageId) {
                  this.tabrow.employeeManageId = Number(val.employeeManageId);
                  EmployeeManageService.Get(val.employeeManageId).then((res) => {
                     const { data } = res;
                     this.tabrow.employee = data.employee;
                     this.tabrow.department = data.department;
                  });
                  this.TabrowModal = true;
               }
            }
         },
         immediate: true
      }
   },
   methods: {
      onUpdateEmployeeManage(e) {
         this.tabrow.employee = e?.employee;
         this.tabrow.department = e?.department;
      },
      DeleteTabrow(index) {
         this.Data.tables.splice(index, 1);
      },
      OpenTabrow() {
         this.TabrowModal = true;
         this.tabrow = { ...defaultTableRow };
         this.editedIndex1 = -1;
      },
      EditTabrow(item) {
         this.editedIndex1 = this.Data.tables.indexOf(item);
         this.tabrow = Object.assign({}, item);
         this.TabrowModal = true;
      },
      AddTabrow() {
         this.$refs.ValidationTabrow.validate().then((success) => {
            if (success) {
               if (this.editedIndex1 > -1) {
                  Object.assign(this.Data.tables[this.editedIndex1], this.tabrow);
               } else {
                  this.Data.tables.push(this.tabrow);
               }
               this.TabrowModal = false;
            }
         });
      },
      SaveData() {
         this.$refs.ValidationDTO.validate().then((success) => {
            if (success) {
               this.saveLoading = true;
               EmployeeSickLeaveService.Update(this.Data)
                  .then(() => {
                     this.makeToast(this.$t('SaveSuccess'), 'success');
                     this.$router.push({ name: 'EmployeeSickLeave' });
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
