<template>
   <div>
      <b-card>
         <validation-observer ref="ValidationDTO">
            <b-row>
               <b-col sm="12" md="3" class="mb-1">
                  <form-input v-model="Data.orderCode" type="number" :label="$t('orderCode')" />
               </b-col>
               <b-col sm="12" md="3">
                  <div class="form-group">
                     <form-input
                        v-model="Data.inn"
                        :label="$t('inn')"
                        :mask="'#########'"
                        :disabled="$route.params.id != 0"
                     >
                        <b-input-group-append>
                           <b-button variant="primary" @click="GetByInn" :disabled="$route.params.id != 0">
                              <feather-icon icon="SearchIcon"></feather-icon>
                           </b-button>
                        </b-input-group-append>
                     </form-input>
                  </div>
               </b-col>

               <b-col sm="12" md="3">
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
               <b-col sm="12" md="3">
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

               <b-col sm="12" md="3">
                  <form-select
                     :options="OrganizationGroupSelectList"
                     v-model="Data.organizationGroupId"
                     required-star
                     :label="$t('group')"
                  ></form-select>
               </b-col>

               <b-col sm="12" md="3">
                  <form-select
                     :options="CountryList"
                     v-model="Data.countryId"
                     required-star
                     :label="$t('Country')"
                     @input="ChangeCountry"
                  ></form-select>
               </b-col>

               <b-col sm="12" md="3">
                  <form-select
                     :options="RegionList"
                     v-model="Data.regionId"
                     required-star
                     :label="$t('region')"
                     @input="ChangeRegion"
                  ></form-select>
               </b-col>

               <b-col sm="12" md="3">
                  <form-select
                     :options="DistrictList"
                     v-model="Data.districtId"
                     required-star
                     :label="$t('Region')"
                  ></form-select>
               </b-col>

               <b-col sm="12" md="3">
                  <div class="form-group">
                     <form-input v-model="Data.address" :label="$t('address')" />
                  </div>
               </b-col>

               <b-col sm="12" md="8">
                  <form-select
                     :options="OkedList"
                     v-model="Data.okedId"
                     required-star
                     :label="$t('Oked')"
                  ></form-select>
               </b-col>

               <b-col sm="12" md="4">
                  <EmployeeSelect2
                     v-if="isGet"
                     hideAdd
                     v-model="Data.incomingDocReceiverEmployeeId"
                     label="employeeEDOC"
                     :organisation="organisatsiya"
                     @update:data="onUpdateEmployee"
                  />
               </b-col>

               <b-col sm="12" md="3">
                  <div class="form-group">
                     <form-input v-model="Data.director" :label="$t('Director')" />
                  </div>
               </b-col>

               <b-col sm="12" md="3">
                  <div class="form-group">
                     <form-input v-model="Data.accounter" :label="$t('Accounter')" />
                  </div>
               </b-col>

               <b-col sm="12" md="3">
                  <div class="form-group">
                     <form-input v-model="Data.phoneNumber" :label="$t('phoneNumber')" :mask="'(998) ## ### ## ##'" />
                  </div>
               </b-col>

               <b-col sm="12" md="3">
                  <div class="form-group">
                     <form-input-hrm
                        v-model="Data.email"
                        :label="$t('email')"
                        :rules="Data.organizationGroupId == 1 ? 'required|email' : ''"
                        type="email"
                     />
                  </div>
               </b-col>

               <b-col sm="12" md="12">
                  <form-select :options="ParentList" v-model="Data.parentId" :label="$t('OrgParent')"></form-select>
               </b-col>
               <b-col sm="12" md="3">
                  <div class="form-group">
                     <form-input v-model="Data.vatCode" :label="$t('vatCode')" />
                  </div>
               </b-col>

               <b-col sm="12" md="3">
                  <div class="form-group">
                     <form-input v-model="Data.zipCode" :label="$t('zipCode')" />
                  </div>
               </b-col>

               <b-col sm="12" md="3">
                  <form-select
                     :options="OrganizationLegalFormList"
                     v-model="Data.organizationLegalFormId"
                     :label="$t('OrganizationLegalForm')"
                  ></form-select>
               </b-col>

               <b-col sm="12" md="3">
                  <form-select
                     :options="SignOrganizationTypeList"
                     v-model="Data.signOrganizationTypeId"
                     :label="$t('signOrganizationType')"
                     @input="ChangeSignOrganizationType"
                     :disabled="Data.signs.length > 0"
                  ></form-select>
               </b-col>
            </b-row>

            <b-row v-show="!!Data.signOrganizationTypeId">
               <b-col md="12">
                  <div>
                     <div class="mx-1 tabheader btn-group-pills mb-2" style="float: left">
                        <button
                           type="button"
                           @click="activeTab = 'signs'"
                           :class="activeTab === 'signs' ? 'btn btn-primary active' : 'btn'"
                        >
                           {{ $t('signs') }}
                        </button>
                        <button
                           type="button"
                           :class="activeTab === 'expiredSigns' ? 'btn btn-primary active' : 'btn'"
                           @click="activeTab = 'expiredSigns'"
                        >
                           {{ $t('expiredSigns') }}
                        </button>
                     </div>
                     <b-button
                        class="my-1"
                        style="float: right"
                        @click="OpenModal"
                        v-show="!!Data.signOrganizationTypeId"
                        size="sm"
                        variant="outline-primary"
                     >
                        <feather-icon icon="PlusIcon"></feather-icon>
                        {{ $t('Add') }}
                     </b-button>
                  </div>
                  <b-table
                     style="vertical-align: middle"
                     :fields="fields"
                     :items="activeTab === 'signs' ? Data.signs : Data.expiredSigns"
                     class="bg-color-table text-center"
                     bordered
                     :responsive="true"
                     :tbody-tr-class="rowClass"
                  >
                     <template #cell(user)="{ item }">{{
                        `${item.lastName} ${item.firstName} ${item.middleName}`
                     }}</template>
                     <template #cell(expireOn)="{ item }" v-if="activeTab === 'signs'">
                        <form-picker
                           v-model="item.expireOn"
                           :label="$t('expireOn')"
                           :clearable="false"
                           :placeholder="$t('expireOn')"
                        ></form-picker>
                     </template>
                  </b-table>
               </b-col>
            </b-row>

            <b-row>
               <!-- files -->
               <b-col cols="12" class="mt-2">
                  <h6 class="inputTitle">{{ $t('photo') }}</h6>
                  <b-form-file
                     type="file"
                     style="max-width: 400px"
                     :placeholder="$t('Faylni tanlang')"
                     @change="UploadFile"
                  ></b-form-file>
                  <div class="mt-1" v-for="item in Data.files" :key="item.id">
                     <b-link variant="primary" target="_blank" :href="FileSrc(item.id)">{{
                        item.fileName || item.id
                     }}</b-link>
                     <b-button variant="danger" size="sm" class="ml-1" @click="DeleteFile(item.id)">
                        <b-icon-trash scale="0.7" />
                     </b-button>
                  </div>
               </b-col>
            </b-row>
         </validation-observer>
      </b-card>
      <label for="">{{ $t('orgSettlementAccount') }}</label>
      <b-card>
         <validation-observer ref="ValidationSettleMent">
            <!-- Settlement Accounts -->
            <b-row>
               <b-col sm="12" md="2">
                  <form-input
                     v-model="settlementAccounts.accountName"
                     :label="$t('accountName')"
                     required
                     max-length="20"
                  ></form-input>
               </b-col>
               <b-col sm="12" md="2">
                  <form-input v-model="settlementAccounts.accountCode" :label="$t('accountCode')" required></form-input>
               </b-col>
               <b-col sm="12" md="3">
                  <form-select
                     v-model="settlementAccounts.bankId"
                     :label="$t('bankid')"
                     required-star
                     :options="bankList"
                  ></form-select>
               </b-col>
               <b-col sm="12" md="2">
                  <form-select
                     :options="StateList"
                     v-model="settlementAccounts.stateId"
                     required-star
                     label="Status"
                  ></form-select>
               </b-col>
               <b-col sm="12" md="3" class="mt-2">
                  <b-button @click="addSettlementAccounts" size="sm" variant="primary">
                     <feather-icon icon="PlusIcon"></feather-icon>
                     <!-- {{ $t('Add') }} -->
                  </b-button>
               </b-col>
            </b-row>
         </validation-observer>
      </b-card>

      <!-- SETTLEMENT ACCOUNTS LIST -->
      <b-table
         v-if="Data.settlementAccounts && Data.settlementAccounts.length > 0"
         style="vertical-align: middle"
         :fields="tabArrowFields"
         :items="Data.settlementAccounts"
         class="bg-color-table text-center"
         bordered
         :responsive="true"
      >
         <template #cell(actions)="{ index }">
            <div class="text-center">
               <!-- <b-link
                  @click="Edit(item)"
                  class="mr-1"
                  v-b-tooltip="{ content: $t('Edit') }"
               >
                  <feather-icon icon="EditIcon"></feather-icon>
               </b-link> -->
               <b-link class="mr-1" v-b-tooltip.hover.top="$t('Delete')" @click="Delete(index)">
                  <feather-icon icon="Trash2Icon"></feather-icon>
               </b-link>
            </div>
         </template>
      </b-table>

      <b-modal v-model="TableModal" static size="xl" no-close-on-backdrop hide-footer :title="$t('organization')">
         <!-- USER FIND INFO AREAS -->
         <validation-observer ref="ValidationTabrow">
            <b-row>
               <b-col sm="12" md="3">
                  <div class="form-group">
                     <form-input
                        v-model="filter.Seria"
                        :label="$t('Seria')"
                        :mask="'AA'"
                        required
                        @input="(val) => (filter.Seria = filter.Seria.toUpperCase())"
                     />
                  </div>
               </b-col>

               <b-col sm="12" md="3">
                  <div class="form-group">
                     <form-input v-model="filter.Number" :mask="'#######'" :label="$t('passportNumber')" required />
                  </div>
               </b-col>
               <b-col sm="12" md="3" class="text-left" style="margin-top: -5px">
                  <form-picker
                     v-model="filter.DateOfBirth"
                     :label="$t('dateofbirth')"
                     required
                     :placeholder="$t('dateofbirth')"
                  ></form-picker>
               </b-col>
               <b-col sm="12" md="3" class="text-left mt-2">
                  <b-button @click="GetPerson" variant="primary">{{ $t('search') }}</b-button>
               </b-col>
            </b-row>
         </validation-observer>
         <!-- USER INFO -->
         <b-row v-show="!!TempPerson.pinfl" class="mb-2 mt-2">
            <b-col sm="12" md="12" class="text-left">
               <h3>
                  {{ `${TempPerson.surnameLatin} ${TempPerson.nameLatin} ${TempPerson.patronymLatin}` }}
               </h3>
            </b-col>
            <b-col sm="12" md="6" class="text-left">
               <b-list-group>
                  <b-list-group-item>
                     <div style="display: flex; justify-content: space-between">
                        <b>{{ $t('dateofbirth') }}</b>
                        <span>{{ TempPerson.birthDate }}</span>
                     </div>
                  </b-list-group-item>
                  <b-list-group-item>
                     <div style="display: flex; justify-content: space-between">
                        <b>{{ $t('nationality') }}</b>
                        <span>{{ TempPerson.nationality }}</span>
                     </div>
                  </b-list-group-item>
                  <b-list-group-item>
                     <div style="display: flex; justify-content: space-between">
                        <b>{{ $t('gender') }}</b>
                        <span>{{ TempPerson.gender }}</span>
                     </div>
                  </b-list-group-item>
                  <b-list-group-item>
                     <div style="display: flex; justify-content: space-between">
                        <b>{{ $t('Oblast') }}</b>
                        <span>{{ TempPerson.livingRegion }}</span>
                     </div>
                  </b-list-group-item>
                  <b-list-group-item>
                     <div style="display: flex; justify-content: space-between">
                        <b>{{ $t('Region') }}</b>
                        <span>{{ TempPerson.livingDistrict }}</span>
                     </div>
                  </b-list-group-item>
               </b-list-group>
            </b-col>

            <b-col sm="12" md="6" class="text-left">
               <b-list-group>
                  <b-list-group-item>
                     <div style="display: flex; justify-content: space-between">
                        <b>{{ $t('Seria') }}</b>
                        <span>{{ TempPerson.passportSeria }}</span>
                     </div>
                  </b-list-group-item>
                  <b-list-group-item>
                     <div style="display: flex; justify-content: space-between">
                        <b>{{ $t('passportNumber') }}</b>
                        <span>{{ TempPerson.passportNumber }}</span>
                     </div>
                  </b-list-group-item>
                  <b-list-group-item>
                     <div style="display: flex; justify-content: space-between">
                        <b>{{ $t('passportDate') }}</b>
                        <span>{{ TempPerson.passportDate }}</span>
                     </div>
                  </b-list-group-item>
                  <b-list-group-item>
                     <div style="display: flex; justify-content: space-between">
                        <b>{{ $t('passportExpiration') }}</b>
                        <span>{{ TempPerson.passportExpiration }}</span>
                     </div>
                  </b-list-group-item>

                  <b-list-group-item>
                     <div style="display: flex; justify-content: space-between">
                        <b>{{ $t('pinfl') }}</b>
                        <span>{{ TempPerson.pinfl }}</span>
                     </div>
                  </b-list-group-item>
               </b-list-group>
            </b-col>
         </b-row>
         <!-- SELECT POSITIONS -->
         <b-row>
            <b-col sm="12" md="3">
               <validation-observer ref="ValidationTabrow2">
                  <form-select
                     :options="TempSigns"
                     v-model="tabrow.prtnContractTypeTableId"
                     requiredStar
                     :label="$t('position')"
                  ></form-select>
               </validation-observer>
            </b-col>
            <b-col sm="12" md="3">
               <form-input-hrm
                  v-model="tabrow.phoneNumber"
                  :mask="'+(998) ## ### ## ##'"
                  :placeholder="$t('phoneNumber')"
                  :label="$t('phoneNumber')"
                  rules="required|validatorPhone"
               />
            </b-col>
         </b-row>

         <!-- SUBMIT BUTTONS -->
         <b-row class="mt-3">
            <b-col class="text-center">
               <b-button class="mr-2" @click="TableModal = false" size="sm" variant="outline-danger">
                  <feather-icon icon="ArrowLeftIcon"></feather-icon>
                  {{ $t('back') }}
               </b-button>
               <b-button @click="AddRow" size="sm" variant="outline-success">
                  <feather-icon icon="CheckIcon"></feather-icon>
                  {{ $t('Save') }}
               </b-button>
            </b-col>
         </b-row>
      </b-modal>
      <b-row class="mt-3">
         <b-col sm="12" md="6" lg="6" class="text-left"></b-col>
         <b-col sm="12" md="6" lg="6" class="text-right">
            <b-button @click="SaveData" size="sm" variant="outline-success">
               <feather-icon icon="CheckIcon"></feather-icon>
               {{ $t('Save') }}
            </b-button>
         </b-col>
      </b-row>
   </div>
