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
                        ></form-picker>
                     </b-col>

                     <b-col sm="12" md="4">
                        <form-select
                           v-if="!disabledType"
                           :disabled="!disabledType"
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

                  <b-row>
                     <b-col class="text-right mt-2">
                        <b-button @click="OpenTabrow" size="sm" variant="outline-primary">
                           <feather-icon icon="PlusIcon"></feather-icon>
                           {{ $t('Add') }}
                        </b-button>
                     </b-col>
                  </b-row>

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
                  <b-row class="text-center">
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

      <validation-observer ref="ValidationTabrow">
         <b-modal size="lg" v-model="TabrowModal" v-if="TabrowModal" static no-close-on-backdrop hide-footer>
            <b-row v-if="page == 'bolaparvarishi'" align-v="center">
               <b-col sm="12" md="6" class="mb-1">
                  <EmployeeManageSelect
                     :isOrganisation="$can('AllAppointEmployeeCreate', 'permissions')"
                     v-model="tabrow.employeeManageId"
                     :departmentId="tabrow.departmentId"
                     :employee="tabrow.employee"
                     :employee-id="tabrow.employeeId"
                     :label="$t('employeeManage')"
                     @update:data="onUpdateEmployeeManage"
                  />
               </b-col>
               <b-col sm="12" md="6">
                  <!-- <form-select
                     :options="DepartmentList"
                     v-model="tabrow.departmentId"
                     required-star
                     label="Department"
                  /> -->
               </b-col>
               <b-col sm="12" md="6">
                  <form-picker
                     v-model="tabrow.startOn"
                     :label="$t('startdate')"
                     required
                     :placeholder="$t('startdate')"
                     @input="calcDays"
                  ></form-picker>
               </b-col>
               <b-col sm="12" md="6">
                  <form-picker
                     v-model="tabrow.endOn"
                     required
                     :label="$t('enddate')"
                     :placeholder="$t('enddate')"
                     @input="calcDays"
                  ></form-picker>
               </b-col>

               <b-col sm="12" md="6" class="mb-1">
                  <form-picker
                     required
                     v-model="tabrow.workStartDate"
                     :label="$t('dayOffOn')"
                     :placeholder="$t('dayOffOn')"
                  ></form-picker>
               </b-col>
               <!-- <b-col sm="12" md="6" class="mb-1">
                  <form-picker
                     v-model="tabrow.forPeriodEndOn"
                     :disabled="tabrow.isWithOutPay"
                     :label="$t('forPeriodEndOn')"
                     :placeholder="$t('forPeriodEndOn')"
                  ></form-picker>
               </b-col>  -->
               <b-col sm="12" md="12">
                  <b-form-textarea
                     v-model="tabrow.details"
                     :placeholder="$t('asos')"
                     rows="2"
                     max-rows="6"
                  ></b-form-textarea>
               </b-col>
            </b-row>

            <b-row v-else-if="page == 'homiladorliktatili'" align-v="center">
               <b-col v-if="!disabledType && page != 'bolaparvarishi'" sm="12" md="6">
                  <EmployeeSickSelect
                     class="mt-2"
                     v-model="Data.employeeSickLeaveId"
                     :employee="Data.employeeNames"
                     :employee-id="Data.employeeSickLeaveTypeId"
                     @update:data="handleEmployeeSick"
                  />
               </b-col>

               <b-col sm="12" md="6">
                  <form-select
                     :options="DepartmentList"
                     v-model="tabrow.departmentId"
                     required-star
                     @change="(e) => (tabrow.department = DepartmentList.filter((item) => item.value == e)[0].text)"
                     label="Department"
                  />
               </b-col>

               <b-col sm="12" md="6">
                  <form-picker
                     v-model="tabrow.startOn"
                     :label="$t('startdate')"
                     required
                     :placeholder="$t('startdate')"
                     @input="calcDays"
                  ></form-picker>
               </b-col>
               <b-col sm="12" md="6">
                  <form-picker
                     v-model="tabrow.endOn"
                     required
                     :label="$t('enddate')"
                     :placeholder="$t('enddate')"
                     @input="calcDays"
                  ></form-picker>
               </b-col>
               <b-col sm="12" md="6">
                  <form-picker
                     v-model="tabrow.workStartDate"
                     required
                     :label="$t('dayOffOn')"
                     :placeholder="$t('dayOffOn')"
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
            </b-row>

            <b-row v-else align-v="center">
               <!-- <b-col sm="12" md="12">
                  <form-select
                     v-model="Data.organizationId"
                     required-star
                     :options="OrganizationList"
                     :label="$t('organization')"
                  />
               </b-col> -->
               <b-col sm="12" md="6" class="mb-1">
                  <EmployeeManageSelect
                     v-model="tabrow.employeeManageId"
                     :departmentId="tabrow.departmentId"
                     :isOrganisation="$can('AllAppointEmployeeCreate', 'permissions')"
                     :employee="tabrow.employee"
                     :employee-id="tabrow.employeeId"
                     :label="$t('employeeManage')"
                     @update:data="onUpdateEmployeeManage"
                  />
               </b-col>
               <b-col sm="12" md="6"> </b-col>
               <b-col sm="12" md="6">
                  <form-picker
                     v-model="tabrow.startOn"
                     :label="$t('startdate')"
                     required
                     :placeholder="$t('startdate')"
                     @input="calcDays"
                  ></form-picker>
               </b-col>
               <b-col sm="12" md="6">
                  <form-picker
                     v-model="tabrow.endOn"
                     required
                     :label="$t('enddate')"
                     :placeholder="$t('enddate')"
                     @input="calcDays"
                  ></form-picker>
               </b-col>
               <b-col sm="12" md="6" class="my-1">
                  <b-form-checkbox v-model="tabrow.isConscription" @change="tabrow.isWithOutPay = false">{{
                     $t('isConscription')
                  }}</b-form-checkbox>
               </b-col>
               <b-col sm="12" md="6" class="my-1">
                  <b-form-checkbox v-model="tabrow.isWithOutPay" @change="tabrow.isConscription = false">{{
                     $t('isWithOutPay')
                  }}</b-form-checkbox>
               </b-col>
               <b-col sm="12" md="6" v-if="!tabrow.isConscription">
                  <form-input disabled v-model="tabrow.days" :label="$t('days')" />
               </b-col>
               <b-col sm="12" md="6" v-if="!tabrow.isConscription">
                  <form-input-hrm
                     :disabled="tabrow.isWithOutPay"
                     type="number"
                     v-model="tabrow.addPayDays"
                     :label="$t('addPayDays')"
                     :placeholder="$t('addPayDays')"
                  />
               </b-col>
               <b-col sm="12" md="6" class="mb-1" v-if="!tabrow.isConscription">
                  <form-picker
                     v-model="tabrow.forPeriodStartOn"
                     :disabled="tabrow.isWithOutPay"
                     :label="$t('forPeriodStartOn')"
                     :placeholder="$t('forPeriodStartOn')"
                  ></form-picker>
               </b-col>
               <b-col sm="12" md="6" class="mb-1" v-if="!tabrow.isConscription">
                  <form-picker
                     v-model="tabrow.forPeriodEndOn"
                     :disabled="tabrow.isWithOutPay"
                     :label="$t('forPeriodEndOn')"
                     :placeholder="$t('forPeriodEndOn')"
                  ></form-picker>
               </b-col>
               <b-col sm="12" md="6">
                  <form-picker
                     v-model="tabrow.workStartDate"
                     required
                     :label="$t('dayOffOn')"
                     :placeholder="$t('dayOffOn')"
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
               <b-col md="12" class="my-1" v-if="!tabrow.isConscription">
                  <vue-editor v-model="Data.conclusionForPrint" :label="$t('content')"></vue-editor>
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
import DepartmentService from '@/services/info/department.service';
import EmployeeSickSelect from '@/views/components/hrm/EmployeeSickLeaveSelect.vue';
import VueEditor from '@/components/VueEditor.vue';
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
import EmployeeManageSelect from '@/views/components/hrm/EmployeeManageSelect.vue';
import EmployeeManageService from '@/services/hrm/employeemanage.service';
import EmployeeSickLeaveService from '@/services/hrm/employeesickleave.service';
import { sortHrmOrder } from '@/views/hrm/utils';
import ManualService from '@/services/others/manual.service';

