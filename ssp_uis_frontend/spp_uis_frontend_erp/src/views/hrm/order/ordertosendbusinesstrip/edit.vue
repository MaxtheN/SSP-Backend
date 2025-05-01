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

               <!-- buyriq matni  -->
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
                  <form-select
                     v-model="tabrow.businessTripTypeId"
                     :options="OrderToSendBusinessTripTypeSelectList"
                     required-star
                     @change="changeBusiness"
                     :label="$t('businessTripType')"
                     @option:selected="(e) => (tabrow.businessTripType = e ? e.text : '')"
                  />
               </b-col>
               <b-col sm="12" md="6">
                  <form-select
                     :disabled="countryDisabled"
                     v-model="tabrow.countryId"
                     :options="CountryList"
                     required-star
                     :label="$t('Country')"
                     @input="ChangeCountry"
                  />
               </b-col>
               <b-col sm="12" md="6" v-if="tabrow.countryId == 211">
                  <form-select v-model="tabrow.regionId" :options="RegionList" required-star :label="$t('Oblast')" />
               </b-col>
               <b-col sm="12" md="6">
                  <EmployeeManageSelect
                     v-model="tabrow.employeeManageId"
                     :isOrganisation="$can('AllAppointEmployeeCreate', 'permissions')"
                     :label="$t('employeeManage')"
                     @update:data="onUpdateEmployeeManage"
                     required
                     :employee="tabrow.employee"
                  />
               </b-col>
               <b-col sm="12" md="6">
                  <form-picker
                     v-model="tabrow.beginOn"
                     required
                     :label="$t('startdate')"
                     :placeholder="$t('startdate')"
                  />
               </b-col>
               <b-col sm="12" md="6">
                  <form-picker v-model="tabrow.endOn" required :label="$t('enddate')" :placeholder="$t('enddate')" />
               </b-col>
               <b-col sm="12" md="6">
                  dsasdasdasdd
                  <form-picker
                     v-model="Data.workStarDate"
                     required
                     :label="$t('dayOffOn')"
                     :placeholder="$t('dayOffOn')"
                  />
               </b-col>
               <b-col sm="12" md="12">
                  <b-form-textarea
                     v-model="tabrow.details"
                     rows="2"
                     max-rows="6"
                     :label="$t('asos')"
                     :placeholder="$t('asos')"
                  />
               </b-col>
               <b-col md="12" class="my-1">
                  <!-- <vue-editor v-model="Data.conclusionForPrint" :label="$t('content')"></vue-editor> -->
                  <form-input v-model="Data.conclusionForPrint" :label="$t('orderDetails')" />
               </b-col>
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
import OrderToSendBusinessTripService from '@/services/hrm/ordertosendbusinesstrip.service';
import RegionService from '@/services/info/region.service';
import CountryService from '@/services/info/country.service';

// components
import { VueEditor } from 'vue2-editor';
import { BOverlay, BCard, BRow, BCol, BTable, BButton, BLink, BFormTextarea, BTabs, BTab, BModal } from 'bootstrap-vue';
import EmployeeManageSelect from '@/views/components/hrm/EmployeeManageSelect.vue';
import EmployeeManageService from '@/services/hrm/employeemanage.service';
import ManualService from '@/services/others/manual.service';
import { sortHrmOrder } from '@/views/hrm/utils';

const HrmSigner = () => import('@/views/components/hrm/HrmSigner.vue');
const HrmSignerTableView = () => import('@/views/components/hrm/HrmSignerTableView.vue');

