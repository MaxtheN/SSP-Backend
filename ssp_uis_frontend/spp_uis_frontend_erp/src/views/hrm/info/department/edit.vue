<template>
   <b-overlay :show="show">
      <b-row>
         <b-col sm="12" md="12" lg="12">
            <b-card>
               <validation-observer ref="ValidationDTO">
                  <b-row>
                     <b-col sm="12" md="3">
                        <div class="form-group">
                           <form-input v-model="Data.code" :label="$t('kode')" required type="number" />
                        </div>
                     </b-col>
                     <b-col sm="12" md="3">
                        <div class="form-group">
                           <form-input v-model="Data.orderCode" :label="$t('orderCode')" required type="number" />
                        </div>
                     </b-col>
                     <b-col sm="12" md="3">
                        <form-input-hrm
                           v-model="Data.indexCode"
                           :placeholder="$t('indexCode')"
                           :label="$t('indexCode')"
                           rules="required"
                        />
                     </b-col>
                     <b-col sm="12" md="3">
                        <form-select
                           :options="IndecatorDepartmentList"
                           v-model="Data.indicatorDepartmentId"
                           :label="$t('IndecatorDepartment')"
                        ></form-select>
                        <!-- <form-input-hrm
                           v-model="Data.indicatorDepartmentId"
                           :placeholder="$t('indicatorDepartmentId')"
                           :label="$t('indicatorDepartmentId')"
                           rules="required"
                        /> -->
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
                        <label>{{ $t('Department2') }}</label>
                        <v-select
                           :options="DepartmentList"
                           :reduce="(item) => item.value"
                           :placeholder="$t('ChooseBelow')"
                           label="text"
                           v-model="Data.parentId"
                        ></v-select>
                     </b-col>

                     <b-col sm="12" md="3">
                        <form-select
                           :options="StateList"
                           v-model="Data.stateId"
                           required-star
                           :label="$t('Status')"
                        ></form-select>
                     </b-col>
                  </b-row>
                  <b-row class="mt-3">
                     <b-col sm="12" md="6" lg="6" class="text-left"></b-col>
                     <b-col sm="12" md="6" lg="6" class="text-right">
                        <b-button :disabled="saveLoading" @click="SaveData" size="sm" variant="outline-success">
                           <feather-icon icon="CheckIcon"></feather-icon>
                           {{ $t('Save') }}
                        </b-button>
                     </b-col>
                  </b-row>
               </validation-observer>
            </b-card>
         </b-col>
      </b-row>
   </b-overlay>
</template>
<script>
import ManualService from '@/services/others/manual.service';
import DepartmentService from '@/services/info/department.service';
import FormInputTranslate from '@/components/translates/FormInputTranslate.vue';
import IndicatorDepartmentService from '@/services/kpi/indicatordepartment.service';

import { ValidationObserver } from 'vee-validate';

import {
   BOverlay,
   BCard,
   BCardBody,
   BRow,
   BCol,
   BFormInput,
   BButton,
   BFormGroup,
   BModal,
   BInputGroup,
   BInputGroupAppend,
   BFormCheckbox,
   BTable,
   BTd,
   BTr
} from 'bootstrap-vue';

export default {
   components: {
      FormInputTranslate,
      BOverlay,
      BCard,
      BCardBody,
      BRow,
      BCol,
      BFormInput,
      BButton,
      BFormGroup,
      BModal,
      BInputGroup,
      BInputGroupAppend,
      BFormCheckbox,
      ValidationObserver,
      BTable,
      BTd,
      BTr
   },
   name: 'DepartmentEdit',
   data() {
      return {
         show: false,
         StateList: [],
         saveLoading: false,
         Data: {
            translates: [],
            shortName: '',
            fullName: ''
         },
         DepartmentList: [],
         IndecatorDepartmentList: [],
         filter: {
            organizationId: 0,
            search: '',
            sortBy: '',
            orderType: '',
            page: 1,
            pageSize: 900
         }
      };
   },

   created() {
      this.show = true;
      DepartmentService.Get(this.$route.params.id).then((res) => {
         this.show = false;
         this.Data = res.data;
      });

      ManualService.StateSelectList()
         .then((res) => {
            this.StateList = res.data;
         })
         .catch((error) => {
            this.showApiError(error);
         });
      IndicatorDepartmentService.GetAsSelectList()
         .then((res) => {
            this.IndecatorDepartmentList = res.data;
         })
         .catch((error) => {
            this.showApiError(error);
         });

      DepartmentService.GetAsSelectList(null, this.filter).then((res) => {
         this.DepartmentList = res.data;
      });
   },
   methods: {
      SaveData() {
         this.$refs.ValidationDTO.validate().then((success) => {
            if (success) {
               this.saveLoading = true;
               DepartmentService.Update(this.Data)
                  .then(() => {
                     this.makeToast(this.$t('SaveSuccess'), 'success');
                     this.$router.push({ name: 'Department' });
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
