<template>
   <b-overlay :show="show">
      <b-row>
         <b-col sm="12" md="12" lg="12">
            <b-card>
               <validation-observer ref="ValidationDTO">
                  <b-row>
                     <b-col sm="12" md="3">
                        <div class="form-group">
                           <form-input v-model="Data.docNumber" required :label="$t('code')" />
                        </div>
                     </b-col>

                     <b-col sm="12" md="3">
                        <form-picker v-model="Data.docOn" :label="$t('docOn')" :placeholder="$t('docOn')"></form-picker>
                     </b-col>

                     <b-col sm="12" md="6">
                        <form-select
                           :options="RoundingTypeList"
                           v-model="Data.roundingTypeId"
                           :label="$t('roundingType')"
                        ></form-select>
                     </b-col>

                     <b-col sm="12" md="6">
                        <CalculationKindSelect
                           v-model="Data.calculationKindId"
                           :calculationKind="Data.calculationKind"
                           @update:data="onUpdateCalculationKind"
                        />
                     </b-col>

                     <b-col sm="12" md="3">
                        <form-select
                           :options="OrgSettlementAccountList"
                           v-model="Data.orgSettlementAccountId"
                           requitred-star
                           label="orgSettlementAccount"
                        />
                     </b-col>

                     <b-col sm="12" md="3">
                        <form-select :options="StateList" v-model="Data.status" :label="$t('Status')"></form-select>
                     </b-col>

                     <b-col sm="12" md="3" class="my-1">
                        <div class="form-group">
                           <b-form-checkbox v-model="Data.isCancelation">
                              {{ $t('isCancelation') }}
                           </b-form-checkbox>
                        </div>
                     </b-col>
                     <b-col sm="12" md="12">
                        <b-form-textarea
                           id="textarea"
                           v-model="Data.details"
                           :placeholder="$t('details')"
                           rows="2"
                           max-rows="6"
                        ></b-form-textarea>
                     </b-col>
                  </b-row>
               </validation-observer>
            </b-card>
         </b-col>
      </b-row>

      <b-card>
         <!-- tables form -->
         <validation-observer ref="ValidationTabrow">
            <b-row>
               <b-col sm="12" md="4">
                  <form-picker
                     v-model="tabrow.startOn"
                     :label="$t('startdate')"
                     required
                     :placeholder="$t('startdate')"
                  ></form-picker>
               </b-col>
               <b-col sm="12" md="4">
                  <form-picker
                     v-model="tabrow.endDate"
                     :label="$t('enddate')"
                     required
                     :placeholder="$t('enddate')"
                  ></form-picker>
               </b-col>
               <b-col sm="12" md="4">
                  <EmployeeManageSelect
                     v-model="tabrow.employeeManageId"
                     :employee="tabrow.employee"
                     :label="$t('employeeManage')"
                     @update:data="onUpdateEmployeeManage"
                  />
               </b-col>
               <b-col sm="12" md="3">
                  <form-select
                     :options="TempCalcKindTypeSelectList"
                     v-model="tabrow.tempCalcKindTypeId"
                     @option:selected="(e) => (tabrow.tempCalcKindType = e ? e.text : '')"
                     requitred-star
                     label="tempCalcKindType"
                  />
               </b-col>
               <b-col sm="12" md="3">
                  <form-currency-input
                     v-model="tabrow.amount"
                     :label="$t('amount')"
                     rules="required|numeric"
                     :placeholder="$t('amount')"
                  />
               </b-col>

               <b-col sm="12" md="2">
                  <form-input-hrm
                     v-model="tabrow.percentage"
                     :label="$t('percentage')"
                     rules="required|numeric"
                     :placeholder="$t('percentage')"
                  />
               </b-col>

               <b-col sm="12" md="1" class="mt-2">
                  <b-button variant="primary" @click="AddTabrow">
                     <feather-icon icon="PlusIcon" size="14"></feather-icon>
                  </b-button>
               </b-col>
            </b-row>
         </validation-observer>

         <!-- tables -->
         <b-table
            :fields="TablesField"
            small
            responsive="sm"
            :items="Data.tables"
            hover
            show-empty
            bordered
            :empty-text="$t('NotFound')"
            class="mt-2"
         >
            <template #cell(actions)="{ item, index }">
               <div class="text-center">
                  <b-link>
                     <feather-icon style="margin-right: 5px" @click="EditTabrow(item)" icon="EditIcon"></feather-icon>
                  </b-link>
                  <b-link>
                     <feather-icon @click="DeleteTabrow(index)" icon="Trash2Icon"></feather-icon>
                  </b-link>
               </div>
            </template>
         </b-table>
      </b-card>

      <b-row class="my-2 justify-content-end px-2">
         <b-button :disabled="saveLoading" @click="SaveData" variant="outline-success">
            <feather-icon icon="CheckIcon"></feather-icon>
            {{ $t('Save') }}
         </b-button>
      </b-row>
   </b-overlay>
