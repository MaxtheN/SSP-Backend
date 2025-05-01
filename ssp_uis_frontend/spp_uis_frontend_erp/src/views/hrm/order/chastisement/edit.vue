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
                     <b-col sm="12" md="4" v-if="$can('AllChastisementCreate', 'permissions')">
                        <form-select
                           v-model="Data.organizationId"
                           :options="OrganizationList"
                           :label="$t('organization')"
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

               <b-tab :disabled="!Data.tables || Data.tables.length === 0 || !validInput" :title="$t('asos')">
                  <b-row>
                     <b-col class="mb-1">
                        <!-- <vue-editor v-model="tabrow.detailForPrint" :label="$t('content')"></vue-editor> -->
                        <form-input v-model="Data.conclusionForPrint" :label="$t('asos')" />
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
                     :isView="isView"
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
                        v-if="!isView"
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
            <b-row align-v="center">
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
               <b-col sm="12" md="3">
                  <b-form-checkbox
                     class="mb-1"
                     @change="() => (tabrow.hasPenalty = false)"
                     v-model="tabrow.hasReprimand"
                     >{{ $t('hasReprimand') }}</b-form-checkbox
                  >
               </b-col>
               <b-col sm="12" md="3">
                  <b-form-checkbox
                     class="mb-1"
                     @change="() => (tabrow.hasReprimand = false)"
                     v-model="tabrow.hasPenalty"
                     >{{ $t('hasPenalty') }}</b-form-checkbox
                  >
               </b-col>

               <b-col sm="12" md="12">
                  <b-form-textarea
                     v-model="tabrow.detailForPrint"
                     :placeholder="$t('asos')"
                     rows="2"
                     max-rows="6"
                  ></b-form-textarea>
               </b-col>
               <b-col class="my-1">
                  <form-input v-model="tabrow.detailPrint" :label="$t('Buyruq matni')" />
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
import VueEditor from '@/components/VueEditor.vue';
import ChastisementService from '@/services/hrm/chastisement.service';
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
import { sortHrmOrder } from '@/views/hrm/utils';
import ManualService from '@/services/others/manual.service';

const HrmSigner = () => import('@/views/components/hrm/HrmSigner.vue');
const HrmSignerTableView = () => import('@/views/components/hrm/HrmSignerTableView.vue');

const defaultTableRow = {
   detailForPrint: '',
   id: 0,
   details: '',
   departmentId: null,
   employeeId: null,
   employeeManageId: null,
   employeeRate: 0,
   reason: '',
   fact: '',
   hasReprimand: false,
   hasPenalty: false
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
      HrmSigner,
      HrmSignerTableView
   },
   data() {
      return {
         OrganizationList: [],
         show: false,
         loadingButton: false,
         TabrowModal: false,
         saveLoading: false,
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
            id: null,
            docNumber: null,
            docOn: null,
            details: null,
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
               key: 'reason',
               label: this.$t('reason')
            },
            {
               key: 'fact',
               label: this.$t('fact')
            },
            // {
            //    key: 'employeeRate',
            //    label: this.$t('employeeRate')
            // },
            {
               key: 'details',
               label: this.$t('details')
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
      ChastisementService.Get(this.$route.params.id)
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

      ManualService.OrganizationAsSelectListByGroup([1, 3]).then((res) => {
         this.OrganizationList = res.data;
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
         return Boolean(this.$route.query.isView);
      },
      signerHr() {
         return this.Data.signer.find((e) => e.isHr);
      },
      signerDirector() {
         return this.Data.signer.find((e) => e.isDirector);
      },
      validInput() {
         const { docNumber, docOn } = this.Data;

         const allInputsFilled = docNumber && docOn;

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
         this.tabrow.employeeId = e?.employeeId;
         this.tabrow.employee = e?.employee;
         this.tabrow.employeeManageId = e?.employeeManageId;
         this.tabrow.departmentId = e?.departmentId;
         this.tabrow.department = e?.department;
      },
      DeleteTabrow(index) {
         this.Data.tables.splice(index, 1);
      },
      OpenTabrow() {
         this.TabrowModal = true;
         this.tabrow = { ...defaultTableRow, detailForPrint: '' };
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
               ChastisementService.Update({ ...this.Data, signer: sortHrmOrder(this.Data.signer) })
                  .then((res) => {
                     this.makeToast(this.$t('SaveSuccess'), 'success');
                     this.$router.push({ name: 'Chastisement' });
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