</template>
<script>
import EmployeeSelect2 from '@/views/components/employee/EmployeeSelect2.vue';
import ManualService from '@/services/others/manual.service';
import OrganizationService from '@/services/managment/organization.service';
import CountryService from '@/services/info/country.service';
import OkedService from '@/services/info/oked.service';
import RegionService from '@/services/info/region.service';
import OrganizationLegalFormService from '@/services/info/organizationlegalform.service';
import DistrictService from '@/services/info/district.service';
import PrtnContractTypeService from '@/services/info/prtncontracttype.service';
import EmployeeService from '@/services/info/employee.service';
import FormInputTranslate from '@/components/translates/FormInputTranslate.vue';

import {
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
   BListGroup,
   BFormFile,
   BListGroupItem,
   BFormSelect,
   BIconTrash
} from 'bootstrap-vue';
import BankService from '@/services/info/bank.service';
import axios from 'axios';

const tabrowDef = {
   prtnContractTypeTablePosition: '',
   id: 0,
   prtnContractTypeTableId: 0,
   expireOn: '',
   pinfl: '',
   firstName: '',
   lastName: '',
   middleName: '',
   passportSeria: '',
   passportNumber: '',
   birthOn: '',
   phoneNumber: ''
};

const filterDef = {
   Seria: '',
   Number: '',
   DateOfBirth: ''
};

