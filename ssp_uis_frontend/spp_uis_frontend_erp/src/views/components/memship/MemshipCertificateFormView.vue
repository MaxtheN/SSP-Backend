<template>
   <div v-if="certificate">
      <b-row>
         <b-col sm="12" md="6" lg="6">
            <form-input-hrm :label="$t('docnumber')" disabled v-model="certificate.docNumber" />
         </b-col>
         <b-col sm="12" md="6" lg="6">
            <form-input-hrm :label="$t('docdate')" disabled v-model="certificate.docOn" />
         </b-col>
         <b-col sm="12" md="12" lg="12">
            <form-input-hrm :label="$t('contractor')" disabled v-model="certificate.contractor" />
         </b-col>
         <b-col sm="12" md="6" lg="6">
            <form-input-hrm :label="$t('expireOn')" disabled v-model="certificate.expireOn" />
         </b-col>
         <b-col sm="12" md="6" lg="6">
            <form-input-hrm
               :label="$t('orgSettlementAccount')"
               disabled
               v-model="certificate.contractorSettlementAccount"
            />
         </b-col>
         <!-- <b-col sm="12" md="6" lg="6">
            <form-input-hrm disabled :label="$t('inn')" v-model="certificate.contractorInn" />
         </b-col> -->
         <b-col sm="12" md="6" lg="6">
            <form-input-hrm :label="$t('AdditionalInfo')" v-model="certificate.details" />
         </b-col>
      </b-row>
   </div>
</template>

<script>
import { BRow, BCol } from 'bootstrap-vue';

export default {
   props: {
      certificate: {
         type: Object,
         default: () => ({})
      }
   },
   components: {
      BRow,
      BCol
   }
};
</script>
