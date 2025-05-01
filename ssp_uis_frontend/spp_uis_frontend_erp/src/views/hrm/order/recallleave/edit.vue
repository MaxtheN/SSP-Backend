<template>
   <b-overlay :show="show">
      <b-card>
         <validation-observer ref="ValidationDTO">
            <b-tabs v-model="tabIndex" small class="nav-tabs" nav-wrapper-class="pb-0 d-flex justify-content-center">
               <b-tab :title="$t('MainPart')" active lazy>
                  <b-row>
                     <b-col sm="12" md="4">
                        <div class="form-group">
                           <form-input v-model="Data.docNumber" required :label="$t('docnumber')" />
                        </div>
                     </b-col>
                     <b-col sm="12" md="4">
                        <form-picker
                           v-model="Data.docOn"
                           required
                           :label="$t('docdate')"
                           :placeholder="$t('docdate')"
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

                  <!-- tables -->
                  <b-row v-if="!isView">
                     <b-col class="text-right mt-2">
                        <b-button @click="OpenTabrow" size="sm" variant="outline-primary">
                           <feather-icon icon="PlusIcon"></feather-icon>
                           {{ $t('Add') }}
                        </b-button>
                     </b-col>
                  </b-row>
                  <b-table
                     class="mt-2"
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

                  <b-row class="text-center">
                     <b-col>
                        <b-button
                           :disabled="!Data.tables || Data.tables.length === 0"
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

               <!-- buyruq matni -->
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

               <!-- signer -->
               <b-tab
                  :disabled="!Data.tables || Data.tables.length === 0 || !validInput"
                  :title="$t('Signatories')"
                  lazy
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
                  :title="$t('Formalization')"
                  lazy
                  :disabled="!Data.tables || Data.tables.length === 0 || !validInput"
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

      <validation-observer ref="ValidationTabrow">
         <b-modal size="lg" v-model="TabrowModal" v-if="TabrowModal" static no-close-on-backdrop hide-footer>
            <b-row>
               <b-col sm="12" md="6">
                  <EmployeeManageSelect
                     :isOrganisation="$can('AllAppointEmployeeCreate', 'permissions')"
                     v-model="tabrow.employeeManageId"
                     :label="$t('employeeManage')"
                     @update:data="onUpdateEmployeeManage"
                     required
                     :employee="tabrow.employee"
                  />
               </b-col>
               <b-col sm="12" md="6">
                  <form-select
                     v-model="tabrow.employeeLeaveOrderId"
                     :disabled="!tabrow.employeeId"
                     :options="EmployeeLeaveOrderList"
                     :label="$t('employeeleaveorder')"
                     @input="getEmployeeLeaveTableList"
                     required-star
                  />
               </b-col>
               <b-col sm="12" md="6" v-show="false">
                  <form-select
                     v-model="tabrow.employeeLeaveOrderTableId"
                     :disabled="!tabrow.employeeId || !tabrow.employeeLeaveOrderId"
                     :options="EmployeeLeaveOrderTableList"
                     required-star
                     :label="$t('employeeLeaveOrderTable')"
                  />
               </b-col>
               <b-col sm="12" md="6">
                  <form-input-hrm :label="$t('department')" disabled v-model="tabrow.department" />
               </b-col>
               <b-col sm="12" md="6">
                  <form-input-hrm :label="$t('position')" disabled v-model="tabrow.position" />
               </b-col>

               <b-col sm="12" md="6">
                  <form-picker
                     v-model="tabrow.forPeriodStartOn"
                     disabled
                     :label="$t('forPeriodStartOn')"
                     :placeholder="$t('forPeriodStartOn')"
                  ></form-picker>
               </b-col>
               <b-col sm="12" md="6">
                  <form-picker
                     v-model="tabrow.forPeriodEndOn"
                     disabled
                     :label="$t('forPeriodEndOn')"
                     :placeholder="$t('forPeriodEndOn')"
                  ></form-picker>
               </b-col>
               <b-col sm="12" md="6">
                  <form-input-hrm disabled v-model="tabrow.addPayDays" :label="$t('addPayDays')" />
               </b-col>
               <b-col sm="12" md="6">
                  <form-picker v-model="tabrow.startOn" required :label="$t('startOn')" :placeholder="$t('startOn')" />
               </b-col>
               <b-col sm="12" md="6">
                  <form-picker
                     v-model="tabrow.workStartDate"
                     required
                     :label="$t('startWorkDate')"
                     :placeholder="$t('startWorkDate')"
                  />
               </b-col>
               <b-col sm="12" md="12">
                  <b-form-textarea
                     v-model="tabrow.details"
                     :placeholder="$t('asos')"
                     rows="2"
                     max-rows="6"
                  ></b-form-textarea>
               </b-col>
               <b-col md="12" class="my-1">
                  <form-input v-model="Data.conclusionForPrint" :label="$t('orderDetailsste')" />
                  <!-- <vue-editor v-model="Data.conclusionForPrint" :label="$t('content')"></vue-editor> -->
               </b-col>
            </b-row>
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
      </validation-observer>
   </b-overlay>
</template>
<script>
// service
import EmployeeLeaveOrderService from '@/services/hrm/employeeleaveorder.service';
import RecallLeaveService from '@/services/hrm/recallleave.service';
// components
import { BOverlay, BCard, BRow, BCol, BTable, BFormTextarea, BButton, BLink, BTabs, BTab, BModal } from 'bootstrap-vue';
import EmployeeManageSelect from '@/views/components/hrm/EmployeeManageSelect.vue';
import EmployeeManageService from '@/services/hrm/employeemanage.service';
import { sortHrmOrder } from '@/views/hrm/utils';
import { VueEditor } from 'vue2-editor';

