<template>
   <div>
      <validation-observer v-if="!hideSearch" ref="ValidationPersonSearch">
         <b-row>
            <b-col sm="12" md="3" v-if="!hideOrganization">
               <PersonSelect
                  :options="OrganizationList"
                  :placeholder="$t('ChooseBelow')"
                  v-model="filter.organizationId"
                  rules="required"
                  :label="$t('Organization')"
               />
            </b-col>
            <b-col sm="12" md="3">
               <PersonSelect
                  :options="IdentityDocumentList"
                  v-model="filter.documentTypeId"
                  rules="required"
                  :label="$t('identitydocumentname')"
               />
            </b-col>

            <!-- uz fuqarosi uichun -->
            <template v-if="[1, 2, 3, 5].includes(filter.documentTypeId)">
               <b-col sm="12" md="2" v-if="filter.documentTypeId == 1">
                  <PersonSelect
                     :options="seriaList"
                     valueid="code"
                     v-model="filter.Seria"
                     rules="required"
                     :label="$t('documentseries')"
                  />
               </b-col>
               <b-col sm="12" md="1" v-if="[2, 3, 5].includes(filter.documentTypeId)">
                  <PersonInput
                     v-model="filter.Seria"
                     v-uppercase
                     mask="AA"
                     placeholder="AA"
                     rules="required|max:2"
                     :label="$t('documentseries')"
                  />
               </b-col>
               <b-col sm="12" md="2">
                  <PersonInput
                     v-model="filter.Number"
                     :mask="filter.documentTypeId == 1 ? '########' : '#######'"
                     :placeholder="filter.documentTypeId == 1 ? '#######' : '#######'"
                     rules="required|max:8"
                     :label="$t('documentnumber')"
                  />
               </b-col>
               <b-col sm="12" md="2" v-if="filter.documentTypeId != 1">
                  <form-picker v-model="filter.DateOfBirth" required rules="required" :label="$t('dateofbirth')" />
               </b-col>
               <b-col sm="12" md="1" class="pt-2">
                  <b-button @click="GetPerson" :disabled="personLoading" variant="primary">
                     <b-spinner v-if="personLoading" small></b-spinner>
                     <feather-icon icon="SearchIcon" />
                     <!-- {{ $t('search') }} -->
                  </b-button>
               </b-col>
            </template>

            <!-- chel el -->
            <template v-if="[4].includes(filter.documentTypeId)">
               <b-col sm="12" md="3">
                  <PersonInput
                     v-model="Data.surname"
                     name="surname"
                     :label="$t('lastName')"
                     :placeholder="$t('lastName')"
                  />
               </b-col>
               <b-col sm="12" md="3">
                  <PersonInput
                     v-model="Data.name"
                     name="name"
                     :label="$t('firstName')"
                     :placeholder="$t('firstName')"
                  />
               </b-col>
               <b-col sm="12" md="3">
                  <PersonInput
                     v-model="Data.patronym"
                     name="patronym"
                     :label="$t('middleName')"
                     :placeholder="$t('middleName')"
                  />
               </b-col>
               <b-col sm="12" md="3">
                  <form-picker v-model="Data.birthDate" name="birthDate" required :label="$t('dateofbirth')" />
               </b-col>
               <b-col sm="12" md="3">
                  <PersonSelect :options="GenderList" v-model="Data.genderId" rules="required" :label="$t('gender')" />
               </b-col>
            </template>

            <template v-if="[6].includes(filter.documentTypeId)">
               <b-col sm="12" md="3">
                  <PersonInput v-model="filter.pinfl" name="surname" :label="$t('tin')" :placeholder="$t('tin')" />
               </b-col>
               <b-col sm="12" md="1" class="pt-2">
                  <b-button @click="GetPerson" :disabled="personLoading" variant="primary">
                     <b-spinner v-if="personLoading" small></b-spinner>
                     <feather-icon icon="SearchIcon" />
                     <!-- {{ $t('search') }} -->
                  </b-button>
               </b-col>
            </template>
         </b-row>
      </validation-observer>

      <b-row v-if="Data.person && (Data.person.pinfl || Data.person.pnfl)" class="mb-2">
         <b-col sm="12" md="12" class="text-left">
            <h3 v-if="filter.documentTypeId == 1 || filter.documentTypeId == 6">
               {{ `${Data.person.surname}  ${Data.person.name} ${Data.person.patronym}` }}
            </h3>
            <h3 v-else>
               {{ `${Data.person.surnameLatin} ${Data.person.nameLatin} ${Data.person.patronymLatin}` }}
            </h3>
         </b-col>
         <b-col sm="12" md="6" class="text-left" v-if="filter.documentTypeId != 1 && filter.documentTypeId != 6">
            <b-list-group>
               <b-list-group-item>
                  <div style="display: flex; justify-content: space-between">
                     <b>{{ $t('dateofbirth') }}</b>
                     <span>{{ formatDate(Data.person.birthDate) }}</span>
                  </div>
               </b-list-group-item>
               <b-list-group-item>
                  <div style="display: flex; justify-content: space-between">
                     <b>{{ $t('nationality') }}</b>
                     <span>{{ Data.person.nationality }}</span>
                  </div>
               </b-list-group-item>
               <b-list-group-item>
                  <div style="display: flex; justify-content: space-between">
                     <b>{{ $t('gender') }}</b>
                     <span>{{ Data.person.gender }}</span>
                  </div>
               </b-list-group-item>
               <b-list-group-item>
                  <div style="display: flex; justify-content: space-between">
                     <b>{{ $t('Oblast') }}</b>
                     <span>{{ Data.person.livingRegion }}</span>
                  </div>
               </b-list-group-item>
               <b-list-group-item>
                  <div style="display: flex; justify-content: space-between">
                     <b>{{ $t('Region') }}</b>
                     <span>{{ Data.person.livingDistrict }}</span>
                  </div>
               </b-list-group-item>
            </b-list-group>
         </b-col>

         <b-col sm="12" md="6" class="text-left" v-if="filter.documentTypeId != 1 && filter.documentTypeId != 6">
            <b-list-group>
               <b-list-group-item>
                  <div style="display: flex; justify-content: space-between">
                     <b>{{ $t('Seria') }}</b>
                     <span>{{ Data.person.passportSeria }}</span>
                  </div>
               </b-list-group-item>
               <b-list-group-item>
                  <div style="display: flex; justify-content: space-between">
                     <b>{{ $t('passportNumber') }}</b>
                     <span>{{ Data.person.passportNumber }}</span>
                  </div>
               </b-list-group-item>
               <b-list-group-item>
                  <div style="display: flex; justify-content: space-between">
                     <b>{{ $t('passportDate') }}</b>
                     <span>{{ formatDate(Data.person.passportDate) }}</span>
                  </div>
               </b-list-group-item>
               <b-list-group-item>
                  <div style="display: flex; justify-content: space-between">
                     <b>{{ $t('passportExpiration') }}</b>
                     <span>{{ formatDate(Data.person.passportExpiration) }}</span>
                  </div>
               </b-list-group-item>

               <b-list-group-item>
                  <div style="display: flex; justify-content: space-between">
                     <b>{{ $t('pinfl') }}</b>
                     <span>{{ Data.person.pinfl }}</span>
                  </div>
               </b-list-group-item>
            </b-list-group>
         </b-col>
      </b-row>
   </div>
