<template>
   <b-overlay :show="show">
      <b-card>
         <validation-observer ref="ValidationDTO">
            <b-row>
               <b-col sm="12" md="3">
                  <form-select
                     :options="RegionList"
                     v-model="Data.regionId"
                     @input="ChangeRegion"
                     :label="$t('Oblast')"
                  ></form-select>
               </b-col>
               <b-col sm="12" md="3">
                  <form-select :options="DistrictList" v-model="Data.districtId" :label="$t('District')"></form-select>
               </b-col>
               <b-col sm="12" md="3">
                  <form-input-hrm
                     v-model="Data.organizationName"
                     :label="$t('organization')"
                     :placeholder="$t('organization')"
                     rules="required"
                  />
               </b-col>
               <b-col sm="12" md="3">
                  <form-input-hrm
                     v-model="Data.positionName"
                     :label="$t('positionClassification')"
                     :placeholder="$t('positionClassification')"
                     rules="required"
                  />
               </b-col>
               <b-col cols="12">
                  <validation-observer ref="ValidationDTO2">
                     <b-row>
                        <b-col sm="12" md="3">
                           <form-input-hrm
                              v-model="Data.passportSeria"
                              :label="$t('documentSeria')"
                              :placeholder="$t('documentSeria')"
                              rules="required"
                              v-mask="'AA'"
                           />
                        </b-col>
                        <b-col sm="12" md="3">
                           <form-input-hrm
                              v-model="Data.passportNumber"
                              :label="$t('passportNumber')"
                              :placeholder="$t('passportNumber')"
                              rules="required"
                           />
                        </b-col>
                        <b-col sm="12" md="3">
                           <label for>{{ $t('birthDate') }}</label>
                           <date-picker
                              v-model="Data.birthDate"
                              style="width: 100%"
                              size="sm"
                              lang="ru"
                              v-mask="'##.##.####'"
                              :placeholder="$t('birthDate')"
                              value-type="format"
                              format="DD.MM.YYYY"
                           ></date-picker>
                        </b-col>
                        <b-col sm="12" md="3">
                           <b-button @click="getPersonData" class="mt-2" variant="primary" size="sm"
                              ><feather-icon icon="SearchIcon" size="21"
                           /></b-button>
                        </b-col>
                        <b-col sm="12" md="3">
                           <form-input-hrm
                              v-model="Data.lastName"
                              :label="$t('lastName')"
                              :placeholder="$t('lastName')"
                              disabled
                           />
                        </b-col>
                        <b-col sm="12" md="3">
                           <form-input-hrm
                              v-model="Data.firstName"
                              :label="$t('firstName')"
                              :placeholder="$t('firstName')"
                              disabled
                           />
                        </b-col>
                     </b-row>
                  </validation-observer>
               </b-col>

               <!-- save button -->
               <b-col cols="12" class="text-right">
                  <b-button :disabled="saveLoading" @click="SaveData" variant="outline-success">
                     <feather-icon icon="CheckIcon"></feather-icon>
                     {{ $t('Save') }}
                  </b-button>
               </b-col>
            </b-row>
         </validation-observer>
      </b-card>
   </b-overlay>
</template>
<script>
// service
import ArbitrationJudgeService from '@/services/arbitrationjudge/arbitrationjudge.service';
import { BOverlay, BCard, BRow, BCol, BButton } from 'bootstrap-vue';
import FormInputTranslate from '@/components/translates/FormInputTranslate.vue';
import EmployeeService from '@/services/info/employee.service';
import RegionService from '@/services/info/region.service';
import DistrictService from '@/services/info/district.service';
// components
export default {
   components: {
      BOverlay,
      BCard,
      BRow,
      BCol,
      BButton,
      FormInputTranslate
   },
   name: 'NeedChamberServiceGroupEdit',
   data() {
      return {
         show: false,
         saveLoading: false,
         RegionList: [],
         DistrictList: [],
         Data: {
            districtId: '',
            regionId: '',
            organizationName: '',
            positionName: '',
            passportNumber: '',
            passportSeria: '',
            birthDate: '',
            lastName: '',
            firstName: ''
         }
      };
   },
   created() {
      RegionService.GetAsSelectList(211)
         .then((res) => {
            this.RegionList = res.data;
         })
         .catch((err) => {
            this.showApiError(err);
         })
         .finally(() => {
            this.saveLoading = false;
         });
      this.show = true;
      ArbitrationJudgeService.Get(this.$route.params.id)
         .then((res) => {
            this.Data = res.data;
            this.ChangeRegion();
         })
         .catch((error) => {
            this.makeToast(error.response.data.errors, 'danger');
         })
         .finally(() => {
            this.show = false;
         });
   },
   methods: {
      getPersonData() {
         this.$refs.ValidationDTO2.validate().then((success) => {
            if (success) {
               EmployeeService.GetByPassportData(
                  this.Data.passportSeria,
                  this.Data.passportNumber,
                  this.Data.birthDate
               ).then((res) => {
                  this.Data.lastName = res.data.person.surnameLatin;
                  this.Data.firstName = res.data.person.nameLatin;
               });
            }
         });
      },

      ChangeRegion() {
         if (this.Data.regionId)
            DistrictService.GetAsSelectList(this.Data.regionId)
               .then((res) => {
                  this.DistrictList = res.data;
               })
               .catch((err) => {
                  this.showApiError(err);
               })
               .finally(() => {
                  this.saveLoading = false;
               });
      },

      SaveData() {
         this.$refs.ValidationDTO.validate().then((success) => {
            if (success) {
               this.saveLoading = true;
               ArbitrationJudgeService.Update(this.Data)
                  .then(() => {
                     this.makeToast(this.$t('SaveSuccess'), 'success');
                     this.$router.push({ name: 'arbitrationjudge' });
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

<style>
.col-form-label,
label {
   line-height: initial;
}
</style>
