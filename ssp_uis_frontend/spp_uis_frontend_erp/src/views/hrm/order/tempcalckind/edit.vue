<template>
   <b-overlay :show="show">
      <b-card>
         <validation-observer ref="ValidationDTO">
            <b-tabs v-model="tabIndex" small class="nav-tabs" nav-wrapper-class="pb-0 d-flex justify-content-center">
               <b-tab :title="$t('MainPart')" active lazy>
                  <b-row>
                     <b-col sm="12" md="3">
                        <div class="form-group">
                           <form-input v-model="Data.docNumber" required :label="$t('docnumber')" />
                        </div>
                     </b-col>
                     <b-col sm="12" md="3">
                        <form-picker
                           v-model="Data.docOn"
                           :label="$t('docOn')"
                           required
                           :placeholder="$t('docOn')"
                        ></form-picker>
                     </b-col>
                     <!-- <b-col sm="12" md="3">
                  <form-select
                     :options="RoundingTypeList"
                     v-model="Data.roundingTypeId"
                     requitred-star
                     :label="$t('roundingType')"
                  ></form-select>
               </b-col> -->
                     <b-col sm="12" md="3">
                        <form-select
                           :options="CalculationKindList"
                           v-model="Data.calculationKindId"
                           required-star
                           :label="$t('calculationKind')"
                        ></form-select>
                     </b-col>
                     <b-col sm="12" md="3">
                        <form-select
                           :options="TempCalcKindTypeSelectList"
                           v-model="Data.tempCalcKindTypeId"
                           @option:selected="(e) => (Data.tempCalcKindType = e ? e.text : '')"
                           required-star
                           label="businessTripType"
                        />
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
                  <hr class="mt-2" />
                  <validation-observer ref="ValidationTabrow">
                     <b-row>
                        <b-col sm="12" md="3" class="mb-1">
                           <EmployeeManageSelect
                              :isOrganisation="$can('AllAppointEmployeeCreate', 'permissions')"
                              v-model="tabrow.employeeManageId"
                              required
                              :employee="tabrow.employee"
                              :label="$t('employeeManage')"
                              @update:data="onUpdateEmployeeManage"
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
                        <b-col sm="12" md="3">
                           <form-currency-input
                              v-model="tabrow.amount"
                              :label="$t('amount')"
                              rules="required|numeric"
                              :placeholder="$t('amount')"
                           />
                        </b-col>
                        <b-col sm="12" md="1" class="mt-2">
                           <b-button variant="primary" @click="AddTabrow" class="d-flex">
                              <feather-icon icon="PlusIcon" size="14"></feather-icon>
                              {{ $t('Add') }}
                           </b-button>
                        </b-col>
                     </b-row>
                  </validation-observer>

                  <b-row class="mt-2">
                     <b-col>
                        <b-table
                           :fields="TablesField"
                           hover
                           bordered
                           show-empty
                           :empty-text="$t('NotFound')"
                           small
                           responsive="sm"
                           :items="Data.tables"
                        >
                           <template #cell(actions)="{ item, index }">
                              <div class="text-center">
                                 <b-link>
                                    <feather-icon
                                       style="margin-right: 5px"
                                       @click="EditTabrow(item)"
                                       icon="EditIcon"
                                    ></feather-icon>
                                 </b-link>
                                 <b-link class="text-danger">
                                    <feather-icon @click="DeleteTabrow(index)" icon="Trash2Icon"></feather-icon>
                                 </b-link>
                              </div>
                           </template>
                           <template #cell(order)="{ index }">
                              <span>{{ index + 1 }}</span>
                           </template>
                        </b-table>
                     </b-col>
                  </b-row>

                  <b-row class="text-center mt-2">
                     <b-col>
                        <b-button
                           :disabled="!Data.tables || Data.tables.length === 0 || !validInput"
                           @click="Continue"
                           size="sm"
                           variant="outline-primary"
                        >
                           <feather-icon icon="ArrowRightIcon"></feather-icon>
                           {{ $t('Continue') }}
                        </b-button>
                     </b-col>
                  </b-row>
               </b-tab>
               <b-tab :disabled="!Data.tables || Data.tables.length === 0 || !validInput" :title="$t('Buyruq matni')">
                  <b-row>
                     <b-col class="mb-1">
                        <vue-editor v-model="tabrow.detailForPrint" :label="$t('content')"></vue-editor>
                     </b-col>
                  </b-row>
                  <b-row class="text-center">
                     <b-col>
                        <b-button @click="Back" size="sm" variant="outline-danger" class="mx-1">
                           <feather-icon icon="ArrowLeftIcon"></feather-icon>
                           {{ $t('back') }}
                        </b-button>
                        <b-button
                           :disabled="!Data.tables || Data.tables.length === 0 || !validInput || !tabrow.detailForPrint"
                           @click="Continue"
                           size="sm"
                           variant="outline-primary"
                        >
                           <feather-icon icon="ArrowRightIcon"></feather-icon>
                           {{ $t('Continue') }}
                        </b-button>
                     </b-col>
                  </b-row>
               </b-tab>
               <b-tab
                  :title="$t('Signatories')"
                  lazy
                  :disabled="!Data.tables || Data.tables.length === 0 || !validInput"
               >
                  <HrmSigner
                     :signer.sync="Data.signer"
                     :organizationId="Data.organizationId"
                     @back="Back"
                     @continue="Continue"
                  />
               </b-tab>

               <!-- forma -->
               <b-tab
                  :disabled="!Data.tables || Data.tables.length === 0 || !validInput"
                  :title="$t('Formalization')"
                  lazy
               >
                  <div class="d-flex justify-content-around">
                     <div>
                        <span style="font-weight: bold">{{ $t('docnumber') }}</span
                        >: {{ Data.docNumber }}
                     </div>
                     <div>
                        <span style="font-weight: bold">{{ $t('docdate') }}</span
                        >: {{ Data.docOn }}
                     </div>
                  </div>
                  <div>
                     <span style="font-weight: bold">{{ $t('orderDetails') }}</span
                     >: {{ Data.details }}
                  </div>
                  <b-row class="mt-2">
                     <b-col>
                        <b-table
                           :fields="TablesField"
                           small
                           responsive="sm"
                           :items="Data.tables"
                           hover
                           show-empty
                           bordered
                           :empty-text="$t('NotFound')"
                        >
                           <template #cell(order)="{ index }">
                              <span>{{ index + 1 }}</span>
                           </template>
                        </b-table>
                     </b-col>
                  </b-row>
                  <div class="d-flex justify-content-between">
                     <div v-if="signerDirector">
                        <span style="font-weight: bold">{{ $t('Principal signatory') }}</span
                        >:
                        {{ signerDirector.employee }}
                     </div>

                     <div v-if="signerHr">
                        <span style="font-weight: bold">{{ $t('Entered') }}</span
                        >: {{ signerHr.employee }}
                     </div>
                  </div>

                  <HrmSignerTableView :signer="Data.signer" />

                  <div class="d-flex justify-content-center">
                     <b-button @click="Back" size="sm" variant="outline-danger" class="mx-1">
                        <feather-icon icon="ArrowLeftIcon"></feather-icon>
                        {{ $t('back') }}
                     </b-button>

                     <b-button
                        :disabled="saveLoading"
                        @click="SaveData"
                        size="sm"
                        variant="outline-success"
                        class="mx-1"
                     >
                        <feather-icon icon="CheckIcon"></feather-icon>
                        {{ $t('Save') }}
                     </b-button>
                  </div>
               </b-tab>
            </b-tabs>
         </validation-observer>
      </b-card>
   </b-overlay>