export default {
   components: {
      EmployeeSelect2,
      BFormSelect,
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
      BModal,
      VBModal,
      BFormFile,
      BCardText,
      BInputGroup,
      BInputGroupAppend,
      BTr,
      BTd,
      BFormCheckbox,
      BListGroup,
      BListGroupItem,
      FormInputTranslate,
      BIconTrash
   },
   name: 'Edit',
   directives: {
      'b-tooltip': VBTooltip
   },
   computed: {
      FileSrc() {
         return (id) => axios.defaults.baseURL + `Organization/DownloadFile/${id}`;
      },
      organisatsiya() {
         return this.Data.id;
      }
   },
   data() {
      return {
         activeTab: 'signs',
         loadingButton: false,
         isGet: false,
         SignOrganizationLoading: false,
         Data: {
            translates: [],
            incomingDocReceiverEmployeeId: null,
            person: {},
            signs: [],
            expiredSigns: []
         },
         ParentList: [],
         CountryList: [],
         RegionList: [],
         DistrictList: [],
         OrganizationGroupSelectList: [],
         settlementAccounts: {
            accountName: '',
            accountCode: '',
            bankId: null,
            stateId: 1,
            id: 0
         },
         OkedList: [],
         SignOrganizationTypeList: [],
         TempSigns: [],
         OrganizationLegalFormList: [],
         innLoading: false,
         TableModal: false,
         StateList: [],
         TempPerson: {},
         tabrow: { ...tabrowDef },
         filter: { ...filterDef },
         editedIndex: -1,
         fields: [
            {
               key: 'prtnContractTypeTablePosition',
               label: this.$t('prtnContractTypeTablePosition'),
               tdClass: 'text-left',
               thClass: 'text-center'
            },
            {
               key: 'user',
               label: this.$t('user'),
               tdClass: 'text-left',
               thClass: 'text-center'
            },
            {
               key: 'phoneNumber',
               label: this.$t('phoneNumber'),
               tdClass: 'text-left',
               thClass: 'text-center'
            },
            {
               key: 'expireOn',
               label: this.$t('expireOn'),
               tdClass: 'text-left',
               thClass: 'text-center'
            }
         ],
         bankList: [],
         tabArrowFields: [
            {
               key: 'accountCode',
               label: this.$t('accountCode'),
               tdClass: 'text-left',
               thClass: 'text-center'
            },
            {
               key: 'accountName',
               label: this.$t('accountName'),
               tdClass: 'text-left',
               thClass: 'text-center'
            },
            {
               key: 'bankId',
               label: this.$t('bankId'),
               tdClass: 'text-left',
               thClass: 'text-center'
            },
            {
               key: 'stateId',
               label: this.$t('stateId'),
               tdClass: 'text-left',
               thClass: 'text-center'
            },
            { key: 'actions', label: this.$t('actions') }
         ]
      };
   },
   created() {
      OrganizationService.Get(this.$route.params.id)
         .then((res) => {
            this.Data = res.data;

            this.GetRegion(this.Data.countryId);
            if (this.$route.params.id != 0) {
               this.GetDistrict(this.Data.regionId);
               if (this.Data.signOrganizationTypeId) {
                  this.ChangeSignOrganizationType(this.Data.signOrganizationTypeId);
               }
            }
         })
         .finally(() => {
            this.isGet = true;
         });

      BankService.GetAsSelectList().then((res) => {
         this.bankList = res.data;
      });

      ManualService.StateSelectList()
         .then((res) => {
            this.StateList = res.data;
         })
         .catch(this.showApiError);

      OrganizationService.GetAsSelectList()
         .then((res) => {
            this.ParentList = res.data;
         })
         .catch(this.showApiError);

      OrganizationLegalFormService.GetAsSelectList()
         .then((res) => {
            this.OrganizationLegalFormList = res.data;
         })
         .catch(this.showApiError);
      CountryService.GetAsSelectList()
         .then((res) => {
            this.CountryList = res.data;
         })
         .catch(this.showApiError);

      OkedService.GetAsSelectList()
         .then((res) => {
            this.OkedList = res.data;
         })
         .catch(this.showApiError);

      ManualService.SignOrganizationTypeSelectList({
         incluceBusinessman: false
      })
         .then((res) => {
            this.SignOrganizationTypeList = res.data;
         })
         .catch(this.showApiError);
      ManualService.OrganizationGroupSelectList()
         .then((res) => {
            this.OrganizationGroupSelectList = res.data;
         })
         .catch(this.showApiError);
   },
   methods: {
      UploadFile(event) {
         const formData = new FormData();
         formData.append('files', event.target.files[0]);
         this.fileLoading = true;
         OrganizationService.UploadFile(formData).then((res) => {
            this.Data.files.push(...res.data.map((e) => ({ ...e, columnName: 'photo' })));
            this.fileLoading = false;
         });
      },
      DeleteFile(id) {
         OrganizationService.DeleteFile(id).then(() => {
            this.Data.files = this.Data.files.filter((item) => item.id != id);
         });
      },
      Delete(index) {
         this.Data.settlementAccounts.splice(index, 1);
      },
      BindValueTabrow(value) {
         this.filter.DateOfBirth = value;
      },
      addSettlementAccounts() {
         this.$refs.ValidationSettleMent.validate().then((success) => {
            if (success) {
               if (!this.settlementAccounts.stateId) {
                  this.makeToast(this.$t('ordernumberNotSelected'), 'danger');
                  return false;
               }
               // if (this.Data.settlementAccounts.some((item) => item.stateId == this.settlementAccounts.stateId)) {
               //    this.makeToast(this.$t('sameOredrNumberSelected'), 'danger');
               //    return false;
               // }
               this.Data.settlementAccounts.push(this.settlementAccounts);
               this.settlementAccounts = {
                  accountName: '',
                  stateId: this.Data.settlementAccounts.length + 1,
                  bankId: null,
                  accountCode: null,
                  id: 0
               };
            }
         });
      },
      GetPerson() {
         this.$refs.ValidationTabrow.validate().then((success) => {
            if (success) {
               this.personLoading = true;
               EmployeeService.GetByPassportDataFromDigital(
                  this.filter.Seria + this.filter.Number,
                  this.filter.DateOfBirth
               )
                  .then((res) => {
                     this.TempPerson = res.data;
                     this.personLoading = false;
                  })
                  .catch((error) => {
                     this.showApiError(error);
                     this.personLoading = false;
                  });
            }
         });
      },

      onUpdateEmployee(e) {
         console.log(e);
      },
      OpenModal() {
         this.TableModal = true;
         // this.tabrow = {
         //   prtnContractTypeTablePosition: "",
         //   id: 0,
         //   prtnContractTypeTableId: 0,
         //   expireOn: "",
         //   pinfl: "",
         //   firstName: "",
         //   lastName: "",
         //   middleName: "",
         //   passportSeria: "",
         //   passportNumber: "",
         //   birthOn: ""
         // };
         // (this.filter = {
         //   Seria: "",
         //   Number: "",
         //   DateOfBirth: ""
         // }),
         //   (this.TempPerson = {});
         this.editedIndex = -1;
      },
      AddRow() {
         this.$refs.ValidationTabrow.validate().then((res) => {
            if (res) {
               this.$refs.ValidationTabrow2.validate().then((success) => {
                  if (success) {
                     if (Object.keys(this.TempPerson).length === 0 && this.TempPerson.constructor === Object) {
                        this.makeToast(this.$t('NotFildUser'), 'danger');
                        return false;
                     }
                     this.tabrow.prtnContractTypeTablePosition = this.tabrow.prtnContractTypeTableId
                        ? this.TempSigns.filter((item) => item.value === this.tabrow.prtnContractTypeTableId)[0].text
                        : '';

                     this.tabrow.pinfl = this.TempPerson.pinfl;
                     this.tabrow.firstName = this.TempPerson.nameLatin;
                     this.tabrow.lastName = this.TempPerson.surnameLatin;
                     this.tabrow.middleName = this.TempPerson.patronymLatin;
                     this.tabrow.passportSeria = this.TempPerson.passportSeria;
                     this.tabrow.passportNumber = this.TempPerson.passportNumber;
                     this.tabrow.birthOn = this.TempPerson.birthDate;

                     this.tabrow.phoneNumber = (this.tabrow.phoneNumber || '').replace(/\D/g, '');

                     if (this.editedIndex > -1) {
                        Object.assign(this.Data.signs[this.editedIndex], this.tabrow);
                     } else {
                        this.Data.signs.push(this.tabrow);
                     }
                     this.tabrow = { ...tabrowDef };
                     this.filter = { ...filterDef };
                     this.TempPerson = {};
                     this.TableModal = false;
                  }
               });
            }
         });
      },
      BindValue(value, item) {
         item.expireOn = value;
      },

      ChangeSignOrganizationType(id = 0) {
         if (id) {
            const tempOrganizationId =
               this.Data.signOrganizationTypeId == 4 && this.$route.params.id != 0 ? this.$route.params.id : null;
            this.SignOrganizationLoading = true;
            PrtnContractTypeService.GetTablesBySignOrganizationType({
               SignOrganizationTypeId: id,
               includeBusinessman: false,
               regionId: this.Data.regionId,
               organizationId: tempOrganizationId
            })
               .then((res) => {
                  this.TempSigns = res.data;
                  this.SignOrganizationLoading = false;
               })
               .catch((error) => {
                  this.SignOrganizationLoading = false;
                  this.showApiError(error);
               });
         }
      },
      rowClass(item, type) {
         if (item.Status === 3) {
            return 'd-none';
         }
      },
      GetByInn() {
         this.innLoading = true;
         OrganizationService.GetByInn(this.Data.inn)
            .then((res) => {
               this.Data = res.data;

               this.GetDistrict(res.data.regionId);
            })
            .catch(this.showApiError);
      },
      ChangeCountry(id) {
         this.Data.regionId = 0;
         this.Data.region = null;

         this.Data.districtId = 0;
         this.Data.district = null;
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
            .catch(this.showApiError);
      },
      ChangeRegion(id) {
         this.Data.districtId = 0;
         this.Data.district = null;
         if (id) {
            this.GetDistrict(id);
         } else {
            this.DistrictList = [];
         }
      },
      GetDistrict(id) {
         DistrictService.GetAsSelectList(id)
            .then((res) => {
               this.DistrictList = res.data;
            })
            .catch(this.showApiError);
      },
      backToList() {
         this.$router.push({ name: 'role' });
      },
      SaveData() {
         this.$refs.ValidationDTO.validate().then((success) => {
            if (success) {
               if (this.$route.params.id == 0 && this.Data.settlementAccounts.length === 0) {
                  this.makeToast(this.$t('requiredOrgSettleMentAccount'), 'danger');

                  return;
               }
               OrganizationService.Update(this.Data)
                  .then((res) => {
                     this.makeToast(this.$t('SaveSuccess'), 'success');
                     this.$router.push({ name: 'Organization' });
                  })
                  .catch(this.showApiError);
            }
         });
         this.loadingButton = true;
      }
   }
};
</script>
<style scoped>
legend {
   background-color: #000;
   color: #fff;
   padding: 3px 6px;
}

.output {
   font: 1rem 'Fira Sans', sans-serif;
}

input {
   margin: 0.4rem;
}
</style>
<style lang="scss">
.tabheader {
   background: #f3f2f7;
   padding: 10px 15px;
   width: fit-content;
   font-weight: bold;
   border-radius: 5px;
}
</style>
