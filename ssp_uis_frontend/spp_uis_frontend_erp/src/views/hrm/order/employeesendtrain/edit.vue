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
                           :label="$t('docdate')"
                           :placeholder="$t('docdate')"
                        ></form-picker>
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
                           <b-link>
                              <feather-icon
                                 class="text-danger"
                                 @click="DeleteTabrow(index)"
                                 icon="Trash2Icon"
                              ></feather-icon>
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
               <!-- buyruq matni  -->

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
                     :signer="Data.signer"
                     @update:signer="(e) => (Data.signer = e)"
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

      <!-- table modal -->
      <validation-observer ref="ValidationTabrow">
         <b-modal
            size="lg"
            v-model="TabrowModal"
            no-close-on-backdrop
            v-if="TabrowModal"
            static
            hide-footer
            :title="$t('employeesendtrain')"
         >
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
                  <!-- <form-select
                     :options="DepartmentList"
                     required-star
                     v-model="tabrow.departmentId"
                     label="Department"
                  ></form-select> -->
               </b-col>

               <b-col sm="12" md="6">
                  <EmployeeManageSelect
                     :isOrganisation="$can('AllAppointEmployeeCreate', 'permissions')"
                     v-model="tabrow.employeeManageId"
                     :label="$t('employeeManage')"
                     @update:data="onUpdateEmployeeManage"
                     required
                     :employee="tabrow.employee"
                     :departmentId="tabrow.departmentId"
                  />
               </b-col>
               <b-col sm="12" md="6">
                  <form-picker
                     v-model="defaultTableRow.workStartDate"
                     required
                     :label="$t('dayOffOn')"
                     :placeholder="$t('dayOffOn')"
                  />
               </b-col>
               <b-col cols="12">
                  <OrganizationListSelect
                     label="organization"
                     v-model="tabrow.organizationId"
                     :valuename="tabrow.organization || tabrow.anotherOrganization"
                     @update:valuename="(e) => (tabrow.anotherOrganization = e)"
                     @update:data="
                        (e) => {
                           tabrow.organization = e ? e.fullName : '';
                           tabrow.anotherOrganization = e ? e.fullName : '';
                        }
                     "
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
                  <form-input v-model="Data.conclusionForPrint" :label="$t('orderDetails')" />
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
import EmployeeSendTrainService from '@/services/hrm/employeesendtrain.service';
import DepartmentService from '@/services/info/department.service';
import EmployeeService from '@/services/info/employee.service';

// components
import EmployeeManageSelect from '@/views/components/hrm/EmployeeManageSelect.vue';
import OrganizationListSelect from '@/views/components/organization/OrganizationListSelect.vue';
import { VueEditor } from 'vue2-editor';
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
   BFormTextarea,
   BTabs,
   BTab
} from 'bootstrap-vue';
import { sortHrmOrder } from '@/views/hrm/utils';

const HrmSigner = () => import('@/views/components/hrm/HrmSigner.vue');
const HrmSignerTableView = () => import('@/views/components/hrm/HrmSignerTableView.vue');

const defaultTableRow = {
   workStartDate: '',
   detailForPrint: '',
   id: 0,
   department: '',
   departmentId: null,
   employee: '',
   employeeId: null,
   details: '',
   startOn: '',
   endOn: '',
   organizationId: null,
   anotherOrganization: ''
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
      BFormTextarea,
      BTabs,
      BTab,
      HrmSigner,
      HrmSignerTableView,
      EmployeeManageSelect,
      OrganizationListSelect
   },
   name: 'Edit',
   data() {
      return {
         show: false,
         DepartmentList: [],
         EmployeeList: [],
         loadingButton: false,
         TabrowModal: false,
         saveLoading: false,
         Data: {
            id: null,
            docNumber: null,
            docOn: null,
            details: null,
            tables: [],
            signer: []
         },
         tabIndex: 1,
         tabrow: { ...defaultTableRow },
         TablesField: [
            {
               key: 'department',
               label: this.$t('Department'),
               sortable: true
            },
            {
               key: 'organization',
               label: this.$t('organization'),
               sortable: true,
               formatter(value, key, item) {
                  return item['organization'] || item['anotherOrganization'];
               }
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
      EmployeeSendTrainService.Get(this.$route.params.id)
         .then((res) => {
            this.Data = res.data;
            if (!Array.isArray(this.Data.signer)) {
               this.Data.signer = [];
            }
         })
         .catch((error) => {
            this.showApiError(error);
         })
         .finally(() => {
            this.show = false;
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

      EmployeeService.GetAsSelectList({})
         .then((res) => {
            const { rows } = res.data;
            if (Array.isArray(rows)) {
               this.EmployeeList = rows;
            }
         })
         .catch((error) => {
            this.showApiError(error);
         });
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
   watch: {
      '$route.query': {
         handler(val) {
            if (val) {
               if (val.departmentId && val.employeeId) {
                  this.tabrow.departmentId = Number(val.departmentId);
                  this.tabrow.employeeId = Number(val.employeeId);
                  this.TabrowModal = true;
               }
            }
         },
         immediate: true
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
         this.tabrow.employeeId = e?.employeeId;
         this.tabrow.department = e?.department;
         this.tabrow.departmentId = e?.departmentId;
      },
      onUpdateEmployee(e) {
         this.tabrow.employee = e?.fullName;
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
               EmployeeSendTrainService.Update({ ...this.Data, signer: sortHrmOrder(this.Data.signer) })
                  .then(() => {
                     this.makeToast(this.$t('SaveSuccess'), 'success');
                     this.$router.push({ name: 'EmployeeSendTrain' });
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
