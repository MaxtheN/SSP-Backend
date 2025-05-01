<template>
   <b-overlay :show="show">
      <b-card>
         <validation-observer ref="ValidationDTO">
            <b-row>
               <b-col sm="12" md="4">
                  <form-input-hrm
                     v-model="Data.docNumber"
                     :label="$t('docnumber')"
                     :placeholder="$t('docnumber')"
                     rules="required"
                  />
               </b-col>
               <b-col sm="12" md="4">
                  <form-picker v-model="Data.docOn" required :label="$t('docOn')" :placeholder="$t('docOn')" />
               </b-col>
               <b-col sm="12" md="4">
                  <ContractorListSelect
                     :label="$t('contractor')"
                     v-model="Data.contractorId"
                     :valuename="Data.contractor"
                     @update:valuename="(e) => (Data.contractor = e)"
                     @update:data="(e) => (Data.contractor = e ? e.fullName : '')"
                  />
               </b-col>
               <b-col sm="12" md="4">
                  <form-select
                     v-model="Data.serviceContractId"
                     :options="serviceContractList"
                     :label="$t('ServiceContract')"
                     required-star
                  />
               </b-col>
               <b-col sm="12" md="4">
                  <form-select
                     v-model="Data.serviceContractTableId"
                     :options="serviceContractTableList"
                     :label="$t('serviceContractTable')"
                     required-star
                  />
               </b-col>
               <b-col sm="12" md="4">
                  <EmployeeManageSelect
                     v-model="Data.employeeManageId"
                     :label="$t('employeeManage')"
                     :employee="Data.employeeFull"
                     :employee-id="Data.employeeId"
                     @update:data="onUpdateEmployeeManage"
                     required-star
                  />
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
import CompleteServiceService from '@/services/srv/completeservice.service';
import ContractorService from '@/services/info/contractor.service';
// components
import { BOverlay, BCard, BRow, BCol, BButton } from 'bootstrap-vue';
import EmployeeManageSelect from '@/views/components/hrm/EmployeeManageSelect.vue';
import ContractorListSelect from '@/views/components/info/ContractorListSelect.vue';

export default {
   components: {
      BOverlay,
      BCard,
      BRow,
      BCol,
      BButton,
      EmployeeManageSelect,
      ContractorListSelect
   },
   name: 'CompleteServiceEdit',
   data() {
      return {
         show: false,
         saveLoading: false,
         Data: {
            docNumber: '',
            docOn: '',
            details: '',
            contractorId: null,
            serviceContractId: null,
            serviceContractTableId: null,
            employeeManageId: null
         },
         serviceContractList: [],
         serviceContractTableList: [],
         employeeManageList: []
      };
   },
   created() {
      this.show = true;
      CompleteServiceService.Get(this.$route.params.id)
         .then((res) => {
            this.Data = res.data;
         })
         .catch((error) => {
            this.showApiError(error);
         })
         .finally(() => {
            this.show = false;
         });
   },
   methods: {
      onUpdateEmployeeManage(e) {
         this.Data.employeeId = e?.employeeId;
         this.Data.employeeFull = e?.employee;
      },
      SaveData() {
         this.$refs.ValidationDTO.validate().then((success) => {
            if (success) {
               this.saveLoading = true;
               CompleteServiceService.Update(this.Data)
                  .then(() => {
                     this.makeToast(this.$t('SaveSuccess'), 'success');
                     this.$router.push({ name: 'CompleteService' });
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