</template>
<script>
// service
import ManualService from '@/services/others/manual.service';
import TempCalcKindService from '@/services/hrm/tempcalckind.service';
import CalculationKindService from '@/services/hrm/calculationkind.service';
// components
import {
   BOverlay,
   BCard,
   BRow,
   BCol,
   BFormInput,
   BTable,
   BButton,
   BLink,
   BFormGroup,
   BModal,
   BInputGroup,
   BInputGroupAppend,
   BFormCheckbox,
   BFormTextarea,
   BTabs,
   BTab
} from 'bootstrap-vue';
import { VueEditor } from 'vue2-editor';
import EmployeeManageSelect from '@/views/components/hrm/EmployeeManageSelect.vue';
import EmployeeSelect2 from '@/views/components/employee/EmployeeSelect2.vue';
import { sortHrmOrder } from '@/views/hrm/utils';
import EmployeeManageService from '@/services/hrm/employeemanage.service';
const HrmSigner = () => import('@/views/components/hrm/HrmSigner.vue');
const HrmSignerTableView = () => import('@/views/components/hrm/HrmSignerTableView.vue');

const defaultTableRow = {
   id: 0,
   employeeManageId: 0,
   percentage: 0,
   amount: 0,
   details: '',
   detailForPrint: ''
};

export default {
   components: {
      VueEditor,
      BOverlay,
      BCard,
      BRow,
      BCol,
      BFormInput,
      BButton,
      BTable,
      BLink,
      BFormGroup,
      BModal,
      BInputGroup,
      BInputGroupAppend,
      BFormCheckbox,
      EmployeeManageSelect,
      BFormTextarea,
      BTabs,
      BTab,
      EmployeeSelect2,
      HrmSigner,
      HrmSignerTableView
   },
   name: 'TempCalcKindEdit',
   data() {
      return {
         show: false,

         RoundingTypeList: [],
         CalculationKindList: [],
         TempCalcKindTypeSelectList: [],
         loadingButton: false,
         saveLoading: false,
         tabIndex: 1,
         Data: {
            id: 0,

            docNumber: null,
            docOn: null,
            details: null,
            calculationKindId: null,
            orgSettlementAccountId: null,
            roundingTypeId: 2,
            tempCalcKindTypeId: null,
            tempCalcKindType: '',
            tables: [],
            signer: []
         },
         tabrow: { ...defaultTableRow },
         TablesField: [
            {
               key: 'department',
               label: this.$t('Department')
            },
            {
               key: 'employee',
               label: this.$t('employee'),
               sortable: true
            },
            {
               key: 'percentage',
               label: this.$t('percentage')
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
      TempCalcKindService.Get(this.$route.params.id)
         .then((res) => {
            this.Data = res.data;
            if (!Array.isArray(res.data?.tables)) {
               this.Data.tables = [];
            }
            if (!Array.isArray(res.data?.signer)) {
               this.Data.signer = [];
            }
            if (this.$route.params.id == 0) {
               this.Data.roundingTypeId = 2;
            }
         })
         .catch((error) => {
            this.showApiError(error);
         })
         .finally(() => {
            this.show = false;
         });

      CalculationKindService.GetAsSelectList({})
         .then((res) => {
            if (Array.isArray(res.data)) {
               this.CalculationKindList = res.data;
            }
         })
         .catch((error) => {
            this.showApiError(error);
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
   computed: {
      signerHr() {
         return this.Data.signer.find((e) => e.isHr);
      },
      signerDirector() {
         return this.Data.signer.find((e) => e.isDirector);
      },
      validInput() {
         const { docNumber, docOn, details } = this.Data;

         const allInputsFilled = docNumber && docOn && details;

         return allInputsFilled;
      }
   },
   methods: {
      Continue() {
         if (this.tabIndex != 3) {
            this.tabIndex += 1;
         }
      },
      Back() {
         this.tabIndex -= 1;
      },
      onUpdateEmployeeManage(e) {
         this.tabrow.employee = e?.employee;
         this.tabrow.department = e?.department;
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
      },
      AddTabrow() {
         this.$refs.ValidationTabrow.validate().then((success) => {
            if (success) {
               if (this.editedIndex1 > -1) {
                  Object.assign(this.Data.tables[this.editedIndex1], this.tabrow);
               } else {
                  this.Data.tables.push(this.tabrow);
               }
               this.$refs.ValidationTabrow.reset();
               this.tabrow = { ...defaultTableRow };
            }
         });
      },
      SaveData() {
         this.$refs.ValidationDTO.validate().then((success) => {
            if (success) {
               this.saveLoading = true;
               TempCalcKindService.Update({ ...this.Data, signer: sortHrmOrder(this.Data.signer) })
                  .then((res) => {
                     this.makeToast(this.$t('SaveSuccess'), 'success');
                     this.$router.push({ name: 'TempCalcKind' });
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
   },
   watch: {
      '$route.query': {
         handler(val) {
            if (val) {
               if (val.employeeManageId) {
                  EmployeeManageService.Get(val.employeeManageId).then((res) => {
                     const { data } = res;
                     this.tabrow.employeeManageId = data.id;
                     this.tabrow.employeeId = data.employeeId;
                     this.tabrow.employee = data.employee;
                     this.tabrow.department = data.department;
                     this.tabrow.departmentId = data.departmentId;
                     this.tabrow.position = data.position;
                     this.tabrow.positionId = data.positionId;
                  });
                  this.TabrowModal = true;
               }
            }
         },
         immediate: true
      }
   }
};
</script>
<style scoped>
input {
   margin: 0.4rem;
}
</style>