</template>
<script>
// service
import ManualService from '@/services/others/manual.service';
import PlannedCalculationService from '@/services/hrm/plannedcalculation.service';
import OrganizationService from '@/services/managment/organization.service';

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
   VBTooltip,
   BModal,
   VBModal,
   BCardText,
   BInputGroup,
   BInputGroupAppend,
   BTr,
   BTd,
   BFormCheckbox,
   BFormTextarea
} from 'bootstrap-vue';
import EmployeeManageSelect from '@/views/components/hrm/EmployeeManageSelect.vue';
import CalculationKindSelect from '@/views/components/hrm/CalculationKindSelect.vue';

const defaultTableRow = {
   id: 0,
   department: '',
   employee: '',
   employeeManageId: null,
   details: '',
   startOn: '',
   endDate: '',
   percentage: 0,
   amount: 0,
   tempCalcKindTypeId: 0,
   tempCalcKindType: ''
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
      VBTooltip,
      BModal,
      VBModal,
      BCardText,
      BInputGroup,
      BInputGroupAppend,
      BTr,
      BTd,
      BFormCheckbox,
      BFormTextarea,
      EmployeeManageSelect,
      CalculationKindSelect
   },
   name: 'PlannedCalculationEdit',
   data() {
      return {
         show: false,
         StateList: [],
         RoundingTypeList: [],
         OrgSettlementAccountList: [],
         TempCalcKindTypeSelectList: [],
         loadingButton: false,
         TabrowModal: false,
         saveLoading: false,
         Data: {
            id: null,
            docNumber: null,
            docOn: null,
            details: null,
            calculationKindId: null,
            orgSettlementAccountId: null,
            isCancelation: null,
            roundingTypeId: null,
            tables: []
         },
         tabrow: { ...defaultTableRow },
         filter: {
            search: '',
            sortBy: '',
            orderType: 'asc',
            page: 1,
            pageSize: 20,
            perPageOptions: [10, 20, 50, 100],
            total: 0,
            organizationId: 0
         },
         TablesField: [
            {
               key: 'employee',
               label: this.$t('employee'),
               sortable: true
            },
            {
               key: 'department',
               label: this.$t('Department'),
               sortable: true
            },

            {
               key: 'startOn',
               label: this.$t('startdate')
            },
            {
               key: 'endDate',
               label: this.$t('enddate')
            },
            {
               key: 'percentage',
               label: this.$t('percentage')
            },
            {
               key: 'tempCalcKindType',
               label: this.$t('tempCalcKindType')
            },
            {
               key: 'amount',
               label: this.$t('amount')
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
      PlannedCalculationService.Get(this.$route.params.id)
         .then((res) => {
            this.Data = res.data;
         })
         .catch((error) => {
            this.showApiError(error);
         })
         .finally(() => {
            this.show = false;
         });

      ManualService.RoundingTypeSelectList({})
         .then((res) => {
            if (Array.isArray(res.data)) {
               this.RoundingTypeList = res.data;
            }
         })
         .catch((error) => {
            this.showApiError(error);
         });

      ManualService.StateSelectList()
         .then((res) => {
            this.StateList = res.data;
         })
         .catch((error) => {
            this.showApiError(error);
         });

      OrganizationService.GetAsSelectListOrgSettlementAccount()
         .then((res) => {
            if (Array.isArray(res.data)) {
               this.OrgSettlementAccountList = res.data;
            }
         })
         .catch((error) => {
            this.showApiError(error);
         });

      ManualService.TempCalcKindTypeSelectList({})
         .then((res) => {
            if (Array.isArray(res.data)) {
               this.TempCalcKindTypeSelectList = res.data;
            }
         })
         .catch((error) => {
            this.showApiError(error);
         });
   },
   methods: {
      onUpdateEmployeeManage(e) {
         this.tabrow.employee = e?.employee;
         this.tabrow.department = e?.department;
      },
      onUpdateCalculationKind(e) {
         this.Data.calculationKind = e?.fullName;
      },
      DeleteTabrow(index) {
         this.Data.tables.splice(index, 1);
      },
      OpenTabrow() {
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
               PlannedCalculationService.Update(this.Data)
                  .then(() => {
                     this.makeToast(this.$t('SaveSuccess'), 'success');
                     this.$router.push({ name: 'PlannedCalculation' });
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
