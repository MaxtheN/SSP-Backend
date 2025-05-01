<template>
   <b-overlay :show="show">
      <b-row>
         <b-col sm="12" md="12" lg="12">
            <b-card>
               <validation-observer ref="ValidationDTO">
                  <b-row>
                     <b-col sm="12" md="4" class="mb-1">
                        <form-input-hrm
                           v-model="Data.code"
                           rules="required"
                           :label="$t('code')"
                           :placeholder="$t('code')"
                        />
                     </b-col>
                     <b-col sm="12" md="4" class="mb-1">
                        <form-input-translate
                           v-model="Data.shortName"
                           @update:translates="(e) => (Data.translates = e)"
                           :translates="Data.translates"
                           column-name="short_name"
                           required
                           :label="$t('shortname')"
                           :placeholder="$t('shortname')"
                        />
                     </b-col>
                     <b-col sm="12" md="4" class="mb-1">
                        <form-input-translate
                           v-model="Data.fullName"
                           @update:translates="(e) => (Data.translates = e)"
                           :translates="Data.translates"
                           column-name="full_name"
                           required
                           :label="$t('fullname')"
                           :placeholder="$t('fullname')"
                        />
                     </b-col>
                     <b-col sm="12" md="4" class="mb-1">
                        <form-input-hrm
                           v-model="Data.normativeDoc"
                           rules="required"
                           :label="$t('normativedoc')"
                           :placeholder="$t('normativedoc')"
                        />
                     </b-col>
                     <b-col sm="12" md="4">
                        <form-select
                           v-model="Data.minimumValueTypeId"
                           :options="MinimumValueTypeList"
                           label="minimumValueType"
                        />
                     </b-col>
                     <b-col sm="12" md="2" class="mb-1">
                        <form-picker
                           v-model="Data.startOn"
                           :label="$t('startdate')"
                           :placeholder="$t('startdate')"
                        ></form-picker>
                     </b-col>
                     <b-col sm="12" md="2" class="mb-1">
                        <form-picker
                           v-model="Data.endOn"
                           :label="$t('enddate')"
                           :placeholder="$t('enddate')"
                        ></form-picker>
                     </b-col>
                     <b-col sm="12" md="4" class="mb-1">
                        <form-select
                           :options="ItemOfExpenseList"
                           v-model="Data.itemOfExpenseId"
                           label="itemOfExpense"
                        ></form-select>
                     </b-col>
                     <b-col sm="12" md="4" class="mb-1">
                        <form-select
                           :options="CalculationMethodList"
                           v-model="Data.calculationMethodId"
                           label="calculationMethod"
                        ></form-select>
                     </b-col>
                     <b-col sm="12" md="4" class="mb-1">
                        <form-select
                           :options="CalculationTypeList"
                           v-model="Data.calculationTypeId"
                           required-star
                           label="calculationType"
                        ></form-select>
                     </b-col>
                     <b-col sm="12" md="4" class="mb-1">
                        <form-select
                           :options="CalculateByTimeTypeList"
                           v-model="Data.calculateByTimeTypeId"
                           label="calculateByTimeType"
                        ></form-select>
                     </b-col>
                     <b-col sm="12" md="3" class="mb-1" align-self="center">
                        <b-form-checkbox v-model="Data.byEnrolment" inline>
                           {{ $t('byEnrolment') }}
                        </b-form-checkbox>
                     </b-col>
                     <b-col sm="12" md="3" class="mb-1" align-self="center">
                        <b-form-checkbox v-model="Data.dependOnRate" inline>
                           {{ $t('dependOnRate') }}
                        </b-form-checkbox>
                     </b-col>
                     <b-col sm="12" md="2" class="mb-1" align-self="center">
                        <b-form-checkbox v-model="Data.isMandatory" inline>
                           {{ $t('isMandatory') }}
                        </b-form-checkbox>
                     </b-col>
                     <b-col sm="12" md="4" class="mb-1">
                        <form-select :options="StateList" v-model="Data.stateId" label="Status"></form-select>
                     </b-col>
                  </b-row>
               </validation-observer>
            </b-card>
         </b-col>

         <!-- tabs -->
         <b-col cols="12">
            <b-card no-body>
               <b-tabs pills card small nav-class="mb-0 mx-2" nav-wrapper-class="pb-0 align-items-center">
                  <!-- percents -->
                  <b-tab :title="$t('percents')" active lazy>
                     <b-card-text>
                        <validation-observer ref="ValidationPercentsRow">
                           <b-row>
                              <b-col sm="12" md="3">
                                 <form-picker
                                    v-model="parcentTableRow.dateOn"
                                    :label="$t('ondate')"
                                    required
                                    :placeholder="$t('ondate')"
                                 />
                              </b-col>
                              <b-col sm="12" md="3">
                                 <form-select
                                    :options="LimitOperTypeList"
                                    v-model="parcentTableRow.limitOperTypeId"
                                    :label="$t('limitOperType')"
                                    :placeholder="$t('limitOperType')"
                                 />
                              </b-col>
                              <b-col sm="12" md="3">
                                 <form-currency-input
                                    v-model="parcentTableRow.amount"
                                    :label="$t('amount')"
                                    :placeholder="$t('amount')"
                                 />
                              </b-col>
                              <b-col sm="12" md="2">
                                 <form-input-hrm
                                    v-model="parcentTableRow.percentRate"
                                    :label="$t('percentRate')"
                                    rules="required|numeric"
                                    :placeholder="$t('percentRate')"
                                 />
                              </b-col>
                              <b-col sm="12" md="1" class="mt-2">
                                 <b-button variant="primary" @click="addParcentsTableRow">
                                    <feather-icon icon="PlusIcon"></feather-icon>
                                 </b-button>
                              </b-col>
                           </b-row>
                        </validation-observer>

                        <b-table
                           :fields="fieldsPercents"
                           :items="Data.percents"
                           small
                           responsive="sm"
                           hover
                           show-empty
                           bordered
                           :empty-text="$t('NotFound')"
                        >
                           <template #cell(actions)="{ index }">
                              <div class="text-center">
                                 <b-link class="text-danger">
                                    <feather-icon @click="DeletePercents(index)" icon="Trash2Icon"></feather-icon>
                                 </b-link>
                              </div>
                           </template>
                        </b-table>
                     </b-card-text>
                  </b-tab>
                  <!-- usetables -->
                  <b-tab :title="$t('usedTables')" lazy>
                     <b-card-text>
                        <validation-observer ref="ValidationUsedTableRow">
                           <b-row>
                              <b-col sm="12" md="3">
                                 <form-select
                                    :options="CalculationKindList"
                                    v-model="usedTableRow.formedCalculationKindId"
                                    required-star
                                    label="calculationKind"
                                 />
                              </b-col>
                              <b-col sm="12" md="3">
                                 <form-select
                                    :options="MinimumValueTypeList"
                                    v-model="usedTableRow.minimumValueTypeId"
                                    required
                                    label="minimumValueType"
                                 />
                              </b-col>
                              <b-col sm="12" md="3">
                                 <form-picker
                                    v-model="usedTableRow.startOn"
                                    :label="$t('startdate')"
                                    required
                                    :placeholder="$t('startdate')"
                                 />
                              </b-col>
                              <b-col sm="12" md="3">
                                 <form-picker
                                    v-model="usedTableRow.endOn"
                                    :label="$t('enddate')"
                                    :placeholder="$t('enddate')"
                                 />
                              </b-col>
                              <b-col sm="12" md="2" class="mt-2">
                                 <b-form-checkbox v-model="usedTableRow.calcFromInSum">{{
                                    $t('calcFromInSum')
                                 }}</b-form-checkbox>
                              </b-col>
                              <b-col sm="12" md="3">
                                 <form-currency-input
                                    v-model="usedTableRow.quantityOfMinimumValue"
                                    :label="$t('quantityOfMinimumValue')"
                                    :placeholder="$t('quantityOfMinimumValue')"
                                 />
                              </b-col>
                              <b-col sm="12" md="1" class="mt-2">
                                 <b-button variant="light" @click="addUsedTableRow">
                                    <feather-icon icon="PlusIcon"></feather-icon>
                                 </b-button>
                              </b-col>
                           </b-row>
                        </validation-observer>

                        <b-table
                           :fields="fieldsUsedTables"
                           :items="Data.usedTables"
                           small
                           responsive="sm"
                           hover
                           show-empty
                           bordered
                           :empty-text="$t('NotFound')"
                        >
                           <template #cell(actions)="{ index }">
                              <div class="text-center">
                                 <b-link class="text-danger">
                                    <feather-icon @click="DeleteUsedTables(index)" icon="Trash2Icon"></feather-icon>
                                 </b-link>
                              </div>
                           </template>
                        </b-table>
                     </b-card-text>
                  </b-tab>

                  <!-- allowedDocs -->
                  <b-tab :title="$t('allowedDocs')" lazy>
                     <b-card-text>
                        <validation-observer ref="ValidationAllowedDocsRow">
                           <b-row>
                              <b-col sm="12" md="3">
                                 <form-picker
                                    v-model="allowedDocsTableRow.dateOn"
                                    :label="$t('ondate')"
                                    required
                                    :placeholder="$t('ondate')"
                                 />
                              </b-col>
                              <b-col sm="12" md="3">
                                 <form-select
                                    :options="[]"
                                    v-model="allowedDocsTableRow.tableId"
                                    label="table"
                                    required-star
                                    :placeholder="$t('table')"
                                 />
                              </b-col>
                              <b-col sm="12" md="4" class="mb-1">
                                 <form-select
                                    :options="StateList"
                                    v-model="allowedDocsTableRow.stateId"
                                    label="Status"
                                 ></form-select>
                              </b-col>
                              <b-col sm="12" md="1" class="mt-2">
                                 <b-button variant="light" @click="addAllowedDocsTableRow">
                                    <feather-icon icon="PlusIcon"></feather-icon>
                                 </b-button>
                              </b-col>
                           </b-row>
                        </validation-observer>

                        <b-table
                           :fields="fieldsAllowed"
                           :items="Data.allowedDocs"
                           small
                           responsive="sm"
                           hover
                           show-empty
                           bordered
                           :empty-text="$t('NotFound')"
                        >
                           <template #cell(actions)="{ index }">
                              <div class="text-center">
                                 <b-link class="text-danger">
                                    <feather-icon @click="DeleteAllowedDocs(index)" icon="Trash2Icon"></feather-icon>
                                 </b-link>
                              </div>
                           </template>
                        </b-table>
                     </b-card-text>
                  </b-tab>

                  <!-- allowedTransaction -->
                  <b-tab :title="$t('allowedTransaction')" lazy>
                     <b-card-text>
                        <b-table
                           :fields="fieldsTransactions"
                           :items="Data.transactions || []"
                           small
                           responsive="sm"
                           hover
                           show-empty
                           bordered
                           :empty-text="$t('NotFound')"
                        />
                     </b-card-text>
                  </b-tab>

                  <!-- calculationStructure -->
                  <b-tab :title="$t('organizationalstructure')" lazy>
                     <b-card-text>
                        <validation-observer ref="ValidationCalcRow">
                           <b-row>
                              <b-col sm="12" md="3">
                                 <form-select
                                    :options="OrganizationalStructureList"
                                    v-model="calculationStructureTableRow.organizationalStructureId"
                                    :label="$t('organizationalstructure')"
                                    required-star
                                    :placeholder="$t('organizationalstructure')"
                                 />
                              </b-col>

                              <b-col sm="12" md="1" class="mt-2">
                                 <b-button variant="light" @click="addCalcTableRow">
                                    <feather-icon icon="PlusIcon"></feather-icon>
                                 </b-button>
                              </b-col>
                           </b-row>
                        </validation-observer>

                        <b-table
                           :fields="fieldsCalc"
                           :items="Data.calculationStructure"
                           small
                           responsive="sm"
                           hover
                           show-empty
                           bordered
                           :empty-text="$t('NotFound')"
                        >
                           <template #cell(actions)="{ index }">
                              <div class="text-center">
                                 <b-link class="text-danger">
                                    <feather-icon
                                       @click="DeleteCalculationStructure(index)"
                                       icon="Trash2Icon"
                                    ></feather-icon>
                                 </b-link>
                              </div>
                           </template>
                        </b-table>
                     </b-card-text>
                  </b-tab>
               </b-tabs>
            </b-card>
         </b-col>

         <!-- save button -->
         <b-col cols="12" class="text-right mb-2">
            <b-button :disabled="saveLoading" @click="SaveData" variant="outline-success">
               <feather-icon icon="CheckIcon"></feather-icon>
               {{ $t('Save') }}
            </b-button>
         </b-col>
      </b-row>
   </b-overlay>