</template>

<script>
// services
import ManualService from '@/services/others/manual.service';
import IdentityDocumentService from '@/services/info/identitydocument.service';
import PersonService from '@/services/others/person.service';
import OrganizationService from '@/services/managment/organization.service';
import UserService from '@/services/managment/user.service';
// components
import { BRow, BCol, BCard, BButton, BSpinner, BListGroupItem, BListGroup } from 'bootstrap-vue';
import PersonInput from './PersonInput.vue';
import PersonSelect from './PersonSelect.vue';
import { seriaList } from './seriaList.js';

const organization = JSON.parse(localStorage.getItem('user_info'));
const filterDef = {
   organizationId: organization ? organization?.organizationId : null,
   Seria: '',
   Number: '',
   DateOfBirth: '',
   documentTypeId: 2,
   pinfl: ''
};

export default {
   components: {
      BRow,
      BCol,
      BCard,
      BButton,
      BSpinner,
      PersonInput,
      PersonSelect,
      BListGroupItem,
      BListGroup
   },
   directives: {
      uppercase: {
         update: function (el) {
            const inputEl = el.querySelector('input');
            if (inputEl) {
               inputEl.value = inputEl.value.toUpperCase();
            }
         }
      }
   },
   emits: ['update:person', 'get:img'],
   props: {
      person: {
         type: Object
      },
      dead: {
         type: Boolean,
         default: false
      },
      page: {
         type: String,
         default: 'person'
      },
      hideSearch: {
         type: Boolean,
         default: () => false
      },
      hideOrganization: {
         type: Boolean,
         default: () => false
      }
   },
   data() {
      return {
         IdentityDocumentList: [],
         GenderList: [],
         OrganizationList: [],
         personLoading: false,
         Data: {
            person: null,
            surname: null,
            name: null,
            patronym: null,
            birthDate: null,
            genderId: null
         },
         filter: { ...filterDef },
         seriaList
      };
   },
   created() {
      IdentityDocumentService.GetAsSelectList()
         .then((res) => {
            if (this.$props.dead) {
               this.IdentityDocumentList = res.data;
            } else {
               this.IdentityDocumentList = res.data.filter((item) => item.value != 6);
            }
         })
         .catch((error) => {
            this.makeToast(error, 'danger');
         });
      OrganizationService.GetAsSelectList()
         .then((res) => {
            this.OrganizationList = res.data;
         })
         .catch((error) => {
            this.showApiError(error);
         });
      ManualService.GenderSelectList()
         .then((res) => {
            this.GenderList = res.data;
         })
         .catch((error) => {
            this.showApiError(error);
         });
   },
   computed: {
      compData() {
         return `${this.Data.surname}|${this.Data.name}|${this.Data.patronym}|${this.Data.birthDate}|${this.Data.genderId}`;
      },
      formatDate() {
         return (date) => (date && typeof date == 'string' ? date.split(' ')[0] : '');
      }
   },
   watch: {
      'filter.documentTypeId': function () {
         this.filterReset();
         this.$refs.ValidationPersonSearch.reset();
      },
      person: function (newP) {
         this.Data.person = newP;
      },
      'filter.documentTypeId': function () {
         this.filterReset();
         this.$refs.ValidationPersonSearch.reset();
      },
      compData: function (newData) {
         const [newSurname, newName, newPatronym, birthDate, genderId] = newData.split('|');
         this.$emit('update:person', {
            person: {
               patronymLatin: newPatronym,
               nameLatin: newName,
               surnameLatin: newSurname,
               genderId,
               birthDate
            },
            filter: this.filter
         });
      }
   },
   methods: {
      GetPerson() {
         this.$refs.ValidationPersonSearch.validate().then((success) => {
            if (success) {
               this.personLoading = true;
               this.$emit('update:person', { person: null, filter: this.filter });
               this.Data.person = null;
               if (this.page == 'user') {
                  UserService.GetByPassportDataFromDigital(
                     this.filter.Seria + this.filter.Number,
                     this.filter.DateOfBirth
                  )
                     .then((res) => {
                        this.$emit('update:person', { person: res.data, filter: this.filter });
                        this.$emit('get:img', { person: res.data, filter: this.filter });
                        this.Data.person = res.data;
                     })
                     .catch((err) => {
                        this.showApiError(err);
                     })
                     .finally(() => {
                        this.personLoading = false;
                     });
               } else {
                  if (this.filter.documentTypeId == 1) {
                     PersonService.GetBirthInfoFromFHDYO({
                        cert_series: this.filter.Seria,
                        cert_number: this.filter.Number
                     })
                        .then((res) => {
                           this.$emit('update:person', { person: res.data[0], filter: this.filter });
                           this.Data.person = res?.data[0];
                        })
                        .catch((err) => {
                           this.showApiError(err);
                        })
                        .finally(() => {
                           this.personLoading = false;
                        });
                  } else if (this.filter.documentTypeId == 6) {
                     PersonService.GetDeathInfoByPinflFromFHDYO(this.filter.pinfl)
                        .then((res) => {
                           this.$emit('update:person', { person: res.data[0], filter: this.filter });
                           this.$emit('get:img', { person: res.data, filter: this.filter });
                           this.Data.person = res?.data[0];
                        })
                        .catch((err) => {
                           this.showApiError(err);
                        })
                        .finally(() => {
                           this.personLoading = false;
                        });
                  } else {
                     PersonService.GetByPassportDataFromDigital(
                        this.filter.Seria + this.filter.Number,
                        this.filter.DateOfBirth
                     )
                        .then((res) => {
                           this.$emit('update:person', { person: res.data, filter: this.filter });
                           this.$emit('get:img', { person: res.data, filter: this.filter });
                           this.Data.person = res.data;
                        })
                        .catch((err) => {
                           this.showApiError(err);
                        })
                        .finally(() => {
                           this.personLoading = false;
                        });
                  }
               }
            }
         });
      },
      filterReset() {
         this.filter = { ...this.filter, Seria: '', Number: '', DateOfBirth: '' };
         this.Data.person = null;
         this.Data.surname = null;
         this.Data.name = null;
         this.Data.birthDate = null;
         this.Data.genderId = null;
         this.$refs.ValidationPersonSearch.reset();
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