const defaultTableRow = {
   id: 0,
   detailForPrint: '',
   employeeManageId: 0,
   businessTripTypeId: 0,
   businessTripType: null,
   beginOn: '',
   endOn: '',
   countryId: '',
   regionId: '',
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
      BModal,
      BFormTextarea,
      BTabs,
      BTab,
      HrmSigner,
      HrmSignerTableView
   },
   name: 'OrderToSendBusinessTripEdit',
   data() {
      return {
         show: false,
         countryDisabled: false,
         CountryList: [],
         RegionList: [],
         OrderToSendBusinessTripTypeSelectList: [],
         loadingButton: false,
         saveLoading: false,
         Data: {
            id: 0,
            workStartDate: '',
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
               key: 'businessTripType',
               label: this.$t('businessTripType'),
               sortable: true
            },
            {
               key: 'country',
               label: this.$t('Country'),
               sortable: true
            },
            {
               key: 'region',
               label: this.$t('Oblast'),
               sortable: true
            },
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
               key: 'beginOn',
               label: this.$t('startdate'),
               sortable: true
            },
            {
               key: 'endOn',
               label: this.$t('enddate'),
               sortable: true
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
      OrderToSendBusinessTripService.Get(this.$route.params.id)
         .then((res) => {
            this.Data = res.data;
            if (!Array.isArray(res.data?.tables)) {
               this.Data.tables = [];
            }
         })
         .catch((error) => {
            this.makeToast(error.response.data.errors, 'danger');
         })
         .finally(() => {
            this.show = false;
         });

      CountryService.GetAsSelectList()
         .then((res) => {
            this.CountryList = res.data;
         })
         .catch((error) => {
            this.showApiError(error);
         });

      ManualService.OrderToSendBusinessTripTypeSelectList()
         .then((res) => {
            this.OrderToSendBusinessTripTypeSelectList = res.data;
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
         this.tabIndex += 1;
      },
      Back() {
         this.tabIndex -= 1;
      },
      ChangeCountry(id) {
         this.Data.regionId = 0;
         this.Data.region = null;
         if (id) {
            this.GetRegion(id);
         } else {
            this.RegionList = [];
         }
      },
      GetRegion(id) {
         RegionService.GetAsSelectList(id)
            .then((res) => {
               this.RegionList = res.data;
            })
            .catch((error) => {
               this.showapierror(error);
            });
      },
      OpenTabrow() {
         this.TabrowModal = true;
         this.tabrow = { ...defaultTableRow };
         this.editedIndex1 = -1;
      },
      onUpdateEmployeeManage(e) {
         this.tabrow.employee = e?.employee;
         this.tabrow.employeeId = e?.employeeId;
         this.tabrow.department = e?.department;
      },
      DeleteTabrow(index) {
         this.Data.tables.splice(index, 1);
      },
      EditTabrow(item) {
         this.editedIndex1 = this.Data.tables.indexOf(item);
         this.tabrow = Object.assign({}, item);
         this.TabrowModal = true;
      },
      AddTabrow() {
         this.$refs.ValidationTabrow.validate().then((success) => {
            if (success) {
               this.tabrow.country = this.CountryList.find((e) => e.value == this.tabrow.countryId)?.text;
               this.tabrow.region = this.RegionList.find((e) => e.value == this.tabrow.regionId)?.text;

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
               OrderToSendBusinessTripService.Update({ ...this.Data, signer: sortHrmOrder(this.Data.signer) })
                  .then((res) => {
                     this.makeToast(this.$t('SaveSuccess'), 'success');
                     this.$router.push({ name: 'OrderToSendBusinessTrip' });
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
      changeBusiness(e) {
         if (e == 1) {
            this.countryDisabled = true;
            CountryService.GetAsSelectList()
               .then((res) => {
                  this.CountryList = res.data;
                  this.tabrow.countryId = 211;
                  this.GetRegion(211);
               })
               .catch((error) => {
                  this.showApiError(error);
               });
         } else if (e == 2) {
            this.CountryList = this.CountryList.filter((item) => item.value !== 211);
            this.countryDisabled = false;
            this.tabrow.countryId = null;
         } else {
            this.tabrow.countryId = null;
            this.countryDisabled = false;
            CountryService.GetAsSelectList()
               .then((res) => {
                  this.CountryList = res.data;
               })
               .catch((error) => {
                  this.showApiError(error);
               });
         }
      }
   }
};
</script>
<style scoped>
input {
   margin: 0.4rem;
}
</style>
