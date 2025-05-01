<template>
   <b-overlay :show="show">
      <b-card>
         <validation-observer ref="ValidationDTO">
            <b-tabs
               v-model="tabIndex"
               card
               small
               class="nav-tabs"
               nav-wrapper-class="pb-0 d-flex justify-content-center"
            >
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

                     <b-col sm="12" md="12">
                        <b-form-textarea
                           id="textarea"
                           v-model="Data.details"
                           :placeholder="$t('orderDetails')"
                           rows="2"
                           max-rows="6"
                        ></b-form-textarea>
                     </b-col>
                  </b-row>
                  <b-row v-if="!isView">
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
                           bordered
                           small
                           responsive
                           :items="Data.tables"
                           hover
                           show-empty
                           :empty-text="$t('NotFound')"
                        >
                           <template #cell(actions)="{ item, index }">
                              <div class="text-center" v-if="!isView">
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
                           <template #cell(isProbation)="{ item }">
                              <feather-icon icon="CheckCircleIcon" v-if="item.isProbation" class="text-success" />
                              <feather-icon icon="XCircleIcon" v-else class="text-danger" />
                           </template>
                           <template #cell(interm)="{ item }">
                              <feather-icon icon="CheckCircleIcon" v-if="item.interm" class="text-success" />
                              <feather-icon icon="XCircleIcon" v-else class="text-danger" />
                           </template>
                           <template #cell(acting)="{ item }">
                              <feather-icon icon="CheckCircleIcon" v-if="item.acting" class="text-success" />
                              <feather-icon icon="XCircleIcon" v-else class="text-danger" />
                           </template>
                           <template #cell(position)="{ item }">
                              {{ item.position || item.fromPosition }}
                           </template>
                           <template #cell(department)="{ item }">
                              {{ item.department || item.fromDepartment }}
                           </template>
                           <template #cell(choosenEmployee)="{ item }">
                              {{ item.choosenEmployee ? item.choosenEmployee : '-' }}
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
                  :disabled="!Data.tables || Data.tables.length === 0 || !validInput"
                  v-if="!$can('AppointEmployeeWithoutSigner', 'permissions')"
                  :title="$t('Signatories')"
                  lazy
               >
                  <HrmSigner
                     :signer.sync="Data.signer"
                     :organization-id="Data.organizationId"
                     @back="Back"
                     @continue="Continue"
                  />
               </b-tab>

               <b-tab
                  :disabled="!Data.tables || Data.tables.length === 0 || !validInput || !tabrow.detailForPrint"
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
                           bordered
                           small
                           responsive
                           :items="Data.tables"
                           hover
                           show-empty
                           :empty-text="$t('NotFound')"
                        >
                           <template #cell(isProbation)="{ item }">
                              <feather-icon icon="CheckCircleIcon" v-if="item.isProbation" class="text-success" />
                              <feather-icon icon="XCircleIcon" v-else class="text-danger" />
                           </template>
                           <template #cell(interm)="{ item }">
                              <feather-icon icon="CheckCircleIcon" v-if="item.interm" class="text-success" />
                              <feather-icon icon="XCircleIcon" v-else class="text-danger" />
                           </template>
                           <template #cell(acting)="{ item }">
                              <feather-icon icon="CheckCircleIcon" v-if="item.acting" class="text-success" />
                              <feather-icon icon="XCircleIcon" v-else class="text-danger" />
                           </template>
                           <template #cell(position)="{ item }">
                              {{ item.position || item.fromPosition }}
                           </template>
                           <template #cell(department)="{ item }">
                              {{ item.department || item.fromDepartment }}
                           </template>
                        </b-table>
                     </b-col>
                  </b-row>
                  <div
                     class="d-flex justify-content-between"
                     v-if="!$can('AppointEmployeeWithoutSigner', 'permissions')"
                  >
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

                  <div class="mt-2" v-if="!$can('AppointEmployeeWithoutSigner', 'permissions')">
                     <HrmSignerTableView :signer="Data.signer" />
                  </div>

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
               <b-col
                  sm="12"
                  md="12"
                  v-if="
                     [1, 3].includes(tabrow.empAppointOrderTypeId) && $can('AllAppointEmployeeCreate', 'permissions')
                  "
               >
                  <form-select
                     v-model="Data.organizationId"
                     :options="OrganizationList"
                     required-star
                     @input="getDepartList"
                     :label="$t('organization')"
                  />
               </b-col>
               <b-col sm="12" md="6">
                  <form-select
                     :options="EmpAppointOrderTypeList"
                     v-model="tabrow.empAppointOrderTypeId"
                     required-star
                     label="empAppointOrderType"
                     @change="onChangeEmpAppointOrderType"
                  ></form-select>
               </b-col>
               <b-col sm="12" md="6">
                  <EmployeeManageSelect
                     v-if="[2, 3, 4].includes(tabrow.empAppointOrderTypeId)"
                     v-model="tabrow.fromEmployeeManageId"
                     :label="$t('employeeManage')"
                     :organizationId="Data.organizationId"
                     :isOrganisation="true"
                     :employee="tabrow.employeeFull"
                     :employee-id="tabrow.employeeId"
                     @update:data="onUpdateEmployeeManage"
                     required-star
                  />
                  <b-form-checkbox
                     v-if="[1, 2, 4].includes(tabrow.empAppointOrderTypeId)"
                     class="my-1"
                     v-model="tabrow.acting"
                     name="some-radios"
                     @input="(e) => (e ? (tabrow.interm = false) : null)"
                     >{{ $t('acting') }}</b-form-checkbox
                  >
                  <!-- <input type="checkbox" v-model="tabrow.interm" @input="ChangeBool" /> -->
                  <b-form-checkbox
                     v-if="[2, 4].includes(tabrow.empAppointOrderTypeId)"
                     class="my-1"
                     name="some-radios"
                     v-model="tabrow.interm"
                     @change="
                        (e) => {
                           e ? (tabrow.acting = false) : null;
                           tabrow.employeeRate = 0;
                        }
                     "
                     >{{ $t('interm') }}</b-form-checkbox
                  >
               </b-col>

               <b-col sm="12" md="6">
                  <EmployeeSelect2
                     v-if="[1].includes(tabrow.empAppointOrderTypeId)"
                     v-model="tabrow.employeeId"
                     :organisation="Data.organizationId"
                     @update:data="onUpdateEmployee"
                     required-star
                  />
               </b-col>

               <b-col sm="12" md="6">
                  <b-form-checkbox
                     class="mb-2"
                     v-if="[1].includes(tabrow.empAppointOrderTypeId)"
                     v-model="tabrow.isProbation"
                     >{{ $t('isProbation') }}</b-form-checkbox
                  >
               </b-col>

               <template v-if="[1, 2, 4].includes(tabrow.empAppointOrderTypeId)">
                  <b-col sm="12" md="6">
                     <form-select
                        :disabled="[4].includes(tabrow.empAppointOrderTypeId)"
                        :options="EmploymentTypeList"
                        v-model="tabrow.employmentTypeId"
                        required-star
                        label="employmentType"
                        @option:selected="(e) => (tabrow.employmentType = e ? e.text : '')"
                     ></form-select>
                  </b-col>
                  <b-col sm="12" md="6">
                     <form-select
                        :disabled="[4].includes(tabrow.empAppointOrderTypeId)"
                        :options="WorkScheduleList"
                        v-model="tabrow.workScheduleId"
                        required-star
                        label="workSchedule"
                        @option:selected="(e) => (tabrow.workSchedule = e ? e.text : '')"
                     ></form-select>
                  </b-col>
                  <b-col sm="12" v-if="[2].includes(tabrow.empAppointOrderTypeId)" md="6"></b-col>
                  <b-col
                     sm="12"
                     md="6"
                     v-if="
                        $can('AllAppointEmployeeCreate', 'permissions') && [2].includes(tabrow.empAppointOrderTypeId)
                     "
                  >
                     <form-select
                        v-model="Data.organizationId"
                        :options="OrganizationList"
                        @input="getDepartList2"
                        :label="$t('organization')"
                     />
                  </b-col>
               </template>

               <b-col sm="12" md="6">
                  <form-select
                     v-if="[2, 3].includes(tabrow.empAppointOrderTypeId)"
                     disabled
                     :options="DepartmentList"
                     v-model="tabrow.fromDepartmentId"
                     label="fromDepartment"
                     @option:selected="(e) => (tabrow.fromDepartment = e ? e.text : '')"
                  ></form-select>
               </b-col>

               <b-col sm="12" md="6">
                  <form-select
                     v-if="[1, 2, 4].includes(tabrow.empAppointOrderTypeId)"
                     :options="DepartmentList"
                     v-model="tabrow.departmentId"
                     @change="ChangeDepartment"
                     label="Department"
                     @input="GetStaffLimit"
                     required-star
                     @option:selected="(e) => (tabrow.department = e ? e.text : '')"
                  ></form-select>

                  <EmployeeManageSelect
                     :disabled="!tabrow.employeeId"
                     v-if="[3].includes(tabrow.empAppointOrderTypeId)"
                     v-model="tabrow.choosenEmployeemanageId"
                     :label="$t('acting1')"
                     :employee="tabrow.choosenEmployee"
                     :employee-id="tabrow.choosenEmployeemanageId"
                     @update:data="onUpdateChoosenEmployeeManage"
                  />
               </b-col>

               <b-col sm="12" md="6">
                  <form-select
                     v-if="[2, 3].includes(tabrow.empAppointOrderTypeId)"
                     disabled
                     :options="PositionListFilter(tabrow.fromDepartmentId)"
                     v-model="tabrow.fromPositionId"
                     label="fromPosition"
                     @option:selected="(e) => (tabrow.fromPosition = e ? e.text : '')"
                     valueid="positionId"
                     valuename="positionName"
                  ></form-select>
               </b-col>

               <b-col sm="12" md="6">
                  <form-select
                     v-if="[1, 2, 4].includes(tabrow.empAppointOrderTypeId)"
                     :options="PositionListFilter(tabrow.departmentId)"
                     v-model="tabrow.positionId"
                     label="position"
                     valueid="positionId"
                     valuename="positionName"
                     @input="GetStaffLimit"
                     required-star
                     @option:selected="handleSelectPosition"
                  ></form-select>
               </b-col>

               <b-col sm="12" md="6" class="mb-1">
                  <form-input
                     v-if="[2, 3].includes(tabrow.empAppointOrderTypeId)"
                     disabled
                     v-model="tabrow.fromEmployeeRate"
                     required
                     type="number"
                     :label="$t('fromEmployeeRate')"
                  />
               </b-col>

               <b-col sm="12" md="6" class="mb-1">
                  <form-input-hrm
                     :style="staffLimit < tabrow.employeeRate ? 'color:red' : ''"
                     v-if="[1, 2, 4].includes(tabrow.empAppointOrderTypeId)"
                     :rules="tabrow.interm ? '' : `required|max_value:${staffLimit}`"
                     v-model="tabrow.employeeRate"
                     type="number"
                     :disabled="tabrow.interm"
                     @input="() => GetEmployeeRate"
                     :placeholder="staffLimit + ''"
                     :label="$t('employeeRate')"
                  />
               </b-col>
            </b-row>
            <b-row>
               <b-col sm="12" md="6">
                  <form-picker
                     v-model="tabrow.startOn"
                     @input="(e) => (tabrow.probationStartDate = e)"
                     :label="$t('startdate')"
                     required
                     :cdisabled-date="tabrow.cdisabledDate"
                     :placeholder="$t('startdate')"
                  ></form-picker>
               </b-col>
               <b-col sm="12" md="6">
                  <form-picker
                     v-if="[2, 4].includes(tabrow.empAppointOrderTypeId)"
                     v-model="tabrow.endOn"
                     :label="$t('enddate')"
                     :placeholder="$t('enddate')"
                  ></form-picker>
               </b-col>

               <template v-if="tabrow.isProbation">
                  <b-col sm="12" md="6">
                     <form-picker
                        v-model="tabrow.probationStartDate"
                        :label="$t('probationStartDate')"
                        required
                        disabled
                        :placeholder="$t('probationStartDate')"
                     ></form-picker>
                  </b-col>
                  <b-col sm="12" md="6">
                     <form-picker
                        v-model="tabrow.probationEndDate"
                        :label="$t('probationEndDate')"
                        required
                        :placeholder="$t('probationEndDate')"
                     ></form-picker>
                  </b-col>
               </template>
            </b-row>
            <b-row>
               <b-col class="my-1">
                  <form-input v-model="Data.conclusionForPrint" :label="$t('orderDetails')" />

                  <!-- <vue-editor v-model="Data.conclusionForPrint" :label="$t('content')"></vue-editor> -->
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
import ManualService from '@/services/others/manual.service';
import AppointEmployeeService from '@/services/hrm/appointemployee.service';
import DepartmentService from '@/services/info/department.service';
import WorkScheduleService from '@/services/info/workschedule.service';
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
import VueEditor from '@/components/VueEditor.vue';
import EmployeeManageSelect from '@/views/components/hrm/EmployeeManageSelect.vue';
import EmployeeSelect2 from '@/views/components/employee/EmployeeSelect2.vue';
import StaffingService from '@/services/hrm/staffing.service';
import EmployeeManageService from '@/services/hrm/employeemanage.service';
import { sortHrmOrder } from '@/views/hrm/utils';

