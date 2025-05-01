<template>
   <div v-if="contract">
      <b-row>
         <b-col sm="12" md="6" lg="6">
            <form-input-hrm :disabled="!isChange" :label="$t('docnumber')" v-model="contract.docNumber" />
         </b-col>
         <b-col sm="12" md="6" lg="6">
            <form-input-hrm :disabled="!isChange" :label="$t('docdate')" v-model="contract.docOn" />
         </b-col>
         <b-col sm="12" md="12" lg="12">
            <form-input-hrm :label="$t('contractor')" disabled v-model="contract.contractor" />
         </b-col>
         <b-col sm="12" md="6" lg="6">
            <form-input-hrm :label="$t('director')" disabled v-model="contract.director" />
         </b-col>
         <b-col sm="12" md="6" lg="6">
            <form-input-hrm :label="$t('inn')" disabled v-model="contract.inn" />
         </b-col>
         <b-col sm="12" md="6" lg="6">
            <form-input-hrm :label="$t('pinfl')" disabled v-model="contract.pinfl" />
         </b-col>
         <b-col sm="12" md="12" lg="6">
            <form-input-hrm :label="$t('memshipContractType')" disabled v-model="contract.memshipContractType" />
         </b-col>
         <b-col sm="12" md="6" lg="6">
            <form-input-hrm :label="$t('bankname')" disabled v-model="contract.bank" />
         </b-col>
         <b-col sm="12" md="6" lg="6" v-if="contract.memshipContractTypeId != 2">
            <form-currency-input
               :label="$t('baseFixedMinimumValue')"
               disabled
               v-model="contract.baseFixedMinimumValue"
            />
         </b-col>
         <b-col sm="12" md="6" lg="6">
            <form-input-hrm :disabled="!isChange" :label="$t('AdditionalInfo')" v-model="contract.details" />
         </b-col>
      </b-row>
      <div v-if="isChange" class="d-flex justify-content-end">
         <b-button @click="UpdateContract" size="sm" class="mt-2" variant="outline-success">
            <feather-icon icon="CheckIcon"></feather-icon>
            {{ $t('Save') }}
         </b-button>
      </div>
   </div>
</template>

<script>
import { BRow, BCol, BButton } from 'bootstrap-vue';
import MemshipContractService from '@/services/document/memshipcontract.service';
export default {
   props: {
      contract: {
         type: Object,
         default: () => ({})
      },
      isChange: {
         type: Boolean,
         default: false
      }
   },
   components: {
      BRow,
      BButton,
      BCol
   },
   methods: {
      UpdateContract() {
         MemshipContractService.ChangeContractorDocnumber({
            id: this.$props.contract.id,
            detail: this.$props.contract.details,
            docOn: this.$props.contract.docOn,
            docNumber: this.$props.contract.docNumber
         })
            .then(() => {
               this.makeToast(this.$t('SaveSuccess'), 'success');

               this.$router.push({ name: 'MemshipContract' });
            })
            .catch((err) => {
               this.showApiError(err);
            });
      }
   }
};
</script>
