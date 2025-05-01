<template>
   <b-overlay :show="show">
      <b-row>
         <b-col sm="12" md="12" lg="12">
            <b-card>
               <validation-observer ref="ValidationDTO">
                  <b-row>
                     <b-col sm="12" md="4">
                        <div class="form-group">
                           <form-input v-model="Data.docNumber" required :label="$t('docnumber')" />
                        </div>
                     </b-col>
                     <b-col sm="12" md="4">
                        <form-picker v-model="Data.docOn" required :label="$t('docOn')" :placeholder="$t('docOn')" />
                     </b-col>
                     <b-col sm="12" md="4">
                        <form-picker v-model="Data.endOn" required :label="$t('enddate')" :placeholder="$t('enddate')" />
                     </b-col>
                     <b-col sm="12" md="4">
                        <EmployeeSelect2 v-model="Data.employeeId" @update:data="onUpdateEmployee" required-star />
                     </b-col>
                     <b-col sm="12" md="4">
                        <form-select :options="TaxBenefitTypeList" v-model="Data.taxBenefitTypeId" required-star label="taxbenefittype" />
                     </b-col>
                     <b-col sm="12" md="4">
                        <form-input v-model="Data.details" :placeholder="$t('details')" :label="$t('details')"></form-input>
                     </b-col>
                  </b-row>
                  <b-row>
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
// service
import TaxBenefitTypeService from '@/services/hrm/taxbenefittype.service';
import TaxBenefitService from '@/services/hrm/taxbenefit.service';
// components
import { BOverlay, BCard, BRow, BCol, BButton, BFormGroup, BFormTextarea } from 'bootstrap-vue';
import EmployeeSelect2 from '@/views/components/employee/EmployeeSelect2.vue';

export default {
   components: {
      BOverlay,
      BCard,
      BRow,
      BCol,
      BButton,
      BFormGroup,
      BFormTextarea,
      EmployeeSelect2
   },
   name: 'Edit',
   data() {
      return {
         show: false,
         TaxBenefitTypeList: [],
         loadingButton: false,
         saveLoading: false,
         Data: {
            id: null,
            docNumber: null,
            docOn: null,
            endOn: null,
            details: null,
            employeeId: null,
            taxBenefitTypeId: null
         }
      };
   },
   created() {
      this.show = true;
      TaxBenefitService.Get(this.$route.params.id)
         .then((res) => {
            this.Data = res.data;
         })
         .catch((error) => {
            this.makeToast(error.response.data.errors, 'danger');
         })
         .finally(() => {
            this.show = false;
         });

      TaxBenefitTypeService.GetAsSelectList()
         .then((res) => {
            if (Array.isArray(res.data)) {
               this.TaxBenefitTypeList = res.data;
            }
         })
         .catch((error) => {
            this.makeToast(error.response.data.errors, 'danger');
         });
   },
   methods: {
      onUpdateEmployee(e) {
         this.tabrow.employee = e?.fullName;
      },
      SaveData() {
         this.$refs.ValidationDTO.validate().then((success) => {
            if (success) {
               this.saveLoading = true;
               TaxBenefitService.Update(this.Data)
                  .then(() => {
                     this.makeToast(this.$t('SaveSuccess'), 'success');
                     this.$router.push({ name: 'TaxBenefit' });
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
input {
   margin: 0.4rem;
}
</style>