const HrmSigner = () => import('@/views/components/hrm/HrmSigner.vue');
const HrmSignerTableView = () => import('@/views/components/hrm/HrmSignerTableView.vue');

const defaultTableRow = {
   detailForPrint: '',
   id: 0,
   startOn: '',
   endOn: '',
   empAppointOrderTypeId: 1,
   empAppointOrderType: '',
   details: '',
   departmentId: null,
   department: null,
   positionId: null,
   position: null,
   employeeId: null,
   employmentTypeId: null,
   employmentType: '',
   employeeRate: 0,
   workScheduleId: null,
   employeeManageId: null,
   fromDepartmentId: null,
   fromDepartment: null,
   fromPositionId: null,
   fromPosition: null,
   fromEmployeeManageId: null,
   fromEmployeeRate: null,
   probationStartDate: '',
   probationEndDate: '',
   isProbation: false,
   choosenEmployeemanageId: null,
   choosenEmployee: null,
   interm: false,
   acting: false
};

export default {
   components: {
      BOverlay,
      VueEditor,
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
      EmployeeSelect2,
      BTabs,
      BTab,
      HrmSigner,
      HrmSignerTableView
   },
   data() {
      return {
         orgId: null,
         show: false,
         PositionList: [],
         OrganizationList: [],
         DepartmentList: [],
         EmploymentTypeList: [],
         EmployeeManageList: [],
         WorkScheduleList: [],
         EmpAppointOrderTypeList: [],

         loadingButton: false,
         TabrowModal: false,
         saveLoading: false,

         tabIndex: 1,
         staffLimit: null,
         employeeRate: null,
         TablesField: [
            {
               key: 'employeeFull',
               label: this.$t('employee'),
               sortable: true
            },
            {
               key: 'empAppointOrderType',
               label: this.$t('empAppointOrderType'),
               sortable: true
            },
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
               key: 'employmentType',
               label: this.$t('employmentType'),
               sortable: true
            },
            {
               key: 'workSchedule',
               label: this.$t('workSchedule')
            },
            {
               key: 'isProbation',
               label: this.$t('isProbation'),
               thClass: 'text-center',
               tdClass: 'text-center'
            },
            {
               key: 'interm',
               label: this.$t('interm'),
               thClass: 'text-center',
               tdClass: 'text-center'
            },
            {
               key: 'acting',
               label: this.$t('acting'),
               thClass: 'text-center',
               tdClass: 'text-center'
            },
            {
               key: 'choosenEmployee',
               label: this.$t('acting1'),
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
               key: 'employeeRate',
               label: this.$t('employeeRate'),
               sortable: true
            },
            {
               key: this.tabIndex != 3 ? 'actions' : null,
               label: this.$t('actions'),
               thClass: 'text-center',
               tdClass: 'text-center',
               sortable: true
            }
         ],
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
            id: null,
            docNumber: null,
            docOn: null,
            details: null,
            signer: []
         },
         tabrow: { ...defaultTableRow }
      };
   },
   async created() {
      this.show = true;

      await AppointEmployeeService.Get(this.$route.params.id)
         .then((res) => {
            this.Data = { ...res.data, organizationId: this.orgId ? this.orgId : res.data.organizationId };
            console.log(this.Data, 'getcret');
            this.GetStaffLimit();
         })
         .catch((error) => {
            this.showApiError(error);
         })
         .finally(() => {
            this.show = false;
         });
      DepartmentService.GetAsSelectList(this.Data.organizationId, {})
         .then((res) => {
            if (Array.isArray(res.data)) {
               this.DepartmentList = res.data;
            }
         })
         .catch((error) => {
            this.showApiError(error);
         });
      StaffingService.GetStaffingPositionClassification(null, null, null, this.Data.organizationId).then((res) => {
         this.PositionList = res.data;
      });
      ManualService.OrganizationAsSelectListByGroup([1, 3]).then((res) => {
         this.OrganizationList = res.data;
      });
      WorkScheduleService.GetAsSelectList()
         .then((res) => {
            if (Array.isArray(res.data)) {
               this.WorkScheduleList = res.data.filter((e) => e.value != 14);
            }
         })
         .catch((error) => {
            this.showApiError(error);
         });
      EmployeeManageService.GetList(this.filter)
         .then((res) => {
            this.EmployeeManageList = res.data.rows;
         })
         .finally(() => {
            this.isBusy = false;
         });
      ManualService.EmpAppointOrderTypeSelectList()
         .then((res) => {
            this.EmpAppointOrderTypeList = res.data;
         })
         .catch((error) => {
            this.showApiError(error);
         });

      ManualService.EmploymentTypeSelectList()
         .then((res) => {
            this.EmploymentTypeList = res.data;
         })
         .catch((error) => {
            this.showApiError(error);
         });
   },
   watch: {
      '$route.query': {
         handler(val) {
            if (val) {
               if (val.positionId && val.departmentId) {
                  this.tabrow.departmentId = val.departmentId;
                  this.tabrow.positionId = val.positionId;
                  this.TabrowModal = true;
               }
               if (val.empAppointOrderTypeId) {
                  this.tabrow.empAppointOrderTypeId = +val.empAppointOrderTypeId;
                  if (val.employeeManageId) {
                     EmployeeManageService.Get(val.employeeManageId).then((res) => {
                        const { data } = res;
                        this.tabrow.fromEmployeeManageId = data.id;
                        this.tabrow.employeeId = data.employeeId;
                        this.tabrow.employeeFull = data.employee;
                        this.tabrow.workScheduleId = data.workScheduleId;
                        this.tabrow.employmentTypeId = data.employmentTypeId;
                        this.orgId = data.organizationId;

                        // Ishdan bo‘shatish
                        if ([2, 3, 4].includes(this.tabrow.empAppointOrderTypeId)) {
                           this.tabrow.fromPositionId = data.positionId;
                           this.tabrow.fromPosition = data.position;
                           this.tabrow.fromDepartmentId = data.departmentId;
                           this.tabrow.fromDepartment = data.department;
                           this.tabrow.fromEmployeeRate = data.employmentRate;
                        }
                     });
                  }
                  this.TabrowModal = true;
               }
            }
         },
         immediate: true
      }
   },
   computed: {
      PositionListFilter() {
         return (departmentId) => {
            return departmentId ? this.PositionList.filter((e) => e.departmentId == departmentId) : this.PositionList;
         };
      },
      signerHr() {
         return this.Data.signer.find((e) => e.isHr);
      },
      signerDirector() {
         return this.Data.signer.find((e) => e.isDirector);
      },
      validInput() {
         const { docNumber, docOn, organizationId, details } = this.Data;

         const allInputsFilled =
            docNumber && docOn && (organizationId || !this.$can('AllAppointEmployeeCreate', 'permissions'));

         return allInputsFilled;
      },

      isView() {
         return this.$route.params.isView;
      }
   },
   methods: {
      ChangeBool(e) {
         e ? (this.tabrow.acting = false) : null;
         this.tabrow.employeeRate = 0;
      },
      getDepartList(e) {
         this.tabrow.fromEmployeeRate = null;
         this.tabrow.employeeFull = '';
         this.tabrow.fromPositionId = null;
         this.tabrow.employeeId = null;
         this.tabrow.fromPosition = null;
         this.tabrow.fromDepartmentId = null;
         this.tabrow.fromDepartment = null;
         DepartmentService.GetAsSelectList(this.Data.organizationId, {})
            .then((res) => {
               if (Array.isArray(res.data)) {
                  this.DepartmentList = res.data;
               }
            })
            .catch((error) => {
               this.showApiError(error);
            });
         StaffingService.GetStaffingPositionClassification(null, null, null, this.Data.organizationId).then((res) => {
            this.PositionList = res.data;
         });
      },
      getDepartList2(e) {
         DepartmentService.GetAsSelectList(this.Data.organizationId, {})
            .then((res) => {
               if (Array.isArray(res.data)) {
                  this.DepartmentList = res.data;
               }
            })
            .catch((error) => {
               this.showApiError(error);
            });
         StaffingService.GetStaffingPositionClassification(null, null, null, this.Data.organizationId).then((res) => {
            this.PositionList = res.data;
         });
      },
      GetEmployeeRate() {
         if (this.tabrow.employeeRate && this.Data.docOn && this.tabrow.employeeId && !this.tabrow.fromPositionId) {
            AppointEmployeeService.CheckEmploymentRateBeforSave(
               this.tabrow.employeeId,
               this.Data.docOn,
               this.tabrow.employeeRate,
               0
            )
               .then((res) => {
                  this.employeeRate = res.data;
               })
               .catch((error) => {
                  console.log(error.response.data.errors['']);
                  this.makeToast(error.response.data.errors[''], 'danger');
               });
         }
         if (this.tabrow.employeeRate && this.Data.docOn && this.tabrow.employeeId && this.tabrow.fromPositionId) {
            AppointEmployeeService.CheckEmploymentRateBeforSave(
               this.tabrow.employeeId,
               this.Data.docOn,
               this.tabrow.employeeRate,
               this.tabrow.fromPositionId
            )
               .then((res) => {
                  this.employeeRate = res.data;
               })
               .catch((error) => {
                  this.showApiError(error);
               });
         }
      },

      Continue() {
         if (this.tabIndex != 3) {
            if (!this.$can('AppointEmployeeWithoutSigner', 'permissions')) {
               this.tabIndex += 1;
            } else {
               this.tabIndex += 1;
            }
         }
      },
      Back() {
         console.log(this.tabIndex);
         if (this.tabIndex != 0) {
            if (!this.$can('AppointEmployeeWithoutSigner', 'permissions')) {
               this.tabIndex -= 1;
            } else {
               this.tabIndex -= 1;
            }
         }
      },
      GetDepartName(agree, index) {
         this.Data.signer[index].department = this.DepartmentList.filter(
            (item) => item.value === agree.departmentId
         )[0].text;
      },
      GetPosName(agree, index) {
         this.Data.signer[index].position = this.PositionList.filter(
            (item) => item.id === agree.positionId
         )[0].positionName;
      },
      onChangeEmpAppointOrderType(newVal) {
         this.tabrow = {
            ...defaultTableRow,
            empAppointOrderTypeId: newVal
         };
      },

      parseDate(ddmmyyyy = '') {
         const parts = ddmmyyyy.split('.');
         const mmddyyyy = new Date(parts[2], parts[1] - 1, parts[0]);
         return mmddyyyy;
      },
      GetStaffLimit() {
         if (this.tabrow.departmentId && this.tabrow.positionId && !this.tabrow.interm) {
            if (
               this.tabrow.fromDepartmentId == this.tabrow.departmentId &&
               this.tabrow.fromPositionId == this.tabrow.positionId &&
               this.tabrow.fromEmployeeRate == 0.5
            ) {
               // this.tabrow.interm = null;
               this.tabrow.employeeRate = 1;
               this.staffLimit = 1;
            } else {
               StaffingService.GetAllStaffingPositionsForQuantity(
                  this.tabrow.positionId,
                  this.tabrow.departmentId,
                  this.Data.organizationId
               ).then((res) => {
                  this.staffLimit = res.data.quantityForNow;
               });
            }
         }
      },
      handleSelectPosition(position) {
         this.tabrow.position = position?.positionName;
         const minTabrowStartDate = position ? position.ownerDocDate.slice(0, 10) : '';
         if (minTabrowStartDate) {
            const date2 = this.parseDate(minTabrowStartDate);
            this.tabrow.cdisabledDate = (d) => {
               return d < date2;
            };
            if (this.tabrow.startOn) {
               const date1 = this.parseDate(this.tabrow.startOn);
               if (date2 > date1) {
                  this.tabrow.startOn = '';
               }
            }
         }
      },

      ChangeDepartment(item) {
         this.tabrow.positionId = null;
         this.tabrow.department = item?.text;
      },
      onUpdateEmployeeManage(e) {
         // this.tabrow.choosenEmployeemanageId = e?.employeeId;
         this.tabrow.employeeId = e?.employeeId;
         this.tabrow.employeeManageId = e?.employeeManageId;
         this.tabrow.employeeFull = e?.employee;
         this.tabrow.fromEmployeeRate = e?.employmentRate;
         this.tabrow.fromPositionId = e?.positionId;
         this.tabrow.fromPosition = e?.positionName;
         this.tabrow.fromDepartmentId = e?.departmentId;
         this.tabrow.fromDepartment = e?.department;
         this.tabrow.workScheduleId = e?.workScheduleId;
         this.tabrow.employmentTypeId = e?.employmentTypeId;
      },

      onUpdateChoosenEmployeeManage(e) {
         this.tabrow.choosenEmployeemanageId = e?.employeeId;
         this.tabrow.choosenEmployee = e?.employee;
      },

      onUpdateEmployee(e) {
         this.tabrow.employeeFull = e?.fullName;
      },
      DeleteTabrow(index) {
         this.Data.tables.splice(index, 1);
      },
      OpenTabrow() {
         this.TabrowModal = true;
         this.tabrow = { ...defaultTableRow };
         this.tabrow.empAppointOrderType = this.EmpAppointOrderTypeList.find((e) => e.value == 1)?.text;
         this.editedIndex1 = -1;
      },
      EditTabrow(item) {
         this.editedIndex1 = this.Data.tables.indexOf(item);
         this.tabrow = Object.assign({}, item);
         this.TabrowModal = true;
         this.GetStaffLimit();
      },
      AddTabrow() {
         this.$refs.ValidationTabrow.validate().then((success) => {
            if (success) {
               this.tabrow.empAppointOrderType = this.EmpAppointOrderTypeList.find(
                  (e) => e.value == this.tabrow.empAppointOrderTypeId
               )?.text;

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
               AppointEmployeeService.Update({ ...this.Data, signer: sortHrmOrder(this.Data.signer) })
                  .then((res) => {
                     this.makeToast(this.$t('SaveSuccess'), 'success');
                     this.$router.push({ name: 'AppointEmployee' });
                  })
                  .catch((err) => {
                     this.showApiError(err);
                  })
                  .finally(() => {
                     this.saveLoading = false;
                  });
            }
         });
      },
      handleemployeeRate(e) {
         console.log('dd');
      }
   }
};
</script>
<style scoped>
.nav-item {
   background-color: grey;
}
input {
   margin: 0.4rem;
}
</style>