</template>
<script>
// service
import ManualService from '@/services/others/manual.service';
import CalculationKindService from '@/services/hrm/calculationkind.service';
import ItemOfExpenseService from '@/services/hrm/itemofexpense.service';
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
   BTab,
   BTabs,
   BCardText
} from 'bootstrap-vue';
import FormInputTranslate from '@/components/translates/FormInputTranslate.vue';
import OrganizationalStructureService from '@/services/info/organizationalstructure.service';

const usedTableRowDef = {
   id: 0,
   startOn: '',
   endOn: '',
   calcFromInSum: true,
   formedCalculationKindId: 0,
   formedCalculationKind: '',
   minimumValueTypeId: 0,
   minimumValueType: '',
   quantityOfMinimumValue: 0
};

const parcentTableRowDef = {
   id: 0,
   dateOn: '',
   percentRate: 0,
   details: '',
   limitOperType: '',
   limitOperTypeId: 0,
   amount: 0
};

const allowedDocsTableRowDef = {
   id: 0,
   dateOn: '',
   tableId: 0,
   stateId: 0
};

const calculationStructureTableRowDef = {
   id: 0,
   organizationalStructureId: 0,
   organizationalStructure: ''
};

export default {
   components: {
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
      BFormTextarea,
      FormInputTranslate,
      BTab,
      BTabs,
      BCardText
   },
   name: 'Edit',
   data() {
      return {
         show: false,
         MinimumValueTypeList: [],
         CalculateByTimeTypeList: [],
         CalculationMethodList: [],
         CalculationTypeList: [],
         CalculationKindList: [],
         ItemOfExpenseList: [],
         StateList: [],
         LimitOperTypeList: [],
         OrganizationalStructureList: [],
         TabrowModal: false,
         saveLoading: false,
         usedTableRow: { ...usedTableRowDef },
         parcentTableRow: { ...parcentTableRowDef },
         allowedDocsTableRow: { ...allowedDocsTableRowDef },
         calculationStructureTableRow: { ...calculationStructureTableRowDef },
         Data: {
            code: '',
            shortName: '',
            fullName: '',
            normativeDoc: '',
            itemOfExpenseId: null,
            calculationTypeId: null,
            calculationMethodId: null,
            minimumValueTypeId: null,
            calculateByTimeTypeId: null,
            stateId: null,
            dependOnRate: true,
            byEnrolment: true,
            isMandatory: true,
            translates: [],
            usedTables: [],
            percents: [],
            allowedDocs: [],
            calculationStructure: []
         },
         fieldsUsedTables: [
            {
               key: 'formedCalculationKind',
               label: this.$t('formedCalculationKindName')
            },
            {
               key: 'minimumValueType',
               label: this.$t('minimumValueType')
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
               key: 'calcFromInSum',
               label: this.$t('calcFromInSum')
            },
            {
               key: 'quantityOfMinimumValue',
               label: this.$t('quantityOfMinimumValue')
            },
            {
               key: 'actions',
               label: this.$t('actions')
            }
         ],
         fieldsAllowed: [
            {
               key: 'dateOn',
               label: this.$t('ondate')
            },
            {
               key: 'table',
               label: this.$t('table')
            },
            {
               key: 'state',
               label: this.$t('state')
            },
            {
               key: 'actions',
               label: this.$t('actions')
            }
         ],
         fieldsPercents: [
            {
               key: 'dateOn',
               label: this.$t('ondate')
            },
            {
               key: 'percentRate',
               label: this.$t('percentRate')
            },
            {
               key: 'amount',
               label: this.$t('amount')
            },
            {
               key: 'actions',
               label: this.$t('actions')
            }
         ],
         fieldsCalc: [
            {
               key: 'organizationalStructure',
               label: this.$t('organizationalstructure')
            },
            {
               key: 'actions',
               label: this.$t('actions')
            }
         ],
         fieldsTransactions: [
            {
               key: 'allowedTransaction',
               label: this.$t('allowedTransaction')
            },
            {
               key: 'accDb',
               label: this.$t('accDb')
            },
            {
               key: 'accCr',
               label: this.$t('accCr')
            },
            {
               key: 'accountStartCode',
               label: this.$t('accountStartCode')
            },
            {
               key: 'actions',
               label: this.$t('actions')
            }
         ]
      };
   },
   created() {
      this.show = true;
      CalculationKindService.Get(this.$route.params.id)
         .then((res) => {
            this.Data = res.data;
         })
         .catch((error) => {
            this.makeToast(error.response.data.errors, 'danger');
         })
         .finally(() => {
            this.show = false;
         });

      ManualService.MinimumValueTypeSelectList()
         .then((res) => {
            this.MinimumValueTypeList = res.data;
         })
         .catch((error) => {
            this.makeToast(error.response.data.errors, 'danger');
         });

      ManualService.StateSelectList()
         .then((res) => {
            this.StateList = res.data;
         })
         .catch((error) => {
            this.showApiError(error);
         });

      ManualService.CalculateByTimeTypeSelectList()
         .then((res) => {
            this.CalculateByTimeTypeList = res.data;
         })
         .catch((error) => {
            this.makeToast(error.response.data.errors, 'danger');
         });

      ManualService.CalculationMethodSelectList()
         .then((res) => {
            this.CalculationMethodList = res.data;
         })
         .catch((error) => {
            this.makeToast(error.response.data.errors, 'danger');
         });

      ManualService.CalculationTypeSelectList()
         .then((res) => {
            this.CalculationTypeList = res.data;
         })
         .catch((error) => {
            this.makeToast(error.response.data.errors, 'danger');
         });

      ManualService.LimitOperTypeSelectList()
         .then((res) => {
            this.LimitOperTypeList = res.data;
         })
         .catch((error) => {
            this.makeToast(error.response.data.errors, 'danger');
         });

      ItemOfExpenseService.GetAsSelectList()
         .then((res) => {
            this.ItemOfExpenseList = res.data;
         })
         .catch((error) => {
            this.makeToast(error.response.data.errors, 'danger');
         });

      CalculationKindService.GetAsSelectList()
         .then((res) => {
            this.CalculationKindList = res.data;
         })
         .catch((error) => {
            this.showApiError(error);
         });

      OrganizationalStructureService.GetAsSelectList().then((res) => {
         this.OrganizationalStructureList = res.data;
      });
   },
   methods: {
      DeletePercents(e) {
         this.Data.percents.splice(e, 1);
      },
      DeleteUsedTables(e) {
         this.Data.usedTables.splice(e, 1);
      },
      DeleteAllowedDocs(e) {
         this.Data.allowedDocs.splice(e, 1);
      },
      DeleteCalculationStructure(e) {
         this.Data.calculationStructure.splice(e, 1);
      },
      addUsedTableRow() {
         this.$refs.ValidationUsedTableRow.validate().then((success) => {
            if (success) {
               this.usedTableRow.minimumValueType = this.MinimumValueTypeList.find(
                  (e) => e.value == this.usedTableRow.minimumValueTypeId
               )?.text;
               this.usedTableRow.formedCalculationKind = this.CalculationKindList.find(
                  (e) => e.value == this.usedTableRow.formedCalculationKindId
               )?.text;

               this.Data.usedTables.push(this.usedTableRow);
               this.usedTableRow = Object.assign({}, usedTableRowDef);
               this.$refs.ValidationUsedTableRow.reset();
            }
         });
      },
      addParcentsTableRow() {
         this.$refs.ValidationPercentsRow.validate().then((success) => {
            if (success) {
               this.parcentTableRow.limitOperType = this.LimitOperTypeList.find(
                  (e) => e.value == this.parcentTableRow.limitOperTypeId
               )?.text;
               this.Data.percents.push(this.parcentTableRow);
               this.parcentTableRow = Object.assign({}, parcentTableRowDef);
               this.$refs.ValidationPercentsRow.reset();
            }
         });
      },
      addAllowedDocsTableRow() {
         this.$refs.ValidationAllowedDocsRow.validate().then((success) => {
            if (success) {
               this.Data.allowedDocs.push(this.allowedDocsTableRow);
               this.allowedDocsTableRow = Object.assign({}, allowedDocsTableRowDef);
               this.$refs.ValidationAllowedDocsRow.reset();
            }
         });
      },
      addCalcTableRow() {
         this.$refs.ValidationCalcRow.validate().then((success) => {
            if (success) {
               this.calculationStructureTableRow.organizationalStructure = this.OrganizationalStructureList.find(
                  (e) => e.value == this.calculationStructureTableRow.organizationalStructureId
               )?.text;

               this.Data.calculationStructure.push(this.calculationStructureTableRow);
               this.calculationStructureTableRow = Object.assign({}, calculationStructureTableRowDef);
               this.$refs.ValidationCalcRow.reset();
            }
         });
      },
      SaveData() {
         this.$refs.ValidationDTO.validate().then((success) => {
            if (success) {
               this.saveLoading = true;
               CalculationKindService.Update(this.Data)
                  .then(() => {
                     this.makeToast(this.$t('SaveSuccess'), 'success');
                     this.$router.push({ name: 'CalculationKind' });
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

<style>
.col-form-label,
label {
   line-height: initial;
}
</style>