const defaultTableRow = {
   workStartDate: '',
   detailForPrint: '',
   isConscription: false,
   id: 0,
   startOn: '',
   endOn: '',
   details: '',
   departmentId: null,
   employeeId: null,
   employeeManageId: null,
   isWithOutPay: true,
   days: 0,
   addPayDays: 0,
   forPeriodStartOn: '',
   forPeriodEndOn: '',
   diagnosis: ''
};

const HrmSigner = () => import('@/views/components/hrm/HrmSigner.vue');
const HrmSignerTableView = () => import('@/views/components/hrm/HrmSignerTableView.vue');

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
      HrmSigner,
      HrmSignerTableView,
      EmployeeSickSelect
   },
   name: 'Edit',
   props: {
      page: {
         type: String,
         default: 'index'
      },
      type: {
         type: Number,
         default: 1
      }
   },
   data() {
      return {
         disabledType: true,
         show: false,
         DepartmentList: [],
         EmployeeSickLeaveTypeSelectList: [],
         loadingButton: false,
         TabrowModal: false,
         OrganizationList: [],
         saveLoading: false,
         EmployeeSickLeaveItems: [],
         tabIndex: 1,
         filter: {
            search: '',
            sortBy: '',
            orderType: 'asc',
            page: 1,
            pageSize: 500,
            perPageOptions: [10, 20, 50, 100],
            total: 0,
            organizationId: 0,
            employeeId: null,
            departmentId: null,
            positionId: null,
            isOnlyWorkingEmployee: false
         },
         Data: {
            conclusionForPrint: '',
            employeeSickLeaveType: '',
            employeeNames: '',
            id: null,
            docNumber: null,
            docOn: null,
            details: null,
            employeeSickLeaveTypeId: null,
            employeeSickLeaveId: null,
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
               key: 'startOn',
               label: this.$t('startdate')
            },
            {
               key: 'endOn',
               label: this.$t('enddate')
            },
            {
               key: this.$props.page == 'index' ? 'isWithOutPay' : '',
               label: this.$t('isWithOutPay')
            },
            {
               key: this.$props.page == 'index' ? 'days' : '',
               label: this.$t('days')
            },
            {
               key: this.$props.page == 'index' ? 'addPayDays' : '',

               label: this.$t('addPayDays')
            },
            {
               key: this.$props.page == 'index' ? 'forPeriodStartOn' : '',
               label: this.$t('forPeriodStartOn')
            },
            {
               key: this.$props.page == 'index' ? 'forPeriodEndOn' : '',
               label: this.$t('forPeriodEndOn')
            },
            // {
            //    key: this.$props.page != 'index' ? 'dayOfStartWork' : '',
            //    label: this.$t('dayOffOn')
            // },
            {
               key: 'details',
               label: this.$t('asos')
            },

            {
               key: this.tabIndex != 3 ? 'actions' : null,
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
      EmployeeLeaveOrderService.Get(this.$route.params.id)
         .then((res) => {
            this.Data = res.data;
            if (!Array.isArray(res.data?.tables)) {
               this.Data.tables = [];
            }

            if (this.$props.page == 'bolaparvarishi') {
               this.disabledType = false;
               this.Data.employeeSickLeaveTypeId = +this.$route.query.employeeSickLeaveTypeId;
               this.Data.employeeSickLeaveTypeId = 1;
            } else if (this.$props.page == 'homiladorliktatili') {
               this.Data.employeeSickLeaveTypeId = +this.$route.query.employeeSickLeaveTypeId;
               this.disabledType = false;
               this.Data.employeeSickLeaveTypeId = 2;
            }
         })
         .catch((error) => {
            this.showApiError(error);
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

      ManualService.OrganizationAsSelectListByGroup([1, 3]).then((res) => {
         this.OrganizationList = res.data;
      });

      DepartmentService.GetAsSelectList(null, {})
         .then((res) => {
            if (Array.isArray(res.data)) {
               this.DepartmentList = res.data;
            }
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
   },
   computed: {
      isView() {
         return this.$route.params.isView;
      },
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
      },
      handleName() {
         let a = 'EmployeeLeaveOrder';
         if (this.$props.page === 'bolaparvarishi') {
            a = 'Bolaparvarishi';
         } else if (this.$props.page === 'homiladorliktatili') {
            a = 'Homiladorliktatili';
         } else {
            a = 'EmployeeLeaveOrder';
         }
         return a;
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
      calcDays() {
         if (this.tabrow.startOn && this.tabrow.endOn && this.tabrow.employeeManageId) {
            EmployeeLeaveOrderService.GetCalculatedDays({
               employeeManageId: this.tabrow.employeeManageId,
               startDate: this.tabrow.startOn,
               endDate: this.tabrow.endOn
            })
               .then((res) => {
                  this.tabrow.days = res.data || 0;
               })
               .catch((error) => {
                  this.showApiError(error);
               });
         } else {
            this.tabrow.days = 0;
         }
      },
      onUpdateEmployeeManage(e) {
         this.tabrow.employeeId = e?.employeeId;
         this.tabrow.employee = e?.employee;
         this.tabrow.employeeManageId = e?.employeeManageId;
         this.tabrow.departmentId = e?.departmentId;
         this.tabrow.department = e?.department;
         this.Data.departmentId = e?.departmentId;
         this.Data.department = e?.department;
         this.Data.organizationId = e?.organizationId;
         this.calcDays();
      },
      handleEmployeeSick(e) {
         EmployeeSickLeaveService.Get(e.id).then((res) => {
            this.tabrow.startOn = res.data.tables[0].startOn;
            this.tabrow.giveOn = res.data.tables[0].giveOn;
            this.tabrow.endOn = res.data.tables[0].endOn;
            // this.tabrow.departmentId = res.data.tables[0].departmentId;
            this.tabrow.employeeId = res.data.tables[0].employeeId;
            this.tabrow.employeeManageId = res.data.tables[0].employeeManageId;

            this.tabrow.employee = res.data.tables[0].employee;
         });
         this.Data.employeeNames = e?.employeeNames;
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
               // if (this.tabrow.departmentId) {
               //    this.tabrow.department = this.DepartmentList.find((e) => e.value == this.tabrow.departmentId)?.text;
               // }

               if (this.editedIndex1 > -1) {
                  Object.assign(this.Data.tables[this.editedIndex1], this.tabrow);
               } else {
                  console.log(this.tabrow);
                  this.Data.tables.push(this.tabrow);
                  this.Data.employeeNames = '';
               }
               this.TabrowModal = false;
            }
         });
      },
      SaveData() {
         this.$refs.ValidationDTO.validate().then((success) => {
            if (success) {
               this.saveLoading = true;
               EmployeeLeaveOrderService.Update({
                  ...this.Data,
                  employeeOrderTypeId: this.$props.type,
                  signer: sortHrmOrder(this.Data.signer)
               })
                  .then((res) => {
                     this.makeToast(this.$t('SaveSuccess'), 'success');
                     this.$router.push({
                        name: this.handleName
                     });
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