const HrmSigner = () => import('@/views/components/hrm/HrmSigner.vue');
const HrmSignerTableView = () => import('@/views/components/hrm/HrmSignerTableView.vue');

const defaultTableRow = {
   detailForPrint: '',
   id: 0,
   employeeManageId: 0,
   employeeId: 0,
   employeeLeaveOrderId: 0,
   employeeLeaveOrderTableId: 0,
   startOn: '',
   details: ''
};

export default {
   components: {
      VueEditor,
      BOverlay,
      BCard,
      BRow,
      BCol,
      BButton,
      BTable,
      BLink,
      EmployeeManageSelect,
      BTabs,
      BTab,
      BModal,
      HrmSigner,
      HrmSignerTableView,
      BFormTextarea
   },
   name: 'RecallLeaveEdit',
   data() {
      return {
         show: false,
         EmployeeLeaveOrderTableList: [],
         EmployeeLeaveOrderList: [],
         loadingButton: false,
         saveLoading: false,
         Data: {
            id: 0,
            conclusionForPrint: '',

            docNumber: null,
            docOn: null,
            details: '',
            tables: [],
            signer: []
         },
         tabIndex: 1,
         TabrowModal: false,
         tabrow: { ...defaultTableRow },
         TablesField: [
            {
               key: 'department',
               label: this.$t('Department'),
               sortable: true
            },
            {
               key: 'position',
               label: this.$t('position'),
               sortable: true
            },
            {
               key: 'employee',
               label: this.$t('employee'),
               sortable: true
            },
            {
               key: 'employeeLeaveOrderId',
               label: this.$t('employeeleaveorder'),
               sortable: true
            },
            {
               key: 'employeeLeaveOrderTableId',
               label: this.$t('employeeLeaveOrderTable'),
               sortable: true
            },
            {
               key: 'startOn',
               label: this.$t('startdate'),
               sortable: true
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
      RecallLeaveService.Get(this.$route.params.id)
         .then((res) => {
            this.Data = res.data;
            if (!Array.isArray(res.data?.tables)) {
               this.Data.tables = [];
            }
         })
         .catch((error) => {
            this.showApiError(error);
         })
         .finally(() => {
            this.show = false;
         });
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
                  });
                  this.TabrowModal = true;
               }
            }
         },
         immediate: true
      }
   },
   computed: {
      validInput() {
         const { docNumber, docOn, organizationId, details } = this.Data;

         const allInputsFilled = docNumber && docOn && details;

         return allInputsFilled;
      },
      isView() {
         return this.$route.params.isView;
      },
      signerHr() {
         return this.Data.signer.find((e) => e.isHr);
      },
      signerDirector() {
         return this.Data.signer.find((e) => e.isDirector);
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
      OpenTabrow() {
         this.TabrowModal = true;
         this.tabrow = { ...defaultTableRow };
         this.editedIndex1 = -1;
      },
      async getEmployeeLeaveTableList() {
         if (this.tabrow.employeeId && this.tabrow.employeeLeaveOrderId) {
            await EmployeeLeaveOrderService.GetTableAsSelectList({
               employeeId: this.tabrow.employeeId,
               employeeLeaveOrderId: this.tabrow.employeeLeaveOrderId
            })
               .then((res) => {
                  this.EmployeeLeaveOrderTableList = res.data;
                  this.onUpdateemployeeLeaveOrderTable(res.data[0]);
               })
               .catch((error) => {
                  this.showApiError(error);
               });
         }
      },
      async getEmployeeLeaveList() {
         if (this.tabrow.employeeId) {
            await EmployeeLeaveOrderService.GetAsSelectList({
               employeeId: this.tabrow.employeeId
            })
               .then((res) => {
                  this.EmployeeLeaveOrderList = res.data;
               })
               .catch((error) => {
                  this.showApiError(error);
               });
         }
      },
      onUpdateemployeeLeaveOrderTable(e) {
         this.tabrow.forPeriodStartOn = e?.startOn;
         this.tabrow.forPeriodEndOn = e?.endOn;
         this.tabrow.addPayDays = e?.addPayDays;
         this.tabrow.position = e?.position;
         this.tabrow.department = e?.department;
         this.tabrow.employeeLeaveOrderTableId = e?.value;
         console.log(this.tabrow);
      },
      onUpdateEmployeeManage(e) {
         this.tabrow.employee = e?.employee;
         this.tabrow.employeeId = e?.employeeId;
         this.tabrow.department = e?.department;
         this.tabrow.employeeLeaveOrderTableId = null;
         this.tabrow.employeeLeaveOrderId = null;
         this.getEmployeeLeaveList();
      },
      DeleteTabrow(index) {
         this.Data.tables.splice(index, 1);
      },
      EditTabrow(item) {
         this.editedIndex1 = this.Data.tables.indexOf(item);
         this.tabrow = Object.assign({}, item);
         this.getEmployeeLeaveTableList();
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
               this.$refs.ValidationTabrow.reset();
               this.tabrow = { ...defaultTableRow };
               this.TabrowModal = false;
            }
         });
      },
      SaveData() {
         this.$refs.ValidationDTO.validate().then((success) => {
            if (success) {
               this.saveLoading = true;
               RecallLeaveService.Update({ ...this.Data, signer: sortHrmOrder(this.Data.signer) })
                  .then((res) => {
                     this.makeToast(this.$t('SaveSuccess'), 'success');
                     this.$router.push({ name: 'RecallLeave' });
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
<style scoped>
input {
   margin: 0.4rem;
}
</style>
