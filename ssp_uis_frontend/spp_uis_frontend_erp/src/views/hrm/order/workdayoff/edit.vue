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
                        <form-picker v-model="Data.docOn" :label="$t('docOn')" :placeholder="$t('docOn')"></form-picker>
                     </b-col>
                     <b-col sm="12" md="12">
                        <b-form-textarea
                           id="textarea"
                           v-model="Data.details"
                           :placeholder="$t('detailinfo')"
                           rows="2"
                           max-rows="6"
                        ></b-form-textarea>
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
                           hover
                           show-empty
                           bordered
                           :empty-text="$t('NotFound')"
                           responsive="sm"
                           :items="Data.tables"
                        >
                           <template #cell(status)="{ item }">
                              <b-badge :variant="item.stateId == '1' ? 'light-danger' : 'light-success'">{{
                                 item.state
                              }}</b-badge>
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
         <b-modal size="xl" v-model="TabrowModal" v-if="TabrowModal" static no-close-on-backdrop hide-footer>
            <b-row align-v="center">
               <b-col sm="12" md="6" class="mb-1">
                  <EmployeeManageSelect
                     v-model="tabrow.employeeManageId"
                     required
                     :employee="tabrow.employee"
                     :isOrganisation="$can('AllAppointEmployeeCreate', 'permissions')"
                     :label="$t('employeeManage')"
                     @update:data="onUpdateEmployeeManage"
                  />
               </b-col>
               <b-col sm="12" md="6">
                  <form-picker
                     v-model="tabrow.dayOffOn"
                     :label="$t('dayOffOn')"
                     required
                     :placeholder="$t('dayOffOn')"
                  ></form-picker>
               </b-col>
               <b-col sm="12" md="12">
                  <b-form-textarea
                     v-model="tabrow.details"
                     :placeholder="$t('detailinfo')"
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
import WorkDayOffService from '@/services/hrm/workdayoff.service';
import { VueEditor } from 'vue2-editor';
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
   BFormTextarea,
   BTabs,
   BTab
} from 'bootstrap-vue';
import EmployeeManageSelect from '@/views/components/hrm/EmployeeManageSelect.vue';
import { sortHrmOrder } from '@/views/hrm/utils';
import EmployeeManageService from '@/services/hrm/employeemanage.service';
const HrmSigner = () => import('@/views/components/hrm/HrmSigner.vue');
const HrmSignerTableView = () => import('@/views/components/hrm/HrmSignerTableView.vue');

const defaultTableRow = {
   id: 0,
   employeeId: 0,
   detailForPrint: '',
   employee: '',
   employeeManageId: 0,
   dayOffOn: '',
   details: ''
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
      EmployeeManageSelect,
      BFormTextarea,
      BTabs,
      BTab,
      HrmSigner,
      HrmSignerTableView
   },
   data() {
      return {
         show: false,
         loadingButton: false,
         TabrowModal: false,
         saveLoading: false,
         tabIndex: 1,
         Data: {
            id: 0,
            docNumber: null,
            docOn: null,
            details: null,
            tables: [],
            signer: []
         },
         tabrow: { ...defaultTableRow },
         TablesField: [
            {
               key: 'employee',
               label: this.$t('employee'),
               sortable: true
            },
            {
               key: 'department',
               label: this.$t('Department')
            },
            {
               key: 'dayOffOn',
               label: this.$t('dayOffOn')
            },
            {
               key: 'details',
               label: this.$t('details')
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
      WorkDayOffService.Get(this.$route.params.id)
         .then((res) => {
            this.Data = res.data;
            if (!Array.isArray(res.data?.tables)) {
               this.Data.tables = [];
            }
            if (!Array.isArray(res.data?.signer)) {
               this.Data.signer = [];
            }
         })
         .catch((error) => {
            this.showApiError(error);
         })
         .finally(() => {
            this.show = false;
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
         this.tabrow.employeeId = e?.employeeId;
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
               WorkDayOffService.Update({ ...this.Data, signer: sortHrmOrder(this.Data.signer) })
                  .then((res) => {
                     this.makeToast(this.$t('SaveSuccess'), 'success');
                     this.$router.push({ name: 'WorkDayOff' });
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
