<template>
   <b-overlay :show="show">
      <div>
         <!-- personsearch -->
         <b-card>
            <FormPersonSearch
               @update:person="onUpdatePerson"
               :person="Data.person"
               page="user"
               :hide-search="Data.id > 0"
            />
         </b-card>

         <b-card>
            <validation-observer ref="ValidationDTO">
               <b-row>
                  <b-col sm="12" md="4" class="mb-1">
                     <EmployeeManageSelect
                        :employee="Data.employeeManage"
                        v-model="Data.employeeManageId"
                        :label="$t('employeeManage')"
                        @update:data="onUpdateEmployeeManage"
                        :organization-id="Data.organizationId"
                     />
                  </b-col>
                  <b-col sm="12" md="4">
                     <form-input-hrm
                        v-model="Data.userName"
                        :placeholder="$t('userName')"
                        :label="$t('userName')"
                        rules="required"
                     />
                  </b-col>

                  <b-col sm="12" md="4">
                     <form-input-hrm
                        v-model="Data.email"
                        type="email"
                        rules="email"
                        :placeholder="$t('email')"
                        :label="$t('email')"
                     />
                  </b-col>

                  <b-col sm="12" md="4">
                     <form-select
                        :options="OrganizationList"
                        :label="$t('Organization')"
                        :placeholder="$t('ChooseBelow')"
                        v-model="Data.organizationId"
                     />
                  </b-col>

                  <b-col sm="12" md="4">
                     <form-input-hrm
                        v-model="Data.password"
                        type="password"
                        :rules="Data.id == 0 || Data.password ? 'required|min:6' : ''"
                        :placeholder="$t('Password')"
                        :label="$t('Password')"
                        name="password"
                        vid="password"
                        autocomplete="new-password"
                     />
                  </b-col>

                  <b-col sm="12" md="4">
                     <form-input-hrm
                        v-model="confirmedPassword"
                        type="password"
                        :rules="Data.id == 0 || Data.password ? 'required|confirmed:password' : ''"
                        :placeholder="$t('confirmedPassword')"
                        :label="$t('confirmedPassword')"
                     />
                  </b-col>

                  <b-col sm="12" md="4">
                     <form-input-hrm
                        v-model="Data.phoneNumber"
                        :mask="'(998) ## ### ## ##'"
                        :placeholder="$t('phoneNumber')"
                        :label="$t('phoneNumber')"
                        rules="required|validatorPhone"
                     />
                  </b-col>

                  <b-col sm="12" md="4">
                     <form-select
                        multiple
                        :options="RoleList"
                        :label="$t('role')"
                        :placeholder="$t('ChooseBelow')"
                        v-model="Data.roles"
                     />
                  </b-col>
                  <b-col sm="12" md="4" class="mb-1">
                     <form-select :options="StateList" v-model="Data.stateId" label="Status"></form-select>
                  </b-col>
               </b-row>
               <b-row class="mt-3">
                  <b-col sm="12" md="6" lg="6" class="text-left"></b-col>
                  <b-col sm="12" md="6" lg="6" class="text-right">
                     <b-button @click="SaveData" size="sm" variant="outline-success">
                        <feather-icon icon="CheckIcon"></feather-icon>
                        {{ $t('Save') }}
                     </b-button>
                  </b-col>
               </b-row>
            </validation-observer>
         </b-card>
      </div>
   </b-overlay>
</template>
<script>
import UserService from '@/services/managment/user.service';
import RoleService from '@/services/managment/role.service';
import ManualService from '@/services/others/manual.service';
import OrganizationService from '@/services/managment/organization.service';
import FormPersonSearch from '@/components/forms/PersonSearch/form-person-search.vue';
import EmployeeManageSelect from '@/views/components/hrm/EmployeeManageSelect.vue';

import { BOverlay, BCard, BRow, BCol, BFormInput, BButton } from 'bootstrap-vue';

export default {
   components: {
      EmployeeManageSelect,
      BOverlay,
      BCard,
      BRow,
      BCol,
      BFormInput,
      BButton,
      FormPersonSearch
   },
   name: 'Edit',
   data() {
      return {
         show: true,
         OrganizationList: [],
         RoleList: [],
         StateList: [],
         loadingButton: false,
         confirmedPassword: '',
         Data: {
            person: null
         }
      };
   },
   created() {
      this.show = true;
      UserService.Get(this.$route.params.id)
         .then((res) => {
            this.Data = res.data;
         })
         .finally(() => {
            this.show = false;
         });

      RoleService.GetAsSelectList()
         .then((res) => {
            this.RoleList = res.data;
         })
         .catch((error) => {
            this.showApiError(error);
         });
      ManualService.StateSelectList()
         .then((res) => {
            this.StateList = res.data;
         })
         .catch((error) => {
            this.showApiError(error);
         });

      OrganizationService.GetAsSelectList()
         .then((res) => {
            this.OrganizationList = res.data;
         })
         .catch((error) => {
            this.showApiError(error);
         });
   },
   methods: {
      onUpdateEmployeeManage(e) {
         this.Data.employeeManageId = e?.id;
         this.Data.employeeManage = e?.employee;
         this.Data.documentId = e?.docId;
      },
      onUpdatePerson(e) {
         this.Data.employeeManageId = e?.person?.employeeManageId;
         this.Data.documentId = e?.person?.documentId;
         this.Data.employeeManage =
            this.Data.employeeManageId != null
               ? e?.person?.surnameLatin + ' ' + e?.person?.nameLatin + ' ' + e?.person?.patronymLatin
               : null;
         this.Data.person = Object.assign({}, e.person);
      },
      checkValid() {
         let valid = false;
         if (this.Data.shortName !== '' && this.Data.shortName !== null) {
            valid = true;
         } else {
            valid = false;
         }
         return valid;
      },
      SaveData() {
         this.$refs.ValidationDTO.validate().then((success) => {
            if (success) {
               this.loadingButton = true;
               if (this.checkValid()) {
                  if (this.checkValid()) {
                     UserService.Update(this.Data)
                        .then((res) => {
                           this.makeToast(this.$t('SaveSuccess'), 'success');
                           this.$router.push({ name: 'user' });
                        })
                        .catch((err) => {
                           this.showApiError(err);
                        });
                  }
               }
            }
         });
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
