<template>
   <div v-if="data">
      <b-row>
         <b-col sm="12" md="6" lg="6">
            <form-input-hrm :label="$t('docnumber')" disabled v-model="data.docNumber" />
         </b-col>
         <b-col sm="12" md="6" lg="6">
            <form-input-hrm :label="$t('docdate')" disabled v-model="data.docOn" />
         </b-col>
         <b-col sm="12" md="12" lg="12">
            <form-input-hrm :label="$t('contractor')" disabled v-model="data.contractor" />
         </b-col>
         <b-col sm="12" md="6" lg="6">
            <form-input-hrm :label="$t('applicationType')" disabled v-model="data.applicationType" />
         </b-col>
         <b-col sm="12" md="6" lg="6">
            <form-input-hrm :label="$t('baseFixedMinimumValue')" disabled v-model="data.baseFixedMinimumValue" />
         </b-col>
         <b-col sm="12" md="6" lg="6" class="pt-1">
            <b-form-checkbox v-model="data.canPayDivided" disabled> {{ $t('canPayDivided') }} </b-form-checkbox>
         </b-col>
         <hr class="w-100 mx-1" />
         <b-col cols="12">
            <h4>
               {{ $t('MemshipContract') }}
            </h4>
         </b-col>
         <b-col sm="12" md="6" lg="6">
            <form-input-hrm :label="$t('docnumber')" disabled v-model="data.memshipContractDocNumber" />
         </b-col>
         <b-col sm="12" md="6" lg="6">
            <form-input-hrm :label="$t('docdate')" disabled v-model="data.memshipContractDocOn" />
         </b-col>
      </b-row>
   </div>
</template>

<script>
import { BRow, BCol, BFormCheckbox } from 'bootstrap-vue';

export default {
   props: {
      data: {
         type: Object,
         default: () => ({})
      }
   },
   components: {
      BRow,
      BCol,
      BFormCheckbox
   }
};
</script>
