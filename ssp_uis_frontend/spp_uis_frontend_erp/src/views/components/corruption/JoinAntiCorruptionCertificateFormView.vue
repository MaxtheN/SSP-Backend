<template>
   <div v-if="data && data">
      <b-row>
         <b-col sm="12" md="6" lg="6">
            <form-input-hrm :label="$t('docnumber')" disabled v-model="data.docNumber" />
         </b-col>
         <b-col sm="12" md="6" lg="6">
            <form-input-hrm :label="$t('expireOn')" disabled v-model="data.docOn" />
         </b-col>
         <b-col sm="12" md="6" lg="6">
            <form-input-hrm :label="$t('expireOn')" disabled v-model="data.expireOn" />
         </b-col>
         <b-col sm="12" md="12" lg="12">
            <form-input-hrm :label="$t('contractor')" disabled v-model="data.contractor" />
         </b-col>
         <b-col sm="12" md="6" lg="6">
            <form-input-hrm :label="$t('contractorInn')" disabled v-model="data.contractorInn" />
         </b-col>
         <b-col sm="12" md="6" lg="6">
            <form-input-hrm :label="$t('director')" disabled v-model="data.contractorDirector" />
         </b-col>
         <b-col sm="12" md="6" lg="6">
            <form-input-hrm :label="$t('region')" disabled v-model="data.region" />
         </b-col>
         <b-col sm="12" md="6" lg="6">
            <form-input-hrm :label="$t('district')" disabled v-model="data.district" />
         </b-col>
      </b-row>
   </div>
</template>

<script>
import { BRow, BCol } from 'bootstrap-vue';
export default {
   props: {
      data: {
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
<style lang="scss">
.priceTable {
   thead {
      tr {
         background-color: #f0f0f0;
      }
   }

   tr th,
   tr td {
      padding: 7px;
      border-collapse: collapse;
      border: 1px solid #f5f5f5;
   }

   tr:nth-child(even) {
      background-color: #f7f7f7;
   }
}
</style>
